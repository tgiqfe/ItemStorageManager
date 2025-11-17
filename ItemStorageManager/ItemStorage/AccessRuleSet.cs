using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace ItemStorageManager.ItemStorage
{
    public class AccessRuleSet
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

        public AccessRuleSet(NativeObjectSecurity security)
        {
            Owner = security.GetOwner(typeof(NTAccount)).Value;
            IsInherited = security.AreAccessRulesProtected == false;

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
            AccessRuleSummaries = list.ToArray();
        }
    }
}
