using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FS.Web.Models
{
    public class UserInfoModel
    {

        private string _userName;
        [DisplayName("User Name")]
        [Required(ErrorMessage = "User name required")]
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }


      
        private string _password;
        [DisplayName("Password")]
        [Required(ErrorMessage = "Password required")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _confirmPassword;
        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Confirm password required")]
        [Compare("Password", ErrorMessage = "Confirm password do not match with password")]
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { _confirmPassword = value; }
        }


    }
}