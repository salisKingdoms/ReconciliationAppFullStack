using Microsoft.AspNetCore.Mvc;
using ReconciliationApp.Data.dto;
using ReconciliationApp.Data.Repository;
using ReconciliationApp.Helper;
using Newtonsoft.Json;

namespace ReconciliationApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepo _custRepo;

        public CustomerController(ICustomerRepo repo)
        {
            _custRepo = repo;
        }
        public IActionResult Customers()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SubmitOrUpdateCustomer(ParamSaveUpdateCustomer model)
        {
            RespSaveCustomer result = new RespSaveCustomer();
            try
            {
                var saveOrUpdate = _custRepo.SaveorUpdateCustomer(model);
                result.is_ok = true;
                result.messageUI = saveOrUpdate.Result.message;
                result.messageUIError = saveOrUpdate.Result.error;
                result.messageConsole = "Submit Success";
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.messageUIError = "Submit Failed";
                result.messageConsole = ex.Message;
            }
            return Json(result);
        }

        [HttpPost]
        public string GetCustomereList(GridConfigModel gridData)
        {
            GridView modelJSON = new GridView();
            RespCustomerList result = new RespCustomerList();
            try
            {
                ParamGridSearch payload = new ParamGridSearch();
                ParamSearchCustomer filter = new ParamSearchCustomer();
                List<string> param = new List<string>();

                if (gridData.iDisplayLength > 0)
                    filter.size = gridData.iDisplayLength;
                if (gridData.iDisplayLength > 0)
                    filter.page = (Convert.ToInt32(gridData.iDisplayStart) == 0 ? 1 : (Convert.ToInt32(gridData.iDisplayStart) / gridData.iDisplayLength) + 1);
                if (sortCustomerList(gridData.iSortCol) != "")
                {
                    filter.orderBy = sortCustomerList(gridData.iSortCol);
                    filter.sortBy = gridData.sSortDir;
                }

                if (!string.IsNullOrEmpty(gridData.sSearch))
                {
                    filter.name = gridData.sSearch;
                }

                var getList = _custRepo.GetListCustomer(filter);
                result.data = getList;
                result.is_ok = true;
                result.totalRow = getList.Count();
                if (result.is_ok)
                {

                    modelJSON.draw = gridData.sEcho;
                    modelJSON.recordsFiltered = result.totalRow;
                    modelJSON.recordsTotal = result.totalRow;
                    modelJSON.data = result.data;
                    modelJSON.message = result.message;
                    modelJSON.statusCode = result.is_ok;
                }
                else
                {
                    modelJSON.message = result.message;
                    modelJSON.statusCode = result.is_ok;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return JsonConvert.SerializeObject(modelJSON);
        }

        private static string sortCustomerList(int index)
        {
            string value = string.Empty;
            switch (index)
            {
                case 1:
                    value = "CustomerName";
                    break;
                case 2:
                    value = "email";
                    break;
                case 3:
                    value = "phone";
                    break;
                case 4:
                    value = "CustomerAddress";
                    break;


            }
            return value;
        }

        [HttpGet]
        public string GetCustomerById(int custCode)
        {
            RespCustomer result = new RespCustomer();
            try
            {

                var cust = _custRepo.GetCustomerByCode(custCode);
                if (cust.CustomerCode == 0)
                {
                    result.message = "employee not found";
                    result.is_ok = false;
                    return JsonConvert.SerializeObject(result);
                }

                result.CustomerCode = cust.CustomerCode;
                result.CustomerName = cust.CustomerName;
                result.CustomerAddress = cust.CustomerAddress;
                result.email = cust.email;
                result.phone = cust.phone;

                result.message = "success";
                result.is_ok = true;

            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = "Edit Failed";

            }
            return JsonConvert.SerializeObject(result);
        }

        [HttpDelete]
        public string DeleteCustomer(int custCode)
        {
            RespDeleteCustomer result = new RespDeleteCustomer();
            try
            {
                var deleted = _custRepo.DeleteCustomer(custCode);
                result.is_ok = true;
                result.message = "success";
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.message = ex.Message;
            }
            return JsonConvert.SerializeObject(result);
        }
    }
}
