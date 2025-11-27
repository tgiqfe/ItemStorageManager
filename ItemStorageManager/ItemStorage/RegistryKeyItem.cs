using ItemStorageManager.Functions;
using Microsoft.Win32;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ItemStorageManager.ItemStorage
{
    public class RegistryKeyItem : IBaseItem
    {
        #region Public parameter 

        public string Type { get { return "RegistryKey"; } }
        public string Path { get; set; }
        public string Name { get; set; }
        public AccessRuleSet AccessRule { get; set; }

        #endregion

        const string _log_title = "ItemStorage";
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
            Logger.WriteLine("Info", _log_title, $"Creating new {_log_target}. '{newPath}'");
            using (var regKey = RegistryHelper.GetRegistryKey(newPath, true, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        Logger.WriteLine("Info", _log_title, $"Successfully created new {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", _log_title, $"Failed to create new {_log_target}.");
                    Logger.WriteRaw(_log_title, e.ToString());
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
                RegistryHelper.Load(keyName, hiveFile);
            }
            catch { }
            return false;
        }

        public static bool Unload(string keyName)
        {
            try
            {
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
            Logger.WriteLine("Info", _log_title, $"Checking existence of {_log_target} at path '{path}'.");
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
                    Logger.WriteLine("Error", _log_title, $"Failed to check existence of {_log_target}.");
                    Logger.WriteRaw(_log_title, e.ToString());
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
            Logger.WriteLine("Info", _log_title, $"Copying {_log_target}. From '{this.Path}' to '{dstPath}', overwrite: {overwrite}.");
            using (var srcKey = RegistryHelper.GetRegistryKey(this.Path))
            using (var dstKey = RegistryHelper.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    CopyRegistryKey(srcKey, dstKey);
                    Logger.WriteLine("Info", _log_title, $"Successfully copied {_log_target}.");
                    return true;
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", _log_title, $"Failed to copy {_log_target}.");
                    Logger.WriteRaw(_log_title, e.ToString());
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
            Logger.WriteLine("Info", _log_title, $"Removing {_log_target}. '{this.Path}'");
            using (var regKey = RegistryHelper.GetRegistryKey(System.IO.Path.GetDirectoryName(this.Path), true, false))
            {
                try
                {
                    if (regKey != null)
                    {
                        regKey.DeleteSubKeyTree(this.Name);
                        Logger.WriteLine("Info", _log_title, $"Successfully removed {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", _log_title, $"Failed to remove {_log_target}.");
                    Logger.WriteRaw(_log_title, e.ToString());
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
            Logger.WriteLine("Info", _log_title, $"Moving {_log_target}. From '{this.Path}' to '{dstPath}'.");
            using (var srcKey = RegistryHelper.GetRegistryKey(this.Path))
            using (var dstKey = RegistryHelper.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    CopyRegistryKey(srcKey, dstKey);
                    Logger.WriteLine("Info", _log_title, $"Successfully moved (copy before move) {_log_target}.");
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", _log_title, $"Failed to move (copy before move) {_log_target}.");
                    Logger.WriteRaw(_log_title, e.ToString());
                }
            }
            using (var parentKey = RegistryHelper.GetRegistryKey(System.IO.Path.GetDirectoryName(this.Path), true, false))
            {
                try
                {
                    if (parentKey != null)
                    {
                        parentKey.DeleteSubKeyTree(this.Name);
                        Logger.WriteLine("Info", _log_title, $"Successfully moved {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", _log_title, $"Failed to move {_log_target}.");
                    Logger.WriteRaw(_log_title, e.ToString());
                }
            }
            return false;
        }

        public bool Rename(string newName)
        {
            Logger.WriteLine("Info", _log_title, $"Renaming {_log_target}. Key {this.Path}. From '{this.Name}' to '{newName}'.");
            string dstPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Path), newName);
            using (var srcKey = RegistryHelper.GetRegistryKey(this.Path))
            using (var dstKey = RegistryHelper.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    CopyRegistryKey(srcKey, dstKey);
                    Logger.WriteLine("Info", _log_title, $"Successfully renamed (copy before rename) {_log_target}.");
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", _log_title, $"Failed to rename (copy before rename) {_log_target}.");
                    Logger.WriteRaw(_log_title, e.ToString());
                }
            }
            using (var parentKey = RegistryHelper.GetRegistryKey(System.IO.Path.GetDirectoryName(this.Path), true, false))
            {
                try
                {
                    if (parentKey != null)
                    {
                        parentKey.DeleteSubKeyTree(this.Name);
                        Logger.WriteLine("Info", _log_title, $"Successfully renamed {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", _log_title, $"Failed to rename {_log_target}.");
                    Logger.WriteRaw(_log_title, e.ToString());
                }
            }
            return false;
        }

        public bool Grant(string account, string rights, string accessType, string inheritance, string propagation)
        {
            Logger.WriteLine("Info", _log_title, $"Granting access rule to {_log_target}. '{this.Path}': Account='{account}', Rights='{rights}', AccessType='{accessType}', Inheritance='{inheritance}', Propagation='{propagation}'");
            using (var regKey = RegistryHelper.GetRegistryKey(this.Path, true, false))
            {
                try
                {
                    if (regKey != null)
                    {
                        var newRule = new AccessRuleSummary(account, rights, accessType, inheritance, propagation).ToAccessRuleForRegistryKey();
                        var acl = regKey.GetAccessControl();
                        acl.AddAccessRule(newRule);
                        regKey.SetAccessControl(acl);
                        Logger.WriteLine("Info", _log_title, $"Successfully granted access rule to {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", _log_title, $"Failed to grant access rule to {_log_target}.");
                    Logger.WriteRaw(_log_title, e.ToString());
                }
            }
            return false;
        }

        public bool Grant(string accessRuleText)
        {
            Logger.WriteLine("Info", _log_title, $"Granting access rule to {_log_target}. '{this.Path}': AccessRule='{accessRuleText}'");
            using (var regKey = RegistryHelper.GetRegistryKey(this.Path, true, false))
            {
                try
                {
                    if (regKey != null)
                    {
                        var newRule = new AccessRuleSummary(accessRuleText).ToAccessRuleForRegistryKey();
                        var acl = regKey.GetAccessControl();
                        acl.AddAccessRule(newRule);
                        regKey.SetAccessControl(acl);
                        Logger.WriteLine("Info", _log_title, $"Successfully granted access rule to {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", _log_title, $"Failed to grant access rule to {_log_target}.");
                    Logger.WriteRaw(_log_title, e.ToString());
                }
            }
            return false;
        }

        public bool Revoke(string account)
        {
            Logger.WriteLine("Info", _log_title, $"Revoking access rules from {_log_target}. '{this.Path}': Account='{account}'");
            using (var regKey = RegistryHelper.GetRegistryKey(this.Path, true, false))
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
                        Logger.WriteLine("Info", _log_title, $"Successfully revoked access rules from {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", _log_title, $"Failed to revoke access rules from {_log_target}.");
                    Logger.WriteRaw(_log_title, e.ToString());
                }
            }
            return false;
        }

        public bool RevokeAll()
        {
            using (var regKey = RegistryHelper.GetRegistryKey(this.Path, true, false))
            {
                Logger.WriteLine("Info", _log_title, $"Revoking all access rules from {_log_target}. '{this.Path}'");
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
                        Logger.WriteLine("Info", _log_title, $"Successfully revoked all access rules from {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", _log_title, $"Failed to revoke all access rules from {_log_target}.");
                    Logger.WriteRaw(_log_title, e.ToString());
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
                Logger.WriteLine("Warning", _log_title, $"Skip change owner to {_log_target}.");
                return false;
            }
            Logger.WriteLine("Info", _log_title, $"Changing owner of {_log_target}. '{this.Path}' to '{newOwner}'");

            using (var regKey = RegistryHelper.GetRegistryKey(this.Path, true, false))
            {
                try
                {
                    if (regKey != null)
                    {
                        Logger.WriteLine("Info", _log_title, "Adjusting token privilegs (SeTakeOwnershipPrivilege, SeRestorePrivilege, SeBackupPrivilege)");
                        ProcessPrivilege.AdjustToken(Privilege.SeTakeOwnershipPrivilege);
                        ProcessPrivilege.AdjustToken(Privilege.SeRestorePrivilege);
                        ProcessPrivilege.AdjustToken(Privilege.SeBackupPrivilege);

                        var acl = regKey.GetAccessControl();
                        acl.SetOwner(new NTAccount(newOwner));
                        regKey.SetAccessControl(acl);
                        Logger.WriteLine("Info", _log_title, $"Successfully changed owner of {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", _log_title, $"Failed to change owner of {_log_target}.");
                    Logger.WriteRaw(_log_title, e.ToString());
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
                Logger.WriteLine("Warning", _log_title, $"Skip change inherited to {_log_target}.");
                return false;
            }
            Logger.WriteLine("Info", _log_title, $"Changing inheritance of {_log_target}. '{this.Path}' to '{isInherited}', preserve existing rules: {preserve}.");

            using (var regKey = RegistryHelper.GetRegistryKey(this.Path, true, false))
            {
                try
                {
                    if (regKey != null)
                    {
                        var acl = regKey.GetAccessControl();
                        acl.SetAccessRuleProtection(!(bool)isInherited, preserve);
                        regKey.SetAccessControl(acl);
                        Logger.WriteLine("Info", _log_title, $"Successfully changed inheritance of {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", _log_title, $"Failed to change inheritance of {_log_target}.");
                    Logger.WriteRaw(_log_title, e.ToString());
                }
            }
            return false;
        }
    }
}
