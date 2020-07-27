namespace Globe.Identity.Options
{
    public class GlobeClaim
    {
        public GlobeClaim()
        {
        }

        public GlobeClaim(string type, string[] allowValues)
        {
            Type = type;
            AllowValues = allowValues;
        }

        public string Type { get; set; }
        public string[] AllowValues { get; set; }
    }
}
