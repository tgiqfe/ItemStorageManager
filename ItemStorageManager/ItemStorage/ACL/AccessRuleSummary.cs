using System.Security.AccessControl;
using System.Text.Json.Serialization;

namespace ItemStorageManager.ItemStorage.ACL
{
    internal class AccessRuleSummary
    {
        [JsonIgnore]
        public string Account { get; set; }

        [JsonIgnore]
        public string Rights { get; set; }

        [JsonIgnore]
        public string AccessType { get; set; }

        [JsonIgnore]
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
            SetFromString(ruleString);
        }

        public override string ToString()
        {
            return $"{Account};{Rights};{AccessType};{Inheritacne}";
        }

        public void SetFromString(string ruleString)
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
    }
}
