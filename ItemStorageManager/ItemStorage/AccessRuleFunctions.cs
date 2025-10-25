using System.Security.AccessControl;

namespace ItemStorageManager.ItemStorage
{
    internal class AccessRuleFunctions
    {
        #region Enum parameter parsing maps.

        private static Dictionary<string, FileSystemRights> RightsMap = null;
        private static Dictionary<string, RegistryRights> RegistryRightsMap = null;
        private static Dictionary<string, InheritanceFlags> InheritanceMap = null;
        private static Dictionary<string, PropagationFlags> PropagationMap = null;
        private static Dictionary<string, AccessControlType> AccessTypeMap = null;

        private static void LoadRightsMap()
        {
            RightsMap ??= new Dictionary<string, FileSystemRights>(StringComparer.OrdinalIgnoreCase)
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

        private static void LoadRegistryRightsMap()
        {
            RegistryRightsMap ??= new Dictionary<string, RegistryRights>(StringComparer.OrdinalIgnoreCase)
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

        private static void LoadInheritanceMap()
        {
            InheritanceMap ??= new Dictionary<string, InheritanceFlags>(StringComparer.OrdinalIgnoreCase)
            {
                { "ContainerInherit", InheritanceFlags.ContainerInherit },
                { "None", InheritanceFlags.None },
                { "ObjectInherit", InheritanceFlags.ObjectInherit }
            };
        }

        private static void LoadPropagationMap()
        {
            PropagationMap ??= new Dictionary<string, PropagationFlags>(StringComparer.OrdinalIgnoreCase)
            {
                { "None", PropagationFlags.None },
                { "NoPropagateInherit", PropagationFlags.NoPropagateInherit },
                { "InheritOnly", PropagationFlags.InheritOnly },
            };
        }

        private static void LoadRightsTypeMap()
        {
            AccessTypeMap ??= new Dictionary<string, AccessControlType>(StringComparer.OrdinalIgnoreCase)
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
            LoadRightsMap();
            FileSystemRights rights = 0;
            string[] rightsParts = rightsText.Split(',').Select(x => x.Trim()).ToArray();
            foreach (var part in rightsParts)
            {
                if (RightsMap.ContainsKey(part))
                {
                    rights |= RightsMap[part];
                }
            }
            return rights;
        }

        public static RegistryRights ParseRegistryRights(string rightsText)
        {
            LoadRegistryRightsMap();
            RegistryRights rights = 0;
            string[] rightsParts = rightsText.Split(',').Select(x => x.Trim()).ToArray();
            foreach (var part in rightsParts)
            {
                if (RegistryRightsMap.ContainsKey(part))
                {
                    rights |= RegistryRightsMap[part];
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
            LoadInheritanceMap();
            return InheritanceMap.ContainsKey(inheritanceText) ?
                InheritanceMap[inheritanceText] :
                InheritanceFlags.None;
        }

        /// <summary>
        /// Parse PropagationFlags from string
        /// </summary>
        /// <param name="propagationText"></param>
        /// <returns></returns>
        public static PropagationFlags ParsePropagationFlags(string propagationText)
        {
            LoadPropagationMap();
            return PropagationMap.ContainsKey(propagationText) ?
                PropagationMap[propagationText] :
                PropagationFlags.None;
        }

        /// <summary>
        /// Parse AccessControlType from string
        /// </summary>
        /// <param name="accessTypeText"></param>
        /// <returns></returns>
        public static AccessControlType ParseAccessControlType(string accessTypeText)
        {
            LoadRightsTypeMap();
            return AccessTypeMap.ContainsKey(accessTypeText) ?
                AccessTypeMap[accessTypeText] :
                AccessControlType.Allow;
        }
    }
}
