using ItemStorageManager.Functions;
using Microsoft.Win32;
using System.Linq.Expressions;

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
                    switch (this.ValueKind)
                    {
                        case RegistryValueKind.String:
                            this.DataAsString = this.Data as string;
                            break;
                        case RegistryValueKind.ExpandString:
                            this.DataAsString = regKey.GetValue(valueName, Data, RegistryValueOptions.DoNotExpandEnvironmentNames) as string;
                            break;
                        case RegistryValueKind.DWord:
                            this.DataAsString = ((int)this.Data).ToString();
                            break;
                        case RegistryValueKind.QWord:
                            this.DataAsString = ((long)this.Data).ToString();
                            break;
                        case RegistryValueKind.MultiString:
                            this.DataAsString = "[ " + string.Join(", ", (string[])this.Data) + " ]";
                            break;
                        case RegistryValueKind.Binary:
                            this.DataAsString = BitConverter.ToString((byte[])this.Data).Replace("-", " ");
                            break;
                        default:
                            this.DataAsString = "Unsupported Value Kind";
                            break;
                    }
                }
            }
        }

        #region from IBaseItem

        public bool Exists()
        {
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
                catch { }
            }
            return false;
        }

        public bool Copy(string dstPath, bool overwrite)
        {
            using (var srcKey = RegistryFunctions.GetRegistryKey(this.Path))
            using (var dstKey = RegistryFunctions.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    if (overwrite || !dstKey.GetValueNames().Contains(this.Name))
                    {
                        dstKey.SetValue(this.Name, this.Data, this.ValueKind);
                        return true;
                    }
                }
                catch { }
            }
            return false;
        }

        public bool Copy(string dstPath, string dstName, bool overwrite)
        {
            using (var srcKey = RegistryFunctions.GetRegistryKey(this.Path))
            using (var dstKey = RegistryFunctions.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    if (overwrite || !dstKey.GetValueNames().Contains(dstName))
                    {
                        dstKey.SetValue(dstName, this.Data, this.ValueKind);
                        return true;
                    }
                }
                catch { }
            }
            return false;
        }

        public bool Delete()
        {
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path, true))
            {
                try
                {
                    if (regKey != null)
                    {
                        regKey.DeleteValue(this.Name);
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
            using (var srcKey = RegistryFunctions.GetRegistryKey(this.Path))
            using (var dstKey = RegistryFunctions.GetRegistryKey(dstPath, true, true))
            {
                try
                {
                    var valueData = srcKey.GetValue(this.Name);
                    var valueKind = srcKey.GetValueKind(this.Name);
                    dstKey.SetValue(this.Name, valueData, valueKind);
                    return true;
                }
                catch { }
            }
            return false;
        }

        public bool Rename(string newName)
        {
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path, true))
            {
                try
                {
                    var valueData = regKey.GetValue(this.Name);
                    var valueKind = regKey.GetValueKind(this.Name);
                    regKey.SetValue(newName, valueData, valueKind);
                    regKey.DeleteValue(this.Name);
                    this.Name = newName;
                    return true;
                }
                catch { }
            }
            return false;
        }

        #endregion
        #region from ISecurityItem

        //  Nothing

        #endregion
    }
}
