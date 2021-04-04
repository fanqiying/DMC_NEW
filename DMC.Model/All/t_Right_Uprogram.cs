using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Right_Uprogram
    {
        private int _AutoID = 0;
        private string _UserId = null;
        private string _ProgramId = null;

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
        public string ProgramId
        {
            get { return _ProgramId; }
            set { _ProgramId = value; }
        }
    }
}
