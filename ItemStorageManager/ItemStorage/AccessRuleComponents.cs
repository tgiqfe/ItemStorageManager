using System.ComponentModel;

namespace ItemStorageManager.ItemStorage
{
    internal class AccessRuleComponents
    {
        /// <summary>
        /// Rights mapping helper class
        /// Enum: FileSystemRights
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class FileRightsMap<T> where T : Enum
        {
            private static Dictionary<string[], T> _map = null;
            private static void Initialize()
            {
                _map = new()
                {
                    { new string[]{ "AppendData" }, (T)Enum.Parse(typeof(T), "AppendData") },
                    { new string[]{ "ChangePermissions" }, (T)Enum.Parse(typeof(T), "ChangePermissions") },
                    { new string[]{ "CreateDirectories" }, (T)Enum.Parse(typeof(T), "CreateDirectories") },
                    { new string[]{ "CreateFiles" }, (T)Enum.Parse(typeof(T), "CreateFiles") },
                    { new string[]{ "Delete", "Del" }, (T)Enum.Parse(typeof(T), "Delete") },
                    { new string[]{ "DeleteSubdirectoriesAndFiles" }, (T)Enum.Parse(typeof(T), "DeleteSubdirectoriesAndFiles") },
                    { new string[]{ "ExecuteFile" }, (T)Enum.Parse(typeof(T), "ExecuteFile") },
                    { new string[]{ "FullControl", "Full" }, (T)Enum.Parse(typeof(T), "FullControl") },
                    { new string[]{ "ListDirectory" }, (T)Enum.Parse(typeof(T), "ListDirectory") },
                    { new string[]{ "Modify", "Mod" }, (T)Enum.Parse(typeof(T), "Modify") },
                    { new string[]{ "Read" }, (T)Enum.Parse(typeof(T), "Read") },
                    { new string[]{ "ReadAndExecute" }, (T)Enum.Parse(typeof(T), "ReadAndExecute") },
                    { new string[]{ "ReadAttributes" }, (T)Enum.Parse(typeof(T), "ReadAttributes") },
                    { new string[]{ "ReadData" }, (T)Enum.Parse(typeof(T), "ReadData") },
                    { new string[]{ "ReadExtendedAttributes" }, (T)Enum.Parse(typeof(T), "ReadExtendedAttributes") },
                    { new string[]{ "ReadPermissions" }, (T)Enum.Parse(typeof(T), "ReadPermissions") },
                    { new string[]{ "Synchronize" }, (T)Enum.Parse(typeof(T), "Synchronize") },
                    { new string[]{ "TakeOwnership" }, (T)Enum.Parse(typeof(T), "TakeOwnership") },
                    { new string[]{ "Traverse" }, (T)Enum.Parse(typeof(T), "Traverse") },
                    { new string[]{ "Write" }, (T)Enum.Parse(typeof(T), "Write") },
                    { new string[]{ "WriteAttributes" }, (T)Enum.Parse(typeof(T), "WriteAttributes") },
                    { new string[]{ "WriteData" }, (T)Enum.Parse(typeof(T), "WriteData") },
                    { new string[]{ "WriteExtendedAttributes" }, (T)Enum.Parse(typeof(T), "WriteExtendedAttributes") },
                };
            }
            public static T StringToValue(string text)
            {
                if (_map == null) Initialize();
                foreach (var kvp in _map)
                {
                    if (kvp.Key.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase)))
                    {
                        return kvp.Value;
                    }
                }
                throw new InvalidEnumArgumentException($"Invalid direction string: {text}");
            }
            public static string ValueToString(T val)
            {
                if (_map == null) Initialize();
                foreach (var kvp in _map)
                {
                    if (kvp.Value.Equals(val))
                    {
                        return kvp.Key[0];
                    }
                }
                return "Unknown";
            }
        }

        /// <summary>
        /// Rights mapping helper class
        /// Enum: RegistryRights
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class RegistryRightsMap<T>
        {
            private static Dictionary<string[], T> _map = null;
            private static void Initialize()
            {
                _map = new()
                {
                    { new string[] { "QueryValues" }, (T)Enum.Parse(typeof(T), "QueryValues") },
                    { new string[] { "SetValue" }, (T)Enum.Parse(typeof(T), "SetValue") },
                    { new string[] { "CreateSubKey" }, (T)Enum.Parse(typeof(T), "CreateSubKey") },
                    { new string[] { "EnumerateSubKeys" }, (T)Enum.Parse(typeof(T), "EnumerateSubKeys") },
                    { new string[] { "Notify" }, (T)Enum.Parse(typeof(T), "Notify") },
                    { new string[] { "CreateLink" }, (T)Enum.Parse(typeof(T), "CreateLink") },
                    { new string[] { "Delete", "Del" }, (T)Enum.Parse(typeof(T), "Delete") },
                    { new string[] { "ReadPermissions" }, (T)Enum.Parse(typeof(T), "ReadPermissions") },
                    { new string[] { "WriteKey" }, (T)Enum.Parse(typeof(T), "WriteKey") },
                    { new string[] { "ExecuteKey" }, (T)Enum.Parse(typeof(T), "ExecuteKey") },
                    { new string[] { "ReadKey" }, (T)Enum.Parse(typeof(T), "ReadKey") },
                    { new string[] { "ChangePermissions" }, (T)Enum.Parse(typeof(T), "ChangePermissions") },
                    { new string[] { "TakeOwnership" }, (T)Enum.Parse(typeof(T), "TakeOwnership") },
                    { new string[] { "FullControl", "Full" }, (T)Enum.Parse(typeof(T), "FullControl") },
                };
            }
            public static T StringToValue(string text)
            {
                if (_map == null) Initialize();
                foreach (var kvp in _map)
                {
                    if (kvp.Key.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase)))
                    {
                        return kvp.Value;
                    }
                }
                throw new InvalidEnumArgumentException($"Invalid direction string: {text}");
            }
            public static string ValueToString(T val)
            {
                if (_map == null) Initialize();
                foreach (var kvp in _map)
                {
                    if (kvp.Value.Equals(val))
                    {
                        return kvp.Key[0];
                    }
                }
                return "Unknown";
            }
        }

        /// <summary>
        /// InheritanceFlags mapping helper class
        /// Enum: InheritanceFlags
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class InheritanceFlagsMap<T>
        {
            private static Dictionary<string[], T> _map = null;
            private static void Initialize()
            {
                _map = new()
                {
                    { new string[] { "ContainerInherit", "Container", "CI" }, (T)Enum.Parse(typeof(T), "ContainerInherit") },
                    { new string[] { "None" }, (T)Enum.Parse(typeof(T), "None") },
                    { new string[] { "ObjectInherit", "Object", "OI" }, (T)Enum.Parse(typeof(T), "ObjectInherit") },
                };
            }
            public static T StringToValue(string text)
            {
                if (_map == null) Initialize();
                foreach (var kvp in _map)
                {
                    if (kvp.Key.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase)))
                    {
                        return kvp.Value;
                    }
                }
                throw new InvalidEnumArgumentException($"Invalid direction string: {text}");
            }
            public static string ValueToString(T val)
            {
                if (_map == null) Initialize();
                foreach (var kvp in _map)
                {
                    if (kvp.Value.Equals(val))
                    {
                        return kvp.Key[0];
                    }
                }
                return "Unknown";
            }
        }

        /// <summary>
        /// PropagationFlags mapping helper class
        /// Enum: PropagationFlags
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class PropagationFlagsMap<T>
        {
            private static Dictionary<string[], T> _map = null;
            private static void Initialize()
            {
                _map = new()
                {
                    { new string[] { "None" }, (T)Enum.Parse(typeof(T), "None") },
                    { new string[] { "NoPropagateInherit", "NoPropagate", "NPI" }, (T)Enum.Parse(typeof(T), "NoPropagateInherit") },
                    { new string[] { "InheritOnly", "IO" }, (T)Enum.Parse(typeof(T), "InheritOnly") },
                };
            }
            public static T StringToValue(string text)
            {
                if (_map == null) Initialize();
                foreach (var kvp in _map)
                {
                    if (kvp.Key.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase)))
                    {
                        return kvp.Value;
                    }
                }
                throw new InvalidEnumArgumentException($"Invalid direction string: {text}");
            }
            public static string ValueToString(T val)
            {
                if (_map == null) Initialize();
                foreach (var kvp in _map)
                {
                    if (kvp.Value.Equals(val))
                    {
                        return kvp.Key[0];
                    }
                }
                return "Unknown";
            }
        }

        /// <summary>
        /// AccessControlType mapping helper class
        /// Enum: AccessControlType
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class AccessControlTypeMap<T>
        {
            private static Dictionary<string[], T> _map = null;
            private static void Initialize()
            {
                _map = new()
                {
                    { new string[] { "Allow" }, (T)Enum.Parse(typeof(T), "Allow") },
                    { new string[] { "Deny", "Block" }, (T)Enum.Parse(typeof(T), "Deny") },
                };
            }
            public static T StringToValue(string text)
            {
                if (_map == null) Initialize();
                foreach (var kvp in _map)
                {
                    if (kvp.Key.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase)))
                    {
                        return kvp.Value;
                    }
                }
                throw new InvalidEnumArgumentException($"Invalid direction string: {text}");
            }
            public static string ValueToString(T val)
            {
                if (_map == null) Initialize();
                foreach (var kvp in _map)
                {
                    if (kvp.Value.Equals(val))
                    {
                        return kvp.Key[0];
                    }
                }
                return "Unknown";
            }
        }
    }
}
