namespace ItemStorageManager.ItemStorage
{
    internal class BaseItem
    {
        public virtual ItemType Type { get; }
        
        public virtual string Path { get; set; }
        public virtual string Name { get; set; }
    }
}
