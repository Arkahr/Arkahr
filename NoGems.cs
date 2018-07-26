// 25/07/2018

using Turbo.Plugins.Default;
using System.Linq;
using Turbo.Plugins.Jack.Extensions;

namespace Turbo.Plugins.Arkahr
{
    public class NoGems : BasePlugin, IInGameTopPainter
    {
        public TopLabelWithTitleDecorator LabelDecorator { get; set; }

        public NoGems()
        {
            Enabled = false;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            LabelDecorator = new TopLabelWithTitleDecorator(Hud)             
            {
                BorderBrush = Hud.Render.CreateBrush(255, 230, 30, 30, -1),
                BackgroundBrush = Hud.Render.CreateBrush(190, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 8, 255,  230, 30, 30, false, false, false),
                TitleFont = Hud.Render.CreateFont("tahoma", 10, 255, 255, 0, 0, true, false, false),
            };
        }

        public void PaintTopInGame(ClipState clipState)
        {
            if (clipState != ClipState.BeforeClip) return;
            var showWarning = false;
            var popUpWidth =  300;
            var popUpHeight =  50;
            var popUpX =  Hud.Window.Size.Width/2 - popUpWidth/2;
            var popUpY =  Hud.Window.Size.Height * 0.3f;
            var text = ""; 
            var gemCount = 666;
            foreach (IPlayer player in Hud.Game.Players)
            {
                var gemBuffs = player.Powers.UsedLegendaryGems.AllGemPrimaryBuffs().Where(b => b.Active);      
                gemCount = gemBuffs.Count();
                
                if (gemCount<3)  {
                    showWarning = true;
                    text += string.Format("{0} ({1}) -> {2} missing\n", player.HeroName, player.HeroClassDefinition.HeroClass, 3 - gemCount);                        
                    popUpHeight += 20;
                }
            }
            if (showWarning) LabelDecorator.Paint(popUpX, popUpY, popUpWidth, popUpHeight, text, "No Gems!");                        
        }

        // protected int countPlayerSocketsInJewelry() {
        //     //if no sockets in jewelry,
        //     return 3;
        // }  
    }
}
