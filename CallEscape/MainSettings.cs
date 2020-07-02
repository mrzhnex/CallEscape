using EXILED;

namespace CallEscape
{
    public class MainSettings : Plugin
    {
        public override string getName => "CallEscape";
        private SetEvents SetEvents;

        public override void OnEnable()
        {
            SetEvents = new SetEvents();
            Events.WaitingForPlayersEvent += SetEvents.OnWaitingForPlayers;
            Events.RoundStartEvent += SetEvents.OnRoundStart;
            Events.ConsoleCommandEvent += SetEvents.OnCallCommand;
            Events.CheckEscapeEvent += SetEvents.OnCheckEscape;
            Events.PlayerSpawnEvent += SetEvents.OnPlayerSpawn;
            Log.Info(getName + " on");
        }

        public override void OnDisable()
        {
            Events.WaitingForPlayersEvent -= SetEvents.OnWaitingForPlayers;
            Events.RoundStartEvent -= SetEvents.OnRoundStart;
            Events.ConsoleCommandEvent -= SetEvents.OnCallCommand;
            Events.CheckEscapeEvent -= SetEvents.OnCheckEscape;
            Events.PlayerSpawnEvent -= SetEvents.OnPlayerSpawn;
            Log.Info(getName + " off");
        }

        public override void OnReload() { }
    }
}