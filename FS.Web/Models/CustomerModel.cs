
using FS.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FS.Web.Models
{
    public class CustomerModel : BaseModel
    {


        public UserLogin UserLogin { get; set; }

        public ContactPerson ContactPerson { get; set; }

        public List<ContactPerson> ContactPersonList { get; set; }

        public List<CustomerDocument> Documents { get; set; }

       
        public CheckList CheckList { get; set; }
        

    }
}