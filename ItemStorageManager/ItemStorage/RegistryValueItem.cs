using ItemStorageManager.Functions;
using Microsoft.Win32;

namespace ItemStorageManager.ItemStorage
{
    internal class RegistryValueItem : IBaseItem
    {
        #region Public parameter

        public string Type { get { return "RegistryValue"; } }
        public string Path { get; set; }
        public string Name { get; set; }
        public string ValueKind { get; set; }
        public string Data { get; set; }

        #endregion

        public object _dataObject { get; set; }

        const string _log_target = "registry value";

        public RegistryValueItem(string keyPath, string valueName)
        {
            this.Path = $"{keyPath}\\{valueName}";
            this.Name = valueName;
            using (var regKey = RegistryHelper.GetRegistryKey(keyPath))
            {
                if (regKey != null)
                {
                    var valueKind = regKey.GetValueKind(valueName);
                    this.ValueKind = RegistryParser.RegistryValueKindToString(valueKind);
                    this._dataObject = valueKind == RegistryValueKind.ExpandString ?
                        regKey.GetValue(valueName, null, RegistryValueOptions.DoNotExpandEnvironmentNames) :
                        regKey.GetValue(valueName);
                    this.Data = RegistryParser.RegistryValueToString(_dataObject, valueKind);
                }
            }
        }

        public static bool Set(string keyPath, string name, object data, string valueKindString)
        {
            using (var regKey = RegistryHelper.GetRegistryKey(keyPath, true, true))
            {
                try
                {
                    regKey.SetValue(name, data, RegistryParser.StringToRegistryValueKind(valueKindString));
                    return true;
                }
                catch { }
            }
            return false;
        }

        public static bool Set(string keyPath, string name, object data, RegistryValueKind valueKind)
        {
            using (var regKey = RegistryHelper.GetRegistryKey(keyPath, true, true))
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
            using (var regKey = RegistryHelper.GetRegistryKey(keyPath, true, true))
            {
                try
                {
                    object data = RegistryParser.StringToRegistryValue(dataString, valueKind);
                    regKey.SetValue(name, data, valueKind);
                    return true;
                }
                catch { }
            }
            return false;
        }

        public static bool Set(string keyPath, string name, string dataString, string valueKindString)
        {
            using (var regKey = RegistryHelper.GetRegistryKey(keyPath, true, true))
            {
                try
                {
                    RegistryValueKind valueKind = RegistryParser.StringToRegistryValueKind(valueKindString);
                    object data = RegistryParser.StringToRegistryValue(dataString, valueKind);
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
        public static bool Exists(string path, string name)
        {
            Logger.WriteLine("Info", $"Checking existence of {_log_target} at path '{path}', value '{name}'.");
            using (var regKey = RegistryHelper.GetRegistryKey(path))
            {
                try
                {
                    if (regKey != null)
                    {
                        var valueNames = regKey.GetValueNames();
                        return valueNames.Contains(name);
                    }
                }
                catch(Exception e) {

                    Logger.WriteLine("Error", $"Failed to check existence of {_log_target}.");
                    Logger.WriteRaw(e.ToString());
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
            using (var srcKey = RegistryHelper.GetRegistryKey(this.Path))
            using (var dstKey = RegistryHelper.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    if (overwrite || !dstKey.GetValueNames().Contains(this.Name))
                    {
                        var valueKind = RegistryParser.StringToRegistryValueKind(this.ValueKind);
                        dstKey.SetValue(this.Name, this._dataObject, valueKind);
                        Logger.WriteLine("Info", $"Successfully copied {_log_target}.");
                        return true;
                    }
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
        /// Copy the registry value. new key and new name.
        /// </summary>
        /// <param name="dstPath"></param>
        /// <param name="dstName"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public bool Copy(string dstPath, string dstName, bool overwrite)
        {
            Logger.WriteLine("Info", $"Copying {_log_target}. From '{this.Path}' '{this.Name}' to '{dstPath}' '{dstName}', overwrite: {overwrite}.");
            using (var srcKey = RegistryHelper.GetRegistryKey(this.Path))
            using (var dstKey = RegistryHelper.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    if (overwrite || !dstKey.GetValueNames().Contains(dstName))
                    {
                        var valueKind = RegistryParser.StringToRegistryValueKind(this.ValueKind);
                        dstKey.SetValue(dstName, this._dataObject, valueKind);
                        Logger.WriteLine("Info", $"Successfully copied {_log_target}.");
                        return true;
                    }
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
        /// Remove the registry value.
        /// </summary>
        /// <returns></returns>
        public bool Remove()
        {
            Logger.WriteLine("Info", $"Removing {_log_target}. '{this.Path}'");
            using (var regKey = RegistryHelper.GetRegistryKey(this.Path, true, false))
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
                    Logger.WriteLine("Error", $"Failed to remove {_log_target}.");
                    Logger.WriteRaw(e.ToString());
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
            using (var srcKey = RegistryHelper.GetRegistryKey(this.Path, true, false))
            using (var dstKey = RegistryHelper.GetRegistryKey(dstPath, true, true))
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
                    Logger.WriteLine("Error", $"Failed to move {_log_target}.");
                    Logger.WriteRaw(e.ToString());
                }
            }
            return false;
        }

        public bool Rename(string newName)
        {
            Logger.WriteLine("Info", $"Renaming {_log_target}. Key {this.Path}. From '{this.Name}' to '{newName}'.");
            using (var regKey = RegistryHelper.GetRegistryKey(this.Path, true, false))
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
                    Logger.WriteLine("Error", $"Failed to rename {_log_target}.");
                    Logger.WriteRaw(e.ToString());
                }
            }
            return false;
        }
    }
}
