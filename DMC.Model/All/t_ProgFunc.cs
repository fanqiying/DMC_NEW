using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_ProgFunc
    {
        private int _autoID = 0;
        private string _programID = null;
        private string _functionID = null;

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
        public string functionID
        {
            get { return _functionID; }
            set { _functionID = value; }
        }
    }
}
