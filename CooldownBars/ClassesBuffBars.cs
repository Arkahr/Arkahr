using Turbo.Plugins.Default;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;




namespace Turbo.Plugins.Arkahr
{

    public class ClassesBuffBars : BasePlugin, IInGameTopPainter
    {        
        IWatch timer;
        // overrideDefualts = true - nothing displays, except given bars in customize section
        public CooldownBarsPainter CooldownBarsPainter {get;set;}
        private IEnumerable<IPlayerSkill> classSkills;
        private List<IPlayerSkill> playerSkills;  
        private IWatch _lastNewGame;  
        private TextDebug TextDebug;    

        // to do skills
        // bar, crus, monk, necro, wd ,wizard
        public static HashSet<uint> rechargeSkillsId = new HashSet<uint>() 
        { 
            129217, //DH Sentry
            75301, //DH Spike Trap
            353447, //Barb Avalanche rune 4  if count from 0
            97435, // Barb Furious Charge rune 5
            312736, //Monk Dashing Strike
            466857, //Necro Bone Armor
            454090,  //Necro Blood Rush, probably only rune 5, rest is normal cooldown ?
        };

        public ClassesBuffBars()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            CooldownBarsPainter = new CooldownBarsPainter(Hud,true);
            playerSkills = new List<IPlayerSkill>();   
            timer = Hud.Time.CreateAndStartWatch();
            _lastNewGame = Hud.Time.CreateAndStartWatch();
            TextDebug = new TextDebug(100, 150, Hud);
            TextDebug.setTextSize(7);
        }

        public void OnNewArea(bool newGame, ISnoArea area)
        {
            if (newGame)
            {
                _lastNewGame.Restart();                
            }
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
            //Because there is /no cooldown information/, its in player attributes so you have to get to it way around ... bothersome
            //or cooldown revolve around fast short ticking cooldown (ex. whirlwind)
            switch (heroClass)
                {
                    case HeroClass.Barbarian: 
                        playerSkills.Remove(powers.UsedBarbarianPowers.Whirlwind);
                        playerSkills.Remove(powers.UsedBarbarianPowers.CallOfTheAncients);
                        //track bonus from immortal king and act accordingly
                        //playerSkills.Remove(powers.UsedBarbarianPowers.CallOfTheAncients);
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
            

//         Stacking Skills Debug

             
             TextDebug.Clear();                     
             foreach (var skill in powers.CurrentSkills)
             {
                if (skill.SnoPower.Sno == 466857) {
                    
                }

                TextDebug.addText(string.Format("{0}\n{1} {2} {3} {4}\n\n",
                skill.SnoPower.NameEnglish,
                skill.Charges,
                skill.IsOnCooldown,
                skill.CooldownStartTick, 
                skill.CooldownFinishTick
                ));
     

             }
             TextDebug.Print();
               //var a = Hud.Game.Actors.Where(act => act.SnoActor.NameEnglish.Contains("Kadala"));
            //     var Recharge_Start_Time = Hud.Game.Me.GetAttributeValue(Hud.Sno.Attributes.Recharge_Start_Time, skill.SnoPower.Sno, -1 );  
            //    var Next_Charge_Gained_time = Hud.Game.Me.GetAttributeValue(Hud.Sno.Attributes.Next_Charge_Gained_time, skill.SnoPower.Sno, -1 );
            //     double recharge_time = 0; 
                // if (Recharge_Start_Time != -1 && Next_Charge_Gained_time!=-1)  {
                //     if (Next_Charge_Gained_time >= 0) //is skill still recharging 
                //     {
                        
                //     }
                // if (skill.SnoPower.Sno == 129217) {
                // }

            // }

             if (playerSkills!=null && playerSkills.Count>0) {
/*                  foreach (var skill in playerSkills)
                 {
                    
                    if (rechargeSkillsId.Contains(skill.SnoPower.Sno))) {
                        //how to know what are max charges? so you coul paint skill when charges are lesser then max
                        CooldownBarsPainter.PaintSkillWithCharge(skill,charge_time_next, charge_time_start,  x,y,w,h,0);
                        var recharge_start = Hud.Game.Me.GetAttributeValue(Hud.Sno.Attributes.Recharge_Start_Time, sentry.SnoPower.Sno );
                        var next_charge_gained_time = Hud.Game.Me.GetAttributeValue(Hud.Sno.Attributes.Next_Charge_Gained_time, sentry.SnoPower.Sno );
                    }
                    else
                        CooldownBarsPainter.PaintSkills(playerSkills,x,y,w,h,0); 
                    } */
                        CooldownBarsPainter.PaintSkills(playerSkills,x,y,w,h,0); 
             }
        }


        private bool isSkillRecharging(IPlayerSkill skill) {

            //if skill.Charges >= 0 and 
            return false;
            
        }
        private double getRechargeTime(ISnoPower skillSno) {

//?? czy aby napewno nie da sie tu sprawdzic?
//rob to zanim wywolasz funkcje, tutaj nie ma jak wyjsc z niej przy braku skillSno
            //check if skill is currently equiped
            //check if its in attribute table

            var skills = Hud.Game.Me.Powers.CurrentSkills.Where(s=> s.SnoPower == skillSno);
            if (skills.Count() == 0) return -1; //false
            {
            var _skill = skills.First();
                
            } 


            //if Hud.Sno.Attributes.Next_Charge_Gained_time == 0 return; there is nothing to calculate
            //else 
            // next_charge = Hud.Sno.Attributes.Next_Charge_Gained_time
            // charge_started = Hud.Sno.Attributes.Recharge_Start_Time
            // return next_charge - charge_started
            return 0;
        }

    }

}