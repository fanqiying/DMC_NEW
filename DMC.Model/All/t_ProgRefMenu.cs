using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_ProgRefMenu
    {
        private int _autoID = 0;
        private string _programID = null;
        private string _menuID = null;
        private string _usy = null;

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
        public string menuID
        {
            get { return _menuID; }
            set { _menuID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string usy
        {
            get { return _usy; }
            set { _usy = value; }
        }
    }
}
