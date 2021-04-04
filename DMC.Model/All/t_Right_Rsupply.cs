using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Right_Rsupply
    {
        private int _AutoID = 0;
        private string _RoseId = null;
        private string _SupplyId = null;

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
        public string SupplyId
        {
            get { return _SupplyId; }
            set { _SupplyId = value; }
        }
    }
}
