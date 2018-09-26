using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json.Converters;

namespace DavidLievrouw.Api
{
    public static class WebApiConfigurator
    {
        public static HttpConfiguration ConfigureWebApi(IContainer container)
        {
            var config = new HttpConfiguration();
            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;

            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            return config;
        }
    }
}