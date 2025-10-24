namespace ItemStorageManager.ItemStorage
{
    internal interface IItem
    {
        ItemType Type { get; }
        string Path { get; set; }
        string Name { get; set; }

        bool Exists();
        bool Copy(string dstPath, bool overwrite);
        bool Delete();
        bool Remove();
        bool Move(string dstPath);
        bool Rename(string newName);
    }
}
