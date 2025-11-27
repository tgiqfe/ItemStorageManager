using ItemStorageManager.Functions;
using ItemStorageManager.Functions.EnumParser;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ItemStorageManager.ItemStorage
{
    public class FileItem : IBaseItem
    {
        #region Public parameter

        public string Type { get { return "File"; } }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string FormatedSize { get; set; }
        public string CreationTime { get; set; }
        public string LastWriteTime { get; set; }
        public string LastAccessTime { get; set; }
        public string Attributes { get; set; }
        public AccessRuleSet AccessRule { get; set; }
        public bool SecurityBlock { get; set; }

        #endregion

        const string _log_title = "ItemStorage";
        const string _log_target = "file";

        public FileItem(string path)
        {
            var fi = new FileInfo(path);

            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            this.Size = string.Format("{0:N0} Byte", fi.Length);
            this.FormatedSize = TextFunctions.FormatFileSize(fi.Length);
            this.CreationTime = fi.CreationTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.LastWriteTime = fi.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.LastAccessTime = fi.LastAccessTime.ToString("yyyy/MM/dd HH:mm:ss");
            this.Attributes = fi.Attributes.ToString();
            this.AccessRule = new AccessRuleSet(fi.GetAccessControl());
            this.SecurityBlock = File.Exists($"{path}:Zone.Identifier");
        }

        /// <summary>
        /// Create new empty file.
        /// </summary>
        /// <param name="newPath"></param>
        /// <returns></returns>
        public static bool New(string newPath)
        {
            Logger.WriteLine("Info", _log_title, $"Creating new {_log_target}. '{newPath}'");
            try
            {
                var parent = System.IO.Path.GetDirectoryName(newPath);
                if (!Directory.Exists(parent))
                {
                    Directory.CreateDirectory(parent);
                }
                File.CreateText(newPath).Close();
                Logger.WriteLine("Info", _log_title, $"Successfully created new {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", _log_title, $"Failed to create new {_log_target}.");
                Logger.WriteRaw(_log_title, e.ToString());
            }
            return false;
        }

        /// <summary>
        /// Create new empty file.
        /// </summary>
        /// <param name="newParentPath"></param>
        /// <param name="newPath"></param>
        /// <returns></returns>
        public static bool New(string newParentPath, string newPath)
        {
            return New(System.IO.Path.Combine(newParentPath, newPath));
        }

        /// <summary>
        /// Create new empty file. (Alias of New)
        /// </summary>
        /// <param name="newPAth"></param>
        /// <returns></returns>
        public static bool Add(string newPAth)
        {
            return New(newPAth);
        }

        /// <summary>
        /// Create new empty file. (Alias of New)
        /// </summary>
        /// <param name="newParentPath"></param>
        /// <param name="newPath"></param>
        /// <returns></returns>
        public static bool Add(string newParentPath, string newPath)
        {
            return New(System.IO.Path.Combine(newParentPath, newPath));
        }

        public bool RemoveSecurityBlock()
        {
            Logger.WriteLine("Info", _log_title, $"Removing security block from {_log_target}. '{this.Path}'");
            try
            {
                var adsPath = $"{this.Path}:Zone.Identifier";
                if (File.Exists(adsPath))
                {
                    File.Delete(adsPath);
                }
                Logger.WriteLine("Info", _log_title, $"Successfully removed security block from {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", _log_title, $"Failed to remove security block from {_log_target}.");
                Logger.WriteRaw(_log_title, e.ToString());
            }
            return false;
        }

        /// <summary>
        /// Exists check file.
        /// </summary>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            Logger.WriteLine("Info", _log_title, $"Checking existence of {_log_target} at path '{path}'.");
            return File.Exists(path);
        }

        /// <summary>
        /// Copy file.
        /// </summary>
        /// <param name="dstPath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public bool Copy(string dstPath, bool overwrite)
        {
            Logger.WriteLine("Info", _log_title, $"Copying {_log_target}. From '{this.Path}' to '{dstPath}', overwrite: {overwrite}.");
            try
            {
                File.Copy(this.Path, dstPath, overwrite);
                Logger.WriteLine("Info", _log_title, $"Successfully copied {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", _log_title, $"Failed to copy {_log_target}.");
                Logger.WriteRaw(_log_title, e.ToString());
            }
            return false;
        }

        /// <summary>
        /// Remove file.
        /// </summary>
        /// <returns></returns>
        public bool Remove()
        {
            Logger.WriteLine("Info", _log_title, $"Removing {_log_target}. '{this.Path}'");
            try
            {
                File.Delete(this.Path);
                Logger.WriteLine("Info", _log_title, $"Successfully removed {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", _log_title, $"Failed to remove {_log_target}.");
                Logger.WriteRaw(_log_title, e.ToString());
            }
            return false;
        }

        /// <summary>
        /// Remove file. (Alias of Remove)
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            return this.Remove();
        }

        public bool Move(string dstPath)
        {
            Logger.WriteLine("Info", _log_title, $"Moving {_log_target}. From '{this.Path}' to '{dstPath}'.");
            try
            {
                File.Move(this.Path, dstPath);
                Logger.WriteLine("Info", _log_title, $"Successfully moved {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", _log_title, $"Failed to move {_log_target}.");
                Logger.WriteRaw(_log_title, e.ToString());
            }
            return false;
        }

        public bool Rename(string newName)
        {
            Logger.WriteLine("Info", _log_title, $"Renaming {_log_target}. From '{this.Name}' to '{newName}'.");
            try
            {
                var dstPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Path), newName);
                File.Move(this.Path, dstPath);
                Logger.WriteLine("Info", _log_title, $"Successfully renamed {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", _log_title, $"Failed to rename {_log_target}.");
                Logger.WriteRaw(_log_title, e.ToString());
            }
            return false;
        }

        public bool Grant(string account, string rights, string accessType, string inheritance = null, string propagation = null)
        {
            Logger.WriteLine("Info", _log_title, $"Granting access rule to {_log_target}. '{this.Path}': Account='{account}', Rights='{rights}', AccessType='{accessType}', Inheritance='{inheritance}', Propagation='{propagation}'");
            try
            {
                var newRule = new AccessRuleSummary(account, rights, accessType, null, null).ToAccessRuleForFile();
                var fi = new FileInfo(this.Path);
                var acl = fi.GetAccessControl();
                acl.AddAccessRule(newRule);
                fi.SetAccessControl(acl);
                Logger.WriteLine("Info", _log_title, $"Successfully granted access rule to {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", _log_title, $"Failed to grant access rule to {_log_target}.");
                Logger.WriteRaw(_log_title, e.ToString());
            }
            return false;
        }

        public bool Grant(string accessRuleText)
        {
            Logger.WriteLine("Info", _log_title, $"Granting access rule to {_log_target}. '{this.Path}': AccessRule='{accessRuleText}'");
            try
            {
                var newRule = new AccessRuleSummary(accessRuleText).ToAccessRuleForFile();
                var fi = new FileInfo(this.Path);
                var acl = fi.GetAccessControl();
                acl.AddAccessRule(newRule);
                fi.SetAccessControl(acl);
                Logger.WriteLine("Info", _log_title, $"Successfully granted access rule to {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", _log_title, $"Failed to grant access rule to {_log_target}.");
                Logger.WriteRaw(_log_title, e.ToString());
            }
            return false;
        }

        public bool Revoke(string account)
        {
            Logger.WriteLine("Info", _log_title, $"Revoking access rules from {_log_target}. '{this.Path}': Account='{account}'");
            try
            {
                var fi = new FileInfo(this.Path);
                var acl = fi.GetAccessControl();
                bool isChange = false;
                foreach (FileSystemAccessRule rule in acl.GetAccessRules(true, true, typeof(NTAccount)))
                {
                    if (rule.IdentityReference.Value.Equals(account, StringComparison.OrdinalIgnoreCase))
                    {
                        acl.RemoveAccessRule(rule);
                        isChange = true;
                    }
                }
                if (isChange) fi.SetAccessControl(acl);
                Logger.WriteLine("Info", _log_title, $"Successfully revoked access rules from {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", _log_title, $"Failed to revoke access rules from {_log_target}.");
                Logger.WriteRaw(_log_title, e.ToString());
            }
            return false;
        }

        public bool RevokeAll()
        {
            Logger.WriteLine("Info", _log_title, $"Revoking all access rules from {_log_target}. '{this.Path}'");
            try
            {
                var fi = new FileInfo(this.Path);
                var acl = fi.GetAccessControl();
                bool isChange = false;
                foreach (FileSystemAccessRule rule in acl.GetAccessRules(true, true, typeof(NTAccount)))
                {
                    acl.RemoveAccessRule(rule);
                    isChange = true;
                }
                if (isChange) fi.SetAccessControl(acl);
                Logger.WriteLine("Info", _log_title, $"Successfully revoked all access rules from {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", _log_title, $"Failed to revoke all access rules from {_log_target}.");
                Logger.WriteRaw(_log_title, e.ToString());
            }
            return false;
        }

        /// <summary>
        /// Change owner of the file.
        /// </summary>
        /// <param name="newOwner"></param>
        /// <returns></returns>
        public bool ChangeOwner(string newOwner)
        {
            if (string.IsNullOrEmpty(newOwner))
            {
                Logger.WriteLine("Warning", _log_title, $"Skip change owner to {_log_target}.");
                return false;
            }
            Logger.WriteLine("Info", _log_title, $"Changing owner of {_log_target}. '{this.Path}' to '{newOwner}'");

            try
            {
                Logger.WriteLine("Info", _log_title, "Adjusting token privilegs (SeTakeOwnershipPrivilege, SeRestorePrivilege, SeBackupPrivilege)");
                ProcessPrivilege.AdjustToken(Privilege.SeTakeOwnershipPrivilege);
                ProcessPrivilege.AdjustToken(Privilege.SeRestorePrivilege);
                ProcessPrivilege.AdjustToken(Privilege.SeBackupPrivilege);

                var fi = new FileInfo(this.Path);
                var acl = fi.GetAccessControl();
                acl.SetOwner(new NTAccount(newOwner));
                fi.SetAccessControl(acl);
                Logger.WriteLine("Info", _log_title, $"Successfully changed owner of {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", _log_title, $"Failed to change owner of {_log_target}.");
                Logger.WriteRaw(_log_title, e.ToString());
            }
            return false;
        }

        /// <summary>
        /// Change access rule inheritance of the file.
        /// </summary>
        /// <param name="isInherited"></param>
        /// <param name="preserve"></param>
        /// <returns></returns>
        public bool ChangeInherited(bool? isInherited, bool preserve = true)
        {
            if (isInherited == null)
            {
                Logger.WriteLine("Warning", _log_title, $"Skip change inherited to {_log_target}.");
                return false;
            }
            Logger.WriteLine("Info", _log_title, $"Changing inheritance of {_log_target}. '{this.Path}' to '{isInherited}', preserve existing rules: {preserve}.");

            try
            {
                var fi = new FileInfo(this.Path);
                var acl = fi.GetAccessControl();
                acl.SetAccessRuleProtection(!(bool)isInherited, preserve);
                fi.SetAccessControl(acl);
                Logger.WriteLine("Info", _log_title, $"Successfully changed inheritance of {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", _log_title, $"Failed to change inheritance of {_log_target}.");
                Logger.WriteRaw(_log_title, e.ToString());
            }
            return false;
        }

        /// <summary>
        /// Set file attributes.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public bool SetAttributes(string attributes)
        {
            if (string.IsNullOrEmpty(attributes))
            {
                Logger.WriteLine("Warning", _log_title, $"Skip set attributes to {_log_target}.");
                return false;
            }
            Logger.WriteLine("Info", _log_title, $"Setting attributes of file '{this.Path}' to '{attributes}'.");

            try
            {
                var fi = new FileInfo(this.Path);
                fi.Attributes = FileAttributesParser.MergeAttributes(attributes, fi.Attributes);
                this.Attributes = fi.Attributes.ToString();
                Logger.WriteLine("Info", _log_title, $"Successfully set attributes of file.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", _log_title, $"Failed to set attributes of file.");
                Logger.WriteRaw(_log_title, e.ToString());
            }
            return false;
        }
    }
}
