using FS.Common;
using FS.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FS.Web.Models
{
    public class DocumentModel : BaseModel
    {

        public CustomerDocument Document { get; set; }

        public List<ValuseData> ReasonList
        {
            get
            {
                return ValuseData.GetValueData("REASON");
            }
        }


        public List<SelectListItem> ReasonLessList
        {
            get
            {
                return ValuseData.GetValueData("REASON").Where(r=> !new string[]{"A","B","C","D"}.Contains(r.DataValue))
                    .Select(d=> new SelectListItem{Value=d.ValuseDataId.ToString(), Text=d.DataText}).ToList()
                    ;
            }
        }
    }
}