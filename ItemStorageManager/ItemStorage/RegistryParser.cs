using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace ItemStorageManager.ItemStorage
{
    internal class RegistryParser
    {
        #region RegistryValueKind mapping.

        private static Dictionary<string[], RegistryValueKind> _mapRegistryValueKind = null;
        private static void InitializeRegistryValueKind()
        {
            _mapRegistryValueKind = new Dictionary<string[], RegistryValueKind>
            {
                { new string[] { "REG_SZ", "String" }, RegistryValueKind.String },
                { new string[] { "REG_BINARY", "Binary", "Bytes" }, RegistryValueKind.Binary },
                { new string[] { "REG_DWORD", "Dword", "Int" }, RegistryValueKind.DWord },
                { new string[] { "REG_QWORD", "Qword", "Long" }, RegistryValueKind.QWord },
                { new string[] { "REG_MULTI_SZ", "MultiString", "Strings" }, RegistryValueKind.MultiString },
                { new string[] { "REG_EXPAND_SZ", "ExpandString" }, RegistryValueKind.ExpandString },
                { new string[] { "REG_NONE", "None" }, RegistryValueKind.None },
            };
        }
        public static RegistryValueKind StringToRegistryValueKind(string text)
        {
            if (_mapRegistryValueKind == null) InitializeRegistryValueKind();
            foreach (var kvp in _mapRegistryValueKind)
            {
                if (kvp.Key.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase)))
                {
                    return kvp.Value;
                }
            }
            throw new ArgumentException($"Invalid RegistryValueKind string: {text}");
        }
        public static string RegistryValueKindToString(RegistryValueKind valueKind)
        {
            if (_mapRegistryValueKind == null) InitializeRegistryValueKind();
            foreach (var kvp in _mapRegistryValueKind)
            {
                if (kvp.Value == valueKind)
                {
                    return kvp.Key[0];
                }
            }
            return "Unknown";
        }

        #endregion
        #region Registry value (data) parsing.

        public static object StringToRegistryValue(string dataString, RegistryValueKind valueKind)
        {
            return valueKind switch
            {
                RegistryValueKind.String => dataString,
                RegistryValueKind.DWord => int.TryParse(dataString, out int dwordValue) ? dwordValue.ToString() : 0,
                RegistryValueKind.QWord => long.TryParse(dataString, out long qwordValue) ? qwordValue.ToString() : 0,
                RegistryValueKind.ExpandString => dataString,
                RegistryValueKind.Binary => stringToRegBinary(dataString),
                RegistryValueKind.MultiString => Regex.Split(dataString, "\\0").Select(s => s.Trim()).ToArray(),
                RegistryValueKind.None => null,
                _ => null,
            };

            byte[] stringToRegBinary(string val)
            {
                if (Regex.IsMatch(val, @"^([0-9A-Fa-f]{2})+$"))
                {
                    var bytes = new byte[val.Length / 2];
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        bytes[i] = Convert.ToByte(val.Substring(i * 2, 2), 16);
                    }
                    return bytes;
                }
                return new byte[0] { };
            }
        }

        public static string RegistryValueToString(object data, RegistryValueKind valueKind)
        {
            return valueKind switch
            {
                RegistryValueKind.String => data as string,
                RegistryValueKind.DWord => data.ToString(),
                RegistryValueKind.QWord => data.ToString(),
                RegistryValueKind.ExpandString => data as string,
                RegistryValueKind.Binary => BitConverter.ToString(data as byte[]).Replace("-", "").ToUpper(),
                RegistryValueKind.MultiString => string.Join("\\0", data as string[]),
                RegistryValueKind.None => null,
                _ => null,
            };
        }

        #endregion
    }
}
