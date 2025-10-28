using ItemStorageManager.Functions;
using Microsoft.Win32;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ItemStorageManager.ItemStorage
{
    internal class RegistryKeyItem : IBaseItem
    {
        #region Public parameter 

        public ItemType Type { get { return ItemType.RegistryKey; } }
        public string Path { get; set; }
        public string Name { get; set; }
        public AccessRuleSet AccessRule { get; set; }

        #endregion

        const string _log_TargetItem = "registry key";

        public RegistryKeyItem(string path)
        {
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            using (var regKey = RegistryFunctions.GetRegistryKey(path))
            {
                if (regKey != null)
                {
                    this.AccessRule = new AccessRuleSet(regKey.GetAccessControl());
                }
            }
        }

        public static bool New(string newPath)
        {
            Logger.WriteLine("Info", $"Creating new {_log_TargetItem}. '{newPath}'");
            using (var regKey = RegistryFunctions.GetRegistryKey(newPath, true, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        Logger.WriteLine("Info", $"Successfully created new {_log_TargetItem}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to create new {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        public static bool Add(string newPath)
        {
            return New(newPath);
        }

        public static bool Load(string keyName, string hiveFile)
        {
            try
            {
                ItemStorageManager.Functions.RegistryHive.Load(keyName, hiveFile);
            }
            catch { }
            return false;
        }

        public static bool Unload(string keyName)
        {
            try
            {
                ItemStorageManager.Functions.RegistryHive.Unload(keyName);
            }
            catch { }
            return false;
        }

        public bool Exists()
        {
            Logger.WriteLine("Info", $"Checking existence of {_log_TargetItem} at path '{this.Path}'.");
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path))
            {
                try
                {
                    if (regKey != null)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to check existence of {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        public bool Copy(string dstPath, bool overwrite)
        {
            Logger.WriteLine("Info", $"Copying {_log_TargetItem}. From '{this.Path}' to '{dstPath}', overwrite: {overwrite}.");
            using (var srcKey = RegistryFunctions.GetRegistryKey(this.Path))
            using (var dstKey = RegistryFunctions.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    CopyRegistryKey(srcKey, dstKey);
                    Logger.WriteLine("Info", $"Successfully copied {_log_TargetItem}.");
                    return true;
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to copy {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        private void CopyRegistryKey(RegistryKey srcKey, RegistryKey dstKey)
        {
            foreach (var valueName in srcKey.GetValueNames())
            {
                var valueKind = srcKey.GetValueKind(valueName);
                dstKey.SetValue(
                    valueName,
                    valueKind == RegistryValueKind.ExpandString ?
                        srcKey.GetValue(valueName, null, RegistryValueOptions.DoNotExpandEnvironmentNames) :
                        srcKey.GetValue(valueName),
                    valueKind);
            }
            foreach (var subKeyName in srcKey.GetSubKeyNames())
            {
                using (var srcSubKey = srcKey.OpenSubKey(subKeyName))
                using (var dstSubKey = dstKey.CreateSubKey(subKeyName, true))
                {
                    try
                    {
                        CopyRegistryKey(srcSubKey, dstSubKey);
                    }
                    catch (System.Security.SecurityException)
                    {
                        Console.WriteLine("Access Denied: SecurityException " + srcSubKey.Name);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.WriteLine("Access Denied: UnauthorizedAccessException" + srcSubKey.Name);
                    }
                    catch (ArgumentException)
                    {
                        using (var pro = new Process())
                        {
                            pro.StartInfo.FileName = "reg.exe";
                            pro.StartInfo.Arguments = $@"copy ""{srcSubKey.ToString()}"" ""{dstSubKey.ToString()}"" /s /f";
                            pro.StartInfo.UseShellExecute = false;
                            pro.StartInfo.CreateNoWindow = true;
                            pro.Start();
                            pro.WaitForExit();
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Unknown: Exception " + srcSubKey.Name);
                    }
                    CopyRegistryKey(srcSubKey, dstSubKey);
                }
            }
        }

        public bool Delete()
        {
            Logger.WriteLine("Info", $"Deleting {_log_TargetItem}. '{this.Path}'");
            using (var regKey = RegistryFunctions.GetRegistryKey(System.IO.Path.GetDirectoryName(this.Path), false, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        regKey.DeleteSubKeyTree(this.Name);
                        Logger.WriteLine("Info", $"Successfully deleted {_log_TargetItem}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to delete {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        public bool Remove()
        {
            return Delete();
        }

        public bool Move(string dstPath)
        {
            Logger.WriteLine("Info", $"Moving {_log_TargetItem}. From '{this.Path}' to '{dstPath}'.");
            using (var srcKey = RegistryFunctions.GetRegistryKey(this.Path))
            using (var dstKey = RegistryFunctions.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    CopyRegistryKey(srcKey, dstKey);
                    Logger.WriteLine("Info", $"Successfully moved (copy before move) {_log_TargetItem}.");
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to move (copy before move) {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            using (var parentKey = RegistryFunctions.GetRegistryKey(System.IO.Path.GetDirectoryName(this.Path), false, true))
            {
                try
                {
                    if (parentKey != null)
                    {
                        parentKey.DeleteSubKeyTree(this.Name);
                        Logger.WriteLine("Info", $"Successfully moved {_log_TargetItem}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to move {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        public bool Rename(string newName)
        {
            Logger.WriteLine("Info", $"Renaming {_log_TargetItem}. Key {this.Path}. From '{this.Name}' to '{newName}'.");
            string dstPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Path), newName);
            using (var srcKey = RegistryFunctions.GetRegistryKey(this.Path))
            using (var dstKey = RegistryFunctions.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    CopyRegistryKey(srcKey, dstKey);
                    Logger.WriteLine("Info", $"Successfully renamed (copy before rename) {_log_TargetItem}.");
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to rename (copy before rename) {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            using (var parentKey = RegistryFunctions.GetRegistryKey(System.IO.Path.GetDirectoryName(this.Path), false, true))
            {
                try
                {
                    if (parentKey != null)
                    {
                        parentKey.DeleteSubKeyTree(this.Name);
                        Logger.WriteLine("Info", $"Successfully renamed {_log_TargetItem}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to rename {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        public bool Grant(string account, string rights, string accessType, string inheritance, string propagation)
        {
            Logger.WriteLine("Info", $"Granting access rule to {_log_TargetItem}. '{this.Path}': Account='{account}', Rights='{rights}', AccessType='{accessType}', Inheritance='{inheritance}', Propagation='{propagation}'");
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path, false, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        var newRule = new AccessRuleSummary(account, rights, accessType, inheritance, propagation).ToAccessRuleForRegistryKey();
                        var acl = regKey.GetAccessControl();
                        acl.AddAccessRule(newRule);
                        regKey.SetAccessControl(acl);
                        Logger.WriteLine("Info", $"Successfully granted access rule to {_log_TargetItem}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to grant access rule to {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        public bool Grant(string accessRuleText)
        {
            Logger.WriteLine("Info", $"Granting access rule to {_log_TargetItem}. '{this.Path}': AccessRule='{accessRuleText}'");
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path, false, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        var newRule = new AccessRuleSummary(accessRuleText).ToAccessRuleForRegistryKey();
                        var acl = regKey.GetAccessControl();
                        acl.AddAccessRule(newRule);
                        regKey.SetAccessControl(acl);
                        Logger.WriteLine("Info", $"Successfully granted access rule to {_log_TargetItem}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to grant access rule to {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        public bool Revoke(string account)
        {
            Logger.WriteLine("Info", $"Revoking access rules from {_log_TargetItem}. '{this.Path}': Account='{account}'");
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path, false, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        var acl = regKey.GetAccessControl();
                        bool isChange = false;
                        foreach (RegistryAccessRule rule in acl.GetAccessRules(true, false, typeof(System.Security.Principal.NTAccount)))
                        {
                            if (string.Equals(rule.IdentityReference.Value, account, StringComparison.OrdinalIgnoreCase))
                            {
                                acl.RemoveAccessRule(rule);
                                isChange = true;
                            }
                        }
                        if (isChange) regKey.SetAccessControl(acl);
                        Logger.WriteLine("Info", $"Successfully revoked access rules from {_log_TargetItem}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to revoke access rules from {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        public bool RevokeAll()
        {
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path, false, true))
            {
                Logger.WriteLine("Info", $"Revoking all access rules from {_log_TargetItem}. '{this.Path}'");
                try
                {
                    if (regKey != null)
                    {
                        var acl = regKey.GetAccessControl();
                        bool isChange = false;
                        foreach (RegistryAccessRule rule in acl.GetAccessRules(true, false, typeof(System.Security.Principal.NTAccount)))
                        {
                            acl.RemoveAccessRule(rule);
                            isChange = true;
                        }
                        if (isChange) regKey.SetAccessControl(acl);
                        Logger.WriteLine("Info", $"Successfully revoked all access rules from {_log_TargetItem}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to revoke all access rules from {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        /// <summary>
        /// Change owner of the registry key.
        /// </summary>
        /// <param name="newOwner"></param>
        /// <returns></returns>
        public bool ChangeOwner(string newOwner)
        {
            Logger.WriteLine("Info", $"Changing owner of {_log_TargetItem}. '{this.Path}' to '{newOwner}'");
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path, false, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        var acl = regKey.GetAccessControl();
                        acl.SetOwner(new NTAccount(newOwner));
                        regKey.SetAccessControl(acl);
                        Logger.WriteLine("Info", $"Successfully changed owner of {_log_TargetItem}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to change owner of {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        /// <summary>
        /// Change access rule inheritance of the registry key.
        /// </summary>
        /// <param name="isInherited"></param>
        /// <param name="preserve"></param>
        /// <returns></returns>
        public bool ChangeInherited(bool isInherited, bool preserve = true)
        {
            Logger.WriteLine("Info", $"Changing inheritance of {_log_TargetItem}. '{this.Path}' to '{isInherited}', preserve existing rules: {preserve}.");
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path, false, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        var acl = regKey.GetAccessControl();
                        acl.SetAccessRuleProtection(!isInherited, preserve);
                        regKey.SetAccessControl(acl);
                        Logger.WriteLine("Info", $"Successfully changed inheritance of {_log_TargetItem}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to change inheritance of {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }
    }
}
