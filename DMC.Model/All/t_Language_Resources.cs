using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Language_Resources
    {
        private int _AutoID = 0;
        private string _ResourceId = null;
        private string _ResourceType = null;
        private string _DefaultValue = null;
        private string _GroupKey = null;
        private string _GroupValue = null;
        private string _Usy = null;
        private DateTime _CreateTime = DateTime.Now;
        private string _CreateDeptId = null;
        private string _CreateUser = null;
        private DateTime _UpdateTime = DateTime.Now;
        private string _UpdateDeptId = null;
        private string _UpdateUser = null;

        /// <summary> 
        ///  
        /// </summary> 
        public int AutoID
        {
            get { return _AutoID; }
            set { _AutoID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string ResourceId
        {
            get { return _ResourceId; }
            set { _ResourceId = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string ResourceType
        {
            get { return _ResourceType; }
            set { _ResourceType = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string DefaultValue
        {
            get { return _DefaultValue; }
            set { _DefaultValue = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string GroupKey
        {
            get { return _GroupKey; }
            set { _GroupKey = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string GroupValue
        {
            get { return _GroupValue; }
            set { _GroupValue = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string Usy
        {
            get { return _Usy; }
            set { _Usy = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string CreateDeptId
        {
            get { return _CreateDeptId; }
            set { _CreateDeptId = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string CreateUser
        {
            get { return _CreateUser; }
            set { _CreateUser = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public DateTime UpdateTime
        {
            get { return _UpdateTime; }
            set { _UpdateTime = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string UpdateDeptId
        {
            get { return _UpdateDeptId; }
            set { _UpdateDeptId = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string UpdateUser
        {
            get { return _UpdateUser; }
            set { _UpdateUser = value; }
        }
    }
}
