using System.Collections.Generic;

namespace Sandbox.Models.LocalUser
{
    public class ExternalUser
    {
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public List<string> Roles { get; } = new List<string>();
    }
}