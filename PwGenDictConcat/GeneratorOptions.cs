using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwGenDictConcat
{
    public class GeneratorOptions
    {
        public GeneratorOptions()
        {
            MinLength = 12;
            MaxLength = 30;
            DigitCount = 2;
            KeepDigitsTogether = true;
        }

        private GeneratorOptions(string optionsString)
        {
            var splits = optionsString.Split(';');
            MinLength = uint.Parse(splits[0], CultureInfo.InvariantCulture);
            MaxLength = uint.Parse(splits[1], CultureInfo.InvariantCulture);
            DigitCount = uint.Parse(splits[2], CultureInfo.InvariantCulture);
            KeepDigitsTogether = bool.Parse(splits[3]);
        }

        public uint MinLength { get; set; }

        public uint MaxLength { get; set; }

        public uint DigitCount { get; set; }

        public bool KeepDigitsTogether { get; set; }

        public static GeneratorOptions FromString(string optionsString)
        {
            try
            {
                return new GeneratorOptions(optionsString);
            }
            catch
            {
                return new GeneratorOptions();
            }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0};{1};{2};{3}",
                MinLength, MaxLength, DigitCount, KeepDigitsTogether);
        }
    }
}
