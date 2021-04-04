using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_SystemNo
    {
        private int _AutoID = 0;
        private string _CompanyId = null;
        private string _ModuleType = null;
        private string _ModularType = null;
        private string _Category = null;
        private string _ReceiptType = null;
        private string _keyword = null;
        private string _DateType = null;
        private int? _CodeLen = 0;
        private string _Mark = null;
        private string _Usy = null;
        private string _CreateUserId = null;
        private DateTime _createTime = DateTime.Now;
        private string _CreateDeptId = null;
        private string _UpdateUserId = null;
        private DateTime _UpdateTime = DateTime.Now;
        private string _UpdateDeptId = null;
        private int _CurrCode = 0;
        private DateTime? _GeneratTime = DateTime.Now;

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
        public string CompanyId
        {
            get { return _CompanyId; }
            set { _CompanyId = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string ModuleType
        {
            get { return _ModuleType; }
            set { _ModuleType = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string ModularType
        {
            get { return _ModularType; }
            set { _ModularType = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string ReceiptType
        {
            get { return _ReceiptType; }
            set { _ReceiptType = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string keyword
        {
            get { return _keyword; }
            set { _keyword = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string DateType
        {
            get { return _DateType; }
            set { _DateType = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public int? CodeLen
        {
            get { return _CodeLen; }
            set { _CodeLen = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string Mark
        {
            get { return _Mark; }
            set { _Mark = value; }
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
        public string CreateUserId
        {
            get { return _CreateUserId; }
            set { _CreateUserId = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public DateTime createTime
        {
            get { return _createTime; }
            set { _createTime = value; }
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
        public string UpdateUserId
        {
            get { return _UpdateUserId; }
            set { _UpdateUserId = value; }
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
        public int CurrCode
        {
            get { return _CurrCode; }
            set { _CurrCode = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public DateTime? GeneratTime
        {
            get { return _GeneratTime; }
            set { _GeneratTime = value; }
        }
    }
}
