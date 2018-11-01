using Inventory.BLL.Infrastructure;
using Inventory.Web.Util;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Inventory.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            NinjectModule webModule = new WebModule();
            NinjectModule serviceModule = new ServiceModule("DefaultConnection");
            NinjectModule accountModule = new AccountModule("AccountConnection");
            var kernel = new StandardKernel(webModule, serviceModule, accountModule);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}
