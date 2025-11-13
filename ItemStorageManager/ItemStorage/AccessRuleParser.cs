using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ItemStorageManager.ItemStorage
{
    internal class AccessRuleParser
    {
        #region FileSystemRights mapping

        /// <summary>
        /// FileSystemRights mapping dictionary
        /// </summary>
        private static Dictionary<string[], FileSystemRights> _mapFileSystemRights = null;
        private static void InitializeFileSystemRightsMap()
        {
            _mapFileSystemRights = new Dictionary<string[], FileSystemRights>
            {
                { new string[]{ "AppendData" }, FileSystemRights.AppendData },
                { new string[]{ "ChangePermissions" }, FileSystemRights.ChangePermissions },
                { new string[]{ "CreateDirectories" }, FileSystemRights.CreateDirectories },
                { new string[]{ "CreateFiles" }, FileSystemRights.CreateFiles },
                { new string[]{ "Delete", "Del" }, FileSystemRights.Delete },
                { new string[]{ "DeleteSubdirectoriesAndFiles" }, FileSystemRights.DeleteSubdirectoriesAndFiles },
                { new string[]{ "ExecuteFile", "XFile" }, FileSystemRights.ExecuteFile },
                { new string[]{ "FullControl", "Full", "Ful" }, FileSystemRights.FullControl },
                { new string[]{ "ListDirectory" }, FileSystemRights.ListDirectory },
                { new string[]{ "Modify", "Mod", "Modified" }, FileSystemRights.Modify },
                { new string[]{ "Read", "R" }, FileSystemRights.Read },
                { new string[]{ "ReadAndExecute", "ReadAndX", "R&X" }, FileSystemRights.ReadAndExecute },
                { new string[]{ "ReadAttributes" }, FileSystemRights.ReadAttributes },
                { new string[]{ "ReadData" }, FileSystemRights.ReadData },
                { new string[]{ "ReadExtendedAttributes" }, FileSystemRights.ReadExtendedAttributes },
                { new string[]{ "ReadPermissions" }, FileSystemRights.ReadPermissions },
                { new string[]{ "Synchronize" }, FileSystemRights.Synchronize },
                { new string[]{ "TakeOwnership" }, FileSystemRights.TakeOwnership },
                { new string[]{ "Traverse" }, FileSystemRights.Traverse },
                { new string[]{ "Write", "W" }, FileSystemRights.Write },
                { new string[]{ "WriteAttributes" }, FileSystemRights.WriteAttributes },
                { new string[]{ "WriteData" }, FileSystemRights.WriteData },
                { new string[]{ "WriteExtendedAttributes" }, FileSystemRights.WriteExtendedAttributes },
            };
        }
        public static FileSystemRights StringToFileSystemRights(string text)
        {
            if (_mapFileSystemRights == null) InitializeFileSystemRightsMap();
            foreach (var kvp in _mapFileSystemRights)
            {
                if (kvp.Key.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase)))
                {
                    return kvp.Value;
                }
            }
            throw new ArgumentException($"Invalid file rights string: {text}");
        }
        public static string FileSystemRightsToString(FileSystemRights val)
        {
            if (_mapFileSystemRights == null) InitializeFileSystemRightsMap();
            foreach (var kvp in _mapFileSystemRights)
            {
                if (kvp.Value == val)
                {
                    return kvp.Key[0];
                }
            }
            return "Unknown";
        }

        #endregion
        #region RegistryRights mapping

        /// <summary>
        /// RegistryRights mapping dictionary
        /// </summary>
        private static Dictionary<string[], RegistryRights> _mapRegistryRights = null;
        private static void InitializeRegistryRightsMap()
        {
            _mapRegistryRights = new Dictionary<string[], RegistryRights>
            {
                { new string[] { "QueryValues" }, RegistryRights.QueryValues },
                { new string[] { "SetValue", "Set" }, RegistryRights.SetValue },
                { new string[] { "CreateSubKey" }, RegistryRights.CreateSubKey },
                { new string[] { "EnumerateSubKeys" }, RegistryRights.EnumerateSubKeys },
                { new string[] { "Notify", "Notice" }, RegistryRights.Notify },
                { new string[] { "CreateLink" }, RegistryRights.CreateLink },
                { new string[] { "Delete", "Del" }, RegistryRights.Delete },
                { new string[] { "ReadPermissions" }, RegistryRights.ReadPermissions },
                { new string[] { "WriteKey", "Write", "W" }, RegistryRights.WriteKey },
                { new string[] { "ExecuteKey" }, RegistryRights.ExecuteKey },
                { new string[] { "ReadKey", "Read", "R" }, RegistryRights.ReadKey },
                { new string[] { "ChangePermissions" }, RegistryRights.ChangePermissions },
                { new string[] { "TakeOwnership", "TakeOwn", "TakeOwner" }, RegistryRights.TakeOwnership },
                { new string[] { "FullControl", "Full" }, RegistryRights.FullControl },
            };
        }
        public static RegistryRights StringToRegistryRights(string text)
        {
            if (_mapRegistryRights == null) InitializeRegistryRightsMap();
            foreach (var kvp in _mapRegistryRights)
            {
                if (kvp.Key.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase)))
                {
                    return kvp.Value;
                }
            }
            throw new ArgumentException($"Invalid registry rights string: {text}");
        }
        public static string RegistryRightsToString(RegistryRights val)
        {
            if (_mapRegistryRights == null) InitializeRegistryRightsMap();
            foreach (var kvp in _mapRegistryRights)
            {
                if (kvp.Value == val)
                {
                    return kvp.Key[0];
                }
            }
            return "Unknown";
        }

        #endregion
        #region InheritanceFlags mapping

        /// <summary>
        /// InheritanceFlags mapping dictionary
        /// </summary>
        private static Dictionary<string[], InheritanceFlags> _mapInheritanceFlags = null;
        private static void InitializeInheritanceFlagsMap()
        {
            _mapInheritanceFlags = new Dictionary<string[], InheritanceFlags>
            {
                { new string[] { "ContainerInherit", "Container", "CI" }, InheritanceFlags.ContainerInherit },
                { new string[] { "None", "No" }, InheritanceFlags.None },
                { new string[] { "ObjectInherit", "Object", "OI" }, InheritanceFlags.ObjectInherit },
            };
        }
        public static InheritanceFlags StringToInheritanceFlags(string text)
        {
            if (_mapInheritanceFlags == null) InitializeInheritanceFlagsMap();
            foreach (var kvp in _mapInheritanceFlags)
            {
                if (kvp.Key.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase)))
                {
                    return kvp.Value;
                }
            }
            throw new ArgumentException($"Invalid inheritance flags string: {text}");
        }
        public static string InheritanceFlagsToString(InheritanceFlags val)
        {
            if (_mapInheritanceFlags == null) InitializeInheritanceFlagsMap();
            foreach (var kvp in _mapInheritanceFlags)
            {
                if (kvp.Value == val)
                {
                    return kvp.Key[0];
                }
            }
            return "Unknown";
        }

        #endregion
        #region PropagationFlags mapping

        /// <summary>
        /// PropagationFlags mapping dictionary
        /// </summary>
        private static Dictionary<string[], PropagationFlags> _mapPropagationFlags = null;
        private static void InitializePropagationFlagsMap()
        {
            _mapPropagationFlags = new Dictionary<string[], PropagationFlags>
            {
                { new string[] { "None", "No" }, PropagationFlags.None },
                { new string[] { "NoPropagateInherit", "NoPropagate", "NPI" }, PropagationFlags.NoPropagateInherit },
                { new string[] { "InheritOnly", "IO" }, PropagationFlags.InheritOnly },
            };
        }
        public static PropagationFlags StringToPropagationFlags(string text)
        {
            if (_mapPropagationFlags == null) InitializePropagationFlagsMap();
            foreach (var kvp in _mapPropagationFlags)
            {
                if (kvp.Key.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase)))
                {
                    return kvp.Value;
                }
            }
            throw new ArgumentException($"Invalid propagation flags string: {text}");
        }
        public static string PropagationFlagsToString(PropagationFlags val)
        {
            if (_mapPropagationFlags == null) InitializePropagationFlagsMap();
            foreach (var kvp in _mapPropagationFlags)
            {
                if (kvp.Value == val)
                {
                    return kvp.Key[0];
                }
            }
            return "Unknown";
        }

        #endregion
        #region AccessControlType mapping

        /// <summary>
        /// AccessControlType mapping dictionary
        /// </summary>
        private static Dictionary<string[], AccessControlType> _mapAccessControlType = null;
        private static void InitializeAccessControlTypeMap()
        {
            _mapAccessControlType = new Dictionary<string[], AccessControlType>
            {
                { new string[] { "Allow", "A" }, AccessControlType.Allow },
                { new string[] { "Deny", "Block", "D" }, AccessControlType.Deny },
            };
        }
        public static AccessControlType StringToAccessControlType(string text)
        {
            if (_mapAccessControlType == null) InitializeAccessControlTypeMap();
            foreach (var kvp in _mapAccessControlType)
            {
                if (kvp.Key.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase)))
                {
                    return kvp.Value;
                }
            }
            throw new ArgumentException($"Invalid access control type string: {text}");
        }
        public static string AccessControlTypeToString(AccessControlType val)
        {
            if (_mapAccessControlType == null) InitializeAccessControlTypeMap();
            foreach (var kvp in _mapAccessControlType)
            {
                if (kvp.Value == val)
                {
                    return kvp.Key[0];
                }
            }
            return "Unknown";
        }

        #endregion
    }
}
