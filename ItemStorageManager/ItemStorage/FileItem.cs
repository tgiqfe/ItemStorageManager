using ItemStorageManager.Functions;
using ItemStorageManager.ItemStorage.ACL;
using ItemStorageManager.ItemStorage.Attrib;

namespace ItemStorageManager.ItemStorage
{
    internal class FileItem : IBaseItem, ISecurityItem, IAttributeItem
    {
        public ItemType Type { get { return ItemType.File; } }
        public string Path { get; set; }
        public string Name { get; set; }
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
            this.FormatedSize = TextFunctions.FormatFileSize(fi.Length);
            this.CreationTime = fi.CreationTime;
            this.LastWriteTime = fi.LastWriteTime;
            this.LastAccessTime = fi.LastAccessTime;
            this.Attributes = fi.Attributes.ToString();
            this.AccessRule = new AccessRule(fi.GetAccessControl());
            this.SecurityBlock = File.Exists($"{path}:Zone.Identifier");
        }

        #region from IBaseItem

        public bool Exists()
        {
            return File.Exists(this.Path);
        }

        public bool Copy(string dstPath, bool overwrite)
        {
            try
            {
                File.Copy(this.Path, dstPath, overwrite);
                return true;
            }
            catch { }
            return false;
        }

        public bool Delete()
        {
            try
            {
                File.Delete(this.Path);
                return true;
            }
            catch { }
            return false;
        }

        public bool Remove()
        {
            return this.Delete();
        }

        public bool Move(string dstPath)
        {
            try
            {
                File.Move(this.Path, dstPath);
                return true;
            }
            catch { }
            return false;
        }

        public bool Rename(string newName)
        {
            try
            {
                var dstPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Path), newName);
                File.Move(this.Path, dstPath);
                return true;
            }
            catch { }
            return false;
        }

        #endregion
        #region from ISecurityItem

        public bool Grant(string account, string rights, string accessType, string inheritance, string propageteToSubItems )
        {
            propageteToSubItems = "None";

            return false;
        }

        public bool Grant(string accessRuleText)
        {
            return false;
        }

        public bool Revoke(string account)
        {
            return false;
        }

        public bool Revoke()
        {
            return false;
        }

        public bool ChangeOwner(string newOwner)
        {
            return false;
        }

        public bool ChangeInherited(bool isInherited)
        {
            return false;
        }

        #endregion
        #region from IAttributeItem

        public bool SetAttributes(string attributes)
        {
            try
            {
                var fi = new FileInfo(this.Path);
                fi.Attributes = AttributeFunctions.GetProcessedAttributes(attributes, fi.Attributes);
                this.Attributes = fi.Attributes.ToString();
                return true;
            }
            catch { }
            return false;
        }

        #endregion
    }
}
