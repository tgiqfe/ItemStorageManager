namespace ItemStorageManager.ItemStorage
{
    internal interface IBaseItem
    {
        ItemType Type { get; }
        string Path { get; set; }
        string Name { get; set; }

        bool Copy(string dstPath, bool overwrite);
        bool Remove();
        bool Delete();
        bool Move(string dstPath);
        bool Rename(string newName);
    }
}
