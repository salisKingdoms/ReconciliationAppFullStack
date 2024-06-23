using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReconciliationApp.Data.dto;
using ReconciliationApp.Data.Repository;
using ReconciliationApp.Helper;

namespace ReconciliationApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransRepo _transRepo;

        public TransactionController(ITransRepo repo)
        {
            _transRepo = repo;
        }
        public IActionResult TellerTransaction()
        {
            return View();
        }

        public IActionResult Reconciliation()
        {
            return View();
        }

        [HttpGet]
        public string GetCustomerList()
        {
            RespCustomerCodeList result = new RespCustomerCodeList();
            try
            {
                var listdata = _transRepo.GetAllCustomerCode();
                result.data = listdata;
                result.is_ok = true;
            }
            catch (Exception ex)
            {
                result.is_ok = false;
                result.data = null;
            }
            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public JsonResult SubmitOrUpdateTransaction(ParamSaveUpdateTransaction model)
        {
            RespSaveCustomer result = new RespSaveCustomer();
            try
            {
                var saveOrUpdate = _transRepo.SaveorUpdateTransaction(model);
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
        public string GetTransactionList(GridConfigModel gridData)
        {
            GridView modelJSON = new GridView();
            RespTransactionList result = new RespTransactionList();
            try
            {
                ParamGridSearch payload = new ParamGridSearch();
                ParamSearchTransaction filter = new ParamSearchTransaction();
                List<string> param = new List<string>();

                if (gridData.iDisplayLength > 0)
                    filter.size = gridData.iDisplayLength;
                if (gridData.iDisplayLength > 0)
                    filter.page = (Convert.ToInt32(gridData.iDisplayStart) == 0 ? 1 : (Convert.ToInt32(gridData.iDisplayStart) / gridData.iDisplayLength) + 1);
                if (sortTransactionList(gridData.iSortCol) != "")
                {
                    filter.orderBy = sortTransactionList(gridData.iSortCol);
                    filter.sortBy = gridData.sSortDir;
                }
                filter.custCode = gridData.custCode;
                filter.trxID = gridData.trxID;
                filter.trxType = gridData.trxType;
                filter.currency = gridData.currency;
                filter.amount = gridData.amount;
                filter.trxDateStart = gridData.trxDateStart;
                filter.trxDateEnd = gridData.trxDateEnd;
                filter.pic = gridData.pic;

                var getList = _transRepo.GetListTransaction(filter);
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

        private static string sortTransactionList(int index)
        {
            string value = string.Empty;
            switch (index)
            {
                case 1:
                    value = "TransactionID";
                    break;
                case 2:
                    value = "CustomerCode";
                    break;
                case 3:
                    value = "TransactionType";
                    break;
                case 4:
                    value = "Currency";
                    break;
                case 5:
                    value = "Amount";
                    break;
                case 6:
                    value = "TransactionDateTime";
                    break;
                case 7:
                    value = "BankPIC";
                    break;
            }
            return value;
        }

        [HttpGet]
        public string GetTransactionById(int transID)
        {
            RespTransaction result = new RespTransaction();
            try
            {
                var trans = _transRepo.GetTransactionDetail(transID);
                if (trans.TransactionID == 0)
                {
                    result.message = "transaction not found";
                    result.is_ok = false;
                    return JsonConvert.SerializeObject(result);
                }

                result.TransactionID = trans.TransactionID;
                result.CustomerCode = trans.CustomerCode;
                result.TellerID = trans.TellerID;
                result.TransactionType = trans.TransactionType;
                result.Currency = trans.Currency;
                result.Denomination = trans.Denomination;
                result.Amount = trans.Amount;
                result.TransactionDateTime = trans.TransactionDateTime;
                result.BankPIC = trans.BankPIC;

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
        public string DeleteTransaction(int transID)
        {
            RespDeleteTransaction result = new RespDeleteTransaction();
            try
            {
                var deleted = _transRepo.DeleteTransaction(transID);
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

        [HttpPost]
        public string OpeningBalance()
        {
            RespPost result = new RespPost();
            try
            {
                var balance = _transRepo.SetOpeningBalance();
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

        [HttpPost]
        public string ClosingBalance()
        {
            RespPost result = new RespPost();
            try
            {
                var balance = _transRepo.SetClosingBalance();
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

        [HttpPost]
        public string GetClosingBalance(GridConfigModel gridData)
        {
            GridView modelJSON = new GridView();
            RespReconcilenList result = new RespReconcilenList();
            try
            {
                ParamGridSearch payload = new ParamGridSearch();
                ParamSearchTransaction filter = new ParamSearchTransaction();

                var getList = _transRepo.ClosingBalance();
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

        [HttpPost]
        public string GetReconsiliation(GridConfigModel gridData)
        {
            GridView modelJSON = new GridView();
            RespReconcileList result = new RespReconcileList();
            try
            {
                ParamGridSearch payload = new ParamGridSearch();
                ParamSearchReconsile filter = new ParamSearchReconsile();

                if (gridData.iDisplayLength > 0)
                    filter.size = gridData.iDisplayLength;
                if (gridData.iDisplayLength > 0)
                    filter.page = (Convert.ToInt32(gridData.iDisplayStart) == 0 ? 1 : (Convert.ToInt32(gridData.iDisplayStart) / gridData.iDisplayLength) + 1);
                if (sortReconsiliationList(gridData.iSortCol) != "")
                {
                    filter.orderBy = sortReconsiliationList(gridData.iSortCol);
                    filter.sortBy = gridData.sSortDir;
                }
                filter.tellerID = gridData.tellerID;
                filter.trxDateStart = gridData.trxDateStart;
                filter.trxDateEnd = gridData.trxDateEnd;


                var getList = _transRepo.GetListReconsiliation(filter);
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

        private static string sortReconsiliationList(int index)
        {
            string value = string.Empty;
            switch (index)
            {
                case 1:
                    value = "TellerID";
                    break;
                case 2:
                    value = "TransactionDate";
                    break;
                case 3:
                    value = "TotalDeposits";
                    break;
                case 4:
                    value = "TotalWithdrawals";
                    break;

            }
            return value;
        }
    }
}
