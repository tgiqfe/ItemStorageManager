using System.Security.AccessControl;

namespace ItemStorageManager.ItemStorage
{
    internal class AccessRuleMapping
    {
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
