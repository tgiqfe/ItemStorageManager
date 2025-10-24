using ItemStorageManager.Functions;
using ItemStorageManager.ItemStorage.ACL;
using Microsoft.Win32;
using System.Diagnostics;

namespace ItemStorageManager.ItemStorage
{
    internal class RegistryKeyItem : IItem
    {
        public ItemType Type { get { return ItemType.RegistryKey; } }

        public string Path { get; set; }
        public string Name { get; set; }

        public AccessRule AccessRule { get; set; }

        public RegistryKeyItem(string path)
        {
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            using (var regKey = RegistryFunctions.GetRegistryKey(path))
            {
                if (regKey != null)
                {
                    this.AccessRule = new AccessRule(regKey.GetAccessControl());
                }
            }
        }

        public bool Exists()
        {
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path))
            {
                try
                {
                    if (regKey != null)
                    {
                        return true;
                    }
                }
                catch { }
            }
            return false;
        }

        public bool Copy(string dstPath, bool overwrite)
        {
            using (var srcKey = RegistryFunctions.GetRegistryKey(this.Path, false))
            using (var dstKey = RegistryFunctions.GetRegistryKey(dstPath, true))
            {
                try
                {
                    CopyRegistryKey(srcKey, dstKey);
                    return true;
                }
                catch { }
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
                using (var srcSubKey = srcKey.OpenSubKey(subKeyName, false))
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
            using (var regKey = RegistryFunctions.GetRegistryKey(System.IO.Path.GetDirectoryName(this.Path), true))
            {
                try
                {
                    if (regKey != null)
                    {
                        regKey.DeleteSubKeyTree(this.Name);
                        return true;
                    }
                }
                catch { }
            }
            return false;
        }

        public bool Remove()
        {
            return Delete();
        }

        public bool Move(string dstPath)
        {
            using (var srcKey = RegistryFunctions.GetRegistryKey(this.Path, false))
            using (var dstKey = RegistryFunctions.GetRegistryKey(dstPath, true))
            {
                try
                {
                    CopyRegistryKey(srcKey, dstKey);
                }
                catch { }
            }
            using (var parentKey = RegistryFunctions.GetRegistryKey(System.IO.Path.GetDirectoryName(this.Path), true))
            {
                try
                {
                    if (parentKey != null)
                    {
                        parentKey.DeleteSubKeyTree(this.Name);
                        return true;
                    }
                }
                catch { }
            }
            return false;
        }

        public bool Rename(string newName)
        {
            string dstPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Path), newName);
            using (var srcKey = RegistryFunctions.GetRegistryKey(this.Path, false))
            using (var dstKey = RegistryFunctions.GetRegistryKey(dstPath, true))
            {
                try
                {
                    CopyRegistryKey(srcKey, dstKey);
                }
                catch { }
            }
            using (var parentKey = RegistryFunctions.GetRegistryKey(System.IO.Path.GetDirectoryName(this.Path), true))
            {
                try
                {
                    if (parentKey != null)
                    {
                        parentKey.DeleteSubKeyTree(this.Name);
                        return true;
                    }
                }
                catch { }
            }
            return false;
        }
    }
}
