using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemStorageManager.ItemStorage.Attrib
{
    internal class AttributeFunctions
    {
        private static Dictionary<string, FileAttributes> AttribPair = new Dictionary<string, FileAttributes>(StringComparer.OrdinalIgnoreCase)
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

        public static FileAttributes GetProcessedAttributes(string attributeText, FileAttributes currentAttribute)
        {
            string[] attributes = attributeText.Split(',').Select(x => x.Trim()).ToArray();

            var needReset = true;
            var ret = currentAttribute;
            foreach (var attrib in attributes)
            {
                if (attrib.StartsWith("-"))
                {
                    if (AttribPair.ContainsKey(attrib.TrimStart('-')))
                    {
                        ret = ret & (~AttribPair[attrib.TrimStart('-')]);
                    }
                }
                else if (attrib.StartsWith("+"))
                {
                    if (AttribPair.ContainsKey(attrib.TrimStart('+')))
                    {
                        ret = ret | AttribPair[attrib.TrimStart('+')];
                    }
                }
                else
                {
                    string text = attrib;
                    if (AttribPair.ContainsKey(text))
                    {
                        if (needReset)
                        {
                            ret = FileAttributes.None;
                            needReset = false;
                        }
                        ret = ret | AttribPair[text];
                    }
                }
            }
            return ret;
        }
    }
}
