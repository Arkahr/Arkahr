/*
Works only for own character 
Modified can work for others, then you need to specify other places to show their CoEbar
 */


using System.Collections.Generic;
using System.Linq;
using System;
using Turbo.Plugins.Default;
using SharpDX;

namespace Turbo.Plugins.Arkahr
{

    public class ConventionOfElementsBar : BasePlugin, IInGameTopPainter
    {
        public bool HideWhenUiIsHidden { get; set; }
        public CooldownBarsPainter CooldownBarsPainter { get; set; }
        public Color[] Colors { get; set; }
        private BuffRuleCalculator _ruleCalculator;
        private int _highestElemental_iconIndex; 
        private double _coeElementsCount;
        private int _bestIndex;
        public bool showBest;
        public List<Color> Colors2 {get; set;}

        public ConventionOfElementsBar()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            HideWhenUiIsHidden = false;
            CooldownBarsPainter = new CooldownBarsPainter(Hud, true)
            {
                ShowIcon = false,
                TextAlign = TextAlign.Center,
                BackgroundColor = new Color(255, "000"),
            };       

            Colors = new Color[8];
            Colors[0] = new Color(255, 127,  127, 127);  //Cooldown          
            Colors[1] = new Color(255, 159,  58, 232);  //Arcane          
            Colors[2] = new Color(255,  97, 193, 244);  //Cold
            Colors[3] = new Color(200, 244,  66,  66);  //Fire
            Colors[4] = new Color(255, 255, 208,  68);  //Holy
            Colors[5] = new Color(255,   4,  32, 135);  //Lightning
            Colors[6] = new Color(255, 85, 80,  66);  //Physical
            Colors[7] = new Color(255,  80, 226,  36);  //Poison       
                
            _ruleCalculator = new BuffRuleCalculator(Hud);

            _ruleCalculator.Rules.Add(new BuffRule(430674) { IconIndex = 1, MinimumIconCount = 0, DisableName = true }); // Arcane
            _ruleCalculator.Rules.Add(new BuffRule(430674) { IconIndex = 2, MinimumIconCount = 0, DisableName = true }); // Cold
            _ruleCalculator.Rules.Add(new BuffRule(430674) { IconIndex = 3, MinimumIconCount = 0, DisableName = true }); // Fire
            _ruleCalculator.Rules.Add(new BuffRule(430674) { IconIndex = 4, MinimumIconCount = 0, DisableName = true }); // Holy
            _ruleCalculator.Rules.Add(new BuffRule(430674) { IconIndex = 5, MinimumIconCount = 0, DisableName = true }); // Lightning
            _ruleCalculator.Rules.Add(new BuffRule(430674) { IconIndex = 6, MinimumIconCount = 0, DisableName = true }); // Physical
            _ruleCalculator.Rules.Add(new BuffRule(430674) { IconIndex = 7, MinimumIconCount = 0, DisableName = true }); // Poison

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
                yield return _ruleCalculator.Rules[i - 1];
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
            _ruleCalculator.CalculatePaintInfo(player, classSpecificRules);

            if (_ruleCalculator.PaintInfoList.Count == 0) return;
            if (!_ruleCalculator.PaintInfoList.Any(info => info.TimeLeft > 0)) return;

            var highestElementalBonus = player.Offense.HighestElementalDamageBonus;

            for (int i = 0; i < _ruleCalculator.PaintInfoList.Count; i++)
            {
                var info = _ruleCalculator.PaintInfoList[0];
                if (info.TimeLeft <= 0)
                {
                    _ruleCalculator.PaintInfoList.RemoveAt(0);
                    _ruleCalculator.PaintInfoList.Add(info);
                }
                else break;
            }
            
            for (int orderIndex = 0; orderIndex < _ruleCalculator.PaintInfoList.Count; orderIndex++)
            {
                var info = _ruleCalculator.PaintInfoList[orderIndex];
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
                        _highestElemental_iconIndex = (int)info.Rule.IconIndex;
                        _bestIndex = orderIndex;
                } 
                if (best && orderIndex > 0)
                {                                  
                    info.TimeLeft = (orderIndex - 1) * 4 + _ruleCalculator.PaintInfoList[0].TimeLeft; 
                    _highestElemental_iconIndex = (int)info.Rule.IconIndex;
                }                            
            }     

            _coeElementsCount = _ruleCalculator.PaintInfoList.Count;
                    
            var ui =  Hud.Render.MinimapUiElement.Rectangle;                 
            var y = ui.Bottom;       
            var h = 48f;
            var x = ui.Left;
            var w = ui.Width;

            if (showBest)  // thx gjuz
            {
                var _tmp = _ruleCalculator.PaintInfoList[_bestIndex];
                _ruleCalculator.PaintInfoList.Clear();
                _ruleCalculator.PaintInfoList.Add(_tmp);
                var best = _ruleCalculator.PaintInfoList[0];
                
                Color color = null;
                if (best.Elapsed>0 && best.Elapsed<4) 
                {
                    color = Colors[_highestElemental_iconIndex];                        
                } else 
                {                     
                    color = CooldownBarsPainter.CooldownBarColor;
                    double _time = (_coeElementsCount - 1) * 4;
                    best.Elapsed = _time - best.TimeLeft;                         
                }

                CooldownBarsPainter.PaintBuff(best, x,y,w,h, color);

            } 

        }
       
    }
    
}