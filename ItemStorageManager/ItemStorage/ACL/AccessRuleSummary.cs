using System.Security.AccessControl;

namespace ItemStorageManager.ItemStorage.ACL
{
    internal class AccessRuleSummary
    {
        public string Account { get; set; }
        public string Rights { get; set; }
        public string AccessType { get; set; }
        public string Inheritacne { get; set; }

        public AccessRuleSummary(FileSystemAccessRule rule)
        {
            this.Account = rule.IdentityReference.Value;
            this.Rights = rule.FileSystemRights == FileSystemRights.FullControl ?
                "FullControl" :
                (rule.FileSystemRights & (~FileSystemRights.Synchronize)).ToString();
            this.AccessType = rule.AccessControlType.ToString();
            this.Inheritacne = rule.InheritanceFlags.ToString();
        }

        public AccessRuleSummary(RegistryAccessRule rule)
        {
            this.Account = rule.IdentityReference.Value;
            this.Rights = rule.RegistryRights.ToString();
            this.AccessType = rule.AccessControlType.ToString();
            this.Inheritacne = rule.InheritanceFlags.ToString();
        }

        public AccessRuleSummary(string ruleString)
        {
            var parts = ruleString.Split(';');
            if (parts.Length == 4)
            {
                this.Account = parts[0];
                this.Rights = parts[1];
                this.AccessType = parts[2];
                this.Inheritacne = parts[3];
            }
            else
            {
                throw new ArgumentException("Invalid rule string format.");
            }
        }

        public static AccessRuleSummary[] LoadFromFileSystem(AuthorizationRuleCollection rules)
        {
            var list = new List<AccessRuleSummary>();
            foreach (FileSystemAccessRule rule in rules)
            {
                list.Add(new AccessRuleSummary(rule));
            }
            return list.ToArray();
        }

        public static AccessRuleSummary[] LoadFromRegistry(AuthorizationRuleCollection rules)
        {
            var list = new List<AccessRuleSummary>();
            foreach (RegistryAccessRule rule in rules)
            {
                list.Add(new AccessRuleSummary(rule));
            }
            return list.ToArray();
        }


        public override string ToString()
        {
            return $"{Account};{Rights};{AccessType};{Inheritacne}";
        }
    }
}
