using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace ItemStorageManager.Functions.EnumParser
{
    internal class InheritanceFlagsParser : ParserBase<InheritanceFlags>
    {
        public InheritanceFlagsParser()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            map = new()
            {
                { new string[] { "ContainerInherit", "Container Inherit", "ContainerInheritance", "Container Inheritance", "Container", "CI", "(CI)" }, InheritanceFlags.ContainerInherit },
                { new string[] { "None", "No" }, InheritanceFlags.None },
                { new string[] { "ObjectInherit", "Object inherit", "ObjectInheritance", "Object Inheritance", "Object", "OI", "(OI)" }, InheritanceFlags.ObjectInherit },
            };
        }

        #region Static methods

        private static InheritanceFlagsParser _parser;

        public static InheritanceFlags ParamsToRaw(string text)
        {
            _parser ??= new InheritanceFlagsParser();
            return _parser.TextToFlags(text);
        }

        public static string RawToParams(InheritanceFlags flags)
        {
            _parser ??= new InheritanceFlagsParser();
            return _parser.FlagsToText(flags);
        }

        public static string GetCorrectParameter(string text)
        {
            _parser ??= new InheritanceFlagsParser();
            return _parser.GetCorrect(text);
        }

        #endregion
    }
}
