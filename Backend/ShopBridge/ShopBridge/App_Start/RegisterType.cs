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
            // Register Database manager
            DIUnity.GetUnityContainer().RegisterType<IDatabaseManager, DatabaseManager>();

            // Register Item manager with constructer dependency
            DIUnity.GetUnityContainer().RegisterType<IItemManager, ItemManager>(
                new InjectionConstructor(
                    DIUnity.GetUnityContainer().Resolve<IDatabaseManager>()
                    )
                );
        }
    }
}