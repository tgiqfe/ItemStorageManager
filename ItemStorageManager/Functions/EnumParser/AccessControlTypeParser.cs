using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace ItemStorageManager.Functions.EnumParser
{
    internal class AccessControlTypeParser : ParserBase<AccessControlType>
    {
        public AccessControlTypeParser()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            map = new()
            {
                { new string[] { "Allow", "A" }, AccessControlType.Allow },
                { new string[] { "Deny", "Block", "D" }, AccessControlType.Deny },
            };
        }

        #region Static methods.

        private static AccessControlTypeParser _parser = null;

        public static AccessControlType ParamsToRaw(string text)
        {
            _parser ??= new AccessControlTypeParser();
            return _parser.TextToFlags(text);
        }

        public static string RawToParams(AccessControlType flags)
        {
            _parser ??= new AccessControlTypeParser();
            return _parser.FlagsToText(flags);
        }

        public static string GetCorrectParameter(string text)
        {
            _parser ??= new AccessControlTypeParser();
            return _parser.GetCorrect(text);
        }

        #endregion
    }
}
