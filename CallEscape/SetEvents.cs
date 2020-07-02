using EXILED;
using EXILED.Extensions;
using UnityEngine;

namespace CallEscape
{
    internal class SetEvents
    {
        public void OnCallCommand(ConsoleCommandEvent ev)
        {
            if (!Global.can_use_commands)
            {
                ev.ReturnMessage = "Дождитесь начала раунда!";
                return;
            }
            if (ev.Command.ToLower().Contains("call") && ev.Command.ToLower().Contains("evacuation"))
            {
                if (ev.Player.GetRole() == RoleType.ChaosInsurgency && ev.Player.inventory.curItem == ItemType.Radio && Vector3.Distance(ev.Player.GetPosition(), Global.call_pos_van) < Global.distance_van && !Global.evacuation_process_van && !Global.call_evacuation_van && Global.mainSetter.GetComponent<CheckEvacuationCI>() != null)
                {
                    Global.call_evacuation_van = true;
                    ev.ReturnMessage = "Вы вызвали машину Повстанцев Хаоса. Она прибудет через " + Global.delay_to_van + " секунд";
                    return;
                }
                else if (ev.Player.GetRole() == RoleType.ChaosInsurgency && ev.Player.inventory.curItem == ItemType.Radio && Vector3.Distance(ev.Player.GetPosition(), Global.call_pos_van) < Global.distance_van)
                {
                    ev.ReturnMessage = "В данный момент вызов эвакуации невозможен. Повторите попытку позже";
                    return;
                }

                if ((ev.Player.GetRole() == RoleType.FacilityGuard || ev.Player.GetTeam() == Team.MTF) && ev.Player.inventory.curItem == ItemType.Radio && Vector3.Distance(ev.Player.GetPosition(), Global.call_pos_heli) < Global.distance_heli_call && !Global.evacuation_process_heli && !Global.call_evacuation_heli && Global.mainSetter.GetComponent<CheckEvacuationMTF>() != null && !Global.chopper.NetworkisLanded)
                {
                    Global.call_evacuation_heli = true;
                    ev.ReturnMessage = "Вы вызвали вертолет МОГ. Он прибудет через " + Global.delay_to_heli + " секунд";
                    return;
                }
                else if ((ev.Player.GetRole() == RoleType.FacilityGuard || ev.Player.GetTeam() == Team.MTF) && ev.Player.inventory.curItem == ItemType.Radio && Vector3.Distance(ev.Player.GetPosition(), Global.call_pos_heli) < Global.distance_heli_call)
                {
                    ev.ReturnMessage = "В данный момент вызов эвакуации невозможен. Повторите попытку позже";
                    return;
                }
                ev.ReturnMessage = "Вы не в зоне эвакуации или не имеете доступа к вызову эвакуации";
                return;
            }
        }
        internal void OnPlayerSpawn(PlayerSpawnEvent ev)
        {
            if (ev.Player.GetRole() == RoleType.FacilityGuard)
            {
                if (ev.Player.gameObject.GetComponent<SetRoleOnSpawn>())
                    Object.Destroy(ev.Player.gameObject.GetComponent<SetRoleOnSpawn>());
                ev.Player.gameObject.AddComponent<SetRoleOnSpawn>();
                ev.Player.gameObject.GetComponent<SetRoleOnSpawn>().RemoveItems.Add(ItemType.KeycardGuard);
                ev.Player.gameObject.GetComponent<SetRoleOnSpawn>().RemoveItems.Add(ItemType.WeaponManagerTablet);
                ev.Player.gameObject.GetComponent<SetRoleOnSpawn>().AddItems.Add(ItemType.KeycardSeniorGuard);
            }
            if (ev.Player.GetRole() == RoleType.NtfCadet || ev.Player.GetRole() == RoleType.NtfLieutenant)
            {
                if (ev.Player.gameObject.GetComponent<SetRoleOnSpawn>())
                    Object.Destroy(ev.Player.gameObject.GetComponent<SetRoleOnSpawn>());
                ev.Player.gameObject.AddComponent<SetRoleOnSpawn>();
                ev.Player.gameObject.GetComponent<SetRoleOnSpawn>().RemoveItems.Add(ItemType.WeaponManagerTablet);
            }
            if (ev.Player.GetRole() == RoleType.ChaosInsurgency)
            {
                if (ev.Player.gameObject.GetComponent<SetRoleOnSpawn>())
                    Object.Destroy(ev.Player.gameObject.GetComponent<SetRoleOnSpawn>());
                ev.Player.gameObject.AddComponent<SetRoleOnSpawn>();
                ev.Player.gameObject.GetComponent<SetRoleOnSpawn>().AddItems.Add(ItemType.Radio);
            }
        }

        internal void OnCheckEscape(ref CheckEscapeEvent ev)
        {
            if (ev.Player.GetRole() != RoleType.FacilityGuard && ev.Player.GetTeam() != Team.MTF)
            {
                ev.Player.ClearInventory();
                ev.Allow = false;
                ev.Player.SetRole(RoleType.Spectator);
            }
        }

        public void OnRoundStart()
        {
            Global.can_use_commands = true;
            if (Global.mtfrespawn == null)
            {
                Global.mtfrespawn = Object.FindObjectOfType<MTFRespawn>();
            }
            if (Global.chopper == null)
            {
                Global.chopper = Object.FindObjectOfType<ChopperAutostart>();
            }
            Global.mainSetter = GameObject.FindWithTag("FemurBreaker");
            Global.mainSetter.AddComponent<CheckCustomEscape>();
            Global.mainSetter.AddComponent<CheckEvacuationCI>();
            Global.mainSetter.AddComponent<CheckEvacuationMTF>();
            Global.mainSetter.AddComponent<CheckEvacuationMTFCommand>();
        }

        public void OnWaitingForPlayers()
        {
            Global.can_use_commands = false;
            Global.time_to_escape = 15.0f;
            Global.delay_to_van = 30.0f;
            Global.delay_to_heli = 60.0f;
        }
    }
}