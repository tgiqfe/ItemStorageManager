using System.ComponentModel;
using System.Security.AccessControl;

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



        #region Enum parameter parsing maps.

        private static Dictionary<string, FileSystemRights> _fileRightsMap = null;
        private static Dictionary<string, RegistryRights> _registryRightsMap = null;
        private static Dictionary<string, InheritanceFlags> _inheritanceMap = null;
        private static Dictionary<string, PropagationFlags> _propagationMap = null;
        private static Dictionary<string, AccessControlType> _accessTypeMap = null;

        private static void InitializeRightsMap()
        {
            _fileRightsMap = new Dictionary<string, FileSystemRights>(StringComparer.OrdinalIgnoreCase)
            {
                { "AppendData", FileSystemRights.AppendData },
                { "ChangePermissions", FileSystemRights.ChangePermissions },
                { "CreateDirectories", FileSystemRights.CreateDirectories },
                { "CreateFiles", FileSystemRights.CreateFiles },
                { "Delete", FileSystemRights.Delete },
                { "DeleteSubdirectoriesAndFiles", FileSystemRights.DeleteSubdirectoriesAndFiles },
                { "ExecuteFile", FileSystemRights.ExecuteFile },
                { "FullControl", FileSystemRights.FullControl },
                { "ListDirectory", FileSystemRights.ListDirectory },
                { "Modify", FileSystemRights.Modify },
                { "Read", FileSystemRights.Read },
                { "ReadAndExecute", FileSystemRights.ReadAndExecute },
                { "ReadAttributes", FileSystemRights.ReadAttributes },
                { "ReadData" , FileSystemRights.ReadData },
                { "ReadExtendedAttributes", FileSystemRights.ReadExtendedAttributes },
                { "ReadPermissions", FileSystemRights.ReadPermissions },
                { "Synchronize", FileSystemRights.Synchronize },
                { "TakeOwnership", FileSystemRights.TakeOwnership },
                { "Traverse", FileSystemRights.Traverse },
                { "Write", FileSystemRights.Write },
                { "WriteAttributes", FileSystemRights.WriteAttributes },
                { "WriteData", FileSystemRights.WriteData },
                { "WriteExtendedAttributes", FileSystemRights.WriteExtendedAttributes }
            };
        }

        private static void InitializeRegistryRightsMap()
        {
            _registryRightsMap = new Dictionary<string, RegistryRights>(StringComparer.OrdinalIgnoreCase)
            {
                { "QueryValues", RegistryRights.QueryValues },
                { "SetValue", RegistryRights.SetValue },
                { "CreateSubKey", RegistryRights.CreateSubKey },
                { "EnumerateSubKeys", RegistryRights.EnumerateSubKeys },
                { "Notify", RegistryRights.Notify },
                { "CreateLink", RegistryRights.CreateLink },
                { "Delete", RegistryRights.Delete },
                { "ReadPermissions", RegistryRights.ReadPermissions },
                { "WriteKey", RegistryRights.WriteKey },
                { "ExecuteKey", RegistryRights.ExecuteKey },
                { "ReadKey", RegistryRights.ReadKey },
                { "ChangePermissions", RegistryRights.ChangePermissions },
                { "TakeOwnership", RegistryRights.TakeOwnership },
                { "FullControl", RegistryRights.FullControl },
            };
        }

        private static void InitializeInheritanceMap()
        {
            _inheritanceMap = new Dictionary<string, InheritanceFlags>(StringComparer.OrdinalIgnoreCase)
            {
                { "ContainerInherit", InheritanceFlags.ContainerInherit },
                { "None", InheritanceFlags.None },
                { "ObjectInherit", InheritanceFlags.ObjectInherit }
            };
        }

        private static void InitializePropagationMap()
        {
            _propagationMap = new Dictionary<string, PropagationFlags>(StringComparer.OrdinalIgnoreCase)
            {
                { "None", PropagationFlags.None },
                { "NoPropagateInherit", PropagationFlags.NoPropagateInherit },
                { "InheritOnly", PropagationFlags.InheritOnly },
            };
        }

        
        private static void InitializeRightsTypeMap()
        {
            _accessTypeMap = new Dictionary<string, AccessControlType>(StringComparer.OrdinalIgnoreCase)
            {
                { "Allow", AccessControlType.Allow },
                { "Deny", AccessControlType.Deny }
            };
        }

        #endregion

        /// <summary>
        /// Parse FileSystemRights from string
        /// </summary>
        /// <param name="rightsText"></param>
        /// <returns></returns>
        public static FileSystemRights ParseFileSystemRights(string rightsText)
        {
            if (_fileRightsMap == null) InitializeRightsMap();
            FileSystemRights rights = 0;
            string[] rightsParts = rightsText.Split(',').Select(x => x.Trim()).ToArray();
            foreach (var part in rightsParts)
            {
                if (_fileRightsMap.ContainsKey(part))
                {
                    rights |= _fileRightsMap[part];
                }
            }
            return rights;
        }

        /// <summary>
        /// Prase RegistryRights from string
        /// </summary>
        /// <param name="rightsText"></param>
        /// <returns></returns>
        public static RegistryRights ParseRegistryRights(string rightsText)
        {
            if (_registryRightsMap == null) InitializeRegistryRightsMap();
            RegistryRights rights = 0;
            string[] rightsParts = rightsText.Split(',').Select(x => x.Trim()).ToArray();
            foreach (var part in rightsParts)
            {
                if (_registryRightsMap.ContainsKey(part))
                {
                    rights |= _registryRightsMap[part];
                }
            }
            return rights;
        }

        /// <summary>
        /// Parse InheritanceFlags from string
        /// </summary>
        /// <param name="inheritanceText"></param>
        /// <returns></returns>
        public static InheritanceFlags ParseInheritanceFlags(string inheritanceText)
        {
            if (_inheritanceMap == null) InitializeInheritanceMap();
            return _inheritanceMap.ContainsKey(inheritanceText) ?
                _inheritanceMap[inheritanceText] :
                InheritanceFlags.None;
        }

        /// <summary>
        /// Parse PropagationFlags from string
        /// </summary>
        /// <param name="propagationText"></param>
        /// <returns></returns>
        public static PropagationFlags ParsePropagationFlags(string propagationText)
        {
            if (_propagationMap == null) InitializePropagationMap();
            return _propagationMap.ContainsKey(propagationText) ?
                _propagationMap[propagationText] :
                PropagationFlags.None;
        }

        /// <summary>
        /// Parse AccessControlType from string
        /// </summary>
        /// <param name="accessTypeText"></param>
        /// <returns></returns>
        public static AccessControlType ParseAccessControlType(string accessTypeText)
        {
            if (_accessTypeMap == null) InitializeRightsTypeMap();
            return _accessTypeMap.ContainsKey(accessTypeText) ?
                _accessTypeMap[accessTypeText] :
                AccessControlType.Allow;
        }
    }
}
