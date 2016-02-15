namespace Neo4jDotNetDemo.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    [RoutePrefix("graph")]
    public class GraphController : ApiController
    {
        [HttpGet]
        [Route("{limit:int?}", Name = "getgraph")]
        public IHttpActionResult Index(int limit = 100)
        {
            //query = ("MATCH (m:Movie)<-[:ACTED_IN]-(a:Person) "
            // "RETURN m.title as movie, collect(a.name) as cast "
            // "LIMIT {limit}")

            var statementTemplate = "MATCH (a:Person)-[:ACTED_IN]->(m:Movie) RETURN m.title as movie, collect(a.name) as cast LIMIT {limit}";
            var statementParameters = new Dictionary<string, object> {{"limit", limit}};

            var session = WebApiConfig.Neo4jDriver.Session();

            var cursor = session.Run(statementTemplate, statementParameters);

            var nodes = new List<NodeResult>();
            var relationships = new List<object>();
            var i = 0;

            foreach (var record in cursor.Stream())
            {
                var target = i;
                nodes.Add(new NodeResult {title = record["movie"], label = "movie"});
                var castMembers = new List<object>(record["cast"]).Cast<string>();
                foreach (var castMember in castMembers)
                {
                    var source = nodes.FindIndex(c => c.title == castMember);
                    if (source == -1)
                    {
                        nodes.Add(new NodeResult {title = castMember, label = "actor"});
                        source = i;
                        i += 1;
                    }
                    relationships.Add(new {source, target});
                }
            }

            return Ok(new {nodes, links = relationships});
        }
    }

    public class NodeResult
    {
        public string title { get; set; }
        public string label { get; set; }
    }

    public class Movie
    {
        public string title { get; set; }
        public int released { get; set; }
        public string tagline { get; set; }
    }
}