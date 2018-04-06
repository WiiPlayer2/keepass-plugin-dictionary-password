using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PwGenDictConcat
{
    public partial class GeneratorOptionsDialog : Form
    {
        public GeneratorOptionsDialog()
        {
            InitializeComponent();
        }

        public GeneratorOptions Options { get; set; }

        private void GeneratorOptionsDialog_Load(object sender, EventArgs e)
        {
            if (Options == null)
                Options = new GeneratorOptions();

            minNumeric.Value = Options.MinLength;
            maxNumeric.Value = Options.MaxLength;
            digitNumeric.Value = Options.DigitCount;
            keepTogetherCheck.CheckState = Options.KeepDigitsTogether ? CheckState.Checked : CheckState.Unchecked;
        }

        private void GeneratorOptionsDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            Options.MinLength = (uint)minNumeric.Value;
            Options.MaxLength = (uint)maxNumeric.Value;
            Options.DigitCount = (uint)digitNumeric.Value;
            Options.KeepDigitsTogether = keepTogetherCheck.CheckState == CheckState.Checked;
        }
    }
}
