using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Soccer_Vision
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings
             .Add(new System.Net.Http.Formatting.RequestHeaderMapping("Accept",
                 "text/html",
                 StringComparison.InvariantCultureIgnoreCase,
                 true,
                 "application/json"));
        }
    }
}
