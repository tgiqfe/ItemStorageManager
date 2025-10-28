using ItemStorageManager.Functions;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ItemStorageManager.ItemStorage
{
    internal class FileItem : IBaseItem
    {
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

        public static bool New(string newPath)
        {
            Logger.WriteLine("Info", $"Creating new file at path '{newPath}'.");
            try
            {   
                var parent = System.IO.Path.GetDirectoryName(newPath);
                if (!Directory.Exists(parent))
                {
                    Directory.CreateDirectory(parent);
                }
                File.CreateText(newPath).Close();
                Logger.WriteLine("Info", $"Successfully created new file.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to create new file. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }

        public static bool New(string newParentPath, string newPath)
        {
            return New(System.IO.Path.Combine(newParentPath, newPath));
        }

        public static bool Add(string newPAth)
        {
            return New(newPAth);
        }

        public static bool Add(string newParentPath, string newPath)
        {
            return New(System.IO.Path.Combine(newParentPath, newPath));
        }

        public bool RemoveSecurityBlock()
        {
            try
            {
                var adsPath = $"{this.Path}:Zone.Identifier";
                if (File.Exists(adsPath))
                {
                    File.Delete(adsPath);
                }
                return true;
            }
            catch { }
            return false;
        }

        public bool Exists()
        {
            return File.Exists(this.Path);
        }

        public bool Copy(string dstPath, bool overwrite)
        {
            try
            {
                File.Copy(this.Path, dstPath, overwrite);
                return true;
            }
            catch { }
            return false;
        }

        public bool Delete()
        {
            try
            {
                File.Delete(this.Path);
                return true;
            }
            catch { }
            return false;
        }

        public bool Remove()
        {
            return this.Delete();
        }

        public bool Move(string dstPath)
        {
            try
            {
                File.Move(this.Path, dstPath);
                return true;
            }
            catch { }
            return false;
        }

        public bool Rename(string newName)
        {
            try
            {
                var dstPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Path), newName);
                File.Move(this.Path, dstPath);
                return true;
            }
            catch { }
            return false;
        }

        public bool Grant(string account, string rights, string accessType, string inheritance = null, string propagation = null)
        {
            Logger.WriteLine("Info", $"Granting access rule to file '{this.Path}': Account='{account}', Rights='{rights}', AccessType='{accessType}', Inheritance='{inheritance}', Propagation='{propagation}'.");
            try
            {
                var newRule = new AccessRuleSummary(account, rights, accessType, inheritance, propagation).ToAccessRuleForFile();
                var fi = new FileInfo(this.Path);
                var acl = fi.GetAccessControl();
                acl.AddAccessRule(newRule);
                fi.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully granted access rule to file.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to grant access rule to file. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }

        public bool Grant(string accessRuleText)
        {
            Logger.WriteLine("Info", $"Granting access rule '{accessRuleText}' to file '{this.Path}'.");
            try
            {
                var newRule = new AccessRuleSummary(accessRuleText).ToAccessRuleForFile();
                var fi = new FileInfo(this.Path);
                var acl = fi.GetAccessControl();
                acl.AddAccessRule(newRule);
                fi.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully granted access rule.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to grant access rule to file. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }

        public bool Revoke(string account)
        {
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
                return true;
            }
            catch { }
            return false;
        }

        public bool RevokeAll()
        {
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
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Change owner of the file.
        /// </summary>
        /// <param name="newOwner"></param>
        /// <returns></returns>
        public bool ChangeOwner(string newOwner)
        {
            Logger.WriteLine("Info", $"Changing owner of file '{this.Path}' to '{newOwner}'.");
            try
            {
                var fi = new FileInfo(this.Path);
                var acl = fi.GetAccessControl();
                acl.SetOwner(new NTAccount(newOwner));
                fi.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully changed owner of file.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to change owner of file. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }

        public bool ChangeInherited(bool isInherited, bool preserve = true)
        {
            Logger.WriteLine("Info", $"Changing inheritance of file '{this.Path}' to '{isInherited}', PreserveExistingRules='{preserve}'.");
            try
            {
                var fi = new FileInfo(this.Path);
                var acl = fi.GetAccessControl();
                acl.SetAccessRuleProtection(!isInherited, preserve);
                fi.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully changed inheritance of file.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to change inheritance of file. Exception: {e.ToString()}");
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
            Logger.WriteLine("Info", $"Setting attributes of file '{this.Path}' to '{attributes}'.");
            try
            {
                var fi = new FileInfo(this.Path);
                fi.Attributes = AttributeFunctions.ParseFileAttributes(attributes, fi.Attributes);
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
