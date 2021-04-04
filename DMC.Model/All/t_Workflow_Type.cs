using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Workflow_Type
    {
        private int _AutoId = 0;
        private string _ProcKey = null;
        private string _ProcName = null;
        private string _ProcUrl = null;
        private string _ProcType = null;
        private DateTime _CreateTime = DateTime.Now;
        private string _CreateUserId = null;
        private string _CreateDeptId = null;
        private DateTime _UpdateTime = DateTime.Now;
        private string _UpdateDeptId = null;
        private string _UpdateUserId = null;

        /// <summary> 
        ///  
        /// </summary> 
        public int AutoId
        {
            get { return _AutoId; }
            set { _AutoId = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string ProcKey
        {
            get { return _ProcKey; }
            set { _ProcKey = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string ProcName
        {
            get { return _ProcName; }
            set { _ProcName = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string ProcUrl
        {
            get { return _ProcUrl; }
            set { _ProcUrl = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string ProcType
        {
            get { return _ProcType; }
            set { _ProcType = value; }
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
        public string CreateUserId
        {
            get { return _CreateUserId; }
            set { _CreateUserId = value; }
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
