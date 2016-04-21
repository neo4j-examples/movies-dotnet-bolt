namespace Neo4jDotNetDemo.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Neo4j.Driver.V1;

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

            var result = new MovieResult();
            using (var session = WebApiConfig.Neo4jDriver.Session())
            {
                var statementResult = session.Run(statementTemplate, statementParameters);

                var record = statementResult.Single();

                result.title = record["title"].As<string>();

                var castResults = new List<CastResult>();
                List<object> cast = record["cast"].As<List<object>>();
                foreach (IList<object> castMember in cast)
                {
                    var castResult = new CastResult
                    {
                        name = castMember[0].As<string>(),
                        job = castMember[1].As<string>(),
                        role = castMember[2]?.As<List<string>>()
                    };
                    castResults.Add(castResult);
                }
                result.cast = castResults;
            }

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