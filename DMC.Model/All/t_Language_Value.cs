using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Language_Value
    {
        private int _AutoID = 0;
        private string _LanguageId = null;
        private string _ResourceId = null;
        private string _DisplayValue = null;

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
        public string LanguageId
        {
            get { return _LanguageId; }
            set { _LanguageId = value; }
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
        public string DisplayValue
        {
            get { return _DisplayValue; }
            set { _DisplayValue = value; }
        }
    }
}
