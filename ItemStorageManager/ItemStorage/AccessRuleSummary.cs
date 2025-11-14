using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace ItemStorageManager.ItemStorage
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
            Account = rule.IdentityReference.Value;
            Rights = rule.FileSystemRights == FileSystemRights.FullControl ?
                "FullControl" :
                (rule.FileSystemRights & ~FileSystemRights.Synchronize).ToString();
            AccessType = rule.AccessControlType.ToString();
            Inheritance = rule.InheritanceFlags.ToString();
            Propagation = rule.PropagationFlags.ToString();
        }

        public AccessRuleSummary(RegistryAccessRule rule)
        {
            Account = rule.IdentityReference.Value;
            Rights = rule.RegistryRights.ToString();
            AccessType = rule.AccessControlType.ToString();
            Inheritance = rule.InheritanceFlags.ToString();
            Propagation = rule.PropagationFlags.ToString();
        }

        public AccessRuleSummary(string ruleString)
        {
            var parts = ruleString.Split(';');
            if (parts.Length == 4)
            {
                Account = parts[0];
                Rights = parts[1];
                AccessType = parts[2];
                Inheritance = parts[3];
                Propagation = parts[4];
            }
            else
            {
                throw new ArgumentException("Invalid rule string format.");
            }
        }

        public AccessRuleSummary(string account, string rights, string accessType, string inheritance, string propagation) : this(account)
        {
            Account = account;
            Rights = rights;
            AccessType = accessType;
            Inheritance = inheritance;
            Propagation = propagation;
        }

        #endregion

        public override string ToString()
        {
            return $"{Account};{Rights};{AccessType};{Inheritance};{Propagation}";
        }

        public FileSystemAccessRule ToAccessRuleForFile()
        {
            return new FileSystemAccessRule(
                new NTAccount(Account),
                AccessRuleParser.StringToFileSystemRights(this.Rights),
                AccessRuleParser.StringToAccessControlType(this.AccessType));
        }

        public FileSystemAccessRule ToAccessRuleForDirectory()
        {
            return new FileSystemAccessRule(
                new NTAccount(Account),
                AccessRuleParser.StringToFileSystemRights(this.Rights),
                AccessRuleParser.StringToInheritanceFlags(this.Inheritance),
                AccessRuleParser.StringToPropagationFlags(this.Propagation),
                AccessRuleParser.StringToAccessControlType(this.AccessType));
        }

        public RegistryAccessRule ToAccessRuleForRegistryKey()
        {
            return new RegistryAccessRule(
                new NTAccount(Account),
                AccessRuleParser.StringToRegistryRights(this.Rights),
                AccessRuleParser.StringToInheritanceFlags(this.Inheritance),
                AccessRuleParser.StringToPropagationFlags(this.Propagation),
                AccessRuleParser.StringToAccessControlType(this.AccessType));
        }
    }
}
