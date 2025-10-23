using ItemStorageManager.Functions;
using ItemStorageManager.ItemStorage.ACL;
using System.Security.Principal;

namespace ItemStorageManager.ItemStorage
{
    internal class FileItem : BaseItem
    {
        public override ItemType Type { get { return ItemType.File; } }
        public override string Path { get; set; }
        public override string Name { get; set; }
        public string Size { get; set; }
        public string FormatedSize { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public string Attributes { get; set; }  
        public AccessRule AccessRule { get; set; }
        public bool SecurityBlock { get; set; }

        public FileItem(string path)
        {
            var fi = new FileInfo(path);

            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            this.Size = string.Format("{0:N0} Byte", fi.Length);
            this.FormatedSize= TextFunctions.FormatFileSize(fi.Length);
            this.CreationTime = fi.CreationTime;
            this.LastWriteTime = fi.LastWriteTime;
            this.LastAccessTime = fi.LastAccessTime;
            this.Attributes = fi.Attributes.ToString();
            this.AccessRule = new AccessRule(fi.GetAccessControl());
            this.SecurityBlock = File.Exists($"{path}:Zone.Identifier");
        }
    }
}
