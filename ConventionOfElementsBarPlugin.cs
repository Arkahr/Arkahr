/* 21/07/2018
 */
using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.Arkahr
{

    public class ConventionOfElementsBarPlugin : BasePlugin, IInGameTopPainter
    {
        private bool HideWhenUiIsHidden { get; set; }
        private BuffRuleCalculator ruleCalculator;
        private int highestElemental_iconIndex; 
        private double coeElementsCount;
        private int bestIndex;
        private bool showBest;
        private bool user; //if user set his own bar properties

        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Opacity { get; set; }

        public IFont TimeLeftFont { get; set; }
        public bool ShowTimeLeftNumbers { get; set;}

        public IBrush BackgroundBrush { get; set; }
        public IBrush CooldownBrush { get; set; }   
        public Element ElementBrush { get; set; }

        public class Element {
            public IBrush Arcane;
            public IBrush Cold;
            public IBrush Fire;
            public IBrush Holy;
            public IBrush Lightning;
            public IBrush Physical;
            public IBrush Poison;
        }

        public ConventionOfElementsBarPlugin()
        {
            Enabled = true;
            ShowTimeLeftNumbers = true;
            user = false;
            Opacity = 1f;
            ElementBrush = new Element();
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            HideWhenUiIsHidden = false;
            TimeLeftFont = Hud.Render.CreateFont("tahoma", 8, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
            
            BackgroundBrush = Hud.Render.CreateBrush(255, 0, 0, 0, 0); 
            CooldownBrush = Hud.Render.CreateBrush(255, 127, 127, 127, 0);  //CooldownColor          
            ElementBrush.Arcane = Hud.Render.CreateBrush(255, 159,  58, 232, 0);  //Arcane          
            ElementBrush.Cold = Hud.Render.CreateBrush(255,  97, 193, 244, 0);  //Cold
            ElementBrush.Fire = Hud.Render.CreateBrush(255, 182,  49,  49, 0);  //Fire
            ElementBrush.Holy = Hud.Render.CreateBrush(255, 255, 208,  68, 0);  //Holy
            ElementBrush.Lightning = Hud.Render.CreateBrush(255,   4,  32, 135, 0);  //Lightning
            ElementBrush.Physical  = Hud.Render.CreateBrush(255,  85,  80,  66, 0);  //Physical
            ElementBrush.Poison = Hud.Render.CreateBrush(255,  80, 226,  36, 0);  //Poison              
                
            ruleCalculator = new BuffRuleCalculator(Hud);
            ruleCalculator.Rules.Add(new BuffRule(430674) { IconIndex = 1, MinimumIconCount = 0, DisableName = true }); // Arcane
            ruleCalculator.Rules.Add(new BuffRule(430674) { IconIndex = 2, MinimumIconCount = 0, DisableName = true }); // Cold
            ruleCalculator.Rules.Add(new BuffRule(430674) { IconIndex = 3, MinimumIconCount = 0, DisableName = true }); // Fire
            ruleCalculator.Rules.Add(new BuffRule(430674) { IconIndex = 4, MinimumIconCount = 0, DisableName = true }); // Holy
            ruleCalculator.Rules.Add(new BuffRule(430674) { IconIndex = 5, MinimumIconCount = 0, DisableName = true }); // Lightning
            ruleCalculator.Rules.Add(new BuffRule(430674) { IconIndex = 6, MinimumIconCount = 0, DisableName = true }); // Physical
            ruleCalculator.Rules.Add(new BuffRule(430674) { IconIndex = 7, MinimumIconCount = 0, DisableName = true }); // Poison
            showBest = true;
        }

        private IEnumerable<BuffRule> GetCurrentRules(HeroClass heroClass)
        {
            for (int i = 1; i <= 7; i++)
            {
                switch (heroClass)
                {
                    case HeroClass.Barbarian: if (i == 1 || i == 4 || i == 7) continue; break;
                    case HeroClass.Crusader: if (i == 1 || i == 2 || i == 7) continue; break;
                    case HeroClass.DemonHunter: if (i == 1 || i == 4 || i == 7) continue; break;
                    case HeroClass.Monk: if (i == 1 || i == 7) continue; break;
                    case HeroClass.Necromancer: if (i == 1 || i == 3 || i == 4 || i == 5) continue; break;
                    case HeroClass.WitchDoctor: if (i == 1 || i == 4 || i == 5) continue; break;
                    case HeroClass.Wizard: if (i == 4 || i == 6 || i == 7) continue; break;
                }
                yield return ruleCalculator.Rules[i - 1];
            }
        }

        public void PaintTopInGame(ClipState clipState)
        {
            
            if (clipState != ClipState.BeforeClip) return;
            if (HideWhenUiIsHidden && Hud.Render.UiHidden) return;

            //only for me
            var player = Hud.Game.Me;
            var buff = player.Powers.GetBuff(430674);
            var classSpecificRules = GetCurrentRules(player.HeroClassDefinition.HeroClass);
            ruleCalculator.CalculatePaintInfo(player, classSpecificRules);

            if (ruleCalculator.PaintInfoList.Count == 0) return;
            if (!ruleCalculator.PaintInfoList.Any(info => info.TimeLeft > 0)) return;

            var highestElementalBonus = player.Offense.HighestElementalDamageBonus;

            for (int i = 0; i < ruleCalculator.PaintInfoList.Count; i++)
            {
                var info = ruleCalculator.PaintInfoList[0];
                if (info.TimeLeft <= 0)
                {
                    ruleCalculator.PaintInfoList.RemoveAt(0);
                    ruleCalculator.PaintInfoList.Add(info);
                }
                else break;
            }
            
            for (int orderIndex = 0; orderIndex < ruleCalculator.PaintInfoList.Count; orderIndex++)
            {
                var info = ruleCalculator.PaintInfoList[orderIndex];
                var best = false;
                
                switch (info.Rule.IconIndex)
                {
                    case 1: best = player.Offense.BonusToArcane == highestElementalBonus; break;
                    case 2: best = player.Offense.BonusToCold == highestElementalBonus; break;
                    case 3: best = player.Offense.BonusToFire == highestElementalBonus; break; 
                    case 4: best = player.Offense.BonusToHoly == highestElementalBonus; break;
                    case 5: best = player.Offense.BonusToLightning == highestElementalBonus; break;
                    case 6: best = player.Offense.BonusToPhysical == highestElementalBonus; break;
                    case 7: best = player.Offense.BonusToPoison == highestElementalBonus; break;
                }
                if (best) {
                        highestElemental_iconIndex = (int)info.Rule.IconIndex;
                        bestIndex = orderIndex;
                } 
                if (best && orderIndex > 0)
                {
                    info.TimeLeft = (orderIndex - 1) * 4 + ruleCalculator.PaintInfoList[0].TimeLeft; 
                    highestElemental_iconIndex = (int)info.Rule.IconIndex;
                }
            }

            coeElementsCount = ruleCalculator.PaintInfoList.Count; //how many elements class use

            if (highestElementalBonus>0 && showBest)  // thx gjuz
            {
                var _tmp = ruleCalculator.PaintInfoList[bestIndex];
                ruleCalculator.PaintInfoList.Clear();
                ruleCalculator.PaintInfoList.Add(_tmp);
                var best = ruleCalculator.PaintInfoList[0];
                
                IBrush brush = null;                
                if (best.Elapsed>0 && best.Elapsed<4) 
                {                       
                    brush = getBrush(highestElemental_iconIndex);
                } else 
                {                   
                    brush = CooldownBrush;
                    double _time = (coeElementsCount - 1) * 4;
                    best.Elapsed = _time - best.TimeLeft;
                }

                //bar properties
                if (!user) // user has not specified own coordinates
                {
                    var ui =  Hud.Render.MinimapUiElement.Rectangle;                 
                    X = ui.Left;
                    Y = ui.Bottom; 
                    Width = ui.Width;
                    Height = Hud.Window.Size.Height * 0.05f; // that height clip text under minimap about objective name, does not hide objectives                 
                }                
                PaintBuff(best, X, Y, Width, Height, brush);
            } 
        }

        private void PaintBuff(BuffPaintInfo info, float x, float y, float w, float h, IBrush brush)
        {   
            if (Opacity == 0) return;                        
            //Draw backbround bar  
            BackgroundBrush.Opacity = Opacity;     
            BackgroundBrush.DrawRectangle(x, y, w, h);            
            //Draw cooldown Bar
            brush.Opacity = Opacity;
            brush.DrawRectangle(x, y, (float)(w * info.TimeLeft / (info.TimeLeft + info.Elapsed)), h);
            //Draw time left number
            if (ShowTimeLeftNumbers) {
                var text = "";
                if (info.TimeLeft > 1)
                    text = info.TimeLeft.ToString("F0", CultureInfo.InvariantCulture);
                else
                    text = info.TimeLeft.ToString("F1", CultureInfo.InvariantCulture);

                var layout = TimeLeftFont.GetTextLayout(text);    
                TimeLeftFont.Opacity = Opacity;                                                    
                TimeLeftFont.DrawText(layout, x + (w - (float)Math.Ceiling(layout.Metrics.Width)) / 2.0f, y + (h - layout.Metrics.Height) / 2);
            }
        }

        public void setBar(float x, float y, float width, float height) {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            user = true;
        }   

        private IBrush getBrush(int i) {
            switch (i)
            {
                case 1:
                    return ElementBrush.Arcane;
                case 2:
                    return ElementBrush.Cold;
                case 3:
                    return ElementBrush.Fire;
                case 4:
                    return ElementBrush.Holy;                                                            
                case 5:
                    return ElementBrush.Lightning;
                case 6:
                    return ElementBrush.Physical;
                case 7:
                    return ElementBrush.Poison;                      
            }
            return null; //we have to return something, of course we'll never set i to any other value
        }     
    }
}