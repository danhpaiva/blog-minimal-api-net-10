namespace Blog;

public static class Configuration
{
    public static string JwtKey { get; set; } = "";
    public static SmtpConfiguration Smtp { get; set; } = new SmtpConfiguration();

    public class SmtpConfiguration
    {
        public string Host { get; set; } = "smtp.example.com";
        public int Port { get; set; } = 587;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}