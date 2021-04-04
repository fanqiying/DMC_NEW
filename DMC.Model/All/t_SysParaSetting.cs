using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_SysParaSetting
    {
        private int _AutoId = 0;
        private string _CompanyId = null;
        private string _ParaKey = null;
        private string _ParaName = null;
        private string _ParaContent = null;
        private string _ParaDesc = null;
        private string _Usey = null;
        private string _InUser = null;
        private DateTime _InTime = DateTime.Now;

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
        public string CompanyId
        {
            get { return _CompanyId; }
            set { _CompanyId = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string ParaKey
        {
            get { return _ParaKey; }
            set { _ParaKey = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string ParaName
        {
            get { return _ParaName; }
            set { _ParaName = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string ParaContent
        {
            get { return _ParaContent; }
            set { _ParaContent = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string ParaDesc
        {
            get { return _ParaDesc; }
            set { _ParaDesc = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string Usey
        {
            get { return _Usey; }
            set { _Usey = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string InUser
        {
            get { return _InUser; }
            set { _InUser = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public DateTime InTime
        {
            get { return _InTime; }
            set { _InTime = value; }
        }
    }
}
