using System.Globalization;

namespace JWT_Auth.Helpers
{
    public static class Constants
    {
        public const string Audience = "https://localhost:44361/"; // normally we use our api url
        public const string Issuer = "https://localhost:44361/";
        public const string Secret = "asdgajshgkajhsgfkjyewywvjdbnjhsdgoiittnvncbvxfswafcxhxhd"; // use a any long string

    }
}
