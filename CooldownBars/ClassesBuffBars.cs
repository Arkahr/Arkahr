using Turbo.Plugins.Default;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace Turbo.Plugins.Arkahr
{

    public class ClassesBuffBars : BasePlugin, IInGameTopPainter
    {        
        // overrideDefualts = true - nothing displays, except given bars in customize section
        public CooldownBarsPainter CooldownBarsPainter { get; set; }
        private IEnumerable<IPlayerSkill> classSkills;
        private List<IPlayerSkill> playerSkills;  

        public ClassesBuffBars()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            CooldownBarsPainter = new CooldownBarsPainter(Hud,true);
            playerSkills = new List<IPlayerSkill>();   
        }


        public void PaintTopInGame(ClipState clipState)
        {
            
            if (Hud.Render.UiHidden) return;
            if (clipState != ClipState.BeforeClip) return;
            
            var heroClass = Hud.Game.Me.HeroClassDefinition.HeroClass;
            var powers = Hud.Game.Me.Powers;
            playerSkills.Clear();                        
            classSkills = powers.UsedSkills;
            foreach (var skill in classSkills)
            {
                playerSkills.Add(skill);
            }
        
            //TODO not all classes tested
            //Do not track this skills
            //Because cooldown revolve around fast short ticking cooldown (ex. whirlwind)            
            switch (heroClass)
                {
                    case HeroClass.Barbarian: 
                        playerSkills.Remove(powers.UsedBarbarianPowers.Whirlwind);
                        playerSkills.Remove(powers.UsedBarbarianPowers.CallOfTheAncients);
                        //track bonus from immortal king and act accordingly
                        //playerSkills.Remove(powers.UsedBarbarianPowers.CallOfTheAncients);
                        break;
                    case HeroClass.Crusader:
                        playerSkills.Remove(powers.UsedCrusaderPowers.SteedCharge);
                        break;
                    case HeroClass.DemonHunter:                    
                        playerSkills.Remove(powers.UsedDemonHunterPowers.Multishot);
                        //playerSkills.Remove(powers.UsedDemonHunterPowers.Sentry);
                        playerSkills.Remove(powers.UsedDemonHunterPowers.EvasiveFire);                        
                        break;
                    case HeroClass.Monk:  
                        //TODO turn off buff active while inside innersanctuary, leave cooldown counter
                        playerSkills.Remove(powers.UsedMonkPowers.InnerSanctuary);                            
                        //TODO Mantras are not displayed as buff
                        break;
                    case HeroClass.Necromancer:
                    //TODO
                        break;
                    case HeroClass.WitchDoctor:
                    //TODO
                        break;
                    case HeroClass.Wizard:
                    //TODO
                        break;
                }

            var uiMinimapRect = Hud.Render.MinimapUiElement.Rectangle;
            var iconSize = 0f;
            var h = iconSize = 30f;
            var x = uiMinimapRect.Left + iconSize;
            var y = uiMinimapRect.Bottom + 275f;
            var w = uiMinimapRect.Width - iconSize;            

            if (playerSkills!=null && playerSkills.Count>0)                         
                CooldownBarsPainter.PaintSkills(playerSkills, x, y, w, h, 0);              
        }
    }
}