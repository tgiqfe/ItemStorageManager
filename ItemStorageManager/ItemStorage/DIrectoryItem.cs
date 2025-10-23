using ItemStorageManager.ItemStorage.ACL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemStorageManager.ItemStorage
{
    internal class DirectoryItem : BaseItem
    {
        public override ItemType Type { get { return ItemType.Directory; } }

        public override string Path { get; set; }
        public override string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public string Attributes { get; set; }
        public AccessRules AccessRules { get; set; }

        public DirectoryItem(string path)
        {
            var di = new DirectoryInfo(path);

            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            this.CreationTime = di.CreationTime;
            this.LastWriteTime = di.LastWriteTime;
            this.LastAccessTime = di.LastAccessTime;
            this.Attributes = di.Attributes.ToString();
            this.AccessRules = new AccessRules(di.GetAccessControl());
        }
    }
}
