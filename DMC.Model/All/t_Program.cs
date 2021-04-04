using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Program
    {
        private int _autoID = 0;
        private string _programID = null;
        private string _programName = null;
        private string _menuUrl = null;
        private string _functionStr = null;
        private string _usy = null;
        private string _createrID = null;
        private string _cDeptID = null;
        private string _updaterID = null;
        private string _uDeptID = null;
        private DateTime _createTime = DateTime.Now;
        private DateTime _lastModTime = DateTime.Now;
        private string _menuId = null;
        private string _PathImg = null;
        private int? _orderid = 0;
        private string _IsMobile = null;
        private string _MobileUrl = null;

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
        public string programID
        {
            get { return _programID; }
            set { _programID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string programName
        {
            get { return _programName; }
            set { _programName = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string menuUrl
        {
            get { return _menuUrl; }
            set { _menuUrl = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string functionStr
        {
            get { return _functionStr; }
            set { _functionStr = value; }
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
        /// <summary> 
        ///  
        /// </summary> 
        public string menuId
        {
            get { return _menuId; }
            set { _menuId = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string PathImg
        {
            get { return _PathImg; }
            set { _PathImg = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public int? orderid
        {
            get { return _orderid; }
            set { _orderid = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string IsMobile
        {
            get { return _IsMobile; }
            set { _IsMobile = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string MobileUrl
        {
            get { return _MobileUrl; }
            set { _MobileUrl = value; }
        }
    }
}
