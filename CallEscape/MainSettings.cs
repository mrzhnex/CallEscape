using EXILED;

namespace CallEscape
{
    public class MainSettings : Plugin
    {
        public override string getName => nameof(CallEscape);
        public SetEvents SetEvents { get; set; }

        public override void OnEnable()
        {
            SetEvents = new SetEvents();
            Events.WaitingForPlayersEvent += SetEvents.OnWaitingForPlayers;
            Events.RoundStartEvent += SetEvents.OnRoundStart;
            Events.ConsoleCommandEvent += SetEvents.OnCallCommand;
            Events.CheckEscapeEvent += SetEvents.OnCheckEscape;
            Log.Info(getName + " on");
        }

        public override void OnDisable()
        {
            Events.WaitingForPlayersEvent -= SetEvents.OnWaitingForPlayers;
            Events.RoundStartEvent -= SetEvents.OnRoundStart;
            Events.ConsoleCommandEvent -= SetEvents.OnCallCommand;
            Events.CheckEscapeEvent -= SetEvents.OnCheckEscape;
            Log.Info(getName + " off");
        }

        public override void OnReload() { }
    }
}