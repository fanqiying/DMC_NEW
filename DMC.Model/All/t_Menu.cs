using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Menu
    {
        private int _autoID = 0;
        private string _menuID = null;
        private string _fatherID = null;
        private string _menuName = null;
        private string _usy = null;
        private string _createrID = null;
        private string _cDeptID = null;
        private string _updaterID = null;
        private string _uDeptID = null;
        private DateTime _createTime = DateTime.Now;
        private DateTime _lastModTime = DateTime.Now;
        private string _PathImg = null;
        private int _orderid = 0;

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
        public string menuID
        {
            get { return _menuID; }
            set { _menuID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string fatherID
        {
            get { return _fatherID; }
            set { _fatherID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string menuName
        {
            get { return _menuName; }
            set { _menuName = value; }
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
        public string PathImg
        {
            get { return _PathImg; }
            set { _PathImg = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public int orderid
        {
            get { return _orderid; }
            set { _orderid = value; }
        }
    }
}
