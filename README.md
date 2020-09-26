Dear Evaluator, Greetings!

Hope you and family is safe and healthy during this pandemic time. I really appreciate your efforts for evaluating the coding task.
I have completed the task to add an item to DB with respective image upload, Please have a look on the detailed inputs below:

## Project git repository
We can find the complete code on git under development branch, please refer the git url https://github.com/kalpeshbadgujar3/thinkbridge
Feel free to reach on the contact details provided in signature if any issues while accessing git repository.
Code is updated under different directories as below:
1. Backend = Contains the backend(asp.net web api) code.
2. frontend/ShopBridge = Contains the frontend(Angular) code
3. DB = Contains the DB backup file to restore DB on your environment. we can find the SQL script as well if bak file does not works.

## Steps to run application
1. Restore database using backup file provided in **DB** folder.
2. Load ShopBridge solution i.e. backend application in visual studio from the **Backend\ShopBridge** folder.
	- Change connection string in **Web.config** file
	- Run project, it will open up in new browser window say https://localhost:44319/ Lets refer this as a ApiURL, will be needing in step 3.
3. Run frontend application
	- Open visual code
	- Goto File => Open folder, please load frontend code from **frontend\ShopBridge** folder
	- Update the api url in **frontend\ShopBridge\src\environments\environment.ts** file, api url is nothing but the url we got in step 2. 
	  Only replace the URL keeping **api** keyword as it is.
	- Go to terminal, and enter below command
		- npm install
		- ng s -o  => this will open new browser window with frontend application.

## Functional flow
1. User can add the product using the frontend application, the browser window started in above step 3.
2. We can find a form under items menu where user can enter ItemName, ItemDescription, ItemPrice and select image to upload.
3. Form Validations are applied as per DB schema and functional cases.
	- Max chars and required validations on all text fields.
	- Min value and required validations on item price field.
	- Image upload is mandatory as well.
4. When the form is valid, user can add item. Once item is added into DB, we can see list of recently added(last 5) items in the grid below.
5. Image thumbnail can be seen in image grid column, if there are no image available on server or if any error then no image thumbnail is shown considering better UX.
Note: You can try uploading non-image file so thumbnail can be seen as no image.
 
## Technical implementation
Developed application can be considered as loosely coupled stand-alone application. We can deploy such applications on same or different servers.
Such environment can benefit in individual frontend or backend development/deployment and performance boost in async way.
The application has two major factors, frontend which is developed using angular and backend developed using asp.net web API.

###### Frontend architecture
Beginner development appoach is followed to develop frontend architecture where a component interact with web api using HTTP client.
Tried to use Angular material for design but had some issues so its working partially, focused on functionality.
Reactive form is used for form implementation, gives more control using ts file. Some inline styles are used for basic grid structure design.

###### Backend architecture
Intermediate/Expert approach is followed to develop backend architecture considering performance(async programing), 
generic and reusable code, use of OOPS concepts(interface,DI).
Backend architecture includes several libraries i.e. N-Tier architecture holding individual objective, please find the details as below:
1. WebAPI startup project (ShopBridge)
	- This is the entry point project to which frontend application interacts directly.
	- All APIs can be created under **ShopBridge\Controllers\api** directory.
	- Includes single API at the moment i.e. ItemController with getAllItems() and AddItem() methods.
	- Uploaded images can be found under **ShopBridge\uploads\images** directory.
	- This project holds reference of DataContract, Helper, Managers, MasterInterface and UnityContainer libraries. Each library targets single responsibility.
	Lets discuss objective/benefits of each library one by one below.

2. DataContract
	- DataContract can contain the ViewModel and StoredProcedure definations.
	- Using this, frontend object can be bind to API parameter directly or viewModel can be returned as response object to frontend.
	- Currently, we can find single sp defination under **Backend\DataContract\StoredProcedure** which is used to hold the item data to insert.
	- Holds reference of MasterInterface, all viewModel/storedProcedure should implement interface, which helps us in registering types while application bootstrap.

3. Helper
	- As name states, the library can contain the common code which can be reused through out the application.
	- For example, common utility methods, Constants, enums.
	- Currently, we can find two contexts
		- Constants => Constants are used to avoid data hardcoding.
		- StoredProcedure(enum) => This enum contains the name of all CRUD sps used in the applications.
		Making it as enum benefits code redability and easy access of storedProcedure names.

4. Managers
	- This is the middle layer between api controller and Database, api should not interact with DB directly. Seperation of code.
	- Here we can format the request/response object as per functional or technical requirement.
	- For example, SQL select records are retrived in datatable so records can be mapped to respective List<object> from datatable. 
	  Such kind of operations can performed in managers.
	- Currently, we can find two manager i.e. DataBaseManager and ItemManager.
	- Database manager holds all generic methods for basic CRUD operations like GetAll, AddRecord, Update-Delete(unavailable at the moment). 
	  Generic methods provides code reusability and minimizes the duplicate code.
	- Database Manager interacts with database.
	- ItemManager is the feature specific manager which holds feature specific methods that 
	interacts with database manager to complete DB interaction for CRUD.
	
5. MasterInterface
	- As we already know that the application is loosely coupled, without interface it was not possible.
	- This library holds all the interfaces required/used in the application.
	- Currently we can find the Interfaces for Managers and StoredProcedures.
	- MasterInterface includes an empty interface called IStoredProcedure which enable us to have generic implementation using interface, all stored procedure interface must implement IStoredProcedure.
	  For example, if we see DatabaseManager.AddRecord() receives two params. first enum name and second is IStoredProcedure so this methods results generic and any feature can reuse the method to add an entity without any code changes. 
	- MasterInterfaces are also benificial to register and resolve the types(Class-Object). 

6. UnityContainer
    - This library contains a static class and static method, which is used return the unity container.
	- In order to create/get object of any viewModel, StoredProcedure, Manager we are using unity container.
	- We can refer the RegisterType class inside the **ShopBridge\App_Start** where we register the types at application bootstrap and when ever user needs object, he/she can resolve using DI itself.
	- Elimiates the new keyword and application becomes loosely couples.
	- Constructor dependency implementation can be observed as well.

with the said terms, we can consider the application is scalable enough to extend implementation. 
Basic coding guidelines are strictly followed like naming conventions, code refactoring, code reusability, code readability, code commenting. 
SQL queries(SPs) are also written considering basic guidelines and to get grid recods SQL JSON feature is used, a nice feature to boost performance by eliminating DB object to application object mapping, simple return JSON from SQL.

However, I consider some scope of improvement is there but due to current role and responsibilities could not spend much time on this for additional customization.
- UI/Design can be improved
- Pagination can be implemented on grid, Kendo grid can be used for additional built-in features like searching/sorting/filtering.
- ItemDocument table can be normalized to one more level, so we can have better control over types of file upload, allowed extensions and different directory based on file type.


I request you to have a look on technical implementation and feel free to ask if I can provide more details.
Your feedback-suggestion is highly appreciated.

Looking forward to your thoughts!


**Sincerely,**
**Kalpesh Badgujar,**
**kalpeshbadgujar3@gmail.com,**
**+917249647191**
   

