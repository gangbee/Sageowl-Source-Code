using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.SqlClient;
namespace FS.Customer
{
    [StoredProcedure(CreateProcedure = "ContactPersonInsert",
        ReadProcedure = "ContactPersonSelect", UpdateProcedure = "ContactPersonUpdate", DeleteProcedure = "ContactPersonDelete")]
    public class ContactPerson
    {


        [DataColumn("ContactPersonId")]
        [PrimaryKey]
        private int _ContactPersonId;
        public int ContactPersonId
        {
            get { return _ContactPersonId; }
            set { _ContactPersonId = value; }
        }
        [DataColumn("UserId")]
        private int _userId;
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        [DataColumn("Relationship")]
        private int _Relationship;
        [Required(ErrorMessage = "Relationship required")]
        [DisplayName("Relationship")]
        public int Relationship
        {
            get { return _Relationship; }
            set { _Relationship = value; }
        }
        [DataColumn("FirstName")]
        private string _FirstName;
        [Required(ErrorMessage = "First Name required")]
        [DisplayName("First Name")]
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        [DataColumn("Address")]
        private string _Address;
        [Required(ErrorMessage = "Address required")]
        [DisplayName("Address Line 1")]
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        [DataColumn("LastName")]
        private string _LastName;
        [Required(ErrorMessage = "Last Name required")]
        [DisplayName("Last Name")]
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        [DataColumn("MiddleName")]
        private string _MiddleName;
       
        [DisplayName("Middle Name")]
        public string MiddleName
        {
            get { return _MiddleName; }
            set { _MiddleName = value; }
        }
        [DataColumn("City")]
        private string _City;
         [DisplayName("City")]
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }
        [DataColumn("State")]
        private string _State;
         [DisplayName("State")]
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
        [DataColumn("PicturePath")]
        private string _PicturePath;
         [DisplayName("Photo")]
        public string PicturePath
        {
            get { return _PicturePath; }
            set { _PicturePath = value; }
        }
        [DataColumn("Gender")]
        private string _Gender;
         [DisplayName("Gender")]
         [Required(ErrorMessage = "Gender required")]
        public string Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
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

        [DataColumn("Address2")]
        private string _address2 ="";
        [DisplayName("Address Line 2")]
        public string Address2
        {
            get { return _address2 ?? ""; }
            set { _address2 = value; }
        }

        [DataColumn("DateOfBirth")]
        private DateTime _dob = new DateTime(1920,1,1);
        [DisplayName("Date of Birth")]
      
        public DateTime DateOfBirth
        {
            get { return _dob; }
            set { _dob = value; }
        }

        [DataColumn("Telephone")]
        private string _telephone = "";
        [DisplayName("Telephone")]
        public string Telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
        }

        public string RelationshipName { get; set; }

        public static List<ContactPerson> GetContactPersons(int userId)
        {
            return Persistance.ReadList<ContactPerson>("ContactPersonGetByUser", new SqlParameter("@UserId", userId));

            
        }

      

        public bool IsFromWizaed { get; set; }


        public string FullName
        {
            get
            {
                return this._FirstName + " " + this._LastName;
            }
        }

    }
}
