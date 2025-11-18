using ItemStorageManager.Functions;
using System.Text;

namespace ItemStorageManager.ItemStorage
{
    internal class AttributesParser
    {
        private static Dictionary<string[], FileAttributes> _attributesMap = null;

        private static void InitializeAttributes()
        {
            _attributesMap ??= new()
            {
                { new string[]{ "Archive" }, FileAttributes.Archive },
                { new string[]{ "Compressed" }, FileAttributes.Compressed },
                { new string[]{ "Device", "Dev" }, FileAttributes.Device },
                { new string[]{ "Directory", "Dir" }, FileAttributes.Directory },
                { new string[]{ "Encrypted", "Enc" }, FileAttributes.Encrypted },
                { new string[]{ "Hidden", "H", "Hide" }, FileAttributes.Hidden },
                { new string[]{ "IntegrityStream" }, FileAttributes.IntegrityStream },
                { new string[]{ "None" }, FileAttributes.Normal },
                { new string[]{ "Normal" }, FileAttributes.Normal },
                { new string[]{ "NoScrubData" }, FileAttributes.NoScrubData },
                { new string[]{ "NotContentIndexed" }, FileAttributes.NotContentIndexed },
                { new string[]{ "Offline" }, FileAttributes.Offline },
                { new string[]{ "ReadOnly", "R", "Read" }, FileAttributes.ReadOnly },
                { new string[]{ "ReparsePoint" }, FileAttributes.ReparsePoint },
                { new string[]{ "SparseFile" }, FileAttributes.SparseFile },
                { new string[]{ "System", "S", "Sys" }, FileAttributes.System },
                { new string[]{ "Temporary", "Temp", "tmp" }, FileAttributes.Temporary }
            };
        }

        public static FileAttributes StringToAttributes(string text)
        {
            if (_attributesMap == null) InitializeAttributes();
            return TextFunctions.StringToFlags<FileAttributes>(text, _attributesMap);
        }

        public static string AttributesToString(FileAttributes val)
        {
            if (_attributesMap == null) InitializeAttributes();
            return TextFunctions.FlagsToString<FileAttributes>(val, _attributesMap);
        }

        public static string GetAttributesString(string text)
        {
            if (_attributesMap == null) InitializeAttributes();
            return TextFunctions.GetCorrect<FileAttributes>(text, _attributesMap);
        }

        public static FileAttributes MergeAttributes(string text, FileAttributes baseAttributes)
        {
            if (_attributesMap == null) InitializeAttributes();
            return TextFunctions.MergeFlags<FileAttributes>(text, baseAttributes, _attributesMap);
        }
    }
}
