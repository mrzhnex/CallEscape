using EXILED.Extensions;
using UnityEngine;

namespace CallEscape
{
    internal class CheckCustomEscape : MonoBehaviour
    {
        private float Timer = 0.0f;
        private readonly float TimeIsUp = 0.3f;
        private Vector3 EscapePosition = new Vector3(170.0f, 984.7f, 25.5f);

        public void Update()
        {
            Timer += Time.deltaTime;

            if (Timer > TimeIsUp)
            {
                Timer = 0.0f;

                foreach (ReferenceHub referenceHub in Player.GetHubs())
                {
                    if (referenceHub.GetRole() != RoleType.FacilityGuard && referenceHub.GetTeam() != Team.MTF && referenceHub.GetRole() != RoleType.ClassD && referenceHub.GetRole() != RoleType.Scientist && referenceHub.GetRole() != RoleType.Tutorial)
                    {
                        if (Vector3.Distance(referenceHub.GetPosition(), EscapePosition) < 2.0f)
                        {
                            referenceHub.ClearInventory();
                            referenceHub.SetRole(RoleType.Spectator);
                        }
                    }
                }
            }
        }
    }
}