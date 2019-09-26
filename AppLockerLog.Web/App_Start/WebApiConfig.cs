using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AppLockerLog.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var site = System.Configuration.ConfigurationManager.AppSettings["DefaultPage"].ToString();
            var cors = new EnableCorsAttribute(site, "*", "*") { SupportsCredentials = true };
            config.EnableCors(cors);
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API configuration and services
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            jsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
