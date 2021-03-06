using PhotoContest.Web;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof (NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof (NinjectWebCommon), "Stop")]

namespace PhotoContest.Web
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Data;
    using Data.Contracts;
    using Data.Repositories;
    using Infrastructure.CacheService;
    using Infrastructure.MetaDataProvider;
    using Infrastructure.Utils;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Web.Common;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        ///     Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof (OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof (NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        ///     Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        ///     Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                ObjectFactory.InitializeKernel(kernel);

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        ///     Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(typeof(IRepository<>)).To(typeof(GenericRepository<>))
                .WithConstructorArgument("context", kernel.Get<PhotoContextDbContext>());
            kernel.Bind<IPhotoContestData>().To<PhotoContestData>();
            kernel.Bind<IPhotoDbContext>().To<PhotoContextDbContext>().InRequestScope();
            kernel.Bind<ICacheService>().To<ContestCacheService>();

            kernel.Bind<ModelMetadataProvider>().To<ExtensibleModelMetadataProvider>();
            kernel.Bind(k => k.FromThisAssembly()
                .SelectAllTypes()
                .Where(t => typeof (IModelMetadataFilter).IsAssignableFrom(t))
                .BindAllInterfaces());
        }
    }
}