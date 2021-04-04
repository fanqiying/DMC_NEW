using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace DMC.DAL.Script
{
    public static class ScriptFactory
    {
        private static Dictionary<string, object> InterfaceIOC = new Dictionary<string, object>();

        public static string DBType = ConfigurationManager.AppSettings["DBType"];

        public static object lockobj = new object();

        public static T GetScript<T>()
        {
            T result = default(T);
            lock (lockobj)
            {
                string interfaceName = typeof(T).Name;
                if (InterfaceIOC.ContainsKey(interfaceName))
                {
                    result = (T)InterfaceIOC[interfaceName];
                }
                else
                {
                    Assembly assem = Assembly.GetExecutingAssembly();
                    Type[] types = assem.GetTypes();
                    foreach (var item in types)
                    {
                        string space = item.Namespace;
                        if (space.IndexOf(DBType) > -1)
                        {
                            Type interType = item.GetInterface(interfaceName);
                            if (interType != null)
                            {
                                result = (T)Activator.CreateInstance(item);
                                InterfaceIOC.Add(interfaceName, result);
                                break;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static void Init()
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            Type[] types = assem.GetTypes();
            foreach (var inter in types)
            {
                if (inter.IsInterface)
                {
                    if (!InterfaceIOC.ContainsKey(inter.Name))
                    {
                        foreach (var item in types)
                        {
                            string space = item.Namespace;
                            if (space.IndexOf(DBType) > -1)
                            {
                                Type interType = item.GetInterface(inter.Name);
                                if (interType != null)
                                {
                                    var result = Activator.CreateInstance(item);
                                    InterfaceIOC.Add(inter.Name, result);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
