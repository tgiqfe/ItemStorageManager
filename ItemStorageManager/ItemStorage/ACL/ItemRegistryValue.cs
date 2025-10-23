using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemStorageManager.ItemStorage.ACL
{
    internal class ItemRegistryValue : BaseItem
    {
        public override ItemType Type { get { return ItemType.RegistryValue; } }
        public override string Name { get; set; }
        public override string Path { get; set; }
    }
}
