using Turbo.Plugins;
using Turbo.Plugins.Default;
using System.Linq;

namespace Turbo.Plugins.Arkahr {


    public interface ITimeInfo 
    //helper class,  to be able to return result with two numerical values
    {
        double TimeLeft {get; set;}
        double TimeMax {get; set;}
    }    
    public interface IResourceInfo
    {
        float Value {get;set;}
        float MaxValue {get;set;}       
    }    


    public abstract class GenericBarPainter : IResourceInfo
    {
        
        public float Value {get; set;}
        public float MaxValue {get; set;}

        public abstract void Paint(Bar bar);        
    }

    public class BarPainter : GenericBarPainter, IResourceInfo
    {
        public override void Paint(Bar bar) 
        {

        }
    }

    

    public class PowerTimeInfo 
    //helper class,  to be able to return result with two numerical values
    {
        public double TimeLeft;
        public double TimeMax;
        
    }   
    public abstract class Representation 
    {
        protected float Value {get;set;}
        protected float MaxValue {get;set;}
    }     

    public class TimeRepresentation : Representation 
    {
        public float TimeLeft 
        { 
            get 
            {
                return Value;
            } 
            set 
            {
                Value = TimeLeft;
            } 
        }
        public float TimeMax
        { 
            get 
            {
                return MaxValue;
            } 
            set 
            {
                MaxValue = TimeMax;
            } 
        }

        public TimeRepresentation()
        {

        }
    }

    public class PowerInfoCalculator
    //calculates cooldown time info 
    {
        private IController _hud;
        
        public PowerInfoCalculator(IController hud)
        {
            _hud = hud;
        }

        public PowerTimeInfo GetPowerTimeInfo(ISnoPower snoPower)         
        //calculates Skill with charges
        {
            //calculate power info
            //calculate time
            PowerTimeInfo pti = null;
            var Recharge_Start_Time = _hud.Game.Me.GetAttributeValue(_hud.Sno.Attributes.Recharge_Start_Time, snoPower.Sno, -1 );  
            var Next_Charge_Gained_time = _hud.Game.Me.GetAttributeValue(_hud.Sno.Attributes.Next_Charge_Gained_time, snoPower.Sno, -1 );
            if (Next_Charge_Gained_time==0) //skill is not recharging
                pti = null;
            else 
            {
                pti = new PowerTimeInfo();
                pti.TimeLeft = (Next_Charge_Gained_time - _hud.Game.CurrentGameTick) / 60.0d;
                pti.TimeMax = (Next_Charge_Gained_time - Recharge_Start_Time) / 60.0d;            
            }
            
            return pti; //REMEMBER ABOUT SITUATION THAT pti can BE NULL !!!            
        }

        public PowerTimeInfo GetPowerTimeInfo(BuffPaintInfo buffPaintInfo) 
        //this does for me BuffRuleCalculator by KJ
        {
            var pti = new PowerTimeInfo();
            pti.TimeLeft = buffPaintInfo.TimeLeft;
            pti.TimeMax = pti.TimeLeft + buffPaintInfo.Elapsed; 

            return pti;
        }

        public PowerTimeInfo GetPowerTimeInfo(IPlayerSkill skill) 
        {
            var pti = new PowerTimeInfo();

            if (skill.BuffIsActive)
            {
                pti.TimeLeft = skill.Buff.TimeLeftSeconds.Max(); 
                pti.TimeMax = pti.TimeLeft + skill.Buff.TimeElapsedSeconds.Max();                
            }
            else
            if (skill.IsOnCooldown && (skill.CooldownFinishTick > _hud.Game.CurrentGameTick))
            {
                pti.TimeLeft = (skill.CooldownFinishTick - _hud.Game.CurrentGameTick) / 60.0d;
                pti.TimeMax = (skill.CooldownFinishTick - skill.CooldownStartTick) / 60.0d;                
            }

            return pti;
        }                

    }
}    
