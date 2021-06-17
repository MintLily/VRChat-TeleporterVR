using System.Linq;
using System.Reflection;
using System;
using UnhollowerRuntimeLib.XrefScans;

namespace TeleporterVR.Utils
{
    class XrefThings
    {
        public static bool CheckUsed(MethodBase methodBase, string methodName)
        {
            try
            {
                return XrefScanner.UsedBy(methodBase).Where(instance => instance.TryResolve() != null && instance.TryResolve().Name.Contains(methodName)).Any();
            }
            catch { }
            return false;
        }

        public static bool CheckUsing(MethodInfo method, string match, Type type)
        {
            foreach (XrefInstance instance in XrefScanner.XrefScan(method))
                if (instance.Type == XrefType.Method)
                    try { if (instance.TryResolve().DeclaringType == type && instance.TryResolve().Name.Contains(match))
                            return true;
                    } catch { }
            return false;
        }
    }
}
