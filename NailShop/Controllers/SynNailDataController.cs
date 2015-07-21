using NailShop.Business;
using NailShop.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NailShop.Controllers
{
    public class SynNailDataController : ApiController
    {

        #region Syn data from Client to Server

            [HttpPost]
            public HttpResponseMessage SynNailProduct(string LoginName, string Password, int StoreID, List<NailSyn.DataAccess.SynProductClient> ListProduct)
            {
                bool IsResult = false;
                SynDataClientToServer _cls = new SynDataClientToServer();
                IsResult = _cls.SynProductToServer(LoginName, Password, StoreID, ListProduct);
                if (IsResult)
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Not ok");
            }

            [HttpPost]
            public HttpResponseMessage SynNailCustomer(string LoginName, string Password, int StoreID, List<NailSyn.DataAccess.SynCustomerClient> ListCustomer)
            {
                bool IsResult = false;
                SynDataClientToServer _cls = new SynDataClientToServer();
                IsResult = _cls.SynCustomerToServer(LoginName, Password, StoreID, ListCustomer);
                if (IsResult)
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Not ok");
            }

            [HttpPost]
            public HttpResponseMessage SynInvoiceClientToServer(string LoginName, string Password, int StoreID, NailSyn.DataAccess.Model.InvoiceClient dataInvoice)
            {
                bool IsResult = false;
                SynDataClientToServer _cls = new SynDataClientToServer();
                IsResult = _cls.SynInvoiceClientToServer(LoginName, Password, StoreID, dataInvoice);
                if (IsResult)
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Not ok");
            }


        #endregion

        #region Syn data from Server to Client
 
            [HttpGet]
            public HttpResponseMessage SynInvoiceToClient(string LoginName, string Password, int StoreID)
            {
                SynDataServerToClient _cls = new SynDataServerToClient();
                var data = _cls.GetInvoiceForSyn(LoginName, Password, StoreID);
                if (data!=null)
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Not ok");
            }

            [HttpPost]
            public HttpResponseMessage UpdateInvoiceAfterSyn(string LoginName, string Password, int StoreID, List<SynInvoice> listInvoice)
            {
                bool IsResult = false;
                SynDataServerToClient _cls = new SynDataServerToClient();
                IsResult = _cls.UpdateInvoiceAfterSyn(LoginName, Password, StoreID, listInvoice);
                if (IsResult)
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Not ok");
            }

        #endregion

        #region Public Methods

            [HttpGet]
            public HttpResponseMessage CheckLogin(string LoginName, string Password)
            {
                bool IsResult = false;
                SynDataClientToServer _cls = new SynDataClientToServer();
                IsResult = _cls.CheckLogin(LoginName, Password);
                return Request.CreateResponse(HttpStatusCode.OK, IsResult);
            }

            [HttpGet]
            public HttpResponseMessage CheckLoginTest(string LoginName, string Password)
            {
                string IsResult;
                SynDataClientToServer _cls = new SynDataClientToServer();
                IsResult = _cls.CheckLoginTest(LoginName, Password);
                return Request.CreateResponse(HttpStatusCode.OK, IsResult);
            }

        #endregion
    }
}
