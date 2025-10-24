using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace ItemStorageManager.ItemStorage.ACL
{
    internal class AccessRuleSummary
    {
        [JsonIgnore]
        public string Account { get; private set; }

        [JsonIgnore]
        public string Rights { get; private set; }

        [JsonIgnore]
        public string AccessType { get; private set; }

        [JsonIgnore]
        public string Inheritance { get; private set; }

        [JsonIgnore]
        public string Propagation { get; private set; }

        #region Constructors

        public AccessRuleSummary(FileSystemAccessRule rule)
        {
            this.Account = rule.IdentityReference.Value;
            this.Rights = rule.FileSystemRights == FileSystemRights.FullControl ?
                "FullControl" :
                (rule.FileSystemRights & (~FileSystemRights.Synchronize)).ToString();
            this.AccessType = rule.AccessControlType.ToString();
            this.Inheritance = rule.InheritanceFlags.ToString();
            this.Propagation = rule.PropagationFlags.ToString();
        }

        public AccessRuleSummary(RegistryAccessRule rule)
        {
            this.Account = rule.IdentityReference.Value;
            this.Rights = rule.RegistryRights.ToString();
            this.AccessType = rule.AccessControlType.ToString();
            this.Inheritance = rule.InheritanceFlags.ToString();
            this.Propagation = rule.PropagationFlags.ToString();
        }

        public AccessRuleSummary(string ruleString)
        {
            var parts = ruleString.Split(';');
            if (parts.Length == 4)
            {
                this.Account = parts[0];
                this.Rights = parts[1];
                this.AccessType = parts[2];
                this.Inheritance = parts[3];
                this.Propagation = parts[4];
            }
            else
            {
                throw new ArgumentException("Invalid rule string format.");
            }
        }

        public AccessRuleSummary(string account, string rights, string accessType, string inheritance, string propagation) : this(account)
        {
            this.Account = account;
            this.Rights = rights;
            this.AccessType = accessType;
            this.Inheritance = inheritance;
            this.Propagation = propagation;
        }

        #endregion

        public override string ToString()
        {
            return $"{Account};{Rights};{AccessType};{Inheritance};{Propagation}";
        }

        public FileSystemAccessRule ToAccessRuleForFile()
        {
            return new FileSystemAccessRule(
                new NTAccount(this.Account),
                AccessRuleFunctions.ParseFileSystemRights(this.Rights),
                AccessRuleFunctions.ParseAccessControlType(this.AccessType));
        }

        public FileSystemAccessRule ToAccessRuleForDirectory()
        {
            return new FileSystemAccessRule(
                new NTAccount(this.Account),
                AccessRuleFunctions.ParseFileSystemRights(this.Rights),
                AccessRuleFunctions.ParseInheritanceFlags(this.Inheritance),
                AccessRuleFunctions.ParsePropagationFlags(this.Propagation),
                AccessRuleFunctions.ParseAccessControlType(this.AccessType));
        }

        public RegistryAccessRule ToAccessRuleForRegistryKey()
        {
            return new RegistryAccessRule(
                new NTAccount(this.Account),
                AccessRuleFunctions.ParseRegistryRights(this.Rights),
                AccessRuleFunctions.ParseAccessControlType(this.AccessType));
        }
    }
}
