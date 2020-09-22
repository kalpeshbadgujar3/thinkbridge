using DataContract;
using DI_UnityContainer;
using Managers;
using MasterInterface;
using Unity;
using Unity.Injection;

namespace ShopBridge
{
    public static class RegisterType
    {
        public static void Initialise()
        {
            #region DataContracts
            // Register Storeprocedure
            DIUnity.GetUnityContainer().RegisterType<IspInsertItem, SpInsertItem>();
            #endregion

            #region Managers
            // Register Database manager
            DIUnity.GetUnityContainer().RegisterType<IDatabaseManager, DatabaseManager>();

            // Register Item manager with constructer dependency
            DIUnity.GetUnityContainer().RegisterType<IItemManager, ItemManager>(
                new InjectionConstructor(
                    DIUnity.GetUnityContainer().Resolve<IDatabaseManager>()
                    )
                );  
            #endregion

        }
    }
}