using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Text.Json.Serialization;
using System.Windows.Markup;

namespace ItemStorageManager.ItemStorage.ACL
{
    internal class ItemAccessRule
    {
        public string Owner { get; set; }
        public bool IsInherited { get; set; }

        [JsonIgnore]
        public AccessRuleSummary[] AccessRuleSummaries { get; set; }

        public string[] Rules
        {
            get { return AccessRuleSummaries.Select(x => x.ToString()).ToArray(); }
            set { value.Select(x => new AccessRuleSummary(x)).ToArray(); }
        }

        public ItemAccessRule(NativeObjectSecurity security)
        {
            this.Owner = security.GetOwner(typeof(NTAccount)).Value;
            this.IsInherited = security.AreAccessRulesProtected == false;

            var list = new List<AccessRuleSummary>();
            foreach (var rule in security.GetAccessRules(true, false, typeof(NTAccount)))
            {
                if (rule is FileSystemAccessRule fsRule)
                {
                    list.Add(new AccessRuleSummary(fsRule));
                }
                else if (rule is RegistryAccessRule regRule)
                {
                    list.Add(new AccessRuleSummary(regRule));
                }
            }
            this.AccessRuleSummaries = list.ToArray();
        }
    }
}
