using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace ItemStorageManager.Functions.EnumParser
{
    internal class PropagationFlagsParser : ParserBase<PropagationFlags>
    {
        public PropagationFlagsParser()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            map = new()
            {
                { new string[] { "None", "No" }, PropagationFlags.None },
                { new string[] { "NoPropagateInherit", "NoPropagate", "NPI" }, PropagationFlags.NoPropagateInherit },
                { new string[] { "InheritOnly", "IO" }, PropagationFlags.InheritOnly },
            };
        }

        #region Static methods
        private static PropagationFlagsParser _parser;

        public static PropagationFlags ParamsToRaw(string text)
        {
            _parser ??= new PropagationFlagsParser();
            return _parser.TextToFlags(text);
        }

        public static string RawToParams(PropagationFlags flags)
        {
            _parser ??= new PropagationFlagsParser();
            return _parser.FlagsToText(flags);
        }

        public static string GetCorrectParameter(string text)
        {
            _parser ??= new PropagationFlagsParser();
            return _parser.GetCorrect(text);
        }
        #endregion
    }
}
