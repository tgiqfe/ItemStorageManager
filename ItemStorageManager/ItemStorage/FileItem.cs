using ItemStorageManager.Functions;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ItemStorageManager.ItemStorage
{
    internal class FileItem : IBaseItem
    {
        #region Public parameter

        public ItemType Type { get { return ItemType.File; } }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string FormatedSize { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public string Attributes { get; set; }
        public AccessRuleSet AccessRule { get; set; }
        public bool SecurityBlock { get; set; }

        #endregion

        const string _log_target = "file";

        public FileItem(string path)
        {
            var fi = new FileInfo(path);

            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            this.Size = string.Format("{0:N0} Byte", fi.Length);
            this.FormatedSize = TextFunctions.FormatFileSize(fi.Length);
            this.CreationTime = fi.CreationTime;
            this.LastWriteTime = fi.LastWriteTime;
            this.LastAccessTime = fi.LastAccessTime;
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
            Logger.WriteLine("Info", $"Creating new {_log_target}. '{newPath}'");
            try
            {
                var parent = System.IO.Path.GetDirectoryName(newPath);
                if (!Directory.Exists(parent))
                {
                    Directory.CreateDirectory(parent);
                }
                File.CreateText(newPath).Close();
                Logger.WriteLine("Info", $"Successfully created new {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to create new {_log_target}. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
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
            Logger.WriteLine("Info", $"Removing security block from {_log_target}. '{this.Path}'");
            try
            {
                var adsPath = $"{this.Path}:Zone.Identifier";
                if (File.Exists(adsPath))
                {
                    File.Delete(adsPath);
                }
                Logger.WriteLine("Info", $"Successfully removed security block from {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to remove security block from {_log_target}. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }

        /// <summary>
        /// Exists check file.
        /// </summary>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            Logger.WriteLine("Info", $"Checking existence of {_log_target} at path '{path}'.");
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
            Logger.WriteLine("Info", $"Copying {_log_target}. From '{this.Path}' to '{dstPath}', overwrite: {overwrite}.");
            try
            {
                File.Copy(this.Path, dstPath, overwrite);
                Logger.WriteLine("Info", $"Successfully copied {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to copy {_log_target}. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }

        /// <summary>
        /// Remove file.
        /// </summary>
        /// <returns></returns>
        public bool Remove()
        {
            Logger.WriteLine("Info", $"Removing {_log_target}. '{this.Path}'");
            try
            {
                File.Delete(this.Path);
                Logger.WriteLine("Info", $"Successfully removed {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to remove {_log_target}. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
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
            Logger.WriteLine("Info", $"Moving {_log_target}. From '{this.Path}' to '{dstPath}'.");
            try
            {
                File.Move(this.Path, dstPath);
                Logger.WriteLine("Info", $"Successfully moved {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to move {_log_target}. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }

        public bool Rename(string newName)
        {
            Logger.WriteLine("Info", $"Renaming {_log_target}. From '{this.Name}' to '{newName}'.");
            try
            {
                var dstPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Path), newName);
                File.Move(this.Path, dstPath);
                Logger.WriteLine("Info", $"Successfully renamed {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to rename {_log_target}. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }

        public bool Grant(string account, string rights, string accessType, string inheritance = null, string propagation = null)
        {
            Logger.WriteLine("Info", $"Granting access rule to {_log_target}. '{this.Path}': Account='{account}', Rights='{rights}', AccessType='{accessType}', Inheritance='{inheritance}', Propagation='{propagation}'");
            try
            {
                var newRule = new AccessRuleSummary(account, rights, accessType, inheritance, propagation).ToAccessRuleForFile();
                var fi = new FileInfo(this.Path);
                var acl = fi.GetAccessControl();
                acl.AddAccessRule(newRule);
                fi.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully granted access rule to {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to grant access rule to {_log_target}. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }

        public bool Grant(string accessRuleText)
        {
            Logger.WriteLine("Info", $"Granting access rule to {_log_target}. '{this.Path}': AccessRule='{accessRuleText}'");
            try
            {
                var newRule = new AccessRuleSummary(accessRuleText).ToAccessRuleForFile();
                var fi = new FileInfo(this.Path);
                var acl = fi.GetAccessControl();
                acl.AddAccessRule(newRule);
                fi.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully granted access rule to {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to grant access rule to {_log_target}. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }

        public bool Revoke(string account)
        {
            Logger.WriteLine("Info", $"Revoking access rules from {_log_target}. '{this.Path}': Account='{account}'");
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
                Logger.WriteLine("Info", $"Successfully revoked access rules from {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to revoke access rules from {_log_target}. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }

        public bool RevokeAll()
        {
            Logger.WriteLine("Info", $"Revoking all access rules from {_log_target}. '{this.Path}'");
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
                Logger.WriteLine("Info", $"Successfully revoked all access rules from {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to revoke all access rules from {_log_target}. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
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
            if(string.IsNullOrEmpty(newOwner))
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

                var fi = new FileInfo(this.Path);
                var acl = fi.GetAccessControl();
                acl.SetOwner(new NTAccount(newOwner));
                fi.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully changed owner of {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to change owner of {_log_target}. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
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
                Logger.WriteLine("Warning", $"Skip change inherited to {_log_target}.");
                return false;
            }
            Logger.WriteLine("Info", $"Changing inheritance of {_log_target}. '{this.Path}' to '{isInherited}', preserve existing rules: {preserve}.");
            
            try
            {
                var fi = new FileInfo(this.Path);
                var acl = fi.GetAccessControl();
                acl.SetAccessRuleProtection(!(bool)isInherited, preserve);
                fi.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully changed inheritance of {_log_target}.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to change inheritance of {_log_target}. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
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
                Logger.WriteLine("Warning", $"Skip set attributes to {_log_target}.");
                return false;
            }
            Logger.WriteLine("Info", $"Setting attributes of file '{this.Path}' to '{attributes}'.");
            
            try
            {
                var fi = new FileInfo(this.Path);
                fi.Attributes = AttributeMapping.ParseFileAttributes(attributes, fi.Attributes);
                this.Attributes = fi.Attributes.ToString();
                Logger.WriteLine("Info", $"Successfully set attributes of file.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to set attributes of file. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }
    }
}
