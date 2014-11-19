using DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Web.Mvc;

namespace FS.Customer
{
    [StoredProcedure(CreateProcedure = "ManageUser", ReadProcedure = "GetUser", UpdateProcedure = "ManageUser")]
   // [PropertiesMustMatch("Password", "ConfirmPassword",
   //ErrorMessage = "The password and confirmation password do not match.")]
    public class UserLogin
    {
        [DataColumn("UserId")]
        [PrimaryKey]
        private int _userId;

        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
         [DataColumn("UserName")]
      
        private string _userName;
         [DisplayName("User Name")]
         [Required(ErrorMessage = "User name required")] 
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

         [DataColumn("FirstName")]
        private string _fname;
         [DisplayName("First Name")]
         [Required(ErrorMessage = "First Name required")] 
        public string FirstName
        {
            get { return _fname; }
            set { _fname = value; }
        }
         [DataColumn("LastName")]
        private string _lname;
         [DisplayName("Last Name")]
         [Required(ErrorMessage = "Last Name required")] 
        public string LastName
        {
            get { return _lname; }
            set { _lname = value; }
        }
         [DataColumn("Password")]
        private string _password;
         [DisplayName("Password")]
         [Required(ErrorMessage = "Password required")]
         [RegularExpression(@"^.*(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[\d])(?=.*[\w]).*$", ErrorMessage = "Password should be 8 charactors long, with atleast one lower case, one upper case, one digit and one special charactor")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
         [DataColumn("Address1")]
        private string _address1;
         [DisplayName("Address Line 1")]
         [Required(ErrorMessage = "Address Line 1 required")] 
        public string Address1
        {
            get { return _address1; }
            set { _address1 = value; }
        }
         [DataColumn("Address2")]
        private string _address2;
         [DisplayName("Address Line 2")]         
        public string Address2
        {
            get { return _address2; }
            set { _address2 = value; }
        }

         [DataColumn("City")]
        private string _city;
         [DisplayName("City")]         
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

         [DataColumn("Telephone")]
        private string _telephone;
         [DisplayName("Telephone")]         
        public string Telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
        }

         [DataColumn("Mobile")]
        private string _mobile;
         [DisplayName("Mobile")]         
        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

         [DataColumn("Fax")]
        private string _fax;
         [DisplayName("Fax")]         
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

         
         private string _confirmPassword;
         [DisplayName("Confirm Password")]
         [Required(ErrorMessage = "Confirm password required")]
         [System.Web.Mvc.Compare("Password", ErrorMessage = "Confirm password do not match with password")]
         public string ConfirmPassword
         {
             get { return _confirmPassword; }
             set { _confirmPassword = value; }
         }


         [DataColumn("Email")]
         private string _email;
         [DisplayName("Email")]
         [Required(ErrorMessage = "Email required")] 
         [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "Invalid Email Address")]
         public string Email
         {
             get { return _email; }
             set { _email = value; }
         }
         [DataColumn("PicturePath")]
         private string _PicturePath;
         [DisplayName("Photo")]
         public string PicturePath
         {
             get { return _PicturePath; }
             set { _PicturePath = value; }
         }

         [DataColumn("StateId")]
         private int _stateId;
         [DisplayName("State")]
         public int StateId
         {
             get { return _stateId; }
             set { _stateId = value; }
         }

         [DataColumn("Zip")]
         private string _zip;
         [Required(ErrorMessage = "Zip required")]
         [DisplayName("Zip")]
         public string Zip
         {
             get { return _zip; }
             set { _zip = value; }
         }

        public static UserLogin AuthenticateUser(string userName, string password = null)
        {
            var list = Persistance.ReadList<UserLogin>("AuthenticateUser", new SqlParameter("@UserName", userName));


            if (list.Count == 1)
            {
                if (string.IsNullOrWhiteSpace(password))
                    return list[0];
                else
                {
                    if(list[0].Password == password)
                        return list[0];

                    return null;
                }
            }

            return null;
        }

        public static bool GetUserByEmailAndUserName(string userName, string email)
        {
            var list = Persistance.ReadList<UserLogin>("GetUserByEmail", new SqlParameter("@UserName", userName), new SqlParameter("@Email", email));

            return list.Count > 0;
        }

        public static UserLogin GetUserByEmail(string email)
        {
            var list = Persistance.ReadList<UserLogin>("GetUserByEmail", new SqlParameter("@UserName", ""), new SqlParameter("@Email", email));

            return list.Count > 0 ? list[0] : null;
        }

        public static bool RestPassword(string email,string password)
        {
            Persistance.ExecuteNonQuery("UpdatePassword", new SqlParameter("@Password", password), new SqlParameter("@Email", email));
            return true;
            
        }
        
    }
}
