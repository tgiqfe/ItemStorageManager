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
            var attr = default(FileAttributes);
            foreach (var attrText in text.Split(',').Select(x => x.Trim()))
            {
                bool isFound = false;
                foreach (var kvp in _attributesMap)
                {
                    if (kvp.Key.Any(x => string.Equals(x, attrText, StringComparison.OrdinalIgnoreCase)))
                    {
                        attr |= kvp.Value;
                        isFound = true;
                        break;
                    }
                }
                if (!isFound) throw new ArgumentException($"Invalid attributes string: {text}");
            }
            return attr;
        }

        public static string AttributesToString(FileAttributes attributes)
        {
            if (_attributesMap == null) InitializeAttributes();
            StringBuilder sb = new();
            foreach (var kvp in _attributesMap)
            {
                if (attributes.HasFlag(kvp.Value))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(kvp.Key[0]);
                }
            }
            return sb.ToString();
        }

        public static string GetAttributesString(string text)
        {
            if (_attributesMap == null) InitializeAttributes();
            StringBuilder sb = new();
            foreach (var attrText in text.Split(',').Select(x => x.Trim()))
            {
                bool isFound = false;
                foreach (var kvp in _attributesMap)
                {
                    if (kvp.Key.Any(x => string.Equals(x, attrText, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(", ");
                        }
                        isFound = true;
                        break;
                    }
                }
                if (!isFound) throw new ArgumentException($"Invalid attributes string: {text}");
            }
            return sb.ToString();
        }

        public static FileAttributes MergeAttributes(string text, FileAttributes baseAttributes)
        {
            if (_attributesMap == null) InitializeAttributes();
            var attr = baseAttributes;
            foreach (var attrText in text.Split(',').Select(x => x.Trim()))
            {
                bool isFound = false;
                if (attrText.StartsWith("-"))
                {
                    var trimmedAttrText = attrText.Substring(1).Trim();
                    foreach (var kvp in _attributesMap)
                    {
                        if (kvp.Key.Any(x => string.Equals(x, trimmedAttrText, StringComparison.OrdinalIgnoreCase)))
                        {
                            attr &= ~kvp.Value;
                            isFound = true;
                            break;
                        }
                    }
                }
                else if (attrText.StartsWith("+"))
                {
                    var trimmedAttrText = attrText.Substring(1).Trim();
                    foreach (var kvp in _attributesMap)
                    {
                        if (kvp.Key.Any(x => string.Equals(x, trimmedAttrText, StringComparison.OrdinalIgnoreCase)))
                        {
                            attr |= kvp.Value;
                            isFound = true;
                            break;
                        }
                    }
                }
                if (!isFound) throw new ArgumentException($"Invalid attributes string: {text}");
            }
            return attr;
        }
    }
}
