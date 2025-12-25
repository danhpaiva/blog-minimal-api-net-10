namespace Blog;

public static class Configuration
{
    public static string JwtKey { get; set; } = "C040542A77BC4E4A8AFAA389BFED7D70abcd";
    public static SmtpConfiguration Smtp { get; set; } = new SmtpConfiguration();

    public class SmtpConfiguration
    {
        public string Host { get; set; } = "smtp.example.com";
        public int Port { get; set; } = 587;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}