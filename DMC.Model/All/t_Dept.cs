using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Dept
    {
        private int _autoID = 0;
        private string _deptID = null;
        private string _companyID = null;
        private string _falseDeptID = null;
        private string _simpleName = null;
        private string _fullName = null;
        private string _deptNature = null;
        private string _deptGroup = null;
        private string _deptHeader = null;
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
        public string deptID
        {
            get { return _deptID; }
            set { _deptID = value; }
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
        public string falseDeptID
        {
            get { return _falseDeptID; }
            set { _falseDeptID = value; }
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
        public string fullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string deptNature
        {
            get { return _deptNature; }
            set { _deptNature = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string deptGroup
        {
            get { return _deptGroup; }
            set { _deptGroup = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string deptHeader
        {
            get { return _deptHeader; }
            set { _deptHeader = value; }
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
