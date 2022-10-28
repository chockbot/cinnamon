using Cinnamon.Core.Module.CinnamonMakerService.Handler;
using Dna;
namespace Cinnamon.Core
{
    /// <summary>
    /// The IoC container for our application
    /// </summary>
    public static class CoreDI
    {
        #region Interfaces
        /// A shortcut to access the <see cref="ITaskManager"/>
        /// </summary>
        public static ITaskManager TaskManager => Framework.Service<ITaskManager>() ?? new BaseTaskManager();
        #endregion

        #region View Models
        /// <summary>
        /// A shortcut to access the <see cref="ApplicationViewModel"/>
        /// </summary>
        public static ApplicationViewModel ViewModelApplication => Framework.Service<ApplicationViewModel>();
        #endregion

        public static IDataStore DataStore => Framework.Service<IDataStore>();

        public static IRegisterMaker RegisterMakerHandler => Framework.Service<IRegisterMaker>();

        public static IConfirmEmailMaker ConfirmEmailMakerHandler => Framework.Service<IConfirmEmailMaker>();
    }
}
