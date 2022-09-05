using System;
using System.Collections.Generic;
using System.Text;

namespace HubSpot.Authentication
{
    public interface ITokenSelector
    {
        string SelectToken(Dictionary<string, string> tokens);
        bool IsTokenChanged();
    }
}
