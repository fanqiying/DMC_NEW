using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_User
    {
        private int _autoID = 0;
        private string _userID = null;
        private string _userNo = null;
        private string _userName = null;
        private string _userPwd = null;
        private string _userMail = null;
        private string _userDept = null;
        private string _defLanguage = null;
        private string _userType = null;
        private string _domainID = null;
        private string _domainAddr = null;
        private string _lastLoginIP = null;
        private DateTime _lastLoginTime = DateTime.Now;
        private string _usy = null;
        private string _createrID = null;
        private string _cDeptID = null;
        private string _updaterID = null;
        private string _uDeptID = null;
        private DateTime _createTime = DateTime.Now;
        private DateTime _lastModTime = DateTime.Now;
        private string _defaultRole = null;

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
        public string userID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string userNo
        {
            get { return _userNo; }
            set { _userNo = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string userName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string userPwd
        {
            get { return _userPwd; }
            set { _userPwd = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string userMail
        {
            get { return _userMail; }
            set { _userMail = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string userDept
        {
            get { return _userDept; }
            set { _userDept = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string defLanguage
        {
            get { return _defLanguage; }
            set { _defLanguage = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string userType
        {
            get { return _userType; }
            set { _userType = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string domainID
        {
            get { return _domainID; }
            set { _domainID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string domainAddr
        {
            get { return _domainAddr; }
            set { _domainAddr = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string lastLoginIP
        {
            get { return _lastLoginIP; }
            set { _lastLoginIP = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public DateTime lastLoginTime
        {
            get { return _lastLoginTime; }
            set { _lastLoginTime = value; }
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
        public string defaultRole
        {
            get { return _defaultRole; }
            set { _defaultRole = value; }
        }
    }
}
