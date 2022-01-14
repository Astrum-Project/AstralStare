using Astrum.AstralCore.UI.Attributes;
using MelonLoader;
using System;
using System.Linq;
using UnityEngine;
using VRC;
using VRC.DataModel;
using VRC.SDKBase;

[assembly: MelonInfo(typeof(Astrum.AstralStare), "AstralStare", "0.2.0", downloadLink: "github.com/Astrum-Project/AstralStare")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonColor(ConsoleColor.DarkMagenta)]

namespace Astrum
{
    public class AstralStare : MelonMod
    {
        private static Transform target;
        private static Transform dest;

        private static bool enabled = false;
        [UIProperty<bool>("Stare", "Enabled")]
        public static bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled == value) return;

                if (value)
                    Enable();
                else Disable();
            }
        }

        [UIField<bool>("Stare", "LookAway")]
        public static bool lookAway;

        public static void Disable()
        {
            if (!enabled) return;
            enabled = false;

            AstralCore.Events.OnUpdate -= Stare;
        }

        public static void Enable(Transform transform = null)
        {
            if (transform is null)
            {
                VRCPlayerApi player = VRCPlayerApi.AllPlayers.Find(
                    UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Predicate<VRCPlayerApi>>(
                        new Predicate<VRCPlayerApi>(x => x.displayName == AstralCore.Managers.SelectionManager.SelectedPlayer.displayName)
                    )
                );

                if (player == null)
                {
                    AstralCore.Logger.Notif("You don't have anyone selected");
                    return;
                }

                transform = player.gameObject.transform;
            }

            target = transform;
            dest = Networking.LocalPlayer.gameObject.transform;

            if (enabled) return;
            enabled = true;

            AstralCore.Events.OnUpdate += Stare;
        }

        private static void Stare()
        {
            if (target is null || dest is null)
            {
                Disable();
                return;
            }

            // less optimized by a slight bit but its not work the effort
            if (lookAway)
                // https://forum.unity.com/threads/getting-an-object-to-look-away-from-another.297022/
                dest.rotation = Quaternion.LookRotation(dest.position - target.position);
            else dest.LookAt(target);

            dest.rotation = Quaternion.Euler(0, dest.eulerAngles.y, 0);
        }
    }
}
