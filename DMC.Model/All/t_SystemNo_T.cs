using System;

namespace DMC.Model
{
    [Serializable()]
    public class t_SystemNo_T
    {
        private int _Id = 0;
        private string _Prokey = null;
        private string _Commpany = null;
        private string _ProID = null;
        private string _Space = null;
        private string _Datatype = null;
        private int _NumSite = 0;
        private string _Expression = null;
        private int _MaxNum = 0;
        private DateTime _LastTime = DateTime.Now;

        /// <summary> 
        ///  
        /// </summary> 
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string Prokey
        {
            get { return _Prokey; }
            set { _Prokey = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string Commpany
        {
            get { return _Commpany; }
            set { _Commpany = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string ProID
        {
            get { return _ProID; }
            set { _ProID = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string Space
        {
            get { return _Space; }
            set { _Space = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string Datatype
        {
            get { return _Datatype; }
            set { _Datatype = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public int NumSite
        {
            get { return _NumSite; }
            set { _NumSite = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public string Expression
        {
            get { return _Expression; }
            set { _Expression = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public int MaxNum
        {
            get { return _MaxNum; }
            set { _MaxNum = value; }
        }
        /// <summary> 
        ///  
        /// </summary> 
        public DateTime LastTime
        {
            get { return _LastTime; }
            set { _LastTime = value; }
        }
    }
}
