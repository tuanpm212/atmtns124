using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace NailShop
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
            name: "DefaultGetApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional, action = "Get" },
            constraints: new { id = @"\d+", httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
              );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { id = @"\d+" }
            );

            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional }
            );
        }
    }
}
