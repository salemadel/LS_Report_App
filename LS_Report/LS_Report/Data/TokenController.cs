using LS_Report.Models;
using Plugin.SecureStorage;
using System;

namespace LS_Report.Data
{
    public class TokenController
    {
        private readonly string key = "uyagkqhfquyfviqucvqivqh87236djhdvw";

        public (bool, Token_Model) DecodeToken()
        {
            bool token_exist = CrossSecureStorage.Current.HasKey("acces_token");
            if (token_exist)
            {
                string token = CrossSecureStorage.Current.GetValue("acces_token");
                return (true, JWT.JsonWebToken.DecodeToObject<Token_Model>(token, key, false));
            }
            else
            {
                return (false, null);
            }
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public bool Token_Expired()
        {
            var jwt = DecodeToken();
            if (jwt.Item1)
            {
                DateTime now = DateTime.Now;
                DateTime exp = UnixTimeStampToDateTime(jwt.Item2.exp);
                if (exp >= now)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }
}