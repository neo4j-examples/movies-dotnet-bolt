namespace Neo4jDotNetDemo.Controllers
{
    using Neo4j.Driver.V1;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    [RoutePrefix("search")]
    public class SearchController : ApiController
    {
        [HttpGet]
        [Route("{q?}")]
        public IHttpActionResult SearchMoviesByTitle(string q = "Matrix")
        {
// tag::minimum-viable-snippet[]
            var movies = new List<Movie>();

            using (var session = WebApiConfig.Neo4jDriver.Session())
            {
                var result = session.Run("MATCH (movie:Movie) WHERE movie.title CONTAINS {title} RETURN movie", new {title = q});

                foreach (var record in result)
                {
                    var node = record["movie"].As<INode>();
                    movies.Add(new Movie
                    {
                        released = node["released"].As<int>(),
                        tagline = node["tagline"].As<string>(),
                        title = node["title"].As<string>()
                    });
                }
            }
// end::minimum-viable-snippet[]
            return Ok(movies.Select(m => new {movie = m}));
        }
    }
}