using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;

namespace VoltigeCore.Business.Logic.Import
{
    public class RequestService
    {
        public XLWorkbook GetWorkbook(IFormFile file)
        {
            if (file != null && file.Length > 0)
                return new XLWorkbook(file.OpenReadStream());
            return null;
        }

        public bool IsCheckboxChecked(string checkboxValue)
        {
            return checkboxValue == "on";
        }
    }
}
