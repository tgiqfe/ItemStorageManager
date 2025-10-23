using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemStorageManager.ItemStorage
{
    internal class RegistryValueItem : BaseItem
    {
        public override ItemType Type { get { return ItemType.RegistryValue; } }

        public override string Path { get; set; }
        public override string Name { get; set; }

        public RegistryValueKind ValueKind { get; set; }
        public object Data { get; set; }
        public string DataAsString { get; set; }
    }
}
