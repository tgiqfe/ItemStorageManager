using ItemStorageManager.Functions;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ItemStorageManager.ItemStorage
{
    public class AccessRuleParser
    {
        #region FileSystemRights mapping

        /// <summary>
        /// FileSystemRights mapping dictionary
        /// </summary>
        private static Dictionary<string[], FileSystemRights> _mapFileSystemRights = null;
        private static void InitializeFileSystemRights()
        {
            _mapFileSystemRights = new()
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
            if (_mapFileSystemRights == null) InitializeFileSystemRights();
            /*
            foreach (var kvp in _mapFileSystemRights)
            {
                if (kvp.Key.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase)))
                {
                    return kvp.Value;
                }
            }
            throw new ArgumentException($"Invalid file rights string: {text}");
            */
            return TextFunctions.StringToFlags<FileSystemRights>(text, _mapFileSystemRights);
        }
        public static string FileSystemRightsToString(FileSystemRights val)
        {
            if (_mapFileSystemRights == null) InitializeFileSystemRights();
            /*
            foreach (var kvp in _mapFileSystemRights)
            {
                if (kvp.Value == val)
                {
                    return kvp.Key[0];
                }
            }
            return "Unknown";
            */
            return TextFunctions.FlagsToString<FileSystemRights>(val, _mapFileSystemRights);
        }
        public static string GetFileSystemRightsString(string text)
        {
            if (_mapFileSystemRights == null) InitializeFileSystemRights();
            /*
            foreach (var key in _mapFileSystemRights.Keys)
            {
                if (key.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase)))
                {
                    return key[0];
                }
            }
            throw new ArgumentException($"Invalid file system rights string: {text}");
            */
            return TextFunctions.GetCorrect<FileSystemRights>(text, _mapFileSystemRights);
        }

        #endregion
        #region RegistryRights mapping

        /// <summary>
        /// RegistryRights mapping dictionary
        /// </summary>
        private static Dictionary<string[], RegistryRights> _mapRegistryRights = null;
        private static void InitializeRegistryRights()
        {
            _mapRegistryRights = new()
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
            if (_mapRegistryRights == null) InitializeRegistryRights();
            /*
            foreach (var kvp in _mapRegistryRights)
            {
                if (kvp.Key.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase)))
                {
                    return kvp.Value;
                }
            }
            throw new ArgumentException($"Invalid registry rights string: {text}");
            */
            return TextFunctions.StringToFlags<RegistryRights>(text, _mapRegistryRights);
        }
        public static string RegistryRightsToString(RegistryRights val)
        {
            if (_mapRegistryRights == null) InitializeRegistryRights();
            /*
            foreach (var kvp in _mapRegistryRights)
            {
                if (kvp.Value == val)
                {
                    return kvp.Key[0];
                }
            }
            return "Unknown";
            */
            return TextFunctions.FlagsToString<RegistryRights>(val, _mapRegistryRights);
        }
        public static string GetRegistryRightsString(string text)
        {
            if (_mapRegistryRights == null) InitializeRegistryRights();
            /*
            foreach (var key in _mapRegistryRights.Keys)
            {
                if (key.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase)))
                {
                    return key[0];
                }
            }
            throw new ArgumentException($"Invalid registry rights string: {text}");
            */
            return TextFunctions.GetCorrect<RegistryRights>(text, _mapRegistryRights);
        }

        #endregion
        #region InheritanceFlags mapping

        /// <summary>
        /// InheritanceFlags mapping dictionary
        /// </summary>
        private static Dictionary<string[], InheritanceFlags> _mapInheritanceFlags = null;
        private static void InitializeInheritanceFlags()
        {
            _mapInheritanceFlags = new()
            {
                { new string[] { "ContainerInherit", "Container Inherit", "ContainerInheritance", "Container Inheritance", "Container", "CI", "(CI)" }, InheritanceFlags.ContainerInherit },
                { new string[] { "None", "No" }, InheritanceFlags.None },
                { new string[] { "ObjectInherit", "Object inherit", "ObjectInheritance", "Object Inheritance", "Object", "OI", "(OI)" }, InheritanceFlags.ObjectInherit },
            };
        }
        public static InheritanceFlags StringToInheritanceFlags(string text)
        {
            if (_mapInheritanceFlags == null) InitializeInheritanceFlags();
            /*
            if (text.Contains(","))
            {
                var flags = default(InheritanceFlags);
                foreach (var part in text.Split(',').Select(x => x.Trim()))
                {
                    bool isFound = false;
                    foreach (var kvp in _mapInheritanceFlags)
                    {
                        if (kvp.Key.Any(x => x.Equals(part, StringComparison.OrdinalIgnoreCase)))
                        {
                            flags |= kvp.Value;
                            isFound = true;
                            break;
                        }
                    }
                    if (!isFound) throw new ArgumentException($"Invalid inheritance flags string: {text}");
                }
                return flags;
            }
            else
            {
                foreach (var kvp in _mapInheritanceFlags)
                {
                    if (kvp.Key.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase)))
                    {
                        return kvp.Value;
                    }
                }
            }
            throw new ArgumentException($"Invalid inheritance flags string: {text}");
            */
            return TextFunctions.StringToFlags<InheritanceFlags>(text, _mapInheritanceFlags);
        }
        public static string InheritanceFlagsToString(InheritanceFlags val)
        {
            if (_mapInheritanceFlags == null) InitializeInheritanceFlags();
            /*
            StringBuilder sb = new();
            foreach (var kvp in _mapInheritanceFlags)
            {
                if (val.HasFlag(kvp.Value))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(kvp.Key[0]);
                }
            }
            return sb.Length > 0 ? sb.ToString() : "Unknown";
            */
            return TextFunctions.FlagsToString<InheritanceFlags>(val, _mapInheritanceFlags);
        }
        public static string GetInheritanceFlagsString(string text)
        {
            if (_mapInheritanceFlags == null) InitializeInheritanceFlags();
            /*
            StringBuilder sb = new();
            foreach(var part in text.Split(',').Select(x => x.Trim()))
            {
                bool isFound = false;
                foreach (var key in _mapInheritanceFlags.Keys)
                {
                    if (key.Any(x => string.Equals(x, part, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(", ");
                        }
                        sb.Append(key[0]);
                        isFound = true;
                        break;
                    }
                }
                if (!isFound) throw new ArgumentException($"Invalid inheritance flags string: {text}");
            }
            return sb.Length > 0 ? sb.ToString() : "Unknown";
            */
            return TextFunctions.GetCorrect<InheritanceFlags>(text, _mapInheritanceFlags);
        }

        #endregion
        #region PropagationFlags mapping

        /// <summary>
        /// PropagationFlags mapping dictionary
        /// </summary>
        private static Dictionary<string[], PropagationFlags> _mapPropagationFlags = null;
        private static void InitializePropagationFlagsMap()
        {
            _mapPropagationFlags = new()
            {
                { new string[] { "None", "No" }, PropagationFlags.None },
                { new string[] { "NoPropagateInherit", "NoPropagate", "NPI" }, PropagationFlags.NoPropagateInherit },
                { new string[] { "InheritOnly", "IO" }, PropagationFlags.InheritOnly },
            };
        }
        public static PropagationFlags StringToPropagationFlags(string text)
        {
            if (_mapPropagationFlags == null) InitializePropagationFlagsMap();
            /*
            foreach (var kvp in _mapPropagationFlags)
            {
                if (kvp.Key.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase)))
                {
                    return kvp.Value;
                }
            }
            throw new ArgumentException($"Invalid propagation flags string: {text}");
            */
            return TextFunctions.StringToFlags<PropagationFlags>(text, _mapPropagationFlags);
        }
        public static string PropagationFlagsToString(PropagationFlags val)
        {
            if (_mapPropagationFlags == null) InitializePropagationFlagsMap();
            /*
            foreach (var kvp in _mapPropagationFlags)
            {
                if (kvp.Value == val)
                {
                    return kvp.Key[0];
                }
            }
            return "Unknown";
            */
            return TextFunctions.FlagsToString<PropagationFlags>(val, _mapPropagationFlags);
        }
        public static string GetPropagationFlagsString(string text)
        {
            if (_mapPropagationFlags == null) InitializePropagationFlagsMap();
            /*
            foreach (var key in _mapPropagationFlags.Keys)
            {
                if (key.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase)))
                {
                    return key[0];
                }
            }
            throw new ArgumentException($"Invalid propagation flags string: {text}");
            */
            return TextFunctions.GetCorrect<PropagationFlags>(text, _mapPropagationFlags);
        }

        #endregion
        #region AccessControlType mapping

        /// <summary>
        /// AccessControlType mapping dictionary
        /// </summary>
        private static Dictionary<string[], AccessControlType> _mapAccessControlType = null;
        private static void InitializeAccessControlType()
        {
            _mapAccessControlType = new()
            {
                { new string[] { "Allow", "A" }, AccessControlType.Allow },
                { new string[] { "Deny", "Block", "D" }, AccessControlType.Deny },
            };
        }
        public static AccessControlType StringToAccessControlType(string text)
        {
            if (_mapAccessControlType == null) InitializeAccessControlType();
            /*
            foreach (var kvp in _mapAccessControlType)
            {
                if (kvp.Key.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase)))
                {
                    return kvp.Value;
                }
            }
            throw new ArgumentException($"Invalid access control type string: {text}");
            */
            return TextFunctions.StringToFlags<AccessControlType>(text, _mapAccessControlType);
        }
        public static string AccessControlTypeToString(AccessControlType val)
        {
            if (_mapAccessControlType == null) InitializeAccessControlType();
            /*
            foreach (var kvp in _mapAccessControlType)
            {
                if (kvp.Value == val)
                {
                    return kvp.Key[0];
                }
            }
            return "Unknown";
            */
            return TextFunctions.FlagsToString<AccessControlType>(val, _mapAccessControlType);
        }
        public static string GetAccessControlTypeString(string text)
        {
            if (_mapAccessControlType == null) InitializeAccessControlType();
            /*
            foreach (var key in _mapAccessControlType.Keys)
            {
                if (key.Any(x => string.Equals(x, text, StringComparison.OrdinalIgnoreCase)))
                {
                    return key[0];
                }
            }
            throw new ArgumentException($"Invalid access control type string: {text}");
            */
            return TextFunctions.GetCorrect<AccessControlType>(text, _mapAccessControlType);
        }

        #endregion
    }
}
