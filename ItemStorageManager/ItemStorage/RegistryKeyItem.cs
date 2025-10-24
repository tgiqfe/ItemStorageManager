using ItemStorageManager.Functions;
using ItemStorageManager.ItemStorage.ACL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemStorageManager.ItemStorage
{
    internal class RegistryKeyItem : IItem
    {
        public  ItemType Type { get { return ItemType.RegistryKey; } }

        public  string Path { get; set; }
        public  string Name { get; set; }

        public AccessRule AccessRule { get; set; }

        public RegistryKeyItem(string path)
        {
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            using (var regKey = RegistryFunctions.GetRegistryKey(path))
            {
                if(regKey != null)
                {
                    this.AccessRule = new AccessRule(regKey.GetAccessControl());
                }
            }
        }

        public bool Exists()
        {
            using (var regKey = RegistryFunctions.GetRegistryKey(this.Path))
            {
                if (regKey != null)
                {
                    return true;
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
            throw new NotImplementedException();
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
