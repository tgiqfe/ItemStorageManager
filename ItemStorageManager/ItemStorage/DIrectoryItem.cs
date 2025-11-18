using ItemStorageManager.Functions;
using Microsoft.VisualBasic.FileIO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ItemStorageManager.ItemStorage
{
    public class DirectoryItem : IBaseItem
    {
        #region Public parameter

        public string Type { get { return "Directory"; } }
        public string Path { get; set; }
        public string Name { get; set; }
        public string CreationTime { get; set; }
        public string LastWriteTime { get; set; }
        public string LastAccessTime { get; set; }
        public string Attributes { get; set; }
        public AccessRuleSet AccessRule { get; set; }

        #endregion

        const string _log_target = "directory";

        public DirectoryItem(string path)
        {
            var di = new DirectoryInfo(path);

            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            this.CreationTime = di.CreationTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.LastWriteTime = di.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.LastAccessTime = di.LastAccessTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.Attributes = di.Attributes.ToString();
            this.AccessRule = new AccessRuleSet(di.GetAccessControl());
        }

        public long GetChildDirectoryCount()
        {
            Logger.WriteLine("Info", $"Getting child directory count of {_log_target} '{this.Path}'.");
            try
            {
                var di = new DirectoryInfo(this.Path);
                return di.GetDirectories().LongLength;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to get child directory count of {_log_target}.");
                Logger.WriteRaw(e.ToString());
            }
            return -1;
        }

        public long GetChildFileCount()
        {
            Logger.WriteLine("Info", $"Getting child file count of {_log_target} '{this.Path}'.");
            try
            {
                var di = new DirectoryInfo(this.Path);
                return di.GetFiles().LongLength;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to get child file count of {_log_target}.");
                Logger.WriteRaw(e.ToString());
            }
            return -1;
        }

        public long GetTotalFileSize()
        {
            Logger.WriteLine("Info", $"Getting total file size of {_log_target} '{this.Path}'.");
            try
            {
                long totalSize = 0;
                var di = new DirectoryInfo(this.Path);
                var files = di.GetFiles("*", System.IO.SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    totalSize += file.Length;
                }
                return totalSize;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to get total file size of {_log_target}.");
                Logger.WriteRaw(e.ToString());
            }
            return -1;
        }

        /// <summary>
        /// Create new empty directory.
        /// </summary>
        /// <param name="newPath"></param>
        /// <returns></returns>
        public static bool New(string newPath)
        {
            Logger.WriteLine("Info", $"Creating new {_log_target}. '{newPath}'");
            try
            {
                Directory.CreateDirectory(newPath);
                Logger.WriteLine("Info", $"Successfully created new {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to create new {_log_target}.");
                Logger.WriteRaw(e.ToString());
            }
            return false;
        }

        /// <summary>
        /// Create new empty directory. (Alias of New)
        /// </summary>
        /// <param name="newPath"></param>
        /// <returns></returns>
        public static bool Add(string newPath)
        {
            return New(newPath);
        }

        /// <summary>
        /// Exists check directory.
        /// </summary>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            Logger.WriteLine("Info", $"Checking existence of {_log_target} at path '{path}'.");
            return Directory.Exists(path);
        }

        /// <summary>
        /// Copy directory.
        /// </summary>
        /// <param name="dstPath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public bool Copy(string dstPath, bool overwrite)
        {
            Logger.WriteLine("Info", $"Copying {_log_target}. From '{this.Path}' to '{dstPath}', overwrite: {overwrite}.");
            try
            {
                FileSystem.CopyDirectory(this.Path, dstPath, overwrite);
                Logger.WriteLine("Info", $"Successfully copied {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to copy {_log_target}.");
                Logger.WriteRaw(e.ToString());
            }
            return false;
        }

        /// <summary>
        /// Remove directory.
        /// </summary>
        /// <returns></returns>
        public bool Remove()
        {
            Logger.WriteLine("Info", $"Removing {_log_target}. '{this.Path}'");
            try
            {
                Directory.Delete(this.Path, true);
                Logger.WriteLine("Info", $"Successfully removed {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to remove {_log_target}.");
                Logger.WriteRaw(e.ToString());
            }
            return false;
        }

        /// <summary>
        /// Remove directory. (Alias of Remove)
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            return Remove();
        }

        public bool Move(string dstPath)
        {
            Logger.WriteLine("Info", $"Moving {_log_target}. From '{this.Path}' to '{dstPath}'.");
            try
            {
                Directory.Move(this.Path, dstPath);
                Logger.WriteLine("Info", $"Successfully moved {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to move {_log_target}.");
                Logger.WriteRaw(e.ToString());
            }
            return false;
        }

        public bool Rename(string newName)
        {
            Logger.WriteLine("Info", $"Renaming {_log_target}. From '{this.Name}' to '{newName}'.");
            try
            {
                var parentDir = System.IO.Path.GetDirectoryName(this.Path);
                var newPath = System.IO.Path.Combine(parentDir, newName);
                Directory.Move(this.Path, newPath);
                this.Path = newPath;
                this.Name = newName;
                Logger.WriteLine("Info", $"Successfully renamed {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to rename {_log_target}.");
                Logger.WriteRaw(e.ToString());
            }
            return false;
        }

        public bool Grant(string account, string rights, string accessType, string inheritance, string propagation)
        {
            Logger.WriteLine("Info", $"Granting access rule to {_log_target}. '{this.Path}': Account='{account}', Rights='{rights}', AccessType='{accessType}', Inheritance='{inheritance}', Propagation='{propagation}'");
            try
            {
                var newRule = new AccessRuleSummary(account, rights, accessType, inheritance, propagation).ToAccessRuleForDirectory();
                var di = new DirectoryInfo(this.Path);
                var acl = di.GetAccessControl();
                acl.AddAccessRule(newRule);
                di.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully granted access rule to {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to grant access rule to {_log_target}.");
                Logger.WriteRaw(e.ToString());
            }
            return false;
        }

        public bool Grant(string accessRuleText)
        {
            Logger.WriteLine("Info", $"Granting access rule to {_log_target}. '{this.Path}': AccessRule='{accessRuleText}'");
            try
            {
                var newRule = new AccessRuleSummary(accessRuleText).ToAccessRuleForDirectory();
                var di = new DirectoryInfo(this.Path);
                var acl = di.GetAccessControl();
                acl.AddAccessRule(newRule);
                di.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully granted access rule to {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to grant access rule to {_log_target}.");
                Logger.WriteRaw(e.ToString());
            }
            return false;
        }

        public bool Revoke(string account)
        {
            Logger.WriteLine("Info", $"Revoking access rules from {_log_target}. '{this.Path}': Account='{account}'");
            try
            {
                var di = new DirectoryInfo(this.Path);
                var acl = di.GetAccessControl();
                bool isChange = false;
                foreach (FileSystemAccessRule rule in acl.GetAccessRules(true, true, typeof(NTAccount)))
                {
                    if (string.Equals(rule.IdentityReference.Value, account, StringComparison.OrdinalIgnoreCase))
                    {
                        acl.RemoveAccessRule(rule);
                        isChange = true;
                    }
                }
                if (isChange) di.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully revoked access rules from {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to revoke access rules from {_log_target}.");
                Logger.WriteRaw(e.ToString());
            }
            return false;
        }

        public bool RevokeAll()
        {
            Logger.WriteLine("Info", $"Revoking all access rules from {_log_target}. '{this.Path}'");
            try
            {
                var di = new DirectoryInfo(this.Path);
                var acl = di.GetAccessControl();
                bool isChange = false;
                foreach (FileSystemAccessRule rule in acl.GetAccessRules(true, true, typeof(NTAccount)))
                {
                    acl.RemoveAccessRule(rule);
                    isChange = true;
                }
                if (isChange) di.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully revoked all access rules from {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to revoke all access rules from {_log_target}.");
                Logger.WriteRaw(e.ToString());
            }
            return false;
        }

        /// <summary>
        /// Change owner of the directory.
        /// </summary>
        /// <param name="newOwner"></param>
        /// <returns></returns>
        public bool ChangeOwner(string newOwner)
        {
            if (string.IsNullOrEmpty(newOwner))
            {
                Logger.WriteLine("Warning", $"Skip change owner to {_log_target}.");
                return false;
            }
            Logger.WriteLine("Info", $"Changing owner of {_log_target}. '{this.Path}' to '{newOwner}'");

            try
            {
                Logger.WriteLine("Info", "Adjusting token privilegs (SeTakeOwnershipPrivilege, SeRestorePrivilege, SeBackupPrivilege)");
                ProcessPrivilege.AdjustToken(Privilege.SeTakeOwnershipPrivilege);
                ProcessPrivilege.AdjustToken(Privilege.SeRestorePrivilege);
                ProcessPrivilege.AdjustToken(Privilege.SeBackupPrivilege);

                var di = new DirectoryInfo(this.Path);
                var acl = di.GetAccessControl();
                acl.SetOwner(new NTAccount(newOwner));
                di.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully changed owner of {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to change owner of {_log_target}.");
                Logger.WriteRaw(e.ToString());
            }
            return false;
        }

        /// <summary>
        /// Change access rule inheritance of the directory.
        /// </summary>
        /// <param name="isInherited"></param>
        /// <param name="preserve"></param>
        /// <returns></returns>
        public bool ChangeInherited(bool? isInherited, bool preserve = true)
        {
            if (isInherited == null)
            {
                Logger.WriteLine("Warning", $"Skip change inherited to {_log_target}.");
                return false;
            }
            Logger.WriteLine("Info", $"Changing inheritance of {_log_target}. '{this.Path}' to '{isInherited}', preserve existing rules: {preserve}.");

            try
            {
                var di = new DirectoryInfo(this.Path);
                var acl = di.GetAccessControl();
                acl.SetAccessRuleProtection(!(bool)isInherited, preserve);
                di.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully changed inheritance of {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to change inheritance of {_log_target}.");
                Logger.WriteRaw(e.ToString());
            }
            return false;
        }

        /// <summary>
        /// Set attributes of the directory.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public bool SetAttributes(string attributes)
        {
            if (string.IsNullOrEmpty(attributes))
            {
                Logger.WriteLine("Warning", $"Skip set attributes to {_log_target}.");
                return false;
            }
            Logger.WriteLine("Info", $"Setting attributes of directory '{this.Path}' to '{attributes}'.");

            try
            {
                var di = new DirectoryInfo(this.Path);
                di.Attributes = AttributesParser.MergeAttributes(attributes, di.Attributes);
                this.Attributes = di.Attributes.ToString();
                Logger.WriteLine("Info", $"Successfully set attributes of directory '{this.Path}' to '{attributes}'.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to set attributes of directory '{this.Path}' to '{attributes}'. Exception: {e.Message}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }
    }
}
