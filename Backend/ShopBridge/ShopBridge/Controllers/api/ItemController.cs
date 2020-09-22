using DI_UnityContainer;
using MasterInterface;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Unity;

namespace ShopBridge.Controllers
{
    [RoutePrefix("api/itemController")]
    public class ItemController : ApiController
    {
        IItemManager itemManager;
        HttpResponseMessage responseMessage;
        IspInsertItem spInsertItem = null;

        public ItemController()
        {
            itemManager = DIUnity.GetUnityContainer().Resolve<IItemManager>();
            spInsertItem = DIUnity.GetUnityContainer().Resolve<IspInsertItem>();
        }

        /// <summary>
        /// Get all items from DB 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getAllItems")]
        public async Task<HttpResponseMessage> GetAllItems()
        {
            var response = await itemManager.GetAllItems();

            //responseMessage = Request.CreateResponse(null, HttpStatusCode.OK);

            return responseMessage;
        }

        [HttpPost]
        [Route("addItem")]
        public async Task<HttpResponseMessage> AddDocument()
        {
            var files = HttpContext.Current.Request.Files;

            spInsertItem.ItemName = "Item A";
            spInsertItem.ItemDescription = "this is item";
            spInsertItem.Price = 100;
            spInsertItem.FileName = "abc";
            spInsertItem.FileExtension = ".jpg";

            var response = await itemManager.AddItem(spInsertItem);

            var isUploaded = itemManager.UploadImage(files[0]);

            if (response != null && response)
            {
                //responseMessage = Request.CreateResponse(true, HttpStatusCode.OK);
            }

            return responseMessage;
        }
    }
}