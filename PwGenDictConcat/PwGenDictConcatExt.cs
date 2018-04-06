using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeePass.Plugins;

namespace PwGenDictConcat
{
    public class PwGenDictConcatExt : Plugin
    {
        public override bool Initialize(IPluginHost host)
        {
            PluginHost = host;
            PluginHost.PwGeneratorPool.Add(new DictConcatPwGen());
            return true;
        }

        public IPluginHost PluginHost { get; private set; }
    }
}
