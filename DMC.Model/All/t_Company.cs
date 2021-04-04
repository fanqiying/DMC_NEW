using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Company
    {
        private int _autoID = 0;
        private string _companyID = null;
        private string _compLanguage = null;
        private string _compCategory = null;
        private string _interName = null;
        private string _outerName = null;
        private string _simpleName = null;
        private string _companyNo = null;
        private string _addrOne = null;
        private string _addrTwo = null;
        private string _compTel = null;
        private string _compFax = null;
        private string _compRegNo = null;
        private string _remark = null;
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
        public string companyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string compLanguage
        {
            get { return _compLanguage; }
            set { _compLanguage = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string compCategory
        {
            get { return _compCategory; }
            set { _compCategory = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string interName
        {
            get { return _interName; }
            set { _interName = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string outerName
        {
            get { return _outerName; }
            set { _outerName = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string simpleName
        {
            get { return _simpleName; }
            set { _simpleName = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string companyNo
        {
            get { return _companyNo; }
            set { _companyNo = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string addrOne
        {
            get { return _addrOne; }
            set { _addrOne = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string addrTwo
        {
            get { return _addrTwo; }
            set { _addrTwo = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string compTel
        {
            get { return _compTel; }
            set { _compTel = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string compFax
        {
            get { return _compFax; }
            set { _compFax = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string compRegNo
        {
            get { return _compRegNo; }
            set { _compRegNo = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string remark
        {
            get { return _remark; }
            set { _remark = value; }
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
