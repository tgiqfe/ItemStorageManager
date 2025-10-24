using ItemStorageManager.Functions;
using Microsoft.Win32;

namespace ItemStorageManager.ItemStorage
{
    internal class RegistryValueItem : IItem
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
                    this.Data = regKey.GetValue(valueName);
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

        public bool Exists()
        {
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path))
            {
                if (regKey != null)
                {
                    var valueNames = regKey.GetValueNames();
                    return valueNames.Contains(this.Name);
                }
            }
            return false;
        }

        public bool Copy(string dstPath, bool overwrite)
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path, true))
            {
                if (regKey != null)
                {
                    regKey.DeleteValue(this.Name);
                    return true;
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
            throw new NotImplementedException();
        }

        public bool Rename(string newName)
        {
            throw new NotImplementedException();
        }
    }
}
