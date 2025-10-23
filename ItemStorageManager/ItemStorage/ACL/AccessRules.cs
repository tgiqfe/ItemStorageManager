using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace ItemStorageManager.ItemStorage.ACL
{
    internal class AccessRules
    {
        public string Owner { get; set; }
        public bool IsInherited { get; set; }
        public AccessRuleSummary[] Rules { get; set; }

        public AccessRules(NativeObjectSecurity security)
        {
            this.Owner = security.GetOwner(typeof(NTAccount)).Value;
            this.IsInherited = security.AreAccessRulesProtected == false;

            var lsit = new List<AccessRuleSummary>();
            foreach (var rule in security.GetAccessRules(true, false, typeof(NTAccount)))
            {
                if (rule is FileSystemAccessRule fsRule)
                {
                    lsit.Add(new AccessRuleSummary(fsRule));
                }
                else if (rule is RegistryAccessRule regRule)
                {
                    lsit.Add(new AccessRuleSummary(regRule));
                }
            }
        }
    }
}
