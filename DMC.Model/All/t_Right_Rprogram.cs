using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_Right_Rprogram
    {
        private int _AutoID = 0;
        private string _RoseId = null;
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
        public string RoseId
        {
            get { return _RoseId; }
            set { _RoseId = value; }
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
