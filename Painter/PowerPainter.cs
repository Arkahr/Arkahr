using Turbo.Plugins;
using Turbo.Plugins.Default;
using SharpDX;
using System.Linq;


namespace Turbo.Plugins.Arkahr 
{

    public abstract class Painter <T>
    {
        protected IController _hud;
        public Painter(IController hud, bool setDefaults)
        {
            _hud = hud;            
        }          
        public abstract void Paint(T t);
    }

    


    public abstract class PowerPainter<T>
    //Paint a representation of cooldown
    //( maybe later i will make other representations, like arc, horizontal bar, wheel etc)
    {         
        protected IController _hud;
        public PowerPainter(IController hud, bool setDefaults)
        {
            _hud = hud;            
        }       

        public abstract void CalculatePowerInfo(IPlayerSkill playerSkill);
        public abstract void CalculatePowerInfo(ISnoPower snoPower);
        public abstract void CalculatePowerInfo(BuffPaintInfo buffPaintInfo);

        public abstract void Paint(T t);        
    }
}