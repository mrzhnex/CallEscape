using EXILED.Extensions;
using UnityEngine;

namespace CallEscape
{
    class CheckEvacuationCI : MonoBehaviour
    {
        private float timeProgress = 0f;
        private float timer = 0f;
        private float timeIsUp = 1.0f;
        private float delay_to_van = 0f;

        public void Update()
        {
            timer += Time.deltaTime;
            if (timer >= timeIsUp)
            {
                timer = 0f;
                if (Global.call_evacuation_van)
                {
                    delay_to_van = delay_to_van + timeIsUp;
                    if (delay_to_van >= Global.delay_to_van)
                    {
                        Global.call_evacuation_van = false;
                        delay_to_van = 0f;
                        timeProgress = 0f;
                        Global.evacuation_process_van = true;
                        Global.mtfrespawn.RpcVan();
                    }
                }


                if (Global.evacuation_process_van)
                {
                    timeProgress = timeProgress + timeIsUp;
                    foreach (ReferenceHub p in Player.GetHubs())
                    {
                        if ((p.GetTeam() == Team.MTF && !p.IsHandCuffed()) || p.GetTeam() == Team.SCP || (p.GetRole() == RoleType.FacilityGuard && !p.IsHandCuffed()))
                        {
                            continue;
                        }
                        if (Vector3.Distance(p.GetPosition(), Global.escape_pos_van) < Global.distance_van)
                        {
                            if (p.gameObject.GetComponent<EvacuationTImeStuckCI>() == null)
                            {
                                p.gameObject.AddComponent<EvacuationTImeStuckCI>();
                            }
                            p.gameObject.GetComponent<EvacuationTImeStuckCI>().time_to_escape = p.gameObject.GetComponent<EvacuationTImeStuckCI>().time_to_escape + timeIsUp;
                            if (p.gameObject.GetComponent<EvacuationTImeStuckCI>().time_to_escape >= Global.time_to_escape)
                            {
                                p.ClearBroadcasts();
                                p.Broadcast(1, "<color=#42aaff>Ожидайте эвакуации...</color>", true);
                            }
                            else
                            {
                                p.ClearBroadcasts();
                                p.Broadcast(1, "<color=#228b22>Для эвакуации оставайтесь здесь еще " + (Global.time_to_escape - p.gameObject.GetComponent<EvacuationTImeStuckCI>().time_to_escape) + " секунд</color>", true);
                            }
                        }
                        else
                        {
                            if (p.gameObject.GetComponent<EvacuationTImeStuckCI>() != null)
                            {
                                Destroy(p.gameObject.GetComponent<EvacuationTImeStuckCI>());
                                p.ClearBroadcasts();
                                p.Broadcast(10, "<color=#228b22>Вы покинули зону эвакуации</color>", true);
                            }
                        }
                    }
                }

                if (timeProgress >= Global.time_running_van)
                {
                    timeProgress = -99999999f;
                    Global.evacuation_process_van = false;
                    foreach (ReferenceHub p in Player.GetHubs())
                    {
                        if (p.gameObject.GetComponent<EvacuationTImeStuckCI>() != null)
                        {
                            if (p.gameObject.GetComponent<EvacuationTImeStuckCI>().time_to_escape >= Global.time_to_escape)
                            {
                                p.SetRole(RoleType.Spectator);
                            }
                            else
                            {
                                p.ClearBroadcasts();
                                p.Broadcast(15, "<color=#ff0000>Вы не успели на эвакуацию</color>", true);
                            }
                            Destroy(p.gameObject.GetComponent<EvacuationTImeStuckCI>());
                        }
                    }
                }

            }

        }
    }
}