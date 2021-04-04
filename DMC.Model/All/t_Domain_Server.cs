using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Domain_Server
    {
        private string _CompanyName = null;
        private string _CompanyId = null;
        private string _HostName = null;

        /// <summary> 
        ///  
        /// </summary> 
        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
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
        public string HostName
        {
            get { return _HostName; }
            set { _HostName = value; }
        }
    }
}
