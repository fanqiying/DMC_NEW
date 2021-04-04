using System;

namespace DMC.Model
{
    /// <summary>
    /// 用戶信息，包括：用戶基本信息、當前公司、當前語言
    /// </summary>
    [Serializable]
    public class UserInfo : t_User
    {
        private t_Company company = new t_Company();
        public t_Company Company { get { return company; } set { company = value; } }
        public string LanguageId { get; set; }
        public string extTelNo { get; set; }
    }
}
