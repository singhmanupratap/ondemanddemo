using System.Web.Mvc;
using Unity.Mvc5;
using Common.Interfaces;
using DataAccess;
using BusinessLayer;
using Unity;
namespace WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            container.RegisterType<ISolutionRepository, SolutionRepository>();
            container.RegisterType<ISolutionBusinessLayer, SolutionBusinessLayer>();

            DependencyResolver.SetResolver(resolver: new UnityDependencyResolver(container));
        }
    }
}