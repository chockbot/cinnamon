using Dna;
using Microsoft.Extensions.DependencyInjection;

namespace Cinnamon.Core
{
    public class BaseCore
    {
        #region Private Properties
        /// <summary>
        /// Services Provider
        /// </summary>
        private IServiceProvider Provider { get; set; }
        #endregion

        #region Public Properties
        /// <summary>
        /// Test Flag Indicator
        /// </summary>
        public bool Test { get; private set; } = false;

        /// <summary>
        /// Application View Model
        /// </summary>
        public ApplicationViewModel appVM => Provider.GetService<ApplicationViewModel>();

        /// <summary>
        /// Client Data Store
        /// </summary>
        public IDataStore Data => Provider.GetService<IDataStore>();
        #endregion

        #region Constructor
        /// <summary>
        /// Set Framework Provider
        /// </summary>
        public BaseCore()
        {
            //Use Default Provider if Available
            try
            {
                Provider = Framework.Provider;
            }
            catch
            {
                var Services = new ServiceCollection();
                Services.AddTransient<ITaskManager, BaseTaskManager>();
                Provider = Services.BuildServiceProvider();
            }
        }
        #endregion
        /// <summary>
        /// Set custom Provider for testing purposes
        /// </summary>
        /// <param name="provider"></param>
        public void setProvider(IServiceProvider provider)
        {
            if (provider is null)
                return;

            Provider = provider;
            Test = true;
        }

        /// <summary>
        /// Set custom ServiceCollection for testing purposes
        /// </summary>
        /// <param name="service"></param>
        public void setService(ServiceCollection service)
        {
            setProvider(service.BuildServiceProvider());
        }

        /// <summary>
        /// Return Provider
        /// </summary>
        /// <returns></returns>
        public IServiceProvider getProvider()
        {
            return Provider;
        }

        /// <summary>
        /// Shortcut to Provider GetService
        /// </summary>
        /// <typeparam name="T">Type of Service to get</typeparam>
        /// <returns></returns>
        public T Service<T>()
        {
            if (Test)
                return Provider.GetService<T>();
            else
                return Framework.Service<T>();
        }
    }
}
