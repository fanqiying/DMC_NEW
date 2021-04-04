using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Right_Rose
    {
        private int _AutoID = 0;
        private string _RoseId = null;
        private string _RoseName = null;
        private string _Usy = null;
        private string _SystemType = null;
        private DateTime _CreateTime = DateTime.Now;
        private string _CreateDeptId = null;
        private string _CreateUserId = null;
        private DateTime _UpdateTime = DateTime.Now;
        private string _UpdateDeptId = null;
        private string _UpdateUserId = null;

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
        public string RoseId
        {
            get { return _RoseId; }
            set { _RoseId = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string RoseName
        {
            get { return _RoseName; }
            set { _RoseName = value; }
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
        public string SystemType
        {
            get { return _SystemType; }
            set { _SystemType = value; }
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
        public string CreateUserId
        {
            get { return _CreateUserId; }
            set { _CreateUserId = value; }
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
        public string UpdateUserId
        {
            get { return _UpdateUserId; }
            set { _UpdateUserId = value; }
        }
    }
}
