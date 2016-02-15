namespace Neo4jDotNetDemo.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Neo4j.Driver.Internal;

    [RoutePrefix("search")]
    public class SearchController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult SearchMoviesByTitle(string q)
        {
            //    query = ("MATCH (movie:Movie) "
            //         "WHERE movie.title =~ {title} "
            //         "RETURN movie")
            //    params={"title": "(?i).*" + q + ".*"}

            var statementTemplate = "MATCH (movie:Movie) WHERE movie.title =~ {title} RETURN movie";
            var statementParameters = new Dictionary<string, object> {{"title", $"(?i).*{q}.*"}};

            var session = WebApiConfig.Neo4jDriver.Session();
            var cursor = session.Run(statementTemplate, statementParameters);

            var movies = new List<Movie>();

            foreach (var record in cursor.Stream())
            {
                var node = (Node) record["movie"];
                movies.Add(new Movie {released = (int) (long) node.Properties["released"], tagline = node.Properties["tagline"].ToString(), title = node.Properties["title"].ToString()});
            }

            return Ok(movies.Select(m => new {movie = m}));
        }
    }
}