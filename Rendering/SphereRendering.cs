/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using MelonLoader;
using UnhollowerRuntimeLib;
using VRC;
using UnhollowerBaseLib.Attributes;
using TeleporterVR.Utils;

namespace TeleporterVR.Rendering
{
    class RenderingSphere
    {
        public static void Init()
        {
            bool failed;
            try { ClassInjector.RegisterTypeInIl2Cpp<SphereRendering>(); failed = false; }
            catch (Exception e) { MelonLogger.Error("Unable to Inject Custom SphereRendering Script!\n" + e.ToString()); failed = true; }
            if (Main.isDebug && !failed) MelonLogger.Msg(ConsoleColor.Green, "Finished setting up SphereRendering");
        }
    }

    class SphereRendering : MonoBehaviour
    {
        public SphereRendering(IntPtr ptr) : base(ptr) { }

        private bool isReset = false;
        GameObject sphere;

        [HideFromIl2Cpp]
        void SetupSphere(Vector3 End, Color32 color)
        {
            if (this.sphere != null) {
                this.sphere.active = true;
                return;
            }
            this.sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            DontDestroyOnLoad(this.sphere);
            this.sphere.name = "TeleporterVR_Raycast_EndPointSphere";
            this.sphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            this.sphere.transform.localPosition = End;
            this.sphere.GetComponent<Collider>().enabled = false;
            this.sphere.GetComponent<MeshRenderer>().receiveShadows = false;
            this.sphere.GetComponent<MeshRenderer>().castShadows = false;
            this.sphere.GetComponent<MeshRenderer>().material.color = color;
            this.sphere.GetComponent<MeshRenderer>().material.shader.name = "Standard";
            isReset = false;
        }

        [HideFromIl2Cpp]
        void ResetSphere()
        {
            if (this.sphere != null) this.sphere.active = false;
            this.isReset = true;
        }

        [HideFromIl2Cpp]
        void Update()
        {
            if (Main.EnableSimpleBall.Value && VRUtils.active) {
                if (Physics.Raycast(VRUtils.ray, out RaycastHit raycastHit))
                    this.SetupSphere(raycastHit.point, new Color32(255, 0, 0, 166));
            } else this.ResetSphere();
        }
    }
}
*/