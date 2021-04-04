using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Right_Rdata
    {
        private int _AutoID = 0;
        private string _RoseId = null;
        private string _DeptId = null;
        private string _IsAll = null;

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
        public string RoseId
        {
            get { return _RoseId; }
            set { _RoseId = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string DeptId
        {
            get { return _DeptId; }
            set { _DeptId = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string IsAll
        {
            get { return _IsAll; }
            set { _IsAll = value; }
        }
    }
}
