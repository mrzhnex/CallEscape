using UnityEngine;

namespace CallEscape
{
    internal class Global
    {
        //global
        public static GameObject mainSetter;
        public static MTFRespawn mtfrespawn;
        public static ChopperAutostart chopper;
        public static float time_to_escape = 10.0f;

        ///////ChaosInsurgency
        public static Vector3 call_pos_van = new Vector3(-54.5f, 989f, -49.5f);
        public static Vector3 escape_pos_van = new Vector3(10f, 989f, -49.5f);

        public static float distance_van = 2.5f;
        public static float time_running_van = 20f;
        public static float delay_to_van = 30f;

        public static bool evacuation_process_van = false;
        public static bool call_evacuation_van = false;
        ///////ChaosInsurgency


        ///////NineTailedFox
        public static Vector3 call_pos_heli = new Vector3(182.5f, 994f, -87f);
        public static Vector3 escape_pos_heli = new Vector3(179f, 994f, -58f);

        public static float distance_heli_call = 4.0f;
        public static float distance_heli_escape = 7.0f;
        public static float time_running_heli = 40f;
        public static float delay_to_heli = 60f;

        public static bool evacuation_process_heli = false;
        public static bool call_evacuation_heli = false;

        public static bool fly_away_with_call = false;
        internal static bool can_use_commands;
        ///////NineTailedFox
    }
}