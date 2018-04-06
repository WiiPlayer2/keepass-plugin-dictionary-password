using System;
using KeePassLib;
using KeePassLib.Cryptography;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace PwGenDictConcat
{
    class DictConcatPwGen : CustomPwGenerator
    {
        private PwUuid pwUuid = new PwUuid(new byte[] { 0xA3, 0x78, 0x45, 0xE0, 0x54, 0x3F, 0x45, 0x6C, 0x9D, 0x42, 0x3B, 0x65, 0x01, 0x60, 0x00, 0xDA });

        public override string Name
        {
            get { return "Dictionary Concatenation"; }
        }

        public override PwUuid Uuid
        {
            get { return pwUuid; }
        }

        public override bool SupportsOptions
        {
            get { return true; }
        }

        public override ProtectedString Generate(PwProfile prf, CryptoRandomStream crsRandomSource)
        {
            var words = GetWords();
            var strBuilder = new StringBuilder();
            for(var i = 0; i < 3; i++)
            {
                strBuilder.Append(words[GetRandomIndex(crsRandomSource, words.Length)]);
            }
            return new ProtectedString(true, strBuilder.ToString());
        }

        private int GetRandomIndex(CryptoRandomStream crsRandomSource, int max)
        {
            var rand = crsRandomSource.GetRandomUInt64();
            return (int)(rand % (ulong)max);
        }

        private string[] GetWords()
        {
            using (var stream = typeof(DictConcatPwGen).Assembly.GetManifestResourceStream("PwGenDictConcat.words.txt"))
            using (var reader = new StreamReader(stream))
            {
                var list = new List<string>();
                while (!reader.EndOfStream)
                {
                    list.Add(reader.ReadLine());
                }
                return list.ToArray();
            }
        }

        public override string GetOptions(string strCurrentOptions)
        {
            var options = GeneratorOptions.FromString(strCurrentOptions);
            var form = new GeneratorOptionsDialog() { Options = options };

            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return strCurrentOptions;

            return options.ToString();
        }
    }
}