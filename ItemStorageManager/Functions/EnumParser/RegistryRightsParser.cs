using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace ItemStorageManager.Functions.EnumParser
{
    internal class RegistryRightsParser : ParserBase<RegistryRights>
    {
        public RegistryRightsParser()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            map = new()
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

        #region Static methods.

        private static RegistryRightsParser _parser = null;

        public static RegistryRights ParamsToRaw(string text)
        {
            _parser ??= new RegistryRightsParser();
            return _parser.TextToFlags(text);
        }

        public static string RawToParams(RegistryRights flags)
        {
            _parser ??= new RegistryRightsParser();
            return _parser.FlagsToText(flags);
        }

        public static string GetCorrectParameter(string text)
        {
            _parser ??= new RegistryRightsParser();
            return _parser.GetCorrect(text);
        }

        #endregion
    }
}
