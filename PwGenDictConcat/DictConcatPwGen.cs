using System;
using KeePassLib;
using KeePassLib.Cryptography;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PwGenDictConcat
{
    class DictConcatPwGen : CustomPwGenerator
    {
        private const int MIN_WORD_LENGTH = 3;
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
            var options = GeneratorOptions.FromString(prf.CustomAlgorithmOptions);

            var strings = new List<string>();


            //Handle digits
            var digitStrs = new string[options.DigitCount];
            for(var i = 0; i < options.DigitCount; i++)
            {
                digitStrs[i] = crsRandomSource.GetRandomInt(10).ToString();
            }
            if (options.KeepDigitsTogether)
                strings.Add(string.Concat(digitStrs));
            else
                strings.AddRange(digitStrs);

            
            //Handle words
            if(options.MaxLength == 0)
            {
                if(options.MinLength == 0)
                {
                    for(var i = 0; i < 3; i++)
                    {
                        strings.Add(crsRandomSource.GetRandomElement(words));
                    }
                }
                else
                {
                    var tmpLength = (int)options.DigitCount;
                    while(tmpLength < options.MinLength)
                    {
                        var str = crsRandomSource.GetRandomElement(words);
                        strings.Add(str);
                        tmpLength += str.Length;
                    }
                }
            }
            else
            {
                var left = (int)options.MaxLength - (int)options.DigitCount;
                while (true)
                {
                    var validWords = words.Where(o => o.Length <= left);
                    if (!validWords.Any())
                        break;

                    var str = crsRandomSource.GetRandomElement(validWords);
                    strings.Add(str);
                    left -= str.Length;

                    if (options.MinLength != 0 && options.MinLength <= options.MaxLength - left)
                        break;
                }
            }

            //Permutate and concatenate
            var permutation = crsRandomSource.GetRandomPermutation(strings);
            return new ProtectedString(true, string.Concat(permutation));
        }

        private string[] GetWords()
        {
            using (var stream = typeof(DictConcatPwGen).Assembly.GetManifestResourceStream("PwGenDictConcat.words.txt"))
            using (var reader = new StreamReader(stream))
            {
                var list = new List<string>();
                while (!reader.EndOfStream)
                {
                    var str = reader.ReadLine();
                    if(str.Length >= 3)
                        list.Add(str);
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