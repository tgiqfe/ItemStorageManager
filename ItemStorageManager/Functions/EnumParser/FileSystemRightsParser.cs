using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace ItemStorageManager.Functions.EnumParser
{
    internal class FileSystemRightsParser : ParserBase<FileSystemRights>
    {
        public FileSystemRightsParser()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            map = new()
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

        #region Static methods.

        private static FileSystemRightsParser _parser = null;

        public static FileSystemRights ParamsToRaw(string text)
        {
            _parser ??= new FileSystemRightsParser();
            return _parser.TextToFlags(text);
        }

        public static string RawToParams(FileSystemRights flags)
        {
            _parser ??= new FileSystemRightsParser();
            return _parser.FlagsToText(flags);
        }

        public static string GetCorrectParameter(string text)
        {
            _parser ??= new FileSystemRightsParser();
            return _parser.GetCorrect(text);
        }

        #endregion
    }
}
