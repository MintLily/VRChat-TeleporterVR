using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using VRC.Core;

namespace VRCheat.Utils
{
    // Token: 0x020000D7 RID: 215
    internal static class MiscUtils
    {
        // Token: 0x06000498 RID: 1176 RVA: 0x00018BB0 File Offset: 0x00016DB0
        static MiscUtils()
        {
            try
            {
                PropertyInfo propertyInfo = typeof(VRCFlowManager).GetProperties().FirstOrDefault((PropertyInfo p) => p.PropertyType == typeof(VRCFlowManager));
                MiscUtils.flowManagerMethod = ((propertyInfo != null) ? propertyInfo.GetGetMethod() : null);
                PropertyInfo propertyInfo2 = typeof(VRCUiManager).GetProperties().FirstOrDefault((PropertyInfo p) => p.PropertyType == typeof(VRCUiManager));
                MiscUtils.uiManagerMethod = ((propertyInfo2 != null) ? propertyInfo2.GetGetMethod() : null);
                PropertyInfo propertyInfo3 = typeof(VRCApplicationSetup).GetProperties().FirstOrDefault((PropertyInfo p) => p.PropertyType == typeof(VRCApplicationSetup));
                MiscUtils.appSetupMethod = ((propertyInfo3 != null) ? propertyInfo3.GetGetMethod() : null);
                PropertyInfo propertyInfo4 = typeof(VRCApplicationSetup).GetProperties().FirstOrDefault((PropertyInfo p) => p.PropertyType == typeof(ApiAvatar));
                MiscUtils.currentAvatarMethod = ((propertyInfo4 != null) ? propertyInfo4.GetGetMethod() : null);
            }
            catch
            {
                Console.WriteLine("Error loading VRChat information fields.");
            }
        }

        // Token: 0x06000499 RID: 1177 RVA: 0x00018CB4 File Offset: 0x00016EB4
        public static string CalculateHash<T>(string input) where T : HashAlgorithm
        {
            byte[] array = MiscUtils.CalculateHash<T>(Encoding.UTF8.GetBytes(input));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        // Token: 0x0600049A RID: 1178 RVA: 0x000178FC File Offset: 0x00015AFC
        public static byte[] CalculateHash<T>(byte[] buffer) where T : HashAlgorithm
        {
            byte[] result;
            using (T t = typeof(T).GetMethod("Create", new Type[0]).Invoke(null, null) as T)
            {
                result = t.ComputeHash(buffer);
            }
            return result;
        }

        // Token: 0x0600049B RID: 1179 RVA: 0x000045D9 File Offset: 0x000027D9
        public static VRCFlowManager GetVRCFlowManagerInstance()
        {
            return (VRCFlowManager)MiscUtils.flowManagerMethod.Invoke(null, null);
        }

        // Token: 0x0600049C RID: 1180 RVA: 0x000045EC File Offset: 0x000027EC
        public static VRCApplicationSetup GetVRCApplicationSetup()
        {
            return (VRCApplicationSetup)MiscUtils.appSetupMethod.Invoke(null, null);
        }

        // Token: 0x0600049D RID: 1181 RVA: 0x000045FF File Offset: 0x000027FF
        public static VRCUiManager GetVRCUiManager()
        {
            return (VRCUiManager)MiscUtils.uiManagerMethod.Invoke(null, null);
        }

        // Token: 0x040002F2 RID: 754
        private static MethodInfo flowManagerMethod;

        // Token: 0x040002F3 RID: 755
        private static MethodInfo uiManagerMethod;

        // Token: 0x040002F4 RID: 756
        private static MethodInfo appSetupMethod;

        // Token: 0x040002F5 RID: 757
        private static MethodInfo currentAvatarMethod;
    }
}