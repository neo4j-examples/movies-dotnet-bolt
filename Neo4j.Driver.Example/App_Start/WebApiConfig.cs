namespace Neo4jDotNetDemo
{
    using System.Configuration;
    using System.Linq;
    using System.Web.Http;
    using Neo4j.Driver.V1;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public static class WebApiConfig
    {
        public static IDriver Neo4jDriver { get; private set; }

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            //Use an IoC container and register as a Singleton
            var url = ConfigurationManager.AppSettings["GraphDBUrl"];
            string username = ConfigurationManager.AppSettings["GraphDBUser"];
            string password = ConfigurationManager.AppSettings["GraphDBPassword"];
            IAuthToken authToken =  AuthTokens.Basic(username,password);
            IDriver driver = GraphDatabase.Driver(url);
            if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(username))
            {
                driver = GraphDatabase.Driver(url,authToken: authToken);
            }
            Neo4jDriver = driver;
        }
    }
}
