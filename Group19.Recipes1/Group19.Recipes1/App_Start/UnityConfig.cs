using System;
using System.Configuration;
using System.Web;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Group19.AspNetIdentity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using Group19.Recipes1.DataAccess.Models;
using Group19.Recipes1.DataAccess.Repository;

namespace Group19.Recipes1.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            container.RegisterType<ApplicationUserManager>();
            container.RegisterType<ApplicationSignInManager>();
            container.RegisterType<IAuthenticationManager>(
                new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            var config = Fluently.Configure()
#if DEBUGMS
               .Database(MsSqlConfiguration.MsSql2012
                   .ConnectionString(ConfigurationManager.ConnectionStrings["RecipesMS"].ConnectionString))
#else
               .Database(OracleManagedDataClientConfiguration.Oracle10
                   .ConnectionString(ConfigurationManager.ConnectionStrings["RecipesOracle"].ConnectionString))
#endif
               .Mappings(x => x.FluentMappings.AddFromAssemblyOf<IdentityUserMap>()
                                              .AddFromAssemblyOf<RecipeMap>());
            var sessionFactory = config.BuildSessionFactory();

            container.RegisterType<IUserStore<IdentityUser>, UserStore<IdentityUser>>(
                new InjectionConstructor(sessionFactory));

            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager(), new InjectionConstructor(sessionFactory));
            container.RegisterType<IRepository<Recipe>, RecipeRepository>();
            container.RegisterType<IRepository<IdentityUser>, UsersRepository>();
            container.RegisterType<IRepository<Measurement>, MeasurementRepository>();
            container.RegisterType<IRepository<Category>, CategoryRepository>();
        }
    }
}
