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

        public string Type { get { return "RegistryKey"; } }
        public string Path { get; set; }
        public string Name { get; set; }
        public AccessRuleSet AccessRule { get; set; }

        #endregion

        const string _log_target = "registry key";

        public RegistryKeyItem(string path)
        {
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            using (var regKey = RegistryHelper.GetRegistryKey(path))
            {
                if (regKey != null)
                {
                    this.AccessRule = new AccessRuleSet(regKey.GetAccessControl());
                }
            }
        }

        /// <summary>
        /// Create new registry key.
        /// </summary>
        /// <param name="newPath"></param>
        /// <returns></returns>
        public static bool New(string newPath)
        {
            Logger.WriteLine("Info", $"Creating new {_log_target}. '{newPath}'");
            using (var regKey = RegistryHelper.GetRegistryKey(newPath, true, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        Logger.WriteLine("Info", $"Successfully created new {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to create new {_log_target}.");
                    Logger.WriteRaw(e.ToString());
                }
            }
            return false;
        }

        /// <summary>
        /// Create new registry key. (Alias of New)
        /// </summary>
        /// <param name="newPath"></param>
        /// <returns></returns>
        public static bool Add(string newPath)
        {
            return New(newPath);
        }

        public static bool Load(string keyName, string hiveFile)
        {
            try
            {
                //ItemStorageManager.Functions.RegistryHive.Load(keyName, hiveFile);
                RegistryHelper.Load(keyName, hiveFile);
            }
            catch { }
            return false;
        }

        public static bool Unload(string keyName)
        {
            try
            {
                //ItemStorageManager.Functions.RegistryHive.Unload(keyName);
                RegistryHelper.Unload(keyName);
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Exists registry key.
        /// </summary>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            Logger.WriteLine("Info", $"Checking existence of {_log_target} at path '{path}'.");
            using (var regKey = RegistryHelper.GetRegistryKey(path))
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
                    Logger.WriteLine("Error", $"Failed to check existence of {_log_target}.");
                    Logger.WriteRaw(e.ToString());
                }
            }
            return false;
        }

        /// <summary>
        /// Copy registry key.
        /// </summary>
        /// <param name="dstPath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public bool Copy(string dstPath, bool overwrite)
        {
            Logger.WriteLine("Info", $"Copying {_log_target}. From '{this.Path}' to '{dstPath}', overwrite: {overwrite}.");
            using (var srcKey = RegistryHelper.GetRegistryKey(this.Path))
            using (var dstKey = RegistryHelper.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    CopyRegistryKey(srcKey, dstKey);
                    Logger.WriteLine("Info", $"Successfully copied {_log_target}.");
                    return true;
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to copy {_log_target}.");
                    Logger.WriteRaw(e.ToString());
                }
            }
            return false;
        }

        /// <summary>
        /// Copy registry key recursive function.
        /// </summary>
        /// <param name="srcKey"></param>
        /// <param name="dstKey"></param>
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

        /// <summary>
        /// Remove registry key.
        /// </summary>
        /// <returns></returns>
        public bool Remove()
        {
            Logger.WriteLine("Info", $"Removing {_log_target}. '{this.Path}'");
            using (var regKey = RegistryHelper.GetRegistryKey(System.IO.Path.GetDirectoryName(this.Path), false, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        regKey.DeleteSubKeyTree(this.Name);
                        Logger.WriteLine("Info", $"Successfully removed {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to remove {_log_target}.");
                    Logger.WriteRaw(e.ToString());
                }
            }
            return false;
        }

        /// <summary>
        /// Remove registry key. (Alias of Remove)
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            return Remove();
        }

        public bool Move(string dstPath)
        {
            Logger.WriteLine("Info", $"Moving {_log_target}. From '{this.Path}' to '{dstPath}'.");
            using (var srcKey = RegistryHelper.GetRegistryKey(this.Path))
            using (var dstKey = RegistryHelper.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    CopyRegistryKey(srcKey, dstKey);
                    Logger.WriteLine("Info", $"Successfully moved (copy before move) {_log_target}.");
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to move (copy before move) {_log_target}.");
                    Logger.WriteRaw(e.ToString());
                }
            }
            using (var parentKey = RegistryHelper.GetRegistryKey(System.IO.Path.GetDirectoryName(this.Path), false, true))
            {
                try
                {
                    if (parentKey != null)
                    {
                        parentKey.DeleteSubKeyTree(this.Name);
                        Logger.WriteLine("Info", $"Successfully moved {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to move {_log_target}.");
                    Logger.WriteRaw(e.ToString());
                }
            }
            return false;
        }

        public bool Rename(string newName)
        {
            Logger.WriteLine("Info", $"Renaming {_log_target}. Key {this.Path}. From '{this.Name}' to '{newName}'.");
            string dstPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Path), newName);
            using (var srcKey = RegistryHelper.GetRegistryKey(this.Path))
            using (var dstKey = RegistryHelper.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    CopyRegistryKey(srcKey, dstKey);
                    Logger.WriteLine("Info", $"Successfully renamed (copy before rename) {_log_target}.");
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to rename (copy before rename) {_log_target}.");
                    Logger.WriteRaw(e.ToString());
                }
            }
            using (var parentKey = RegistryHelper.GetRegistryKey(System.IO.Path.GetDirectoryName(this.Path), false, true))
            {
                try
                {
                    if (parentKey != null)
                    {
                        parentKey.DeleteSubKeyTree(this.Name);
                        Logger.WriteLine("Info", $"Successfully renamed {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to rename {_log_target}.");
                    Logger.WriteRaw(e.ToString());
                }
            }
            return false;
        }

        public bool Grant(string account, string rights, string accessType, string inheritance, string propagation)
        {
            Logger.WriteLine("Info", $"Granting access rule to {_log_target}. '{this.Path}': Account='{account}', Rights='{rights}', AccessType='{accessType}', Inheritance='{inheritance}', Propagation='{propagation}'");
            using (var regKey = RegistryHelper.GetRegistryKey(this.Path, false, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        var newRule = new AccessRuleSummary(account, rights, accessType, inheritance, propagation).ToAccessRuleForRegistryKey();
                        var acl = regKey.GetAccessControl();
                        acl.AddAccessRule(newRule);
                        regKey.SetAccessControl(acl);
                        Logger.WriteLine("Info", $"Successfully granted access rule to {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to grant access rule to {_log_target}.");
                    Logger.WriteRaw(e.ToString());
                }
            }
            return false;
        }

        public bool Grant(string accessRuleText)
        {
            Logger.WriteLine("Info", $"Granting access rule to {_log_target}. '{this.Path}': AccessRule='{accessRuleText}'");
            using (var regKey = RegistryHelper.GetRegistryKey(this.Path, false, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        var newRule = new AccessRuleSummary(accessRuleText).ToAccessRuleForRegistryKey();
                        var acl = regKey.GetAccessControl();
                        acl.AddAccessRule(newRule);
                        regKey.SetAccessControl(acl);
                        Logger.WriteLine("Info", $"Successfully granted access rule to {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to grant access rule to {_log_target}.");
                    Logger.WriteRaw(e.ToString());
                }
            }
            return false;
        }

        public bool Revoke(string account)
        {
            Logger.WriteLine("Info", $"Revoking access rules from {_log_target}. '{this.Path}': Account='{account}'");
            using (var regKey = RegistryHelper.GetRegistryKey(this.Path, false, true))
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
                        Logger.WriteLine("Info", $"Successfully revoked access rules from {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to revoke access rules from {_log_target}.");
                    Logger.WriteRaw(e.ToString());
                }
            }
            return false;
        }

        public bool RevokeAll()
        {
            using (var regKey = RegistryHelper.GetRegistryKey(this.Path, false, true))
            {
                Logger.WriteLine("Info", $"Revoking all access rules from {_log_target}. '{this.Path}'");
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
                        Logger.WriteLine("Info", $"Successfully revoked all access rules from {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to revoke all access rules from {_log_target}.");
                    Logger.WriteRaw(e.ToString());
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
            if (string.IsNullOrEmpty(newOwner))
            {
                Logger.WriteLine("Warning", $"Skip change owner to {_log_target}.");
                return false;
            }
            Logger.WriteLine("Info", $"Changing owner of {_log_target}. '{this.Path}' to '{newOwner}'");
            
            using (var regKey = RegistryHelper.GetRegistryKey(this.Path, false, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        Logger.WriteLine("Info", "Adjusting token privilegs (SeTakeOwnershipPrivilege, SeRestorePrivilege, SeBackupPrivilege)");
                        ProcessPrivilege.AdjustToken(Privilege.SeTakeOwnershipPrivilege);
                        ProcessPrivilege.AdjustToken(Privilege.SeRestorePrivilege);
                        ProcessPrivilege.AdjustToken(Privilege.SeBackupPrivilege);

                        var acl = regKey.GetAccessControl();
                        acl.SetOwner(new NTAccount(newOwner));
                        regKey.SetAccessControl(acl);
                        Logger.WriteLine("Info", $"Successfully changed owner of {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to change owner of {_log_target}.");
                    Logger.WriteRaw(e.ToString());
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
        public bool ChangeInherited(bool? isInherited, bool preserve = true)
        {
            if (isInherited == null)
            {
                Logger.WriteLine("Warning", $"Skip change inherited to {_log_target}.");
                return false;
            }
            Logger.WriteLine("Info", $"Changing inheritance of {_log_target}. '{this.Path}' to '{isInherited}', preserve existing rules: {preserve}.");
            
            using (var regKey = RegistryHelper.GetRegistryKey(this.Path, false, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        var acl = regKey.GetAccessControl();
                        acl.SetAccessRuleProtection(!(bool)isInherited, preserve);
                        regKey.SetAccessControl(acl);
                        Logger.WriteLine("Info", $"Successfully changed inheritance of {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to change inheritance of {_log_target}.");
                    Logger.WriteRaw(e.ToString());
                }
            }
            return false;
        }
    }
}
