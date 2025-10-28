using Microsoft.VisualBasic.FileIO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ItemStorageManager.ItemStorage
{
    internal class DirectoryItem : IBaseItem
    {
        public ItemType Type { get { return ItemType.Directory; } }

        public string Path { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public string Attributes { get; set; }
        public AccessRuleSet AccessRule { get; set; }

        public DirectoryItem(string path)
        {
            var di = new DirectoryInfo(path);

            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            this.CreationTime = di.CreationTime;
            this.LastWriteTime = di.LastWriteTime;
            this.LastAccessTime = di.LastAccessTime;
            this.Attributes = di.Attributes.ToString();
            this.AccessRule = new AccessRuleSet(di.GetAccessControl());
        }

        public long GetChildDirectoryCount()
        {
            try
            {
                var di = new DirectoryInfo(this.Path);
                return di.GetDirectories().LongLength;
            }
            catch { }
            return -1;
        }

        public long GetChildFileCount()
        {
            try
            {
                var di = new DirectoryInfo(this.Path);
                return di.GetFiles().LongLength;
            }
            catch { }
            return -1;
        }

        public long GetTotalFileSize()
        {
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
            catch { }
            return -1;
        }

        public static bool New(string newPath)
        {
            Logger.WriteLine("Info", $"Creating new directory at path '{newPath}'.");
            try
            {
                Directory.CreateDirectory(newPath);
                Logger.WriteLine("Info", $"Successfully created new directory.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to create new directory. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }

        public static bool Add(string newPath)
        {
            return New(newPath);
        }

        public bool Exists()
        {
            return Directory.Exists(this.Path);
        }

        public bool Copy(string dstPath, bool overwrite)
        {
            try
            {
                FileSystem.CopyDirectory(this.Path, dstPath, overwrite);
                return true;
            }
            catch { }
            return false;
        }

        public bool Delete()
        {
            try
            {
                Directory.Delete(this.Path, true);
                return true;
            }
            catch { }
            return false;
        }

        public bool Remove()
        {
            return Delete();
        }

        public bool Move(string dstPath)
        {
            try
            {
                Directory.Move(this.Path, dstPath);
                return true;
            }
            catch { }
            return false;
        }

        public bool Rename(string newName)
        {
            try
            {
                var parentDir = System.IO.Path.GetDirectoryName(this.Path);
                var newPath = System.IO.Path.Combine(parentDir, newName);
                Directory.Move(this.Path, newPath);
                this.Path = newPath;
                this.Name = newName;
                return true;
            }
            catch { }
            return false;
        }

        public bool Grant(string account, string rights, string accessType, string inheritance, string propagation)
        {
            Logger.WriteLine("Info", $"Granting access rule to directory '{this.Path}': Account='{account}', Rights='{rights}', AccessType='{accessType}', Inheritance='{inheritance}', Propagation='{propagation}'.");
            try
            {
                var newRule = new AccessRuleSummary(account, rights, accessType, inheritance, propagation).ToAccessRuleForDirectory();
                var di = new DirectoryInfo(this.Path);
                var acl = di.GetAccessControl();
                acl.AddAccessRule(newRule);
                di.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully granted access rule to directory.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to grant access rule to directory. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }

        public bool Grant(string accessRuleText)
        {
            Logger.WriteLine("Info", $"Granting access rule to directory '{this.Path}': '{accessRuleText}'.");
            try
            {
                var newRule = new AccessRuleSummary(accessRuleText).ToAccessRuleForDirectory();
                var di = new DirectoryInfo(this.Path);
                var acl = di.GetAccessControl();
                acl.AddAccessRule(newRule);
                di.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully granted access rule to directory.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to grant access rule to directory. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }

        public bool Revoke(string account)
        {
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
                return true;
            }
            catch { }
            return false;
        }

        public bool RevokeAll()
        {
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
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Change owner of the directory.
        /// </summary>
        /// <param name="newOwner"></param>
        /// <returns></returns>
        public bool ChangeOwner(string newOwner)
        {
            Logger.WriteLine("Info", $"Changing owner of directory '{this.Path}' to '{newOwner}'.");
            try
            {
                var di = new DirectoryInfo(this.Path);
                var acl = di.GetAccessControl();
                acl.SetOwner(new NTAccount(newOwner));
                di.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully changed owner of directory.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to change owner of directory. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
            }
            return false;
        }

        public bool ChangeInherited(bool isInherited, bool preserve = true)
        {
            Logger.WriteLine("Info", $"Changing inheritance of directory '{this.Path}' to '{isInherited}', preserve existing rules: {preserve}.");
            try
            {
                var di = new DirectoryInfo(this.Path);
                var acl = di.GetAccessControl();
                acl.SetAccessRuleProtection(!isInherited, preserve);
                di.SetAccessControl(acl);
                Logger.WriteLine("Info", $"Successfully changed inheritance of directory.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error", $"Failed to change inheritance of directory. Exception: {e.ToString()}");
                Logger.WriteRaw(e.Message);
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
            Logger.WriteLine("Info", $"Setting attributes of directory '{this.Path}' to '{attributes}'.");
            try
            {
                var di = new DirectoryInfo(this.Path);
                di.Attributes = AttributeFunctions.ParseFileAttributes(attributes, di.Attributes);
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
