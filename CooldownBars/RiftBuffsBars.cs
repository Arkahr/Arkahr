using Turbo.Plugins.Default;

namespace Turbo.Plugins.Arkahr
{

    public class RiftBuffsBars : BasePlugin, IInGameTopPainter
    {        
        public BuffRuleCalculator RuleCalculator { get; private set; }
        public CooldownBarsPainter CooldownBarsPainter {get;set;}

        public RiftBuffsBars()
        {
            Enabled = false;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            CooldownBarsPainter = new CooldownBarsPainter(Hud,true);
            RuleCalculator = new BuffRuleCalculator(Hud);
            //RuleCalculator.SizeMultiplier = 0.75f;

            RuleCalculator.Rules.Add(new BuffRule(263029) { MinimumIconCount = 1 }); // Conduit
            RuleCalculator.Rules.Add(new BuffRule(403404) { MinimumIconCount = 1 }); // Conduit in tiered rift
            RuleCalculator.Rules.Add(new BuffRule(278269) { MinimumIconCount = 1 }); // Enlightened
            RuleCalculator.Rules.Add(new BuffRule(030477) { MinimumIconCount = 1 }); // Enlightened
            RuleCalculator.Rules.Add(new BuffRule(278271) { MinimumIconCount = 1 }); // Frenzied
            RuleCalculator.Rules.Add(new BuffRule(030479) { MinimumIconCount = 1 }); // Frenzied
            RuleCalculator.Rules.Add(new BuffRule(278270) { MinimumIconCount = 1 }); // Fortune
            RuleCalculator.Rules.Add(new BuffRule(030478) { MinimumIconCount = 1 }); // Fortune
            RuleCalculator.Rules.Add(new BuffRule(278268) { MinimumIconCount = 1 }); // Blessed
            RuleCalculator.Rules.Add(new BuffRule(030476) { MinimumIconCount = 1 }); // Blessed
            RuleCalculator.Rules.Add(new BuffRule(266258) { MinimumIconCount = 1 }); // Channeling
            RuleCalculator.Rules.Add(new BuffRule(266254) { MinimumIconCount = 1 }); // Shield
            RuleCalculator.Rules.Add(new BuffRule(262935) { MinimumIconCount = 1 }); // Power
            RuleCalculator.Rules.Add(new BuffRule(266271) { MinimumIconCount = 1 }); // Speed
            RuleCalculator.Rules.Add(new BuffRule(260349) { MinimumIconCount = 1 }); // Empowered
            RuleCalculator.Rules.Add(new BuffRule(260348) { MinimumIconCount = 1 }); // Fleeting
            
        }

        public void PaintTopInGame(ClipState clipState)
        {
            
            if (Hud.Render.UiHidden) return;
            if (clipState != ClipState.BeforeClip) return;

            RuleCalculator.CalculatePaintInfo(Hud.Game.Me);
            if (RuleCalculator.PaintInfoList.Count == 0) return;

            var uiMinimapRect = Hud.Render.MinimapUiElement.Rectangle;
            var iconSize = 0f;
            var h = iconSize = 30f;
            var x = uiMinimapRect.Left;
            var y = uiMinimapRect.Bottom + 200f;
            var w = uiMinimapRect.Width - iconSize;
            
            CooldownBarsPainter.PaintBuffs(RuleCalculator.PaintInfoList,uiMinimapRect.Left+30,uiMinimapRect.Bottom+200,uiMinimapRect.Width-30, 30,5);
        }

    }

}