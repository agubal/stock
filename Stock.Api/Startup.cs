using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using Stock.Api;
using Stock.Api.Filters;
using Stock.Dependencies;
using StructureMap;

[assembly: OwinStartup(typeof(Startup))]
namespace Stock.Api
{
    /// <summary>
    /// Owin startup class
    /// </summary>
    public class Startup
    {
        private readonly IContainer _container;

        public Startup()
        {
            //Initialize Inversion of Controle container
            _container = IoC.Initialize();
        }

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration
            {
                DependencyResolver = new DependencyResolution.StructureMapWebApiDependencyResolver(_container),
                Filters = { new ErrorHandler() }
            };

            //Register routing:
            WebApiConfig.Register(config);

            //Allow Cors:
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
    }
}