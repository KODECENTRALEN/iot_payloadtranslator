namespace PayloadTranslator.Handlers.NKEWatteco.Helpers
{
    public class Reportparameters
    {
        public int Reserved { get; set; }

        public string CauseRequest { get; set; }

        public bool SecuredIfAlarm { get; set; }

        public bool Secured { get; set; }

        public bool NoHeaderPort { get; set; }

        public bool Batch { get; set; }
    }
}
