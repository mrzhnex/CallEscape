using EXILED.Extensions;
using UnityEngine;

namespace CallEscape
{
    class CheckEvacuationMTF : MonoBehaviour
    {
        private float timeProgress = 0f;
        private float timer = 0f;
        private float timeIsUp = 1.0f;
        private float delay_to_heli = 0f;

        public void Update()
        {
            timer = timer + Time.deltaTime;
            if (timer >= timeIsUp)
            {
                timer = 0f;
                if (Global.call_evacuation_heli)
                {
                    delay_to_heli = delay_to_heli + timeIsUp;
                    if (delay_to_heli >= Global.delay_to_heli)
                    {
                        Global.call_evacuation_heli = false;
                        delay_to_heli = 0f;
                        timeProgress = 0f;
                        Global.evacuation_process_heli = true;
                        Global.chopper.SetState(true);
                    }
                }

                if (Global.evacuation_process_heli)
                {
                    timeProgress = timeProgress + timeIsUp;
                    foreach (ReferenceHub p in Player.GetHubs())
                    {
                        if (p.GetTeam() == Team.MTF || p.GetTeam() == Team.SCP || p.GetRole() == RoleType.FacilityGuard)
                        {
                            continue;
                        }
                        if (p.GetRole() == RoleType.ChaosInsurgency && !p.IsHandCuffed())
                        {
                            continue;
                        }
                        if (p.GetRole() == RoleType.ClassD)
                        {
                            bool flag = false;
                            foreach (ReferenceHub zoneP in Player.GetHubs())
                            {
                                if (Vector3.Distance(p.GetPosition(), Global.escape_pos_heli) < Global.distance_heli_escape && (zoneP.GetTeam() == Team.MTF || zoneP.GetRole() == RoleType.FacilityGuard))
                                {
                                    flag = true;
                                }
                            }
                            if (!flag)
                            {
                                continue;
                            }
                        }
                        if (Vector3.Distance(p.GetPosition(), Global.escape_pos_heli) < Global.distance_heli_escape)
                        {
                            if (p.gameObject.GetComponent<EvacuationTImeStuckMTF>() == null)
                            {
                                p.gameObject.AddComponent<EvacuationTImeStuckMTF>();
                            }
                            p.gameObject.GetComponent<EvacuationTImeStuckMTF>().time_to_escape = p.gameObject.GetComponent<EvacuationTImeStuckMTF>().time_to_escape + timeIsUp;
                            if (p.gameObject.GetComponent<EvacuationTImeStuckMTF>().time_to_escape >= Global.time_to_escape)
                            {
                                p.ClearBroadcasts();
                                p.Broadcast(1, "<color=#42aaff>Ожидайте эвакуации...</color>", true);
                            }
                            else
                            {
                                p.ClearBroadcasts();
                                p.Broadcast(1, "<color=#228b22>Для эвакуации оставайтесь здесь еще " + (Global.time_to_escape - p.gameObject.GetComponent<EvacuationTImeStuckMTF>().time_to_escape) + " секунд</color>", true);
                            }
                        }
                        else
                        {
                            if (p.gameObject.GetComponent<EvacuationTImeStuckMTF>() != null)
                            {
                                Destroy(p.gameObject.GetComponent<EvacuationTImeStuckMTF>());
                                p.ClearBroadcasts();
                                p.Broadcast(10, "<color=#228b22>Вы покинули зону эвакуации</color>", true);
                            }
                        }
                    }
                }

                if (timeProgress >= Global.time_running_heli)
                {
                    Global.fly_away_with_call = true;
                    timeProgress = -99999999f;
                    Global.chopper.SetState(false);
                    Global.evacuation_process_heli = false;
                    foreach (ReferenceHub p in Player.GetHubs())
                    {
                        if (p.gameObject.GetComponent<EvacuationTImeStuckMTF>() != null)
                        {
                            if (p.gameObject.GetComponent<EvacuationTImeStuckMTF>().time_to_escape >= Global.time_to_escape)
                            {
                                p.SetRole(RoleType.Spectator);
                            }
                            else
                            {
                                p.ClearBroadcasts();
                                p.Broadcast(15, "<color=#ff0000>Вы не успели на эвакуацию</color>", true);
                            }
                            Destroy(p.gameObject.GetComponent<EvacuationTImeStuckMTF>());
                        }
                    }
                    Global.fly_away_with_call = false;
                }

            }

        }
    }
}
