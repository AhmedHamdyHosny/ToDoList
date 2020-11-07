using Classes.Utilities;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ToDoList.DataLayer.TableEntity;
using ToDoList.Models;
using System.Linq;
using Microsoft.AspNet.Identity;

namespace ToDoList.WebApp.Controllers
{
    [Authorize]
    public class ToDoListController : Controller
    {
        private ToDoModel toDoModel;
        public ToDoListController()
        {
            toDoModel = new ToDoModel();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetGridView()
        {
            ToDoModel model = new ToDoModel();
            try
            {
                //get user
                int userId = User.Identity.GetUserId<int>();
                //get grid info
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                int skipItemsCount = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                string orderColumnIndex = Request.Form.GetValues("order[0][column]")[0];
                
                string orderColumn = "ToDoId";
                string orderDir = "desc";
                switch (orderColumnIndex)
                {
                    case "1":
                        orderColumn = "ToDoTitle";
                        orderDir = Request.Form.GetValues("order[0][dir]")[0];
                        break;
                    default:
                        break;
                }
                int totalRecordsCount = 0;
                //get data 
                IEnumerable<ToDo> result = model.GetData(ref totalRecordsCount, ToDoTitle: search, CreateUserId: userId, skipItemsCount: skipItemsCount, pageSize: pageSize, orderColumn: orderColumn, orderDir: orderDir); 
                // Filter record count. 
                int recFilter = result.Count();
                //return result as json
                return Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecordsCount,
                    recordsFiltered = totalRecordsCount,
                    data = result
                });
            }
            catch (Exception ex)
            {

                Console.Write(ex);
            }
            
            return null;
        }

        [HttpGet]
        public ActionResult Create()
        {
            ToDoCreateModel model = new ToDoCreateModel() { Item = new ToDo() };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ToDoCreateBindModel model)
        {
            AlertMessage alertMessage = new AlertMessage()
            {
                Transaction = Classes.Common.Enums.Transactions.Create,
                TransactionCount = 1
            };
            ToDo entity = Utility.CopyEntity<ToDo>(model);
            //set entity info
            entity.CreateUserId = User.Identity.GetUserId<int>();
            entity.CreateDate = DateTime.Now;
            ToDo result = toDoModel.Insert(entity);
            bool success = (result != null);
            JsonResponse<ToDo> response = new JsonResponse<ToDo>().GetResponse(success, result, alertMessage.GetAlertMessage(), "Index", alertMessage);
            return Json(response);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ToDoEditModel model = new ToDoEditModel();
            model.Item = toDoModel.Get(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ToDoEditBindModel model)
        {
            AlertMessage alertMessage = new AlertMessage()
            {
                Transaction = Classes.Common.Enums.Transactions.Edit,
                TransactionCount = 1
            };
            ToDo entity = Utility.CopyEntity<ToDo>(model);
            //set entity info
            entity.ModifyUserId = User.Identity.GetUserId<int>();
            entity.ModifyDate = DateTime.Now;
            ToDo result = toDoModel.Edit(entity);
            bool success = (result != null);
            JsonResponse<ToDo> response = new JsonResponse<ToDo>().GetResponse(success, result, alertMessage.GetAlertMessage(), "Index", alertMessage);
            return Json(response);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            AlertMessage alertMessage = new AlertMessage()
            {
                Transaction = Classes.Common.Enums.Transactions.Delete,
                TransactionCount = 1
            };
            bool result = toDoModel.Delete(id);
            bool success = result;
            JsonResponse<bool> response = new JsonResponse<bool>().GetResponse(success, result, alertMessage.GetAlertMessage(), "Index", alertMessage);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MarkDone(int id)
        {
            AlertMessage alertMessage = new AlertMessage()
            {
                TransactionCount = 1
            };
            int userId = User.Identity.GetUserId<int>();
            bool result = toDoModel.MarkDone(id, userId);
            string message = result ? alertMessage.TransactionCount + " "+ Resources.Resource.AlertDoneSuccessMessage : Resources.Resource.AlertDoneErrorMessage;
            bool success = result;
            JsonResponse<bool> response = new JsonResponse<bool>().GetResponse(success, result, message, "Index", alertMessage);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}