using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_SysSwitch
    {
        private int _switchID = 0;
        private string _compName = null;
        private string _operateType = null;
        private DateTime? _starTime = DateTime.Now;
        private DateTime? _endTime = DateTime.Now;
        private string _reasons = null;
        private string _operaterID = null;
        private string _operaterName = null;
        private string _operateDeptID = null;
        private DateTime _operateTime = DateTime.Now;

        /// <summary> 
        ///  
        /// </summary> 
        public int switchID
        {
            get { return _switchID; }
            set { _switchID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string compName
        {
            get { return _compName; }
            set { _compName = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string operateType
        {
            get { return _operateType; }
            set { _operateType = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public DateTime? starTime
        {
            get { return _starTime; }
            set { _starTime = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public DateTime? endTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string reasons
        {
            get { return _reasons; }
            set { _reasons = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string operaterID
        {
            get { return _operaterID; }
            set { _operaterID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string operaterName
        {
            get { return _operaterName; }
            set { _operaterName = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string operateDeptID
        {
            get { return _operateDeptID; }
            set { _operateDeptID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public DateTime operateTime
        {
            get { return _operateTime; }
            set { _operateTime = value; }
        }
    }
}
