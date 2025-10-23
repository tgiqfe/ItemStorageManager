using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemStorageManager.ItemStorage
{
    internal class DIrectoryItem : BaseItem
    {
        public override ItemType Type { get { return ItemType.Directory; } }
        
        public override string Path { get; set; }
        public override string Name { get; set; }
    }
}
