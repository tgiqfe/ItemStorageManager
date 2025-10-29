using ItemStorageManager.Functions;
using Microsoft.Win32;

namespace ItemStorageManager.ItemStorage
{
    internal class RegistryValueItem : IBaseItem
    {
        #region Public parameter

        public ItemType Type { get { return ItemType.RegistryValue; } }
        public string Path { get; set; }
        public string Name { get; set; }
        public RegistryValueKind ValueKind { get; set; }
        public object Data { get; set; }
        public string DataAsString { get; set; }

        #endregion

        const string _log_target = "registry value";

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

        /// <summary>
        /// Exists the specified registry value.
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            Logger.WriteLine("Info", $"Checking existence of {_log_target} at path '{this.Path}', value '{this.Name}'.");
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

                    Logger.WriteLine("Error", $"Failed to check existence of {_log_target}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        /// <summary>
        /// Copy the registry value. new key.
        /// </summary>
        /// <param name="dstPath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public bool Copy(string dstPath, bool overwrite)
        {
            Logger.WriteLine("Info", $"Copying {_log_target}. From '{this.Path}' to '{dstPath}', overwrite: {overwrite}.");
            using (var srcKey = RegistryFunctions.GetRegistryKey(this.Path))
            using (var dstKey = RegistryFunctions.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    if (overwrite || !dstKey.GetValueNames().Contains(this.Name))
                    {
                        dstKey.SetValue(this.Name, this.Data, this.ValueKind);
                        Logger.WriteLine("Info", $"Successfully copied {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to copy {_log_target}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        /// <summary>
        /// Copy the registry value. new key and new name.
        /// </summary>
        /// <param name="dstPath"></param>
        /// <param name="dstName"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public bool Copy(string dstPath, string dstName, bool overwrite)
        {
            Logger.WriteLine("Info", $"Copying {_log_target}. From '{this.Path}' '{this.Name}' to '{dstPath}' '{dstName}', overwrite: {overwrite}.");
            using (var srcKey = RegistryFunctions.GetRegistryKey(this.Path))
            using (var dstKey = RegistryFunctions.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    if (overwrite || !dstKey.GetValueNames().Contains(dstName))
                    {
                        dstKey.SetValue(dstName, this.Data, this.ValueKind);
                        Logger.WriteLine("Info", $"Successfully copied {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to copy {_log_target}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        /// <summary>
        /// Remove the registry value.
        /// </summary>
        /// <returns></returns>
        public bool Remove()
        {
            Logger.WriteLine("Info", $"Removing {_log_target}. '{this.Path}'");
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path, false, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        regKey.DeleteValue(this.Name);
                        Logger.WriteLine("Info", $"Successfully removed {_log_target}.");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to remove {_log_target}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        /// <summary>
        /// Remove the registry value. (alias of Remove)
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            return Remove();
        }

        public bool Move(string dstPath)
        {
            Logger.WriteLine("Info", $"Moving {_log_target}. value '{this.Name}'. From '{this.Path}' to '{dstPath}'.");
            using (var srcKey = RegistryFunctions.GetRegistryKey(this.Path, false, true))
            using (var dstKey = RegistryFunctions.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    var valueData = srcKey.GetValue(this.Name);
                    var valueKind = srcKey.GetValueKind(this.Name);
                    dstKey.SetValue(this.Name, valueData, valueKind);
                    srcKey.DeleteValue(this.Name);
                    Logger.WriteLine("Info", $"Successfully moved {_log_target}.");
                    return true;
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to move {_log_target}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }

        public bool Rename(string newName)
        {
            Logger.WriteLine("Info", $"Renaming {_log_target}. Key {this.Path}. From '{this.Name}' to '{newName}'.");
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path, false, true))
            {
                try
                {
                    var valueData = regKey.GetValue(this.Name);
                    var valueKind = regKey.GetValueKind(this.Name);
                    regKey.SetValue(newName, valueData, valueKind);
                    regKey.DeleteValue(this.Name);
                    this.Name = newName;
                    Logger.WriteLine("Info", $"Successfully renamed {_log_target}.");
                    return true;
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error", $"Failed to rename {_log_target}. Exception: {e.ToString()}");
                    Logger.WriteRaw(e.Message);
                }
            }
            return false;
        }
    }
}
