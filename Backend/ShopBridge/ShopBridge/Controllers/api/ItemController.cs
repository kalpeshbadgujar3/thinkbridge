using DI_UnityContainer;
using MasterInterface;
using System.Net.Http;
using System.Web.Http;
using Unity;

namespace ShopBridge.Controllers
{
    [RoutePrefix("api/itemController")]
    public class ItemController : ApiController
    {
        IItemManager itemManager;
        HttpResponseMessage responseMessage;

        public ItemController()
        {
            itemManager = DIUnity.GetUnityContainer().Resolve<IItemManager>();
        }


        [HttpGet]
        [Route("getAllItems")]
        public HttpResponseMessage GetAllItems()
        {
            var response = itemManager.GetAllItems();

            responseMessage = Request.CreateResponse();

            return responseMessage;
        }
    }
}