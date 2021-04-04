using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.Model
{
    [Serializable]
    public class LanguageKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string LanguageId { get; set; }
    }
}
