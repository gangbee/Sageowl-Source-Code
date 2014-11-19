using DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FS.Customer
{
    [StoredProcedure(CreateProcedure = "InsertInstitute", ReadProcedure = "GetInstituteById", UpdateProcedure = "UpdateInstitute")]
    public class Institute
    {
        [DataColumn("InstituteId")]
        private int _InstituteId;
        public int InstituteId
        {
            get { return _InstituteId; }
            set { _InstituteId = value; }
        }
        [DataColumn("Name")]
        private string _Name;
        [Required(ErrorMessage = "Name required")]
        [DisplayName("Institute Name")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        [DataColumn("Address")]
        private string _Address;
        [Required(ErrorMessage = "Address required")]
        [DisplayName("Address")]
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        [DataColumn("Zip")]
        private string _Zip;
        [Required(ErrorMessage = "Zip required")]
        [DisplayName("Zip")]
        public string Zip
        {
            get { return _Zip; }
            set { _Zip = value; }
        }
        [DataColumn("Email")]
        private string _Email;
        [Required(ErrorMessage = "Email required")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "Invalid Email Address")]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        [DataColumn("Telephone")]
        private string _Telephone;
        public string Telephone
        {
            get { return _Telephone; }
            set { _Telephone = value; }
        }
        [DataColumn("Attention")]
        private string _Attention;
        [DisplayName("Attention To")]
        public string Attention
        {
            get { return _Attention; }
            set { _Attention = value; }
        }

        public bool IsFromSendCenter { get; set; }


        public static List<Institute> GetAllInstitues()
        {
            return Persistance.ReadList<Institute>("GetAllInstitute");
        }

    }
}
