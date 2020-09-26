using DataContract;
using DI_UnityContainer;
using Helper;
using MasterInterface;
using System.IO;
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
        HttpResponseMessage responseMessage = null;
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
            // Step 1: Get all items
            var response = await itemManager.GetAllItems();

            // Step 2: Deserialize string object 
            var itemsResult = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

            // Step 3: Construct response object
            responseMessage = Request.CreateResponse(HttpStatusCode.OK, 
                                                       new { 
                                                           data = itemsResult, 
                                                           backendServerUrl = string.Format("{0}{1}{2}",
                                                                              HttpContext.Current.Request.Url.Scheme,
                                                                              Constants.SpecialCharacterForURL,
                                                                              HttpContext.Current.Request.Url.Authority)
                              } );

            return responseMessage;
        }

        /// <summary>
        /// Add document to DB
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("addItem")]
        public async Task<HttpResponseMessage> AddDocument()
        {
            var httpRequest = HttpContext.Current.Request;

            // Step 1: Extract form data received from frontend
            var itemData = System.Convert.ToString(httpRequest.Form[Constants.FormName]).Trim();

            // Step 2: Map form-data to the stored procedure object
            spInsertItem = Newtonsoft.Json.JsonConvert.DeserializeObject<SpInsertItem>(itemData);

            // Step 3: Map additional data required for file upload if user selects file
            if (string.IsNullOrEmpty(spInsertItem.FileName) && httpRequest.Files.Count > 0)
            {
                var file = httpRequest.Files[0];

                spInsertItem.FileName = Path.GetFileNameWithoutExtension(file.FileName);
                spInsertItem.FileExtension = Path.GetExtension(file.FileName);
            }

            // Step 4: Insert item
            var itemID = await itemManager.AddItem(spInsertItem);

            // Step 5: If item is inserted and item has file then upload it to defined directory 
            if (httpRequest.Files.Count > 0 && itemID > 0)
            {
                await itemManager.UploadImage(httpRequest.Files[0], itemID);
            }

            // Step 6: Construct response object
            if (itemID > 0)
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, new { Message = "Item added successfullly!" });
            }
            else
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, new { Message = "Something went wrong" });
            }

            return responseMessage;
        }
    }
}