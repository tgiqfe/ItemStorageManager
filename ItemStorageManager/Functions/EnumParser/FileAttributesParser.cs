using System;
using System.Collections.Generic;
using System.Text;

namespace ItemStorageManager.Functions.EnumParser
{
    internal class FileAttributesParser : ParserBase<FileAttributes>
    {
        public FileAttributesParser()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            map = new()
            {
                { new string[]{ "Archive" }, FileAttributes.Archive },
                { new string[]{ "Compressed" }, FileAttributes.Compressed },
                { new string[]{ "Device", "Dev" }, FileAttributes.Device },
                { new string[]{ "Directory", "Dir" }, FileAttributes.Directory },
                { new string[]{ "Encrypted", "Enc" }, FileAttributes.Encrypted },
                { new string[]{ "Hidden", "H", "Hide" }, FileAttributes.Hidden },
                { new string[]{ "IntegrityStream" }, FileAttributes.IntegrityStream },
                { new string[]{ "None" }, FileAttributes.Normal },
                { new string[]{ "Normal" }, FileAttributes.Normal },
                { new string[]{ "NoScrubData" }, FileAttributes.NoScrubData },
                { new string[]{ "NotContentIndexed" }, FileAttributes.NotContentIndexed },
                { new string[]{ "Offline" }, FileAttributes.Offline },
                { new string[]{ "ReadOnly", "R", "Read" }, FileAttributes.ReadOnly },
                { new string[]{ "ReparsePoint" }, FileAttributes.ReparsePoint },
                { new string[]{ "SparseFile" }, FileAttributes.SparseFile },
                { new string[]{ "System", "S", "Sys" }, FileAttributes.System },
                { new string[]{ "Temporary", "Temp", "tmp" }, FileAttributes.Temporary }
            };
        }

        #region Static methods.

        private static FileAttributesParser _parser = null;

        public static FileAttributes ParamsToRaw(string text)
        {
            _parser ??= new FileAttributesParser();
            return _parser.TextToFlags(text);
        }

        public static string RawToParams(FileAttributes flags)
        {
            _parser ??= new FileAttributesParser();
            return _parser.FlagsToText(flags);
        }

        public static string GetCorrectParameter(string text)
        {
            _parser ??= new FileAttributesParser();
            return _parser.GetCorrect(text);
        }

        public static FileAttributes MergeAttributes(string text, FileAttributes baseAttributes)
        {
            _parser ??= new FileAttributesParser();
            return _parser.MergeFlags(text, baseAttributes);
        }


        #endregion
    }
}
