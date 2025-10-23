using ItemStorageManager.ItemStorage.ACL;
using System.Security.Principal;

namespace ItemStorageManager.ItemStorage
{
    internal class FileItem : BaseItem
    {
        public override ItemType Type { get { return ItemType.File; } }
        public override string Path { get; set; }
        public override string Name { get; set; }
        public long Size { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public string Attributes { get; set; }  
        public AccessRules AccessRules { get; set; }
        public bool SecurityBlock { get; set; }

        public FileItem(string path)
        {
            var fi = new FileInfo(path);

            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            this.Size = fi.Length;
            this.CreationTime = fi.CreationTime;
            this.LastWriteTime = fi.LastWriteTime;
            this.LastAccessTime = fi.LastAccessTime;
            this.Attributes = fi.Attributes.ToString();
            //this.Owner = security.GetOwner(typeof(NTAccount)).Value;
            //this.Access = AccessRuleSummary.LoadFromFileSystem(security.GetAccessRules(true, true, typeof(NTAccount)));
            //this.IsInherited = security.AreAccessRulesProtected == false;
            this.AccessRules = new AccessRules(fi.GetAccessControl());
            this.SecurityBlock = File.Exists($"{path}:Zone.Identifier");
        }
    }
}
