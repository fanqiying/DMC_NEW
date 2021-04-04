using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Language_Type
    {
        private int _AutoID = 0;
        private string _LanguageId = null;
        private string _LanguageName = null;
        private string _Description = null;
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
        public string LanguageId
        {
            get { return _LanguageId; }
            set { _LanguageId = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string LanguageName
        {
            get { return _LanguageName; }
            set { _LanguageName = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
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
