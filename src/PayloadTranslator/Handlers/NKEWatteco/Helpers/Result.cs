namespace Handlers.NKEWatteco.Helpers
{
    public class Result
    {
        public int EndPoint { get; set; }

        public string Report { get; set; }

        public string CommandID { get; set; }

        public string ClusterID { get; set; }

        public string AttributeID { get; set; }

        public string AttributeType { get; set; }

        public float Data { get; set; }

        public Cause[] Cause { get; set; }

        public SimpleMetering SimpleMetering { get; set; }
    }
}
