namespace Neo4jDotNetDemo.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    [RoutePrefix("movie")]
    public class MovieController : ApiController
    {
        [HttpGet]
        [Route("{title}")]
        public IHttpActionResult GetMovieByTitle(string title)
        {
            //query = ("MATCH (movie:Movie {title:{title}}) "
            // "OPTIONAL MATCH (movie)<-[r]-(person:Person) "
            // "RETURN movie.title as title,"
            // "collect([person.name, "
            // "         head(split(lower(type(r)), '_')), r.roles]) as cast "
            // "LIMIT 1")

            var statementTemplate = "MATCH (movie:Movie {title:{title}}) OPTIONAL MATCH (movie)<-[r]-(person:Person) RETURN movie.title as title, collect([person.name, head(split(lower(type(r)), '_')), r.roles]) as cast LIMIT 1";
            var statementParameters = new Dictionary<string, object> {{"title", title}};

            var session = WebApiConfig.Neo4jDriver.Session();
            var cursor = session.Run(statementTemplate, statementParameters);

            var cursorResult = cursor.Stream().Single();
            var result = new MovieResult();
            result.title = cursorResult.Values["title"];

            var castResults = new List<CastResult>();
            List<object> cast = cursorResult.Values["cast"];
            foreach (IList<object> castMember in cast)
            {
                var roles = castMember[2] as IList<object>;
                var castResult = new CastResult
                {
                    name = castMember[0].ToString(),
                    job = castMember[1].ToString()
                };
                if (roles != null)
                {
                    castResult.role = roles.Select(c => c.ToString());
                }
                castResults.Add(castResult);
            }
            result.cast = castResults;

            return Ok(result);
        }
    }

    public class CastResult
    {
        public string name { get; set; }
        public string job { get; set; }
        public IEnumerable<string> role { get; set; }
    }

    public class MovieResult
    {
        public string title { get; set; }
        public IEnumerable<CastResult> cast { get; set; }
    }
}