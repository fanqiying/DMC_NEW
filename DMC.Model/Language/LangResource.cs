using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.Model
{
    [Serializable]
    public class LangResources : t_Language_Resources
    {
        private List<LangValue> _ValueList = new List<LangValue>();
        public List<LangValue> ValueList
        {
            get { return this._ValueList; }
            set { this._ValueList = value; }
        }
    }
}
