using ItemStorageManager.Functions;
using ItemStorageManager.ItemStorage.ACL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemStorageManager.ItemStorage
{
    internal class RegistryKeyItem : BaseItem
    {
        public override ItemType Type { get { return ItemType.RegistryKey; } }

        public override string Path { get; set; }
        public override string Name { get; set; }

        public AccessRules AccessRules { get; set; }

        public RegistryKeyItem(string path)
        {
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            using (var regKey = RegistryFunctions.GetRegistryKey(path))
            {
                if(regKey != null)
                {
                    this.AccessRules = new AccessRules(regKey.GetAccessControl());
                }
            }
        }
    }
}
