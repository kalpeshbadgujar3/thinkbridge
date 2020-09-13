using Unity;

namespace DI_UnityContainer
{
    public static class DIUnity
    {
        public static UnityContainer container;

        static DIUnity()
        {
            container = new UnityContainer();
        }

        public static UnityContainer GetUnityContainer()
        {
            return container;
        }
    }
}
