using System.Web;
using ClosedXML.Excel;

namespace WebApplication1.Business.Logic.Import
{
    public class RequestService
    {
        private readonly HttpRequestBase _request;

        public RequestService(HttpRequestBase request)
        {
            _request = request;
        }

        public XLWorkbook GetWorkbook(HttpPostedFileBase file)
        {

            if (file != null && file.ContentLength > 0)
            {
                return new XLWorkbook(file.InputStream);
            }


            return null;
        }

        public bool IsCheckboxChecked(string checkboxValue)
        {
            return checkboxValue == "on";
        }
    }
}