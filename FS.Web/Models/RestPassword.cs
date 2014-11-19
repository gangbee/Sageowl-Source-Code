using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FS.Web.Models
{
    public class RestPassword
    {
  
        private string _email;
        [DisplayName("Email")]
        [Required(ErrorMessage = "Email required")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "Invalid Email Address")]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }



    }
}