using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Supply
    {
        private int _autoID = 0;
        private string _suppNumber = null;
        private string _companyID = null;
        private string _suppName = null;
        private string _suppType = null;
        private string _email = null;
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
        public string suppNumber
        {
            get { return _suppNumber; }
            set { _suppNumber = value; }
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
        public string suppName
        {
            get { return _suppName; }
            set { _suppName = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string suppType
        {
            get { return _suppType; }
            set { _suppType = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string email
        {
            get { return _email; }
            set { _email = value; }
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
