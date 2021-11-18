using System;
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
using UnityEngine.XR;
using TeleporterVR.Patches;

namespace TeleporterVR.Rendering
{
    class RenderingIndicator
    {
        public static void Init()
        {
            bool failed;
            try { ClassInjector.RegisterTypeInIl2Cpp<TPLocationIndicator>(); failed = false; }
            catch (Exception e) { MelonLogger.Error("Unable to Inject Custom TPLocationIndicator Script!\n" + e.ToString()); failed = true; }
            if (Main.isDebug && !failed) MelonLogger.Msg(ConsoleColor.Green, "Finished setting up TPLocationIndicator");
        }
    }

    // Came from https://github.com/d-mageek/VRC-Mods/blob/main/BetterPortalPlacement/Utils/PortalPtr.cs
    // Davi borrowed my stuff, so I borrow theirs, I hope you dont mind.

    internal class TPLocationIndicator : MonoBehaviour
    {
        public TPLocationIndicator(IntPtr ptr) : base(ptr) { }
        public static readonly float defaultLength = Single.PositiveInfinity;
        public Vector3 pos = Vector3.zero;
        private static GameObject previewObj;
        private LineRenderer line;
        private LineRenderer Beam;
        bool launchErrorOnce;

        private void Awake() => SetupLine();

        [HideFromIl2Cpp]
        private void OnEnable() => Toggle(true);

        [HideFromIl2Cpp]
        private void OnDisable() => Toggle(false);

        [HideFromIl2Cpp]
        public static void Toggle(bool isOn)
        {
            previewObj.SetActive(isOn);
            //line.enabled = isOn;
        }

        [HideFromIl2Cpp]
        public static void Toggle()
        {
            if (Main.EnableTeleportIndicator.Value) previewObj.SetActive(VRUtils.active);
        }

        [HideFromIl2Cpp]
        private void SetupLine()//Color color)
        {
            previewObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            previewObj.GetComponent<Collider>().enabled = false;
            try { previewObj.DestroyComponent<Collider>(); } catch { Console.WriteLine("Could not Destroy Component => Collider", ConsoleColor.Red); }
            previewObj.transform.localScale = new Vector3(0.4f, 0.08f, 0.4f);
            previewObj.transform.position = pos;
            previewObj.name = "TeleportLocationPreview";
            DontDestroyOnLoad(previewObj);

            Beam = Resources.FindObjectsOfTypeAll<LineRenderer>().Where(lr => lr.gameObject.name.Contains("RightHandBeam")).First();
            previewObj.GetComponent<Renderer>().material = Beam.GetMaterial();
            line = previewObj.AddComponent<LineRenderer>();
            line.material = Beam.GetMaterial();
            line.textureMode = LineTextureMode.RepeatPerSegment;
            line.positionCount = 22;
            line.SetWidth(.015f, .015f);
            previewObj.SetActive(false);
            line.enabled = false;
            SetColors(GeneralUtils.HexToColor(Main.IndicatorHexColor.Value, true));
        }

        private void Update()
        {
            if (!Main.EnableTeleportIndicator.Value) {
                OnDisable();
                return;
            }
            if ((VRUtils.preferRightHand ? VRUtils.ControllerRight == null : VRUtils.ControllerLeft == null) && !launchErrorOnce)
            {
                launchErrorOnce = true;
                string temp = VRUtils.preferRightHand ? "ControllerRight" : "ControllerLeft";
                Console.WriteLine($"Could not determine {temp} raycast.", ConsoleColor.Red);
                return;
            }
            var endPos = Vector3.zero;
            try { endPos = CalculateEndPoint(); } catch { return; }
            previewObj.transform.position = endPos;
            pos = endPos;
            if (line != null)
            {
                if (line.startWidth != Beam.startWidth) line.SetWidth(Beam.startWidth, Beam.endWidth);
                line.SetPosition(0, endPos);
                line.SetPosition(1, VRUtils.GetControllerPos());
            }
            if ((NewPatches.IsQMOpen || NewPatches.IsQMOpen) && VRUtils.active) previewObj.SetActive(false);
            else if (!(NewPatches.IsQMOpen || NewPatches.IsQMOpen) && VRUtils.active) previewObj.SetActive(true);
            if ((NewPatches.IsQMOpen || NewPatches.IsQMOpen) && VRUtils.active) previewObj.SetActive(false);
            else if (!(NewPatches.IsQMOpen || NewPatches.IsQMOpen) && VRUtils.active) previewObj.SetActive(true);
            SetColors(GeneralUtils.HexToColor(Main.IndicatorHexColor.Value, true));
        }

        [HideFromIl2Cpp]
        private Vector3 CalculateEndPoint()
        {
            var hit = XRDevice.isPresent ? VRUtils.RaycastVR() : Raycast();
            return hit.collider ? hit.point : DefaultPos();
        }

        [HideFromIl2Cpp]
        private RaycastHit Raycast()
        {
            Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit hit, defaultLength);
            return hit;
        }

        [HideFromIl2Cpp]
        private Vector3 DefaultPos()
        {
            return XRDevice.isPresent ?
            VRUtils.ray.origin + VRUtils.ray.direction * defaultLength : transform.position + transform.forward * defaultLength;
        }

        [HideFromIl2Cpp]
        private void SetColors(Color color)
        {
            previewObj.GetComponent<Renderer>().material.SetColor("_TintColor", color);
            if (line != null) line.SetColors(color, color);
        }
    }
}
