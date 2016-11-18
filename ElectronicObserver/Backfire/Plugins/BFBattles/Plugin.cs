using ElectronicObserver.Window.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFBattles
{
    public class Plugin : DockPlugin
    {
        public override string MenuTitle
        {
            get { return "Backfire 戦闘"; }
        }

        public override string Version
        {
            get { return "1.0.0"; }
        }

        public override PluginSettingControl GetSettings()
        {
            return null;
        }
    }
}
