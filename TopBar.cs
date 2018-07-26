using System.Linq;
using Turbo.Plugins.Default;
using SharpDX;

namespace Turbo.Plugins.Arkahr
{
    public class TopBar : BasePlugin, IInGameWorldPainter
	{        
        private Panel topBarPanel;
		public TopBar()
		{
            Enabled = false;
		}

        public override void Load(IController hud)
        {
            base.Load(hud);                
        }
		
		public void PaintWorld(WorldLayer layer)
		{
          
		}
    }
}