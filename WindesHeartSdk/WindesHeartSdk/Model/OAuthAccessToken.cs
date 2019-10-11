using System;
using System.Collections.Generic;
using System.Text;

namespace WindesHeartSdk.Model
{
    public class OAuthAccessToken
    {
        public Guid AccessToken { get; set; }
        public DateTime ValidUntil { get; set; }

        public bool IsExpired => ValidUntil < DateTime.Now;
    }
}
