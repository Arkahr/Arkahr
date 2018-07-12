using Turbo.Plugins.Default;
using System.Collections.Generic;
using System.Linq;

namespace Turbo.Plugins.Arkahr
{
    //link test
    public class ClassesBuffBars : BasePlugin, IInGameTopPainter
    {        
        public CooldownBarsPainter CooldownBarsPainter {get;set;}

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

            //Do not track this skills
            //Because there is no cooldown information, or cooldown revolve around fast short ticking cooldown (ex. whirlwind)
            switch (heroClass)
                {
                    case HeroClass.Barbarian: 
                        break;
                    case HeroClass.Crusader:
                        break;
                    case HeroClass.DemonHunter:                    
                        playerSkills.Remove(powers.UsedDemonHunterPowers.Multishot);
                        playerSkills.Remove(powers.UsedDemonHunterPowers.Sentry);
                        playerSkills.Remove(powers.UsedDemonHunterPowers.EvasiveFire);
                        break;
                    case HeroClass.Monk:                              
                        break;
                    case HeroClass.Necromancer:
                        break;
                    case HeroClass.WitchDoctor:
                        break;
                    case HeroClass.Wizard:
                        break;
                }

            var uiMinimapRect = Hud.Render.MinimapUiElement.Rectangle;
            var iconSize = 0f;
            var h = iconSize = 30f;
            var x = uiMinimapRect.Left + iconSize;
            var y = uiMinimapRect.Bottom + 275f;
            var w = uiMinimapRect.Width - iconSize;

/*          Stacking Skills Debug

            TextDebug TextDebug = new TextDebug(90, 650, Hud);

            var _v = Hud.Game.Me.Powers.UsedDemonHunterPowers.Vengeance;
            var sentry = Hud.Game.Me.Powers.UsedDemonHunterPowers.Sentry;
            if (sentry.BuffIsActive)
            {           
                TextDebug.addText(string.Format("Sentry Debug:\n\n"));
                TextDebug.addText(
                    string.Format("charges:{0}\n First Active {1} \nelapsed time: {2}\n time left {3}\n Last Active {4} \nelapsed time: {5}\n time left {6}\n\n",
                    sentry.Charges,
                    sentry.Buff.FirstActive.IsRunning,
                    sentry.Buff.TimeLeftSeconds[0],
                    sentry.Buff.TimeElapsedSeconds.Max(),

                    sentry.Buff.LastActive.IsRunning,
                    sentry.Buff.LastActive.ElapsedMilliseconds,
                    sentry.Buff.TimeElapsedSeconds.Max()
                    
                    
                    ));
            }
           TextDebug.Print();   
             */
             if (playerSkills!=null && playerSkills.Count>0)
                CooldownBarsPainter.PaintSkills(playerSkills,x,y,w,h,0, new Color(255, "#00cc99")); 
        }

    }

}