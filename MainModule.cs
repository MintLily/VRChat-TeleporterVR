using Mod.Teleport;
using System;
using VRLoader.Modules;


namespace SPS
{
    public class MainModule : VRModule
    {
        public void Start()
        {
            try
            {

                MainModule.tpmodule.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.InnerException.Message);
                Console.WriteLine(ex.InnerException.StackTrace);
            }
        }

        private static Teleport tpmodule = new Teleport();



        public static Teleport GetTeleport()
        {
            return MainModule.tpmodule;
        }
        
    }
}
