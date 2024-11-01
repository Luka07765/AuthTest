namespace AuthLearning.Models
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpiration { get; set; } // in minutes
        public int RefreshTokenExpiration { get; set; } // in days
    }
}
