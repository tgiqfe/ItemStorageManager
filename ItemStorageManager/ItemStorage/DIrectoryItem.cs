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
        public AccessRule AccessRule { get; set; }

        public DirectoryItem(string path)
        {
            var di = new DirectoryInfo(path);

            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            this.CreationTime = di.CreationTime;
            this.LastWriteTime = di.LastWriteTime;
            this.LastAccessTime = di.LastAccessTime;
            this.Attributes = di.Attributes.ToString();
            this.AccessRule = new AccessRule(di.GetAccessControl());
        }

        public long GetChildDirectoryCount()
        {
            try
            {
                var di = new DirectoryInfo(this.Path);
                return di.GetDirectories().LongLength;
            }
            catch
            {
                return -1;
            }
        }

        public long GetChildFileCount()
        {
            try
            {
                var di = new DirectoryInfo(this.Path);
                return di.GetFiles().LongLength;
            }
            catch
            {
                return -1;
            }
        }

        public long GetTotalFileSize()
        {
            try
            {
                long totalSize = 0;
                var di = new DirectoryInfo(this.Path);
                var files = di.GetFiles("*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    totalSize += file.Length;
                }
                return totalSize;
            }
            catch
            {
                return -1;
            }
        }
    }
}
