namespace ItemStorageManager.ItemStorage
{
    internal class AttributeFunctions
    {
        private static Dictionary<string, FileAttributes> AttributeMap = null;

        private static void LoadAttributeMap()
        {
            AttributeMap ??= new Dictionary<string, FileAttributes>(StringComparer.OrdinalIgnoreCase)
            {
                { "Archive", FileAttributes.Archive },
                { "Compressed", FileAttributes.Compressed },
                { "Device", FileAttributes.Device },
                { "Directory", FileAttributes.Directory },
                { "Encrypted", FileAttributes.Encrypted },
                { "Hidden", FileAttributes.Hidden },
                { "H", FileAttributes.Hidden },
                { "IntegrityStream", FileAttributes.IntegrityStream },
                { "None", FileAttributes.Normal },
                { "Normal", FileAttributes.Normal },
                { "NoScrubData", FileAttributes.NoScrubData },
                { "NotContentIndexed", FileAttributes.NotContentIndexed },
                { "Offline", FileAttributes.Offline },
                { "ReadOnly", FileAttributes.ReadOnly },
                { "R", FileAttributes.ReadOnly },
                { "ReparsePoint", FileAttributes.ReparsePoint },
                { "SparseFile", FileAttributes.SparseFile },
                { "System", FileAttributes.System },
                { "S", FileAttributes.System },
                { "Temporary", FileAttributes.Temporary }
            };
        }

        /// <summary>
        /// Parse FileAttributes from string
        /// </summary>
        /// <param name="attributeText"></param>
        /// <param name="currentAttribute"></param>
        /// <returns></returns>
        public static FileAttributes ParseFileAttributes(string attributeText, FileAttributes currentAttribute)
        {
            LoadAttributeMap();

            string[] attributes = attributeText.Split(',').Select(x => x.Trim()).ToArray();

            var needReset = true;
            var ret = currentAttribute;
            foreach (var attrib in attributes)
            {
                if (attrib.StartsWith("-"))
                {
                    if (AttributeMap.ContainsKey(attrib.TrimStart('-')))
                    {
                        ret = ret & ~AttributeMap[attrib.TrimStart('-')];
                    }
                }
                else if (attrib.StartsWith("+"))
                {
                    if (AttributeMap.ContainsKey(attrib.TrimStart('+')))
                    {
                        ret = ret | AttributeMap[attrib.TrimStart('+')];
                    }
                }
                else
                {
                    string text = attrib;
                    if (AttributeMap.ContainsKey(text))
                    {
                        if (needReset)
                        {
                            ret = FileAttributes.None;
                            needReset = false;
                        }
                        ret = ret | AttributeMap[text];
                    }
                }
            }
            return ret;
        }
    }
}
