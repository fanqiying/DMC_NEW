using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_SysLog
    {
        private int _autoID = 0;
        private string _operatorID = null;
        private string _operatorName = null;
        private string _refProgram = null;
        private string _refClass = null;
        private string _refMethod = null;
        private string _refRemark = null;
        private DateTime _refTime = DateTime.Now;
        private string _refIP = null;
        private string _refSql = null;
        private string _refEvent = null;

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
        public string operatorID
        {
            get { return _operatorID; }
            set { _operatorID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string operatorName
        {
            get { return _operatorName; }
            set { _operatorName = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string refProgram
        {
            get { return _refProgram; }
            set { _refProgram = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string refClass
        {
            get { return _refClass; }
            set { _refClass = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string refMethod
        {
            get { return _refMethod; }
            set { _refMethod = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string refRemark
        {
            get { return _refRemark; }
            set { _refRemark = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public DateTime refTime
        {
            get { return _refTime; }
            set { _refTime = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string refIP
        {
            get { return _refIP; }
            set { _refIP = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string refSql
        {
            get { return _refSql; }
            set { _refSql = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string refEvent
        {
            get { return _refEvent; }
            set { _refEvent = value; }
        }
    }
}
