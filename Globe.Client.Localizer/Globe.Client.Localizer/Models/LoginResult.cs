namespace Globe.Client.Localizer.Models
{
    internal class LoginResult
    {
        public LoginResult()
        {
            Successful = true;
            Error = string.Empty;
            Token = string.Empty;
        }

        public bool Successful { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
    }
}
