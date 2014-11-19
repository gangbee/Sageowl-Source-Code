using FS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FS.Web.Models
{
    public class BaseModel
    {


        public List<SelectListItem> Relatives
        {
            get
            {

                return ValuseData.GetValueData("RELATION").Select(v => new SelectListItem { Text = v.DataText, Value = v.ValuseDataId.ToString() }).ToList() ;
            }
        }

        public List<SelectListItem> StateList
        {
            get
            {

                return ValuseData.GetValueData("STATE").Select(v => new SelectListItem { Text = v.DataValue, Value = v.ValuseDataId.ToString() }).ToList();
            }
        }

        public List<ValuseData> DocumentTypes
        {
            get
            {
                return ValuseData.GetValueData("DOCTYPE");
            }
        }
    }
}