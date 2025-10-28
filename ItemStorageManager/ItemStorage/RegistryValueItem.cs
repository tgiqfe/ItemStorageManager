using ItemStorageManager.Functions;
using Microsoft.Win32;

namespace ItemStorageManager.ItemStorage
{
    internal class RegistryValueItem : IBaseItem
    {
        public ItemType Type { get { return ItemType.RegistryValue; } }

        public string Path { get; set; }
        public string Name { get; set; }

        public RegistryValueKind ValueKind { get; set; }
        public object Data { get; set; }
        public string DataAsString { get; set; }

        const string _log_TargetItem = "registry value";

        public RegistryValueItem(string keyPath, string valueName)
        {
            this.Path = $"{keyPath}\\{valueName}";
            this.Name = valueName;
            using (var regKey = RegistryFunctions.GetRegistryKey(keyPath))
            {
                if (regKey != null)
                {
                    this.ValueKind = regKey.GetValueKind(valueName);
                    this.Data = this.ValueKind == RegistryValueKind.ExpandString ?
                        regKey.GetValue(valueName, null, RegistryValueOptions.DoNotExpandEnvironmentNames) :
                        regKey.GetValue(valueName);
                    this.DataAsString = RegistryFunctions.RegistryValueToString(this.Data, this.ValueKind);
                }
            }
        }

        public static bool Set(string keyPath, string name, object data, string valueKindString)
        {
            using (var regKey = RegistryFunctions.GetRegistryKey(keyPath, true, true))
            {
                try
                {
                    regKey.SetValue(name, data, RegistryFunctions.StringToValueKind(valueKindString));
                    return true;
                }
                catch { }
            }
            return false;
        }

        public static bool Set(string keyPath, string name, object data, RegistryValueKind valueKind)
        {
            using (var regKey = RegistryFunctions.GetRegistryKey(keyPath, true, true))
            {
                try
                {
                    regKey.SetValue(name, data, valueKind);
                    return true;
                }
                catch { }
            }
            return false;
        }

        public static bool Set(string keyPath, string name, string dataString, RegistryValueKind valueKind)
        {
            using (var regKey = RegistryFunctions.GetRegistryKey(keyPath, true, true))
            {
                try
                {
                    object data = RegistryFunctions.StringToRegistryValue(dataString, valueKind);
                    regKey.SetValue(name, data, valueKind);
                    return true;
                }
                catch { }
            }
            return false;
        }

        public static bool Set(string keyPath, string name, string dataString, string valueKindString)
        {
            using (var regKey = RegistryFunctions.GetRegistryKey(keyPath, true, true))
            {
                try
                {
                    RegistryValueKind valueKind = RegistryFunctions.StringToValueKind(valueKindString);
                    object data = RegistryFunctions.StringToRegistryValue(dataString, valueKind);
                    regKey.SetValue(name, data, valueKind);
                    return true;
                }
                catch { }
            }
            return false;
        }

        public bool Exists()
        {
            Logger.WriteLine("Info", $"Checking existence of {_log_TargetItem} at path '{this.Path}', value '{this.Name}'.");
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path))
            {
                try
                {
                    if (regKey != null)
                    {
                        var valueNames = regKey.GetValueNames();
                        return valueNames.Contains(this.Name);
                    }
                }
                catch(Exception e) {

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
                    if (overwrite || !dstKey.GetValueNames().Contains(this.Name))
                    {
                        dstKey.SetValue(this.Name, this.Data, this.ValueKind);
                        Logger.WriteLine("Info", $"Successfully copied {_log_TargetItem}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to copy {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        public bool Copy(string dstPath, string dstName, bool overwrite)
        {
            Logger.WriteLine("Info", $"Copying {_log_TargetItem}. From '{this.Path}' '{this.Name}' to '{dstPath}' '{dstName}', overwrite: {overwrite}.");
            using (var srcKey = RegistryFunctions.GetRegistryKey(this.Path))
            using (var dstKey = RegistryFunctions.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    if (overwrite || !dstKey.GetValueNames().Contains(dstName))
                    {
                        dstKey.SetValue(dstName, this.Data, this.ValueKind);
                        Logger.WriteLine("Info", $"Successfully copied {_log_TargetItem}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to copy {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        public bool Delete()
        {
            Logger.WriteLine("Info", $"Deleting {_log_TargetItem}. '{this.Path}'");
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path, false, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        regKey.DeleteValue(this.Name);
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
            Logger.WriteLine("Info", $"Moving {_log_TargetItem}. value '{this.Name}'. From '{this.Path}' to '{dstPath}'.");
            using (var srcKey = RegistryFunctions.GetRegistryKey(this.Path, false, true))
            using (var dstKey = RegistryFunctions.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    var valueData = srcKey.GetValue(this.Name);
                    var valueKind = srcKey.GetValueKind(this.Name);
                    dstKey.SetValue(this.Name, valueData, valueKind);
                    srcKey.DeleteValue(this.Name);
                    Logger.WriteLine("Info", $"Successfully moved {_log_TargetItem}.");
                    return true;
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
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path, false, true))
            {
                try
                {
                    var valueData = regKey.GetValue(this.Name);
                    var valueKind = regKey.GetValueKind(this.Name);
                    regKey.SetValue(newName, valueData, valueKind);
                    regKey.DeleteValue(this.Name);
                    this.Name = newName;
                    Logger.WriteLine("Info", $"Successfully renamed {_log_TargetItem}.");
                    return true;
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to rename {_log_TargetItem}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }
    }
}
