using MelonLoader;
using System;
using System.Linq;
using UnityEngine;
using VRC;
using VRC.DataModel;

[assembly: MelonInfo(typeof(Astrum.AstralStare), "AstralStare", "0.1.1", downloadLink: "github.com/Astrum-Project/AstralStare")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonColor(ConsoleColor.DarkMagenta)]

namespace Astrum
{
    public class AstralStare : MelonMod
    {
        private static Action Update = new Action(() => {});
        private static Transform target;
        private static Transform dest;
        private static bool enabled = false;

        public override void OnUpdate()
        {
            Update();

            if (!Input.GetKey(KeyCode.Tab) || !Input.GetKeyDown(KeyCode.S)) return;

            if (UserSelectionManager.field_Private_Static_UserSelectionManager_0.field_Private_APIUser_1 is null)
                Disable();
            else Enable(PlayerManager.prop_PlayerManager_0
                .field_Private_List_1_Player_0
                .ToArray()
                .FirstOrDefault(a => a.field_Private_APIUser_0.id == UserSelectionManager.field_Private_Static_UserSelectionManager_0.field_Private_APIUser_1.id)
                .transform);
        }

        public static void Disable()
        {
            if (!enabled) return;
            enabled = false;

            Update -= Stare;
        }

        public static void Enable(Transform transform)
        {
            target = transform;
            dest = VRC.SDKBase.Networking.LocalPlayer.gameObject.transform;

            if (!enabled)
            {
                enabled = true;
                Update += Stare;
            }
        }

        private static void Stare()
        {
            if (target is null)
                Disable();

            dest.LookAt(target);
            dest.rotation = Quaternion.Euler(0, dest.eulerAngles.y, 0);
        }
    }
}
