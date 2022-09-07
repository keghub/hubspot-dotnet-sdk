using HubSpot.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.Model
{
    public class TestTokenSelector : ITokenSelector
    {
        public bool IsTokenChanged()
        {
            return false;
        }

        public string SelectToken(Dictionary<string, string> tokens)
        {
            return tokens.FirstOrDefault().Value;
        }
    }
}
