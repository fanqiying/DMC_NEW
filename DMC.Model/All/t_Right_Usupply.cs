using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Right_Usupply
    {
        private int _AutoID = 0;
        private string _UserId = null;
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
        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
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
