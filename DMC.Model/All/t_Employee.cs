using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Employee
    {
        private int _autoID = 0;
        private string _empID = null;
        private string _companyID = null;
        private string _empName = null;
        private string _empDept = null;
        private string _empTitle = null;
        private string _extTelNo = null;
        private string _empMail = null;
        private string _signerID = null;
        private string _signerName = null;
        private string _usy = null;
        private string _createrID = null;
        private string _cDeptID = null;
        private string _updaterID = null;
        private string _uDeptID = null;
        private DateTime _createTime = DateTime.Now;
        private DateTime _lastModTime = DateTime.Now;

        /// <summary> 
        ///  
        /// </summary> 
        public int autoID
        {
            get { return _autoID; }
            set { _autoID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string empID
        {
            get { return _empID; }
            set { _empID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string companyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string empName
        {
            get { return _empName; }
            set { _empName = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string empDept
        {
            get { return _empDept; }
            set { _empDept = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string empTitle
        {
            get { return _empTitle; }
            set { _empTitle = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string extTelNo
        {
            get { return _extTelNo; }
            set { _extTelNo = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string empMail
        {
            get { return _empMail; }
            set { _empMail = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string signerID
        {
            get { return _signerID; }
            set { _signerID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string signerName
        {
            get { return _signerName; }
            set { _signerName = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string usy
        {
            get { return _usy; }
            set { _usy = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string createrID
        {
            get { return _createrID; }
            set { _createrID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string cDeptID
        {
            get { return _cDeptID; }
            set { _cDeptID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string updaterID
        {
            get { return _updaterID; }
            set { _updaterID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string uDeptID
        {
            get { return _uDeptID; }
            set { _uDeptID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public DateTime createTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public DateTime lastModTime
        {
            get { return _lastModTime; }
            set { _lastModTime = value; }
        }
    }
}
