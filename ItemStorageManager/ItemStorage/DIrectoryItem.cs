using ItemStorageManager.ItemStorage.ACL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace ItemStorageManager.ItemStorage
{
    internal class DirectoryItem : IItem
    {
        public ItemType Type { get { return ItemType.Directory; } }

        public string Path { get; set; }
        public string Name { get; set; }
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
            catch { }
            return -1;
        }

        public long GetChildFileCount()
        {
            try
            {
                var di = new DirectoryInfo(this.Path);
                return di.GetFiles().LongLength;
            }
            catch { }
            return -1;
        }

        public long GetTotalFileSize()
        {
            try
            {
                long totalSize = 0;
                var di = new DirectoryInfo(this.Path);
                var files = di.GetFiles("*", System.IO.SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    totalSize += file.Length;
                }
                return totalSize;
            }
            catch { }
            return -1;
        }

        public bool Exists()
        {
            return Directory.Exists(this.Path);
        }

        public bool Copy(string dstPath, bool overwrite)
        {
            try
            {
                FileSystem.CopyDirectory(this.Path, dstPath, overwrite);
                return true;
            }
            catch { }
            return false;
        }

        public bool Delete()
        {
            try
            {
                Directory.Delete(this.Path, true);
                return true;
            }
            catch { }
            return false;
        }

        public bool Remove()
        {
            return Delete();
        }

        public bool Move(string dstPath)
        {
            try
            {
                Directory.Move(this.Path, dstPath);
                return true;
            }
            catch { }
            return false;
        }

        public bool Rename(string newName)
        {
            try
            {
                var parentDir = System.IO.Path.GetDirectoryName(this.Path);
                var newPath = System.IO.Path.Combine(parentDir, newName);
                Directory.Move(this.Path, newPath);
                this.Path = newPath;
                this.Name = newName;
                return true;
            }
            catch { }
            return false;
        }
    }
}
