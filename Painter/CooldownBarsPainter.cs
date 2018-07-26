// 26/07/2018

using SharpDX;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Turbo.Plugins.Default;

//TODO
//FEATURES
//1. show cooldowns last activated on the bottom of bar list bars
//now it is in order they added to Rules list

//2. Bar class for adding bars on the fly
// public List<Bar> BarsList;
//BarsList.Add( new Bar(IPlayerSkill) {Name, X,Y,Width,Height ... })
//BarsList.Add( new Bar(IBuff) {Name, X,Y,Width,Height ... })

//3. group of bars to show together as Frames
//public List<Frame> Frames;
//Frames.Add( new Frame(?) { Title ="Offensive CD", X = Position.TopCenter, Y = Position.Top, With, MaxBars .... } ))

namespace Turbo.Plugins.Arkahr
{   
    public class CooldownBarsPainter :  ITransparentCollection, ITransparent
    {
        public bool Enabled { get; set; }
        public IController Hud { get; set; }

        /// <summary> 
        /// Bar text align.
        /// Default value: TextAlign.Left
        /// </summary>         
        public TextAlign TextAlign { get; set; }        

        /// <summary>
        /// Text space from the bar edge.
        /// Positive value moves time left to the right.
        /// Negative value moves time left to he left.
        /// </summary>
        public float TextSpacing  { get; set;} // to the right, -X if you want to move it to the left

        public IFont TimeLeftFont { get; set; }
        public IFont NameFont { get; set; }

        /// <summary>
        /// Icon opacity.
        /// </summary>         
        public float Opacity { get; set; }

        /// <summary>
        /// Toggle showing toolips.
        /// By default they are shown.
        /// </summary>         
        public bool ShowTooltips { get; set; }

        /// <summary>
        /// Toggle showing buff cooldown time.
        /// By default it is shown.
        /// </summary>         
        public bool ShowTimeLeftNumbers { get; set; }
     
        /// <summary>
        /// Color of buff bar.
        /// </summary>              
        public IBrush BuffBarBrush { get; set;}    

        /// <summary>
        /// Color of bar when skill is on cooldown.
        /// </summary>              
        public IBrush CooldownBarBrush { get; set;}

        /// <summary>
        /// Color of bar when skill is active.
        /// </summary>      
        public IBrush SkillBarBrush { get; set;}

        /// <summary>
        /// Changes size of bar, and icon altogether.
        /// </summary>        
        public float SizeMultiplier { get; set; }

        /// <summary>
        /// Vertical spacing between bars.
        /// </summary>           
        public float VerticalSpacing { get; set; }  

        //TODO    
        //if there is a way of importing textures
        //make bars wow style
        private ITexture barTexture { get; set; }

        /// <summary>
        /// Bar background color.
        /// </summary>        
        public IBrush BackgroundBrush { get; set; }

        //Icon
        /// <summary>
        /// Space between bar and icon.
        /// Positive values push icon further away from bar edge. 
        /// Negative values pull icon to the bar edge and beyond. 
        /// No matter of icon allign.
        /// </summary>  
        public float IconSpacing { get; set; }  

        /// <summary>
        /// Icon visiblility.
        /// </summary>          
        public bool ShowIcon { get; set;}    

        /// <summary>
        /// Icon align to the bar.
        /// Default value: IconAlign.Left
        /// </summary>      
        public IconAlign IconAlign { get; set;}    

        //private TextDebug td;  
        private RectangleF BackgroundRect {get; set;}

        //TODO 
        // feature ad. 3)
        //public List<Frame> Frames;        

        //Skills with charges
        private static HashSet<uint> rechargeSkillsId = new HashSet<uint>() 
        { 
            129217, //DH Sentry all runes
            75301, //DH Spike Trap all runes
            353447, //Barb Avalanche - "Tectonic Rift" rune 
            97435, // Barb Furious Charge all runes -  "Dreadnought" has 3 charges
            312736, //Monk Dashing Strike  all runes - "Quicksilver" 3 charges
            464896, //Necro Bone Spirit
            454090,  //Necro Blood Rush - "Metabolism" rune
        };

        public CooldownBarsPainter(IController hud, bool setDefaultStyle)
        {  

            Enabled = true;
            Hud = hud; 

            if (setDefaultStyle)
            {
                TimeLeftFont = Hud.Render.CreateFont("tahoma", 8, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
                NameFont = Hud.Render.CreateFont("tahoma", 9, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
            }
            Opacity = 1f;

            ShowTooltips = true;
            ShowTimeLeftNumbers = true;
            SizeMultiplier = 1;
            BuffBarBrush = Hud.Render.CreateBrush(255, 218, 165, 32, 0);
            BackgroundBrush = Hud.Render.CreateBrush(255, 0, 0, 0, 0);
            CooldownBarBrush = Hud.Render.CreateBrush(255, 112, 128, 144, 0);

            SkillBarBrush = Hud.Render.CreateBrush(255, 0, 204, 153, 0);
            IconSpacing = 0;    
            ShowIcon = true;      
            IconAlign = IconAlign.Left;  
            TextAlign = TextAlign.Right;  
            TextSpacing = 0f;              
            VerticalSpacing = 1;  

            //Frames = new List<Frame>();
            //td = new TextDebug(100,200,hud);                 
        }
   
        public void PaintBuffs(List<BuffPaintInfo> infoList, float x, float y, float w, float h, float s)        
        {
            PaintBuffs(infoList, x, y, w, h, s, BuffBarBrush);            
        }

        public void PaintBuffs(List<BuffPaintInfo> infoList, float x, float y, float w, float h, float s, IBrush brush)
        {
            foreach (var info in infoList)
            {
                PaintBuff(info, x, y, w, h, brush);
                y += s + h;
            }
        }    

        public void PaintBuff(BuffPaintInfo info, float x, float y, float w, float h, IBrush brush) 
        {
            BackgroundRect = new RectangleF(x, y , w, h);
            PaintBuff(info, new RectangleF(x, y , w, h), brush);
        }

        public void PaintSkills(List<IPlayerSkill> skillList, float x, float y, float w, float h)
        {
            BackgroundRect = new RectangleF(x, y , w, h);
            //TODO skills sort by cooldown    
            foreach (var skill in skillList)
            {                             
                if (skillPaintable(skill)) { 
                    PaintSkill(skill, new RectangleF(x, y , w, h));                           
                    y += VerticalSpacing + h;                             
                }
            }
        }               
        
        private bool skillPaintable(IPlayerSkill skill) {
            //paint skill only if:
            // is rechargable and recharging or
            // is active buff or
            // is not active buff but is on cooldown and cooldown time still ticking
            if ((rechargeSkillsId.Contains(skill.SnoPower.Sno) && 
                Hud.Game.Me.GetAttributeValue(Hud.Sno.Attributes.Next_Charge_Gained_time, skill.SnoPower.Sno, -1 )!=0) || 
                skill.BuffIsActive || 
                (!skill.BuffIsActive && skill.IsOnCooldown && skill.CooldownFinishTick > Hud.Game.CurrentGameTick)) {
                    return true;   
            }
            return false;
        }

        // painting method for skills
        public void PaintSkill(IPlayerSkill skill, RectangleF rect)
        {    
            if (skill == null) return;
            if (Opacity == 0) return;

            double activeBuffTimeleft = 0;
            double cooldownTimeleft = 0;
            double duration = 0; //elapsed + left
            
            var barRect = (SizeMultiplier!=0 && SizeMultiplier!=1)?ResizeBar(rect):rect;
            //icons gets size of bar height
            var iconSize = barRect.Height; 
            //icon new coords            
            var iconRect = new RectangleF(barRect.Left - IconSpacing - iconSize, barRect.Y, iconSize, iconSize);            
            var texture = Hud.Texture.GetTexture(skill.SnoPower.NormalIconTextureId);
            if (IconAlign == IconAlign.Right) iconRect.X = barRect.Right + iconRect.Width + IconSpacing;  
            else iconRect.X = barRect.Left - iconRect.Width - IconSpacing;                 
               
            if (skill.BuffIsActive ) {          
                activeBuffTimeleft = skill.Buff.TimeLeftSeconds.Max(); 
                duration = activeBuffTimeleft + skill.Buff.TimeElapsedSeconds.Max();                
                DrawBar(barRect, SkillBarBrush, activeBuffTimeleft, duration);
                if (texture != null) texture.Draw(iconRect.X, iconRect.Y, iconRect.Width, iconRect.Height , Opacity);
                DrawTimeLeftNumbers(rect, getText(activeBuffTimeleft));
                //Draw skill name                
                DrawName(barRect,skill.SnoPower.NameLocalized);                
            } 
            if (!skill.BuffIsActive) {
                if (skill.IsOnCooldown && (skill.CooldownFinishTick > Hud.Game.CurrentGameTick))      
                {
                    cooldownTimeleft = (skill.CooldownFinishTick - Hud.Game.CurrentGameTick) / 60.0d;
                    duration = (skill.CooldownFinishTick - skill.CooldownStartTick) / 60.0d;                
                    DrawBar(rect, CooldownBarBrush, cooldownTimeleft, duration ); 
                    if (texture != null) texture.Draw(iconRect.X, iconRect.Y, iconRect.Width, iconRect.Height , Opacity);
                    DrawTimeLeftNumbers(rect, getText(cooldownTimeleft)); 
                    //Draw skill name
                    DrawName(barRect,skill.SnoPower.NameLocalized);                    
                }
            }

            if (rechargeSkillsId.Contains(skill.SnoPower.Sno)) { // rechargable skill
                //if (skill.SnoPower.RuneNamesEnglish == "") 
                //Rading skill sno attributes automaticaly resolves problem of choosing skill runes that makes skill rechargeable

                var Recharge_Start_Time = Hud.Game.Me.GetAttributeValue(Hud.Sno.Attributes.Recharge_Start_Time, skill.SnoPower.Sno, -1 );  
                var Next_Charge_Gained_time = Hud.Game.Me.GetAttributeValue(Hud.Sno.Attributes.Next_Charge_Gained_time, skill.SnoPower.Sno, -1 );
                if (Next_Charge_Gained_time==0) return; //skill is not recharging
                cooldownTimeleft = (Next_Charge_Gained_time - Hud.Game.CurrentGameTick) / 60.0d;
                duration = (Next_Charge_Gained_time - Recharge_Start_Time) / 60.0d;
                DrawBar(rect, CooldownBarBrush, cooldownTimeleft, duration); 
                if (texture != null) texture.Draw(iconRect.X, iconRect.Y, iconRect.Width, iconRect.Height , Opacity);
                DrawTimeLeftNumbers(rect, getText(cooldownTimeleft)); 
                var layout = NameFont.GetTextLayout(skill.Charges.ToString());                                                        
                NameFont.DrawText(layout, iconRect.X - iconRect.Width + layout.Metrics.Width, iconRect.Y + (iconRect.Height - layout.Metrics.Height) / 2);            
                //Draw name and "charging" message
                DrawName(barRect,string.Format("{0} - charging", skill.SnoPower.NameLocalized));    
            }
        }


        // protected void PaintFrames() {
        //     // Bar tmpBar = bar;
        //     foreach (var frame in Frames)
        //     {
        //         frame.Paint();
        //     }

        // }
        
        protected void PaintBuff(BuffPaintInfo info, RectangleF rect,  IBrush brush)
        {                        
            var barRect = (SizeMultiplier!=0 && SizeMultiplier!=1)?ResizeBar(rect):rect;
            var firstIcon = info.Icons[0];
            //What it does?
            //var isDebuff = firstIcon.Harmful;

            //icon new coords            
            var iconSize = barRect.Height;
            var iconRect = new RectangleF(barRect.Left - IconSpacing - iconSize, barRect.Y, iconSize, iconSize);            

            //Drawing icon
            if (ShowIcon) 
            {
                if (IconAlign == IconAlign.Right) iconRect.X = barRect.Right + IconSpacing;                                                  
                    if (info.BackgroundTexture != null) 
                    {
                        info.BackgroundTexture.Draw(iconRect , Opacity);
                    }
                    info.Texture.Draw(iconRect , Opacity);                                    
            }
            //Paint bar
            DrawBar(barRect, brush, info.TimeLeft, info.TimeLeft + info.Elapsed);

            /// Hint rectangle detection
            if (Hud.Window.CursorInsideRect(iconRect.X, iconRect.Y, ShowIcon==true?iconRect.Width + IconSpacing:0 + barRect.Width, barRect.Height ) && ShowTooltips)
            {
                if (info.Rule == null)
                {
                    var name = info.SnoPower.NameLocalized;
                    if (name == null)
                    {
                        foreach (var icon in info.Icons) name += (name == null ? "" : "\n") + icon.TitleLocalized;
                    }
                    string desc = null;
                    foreach (var icon in info.Icons) desc += (desc == null ? "\n\n" : "\n") + icon.DescriptionLocalized;
                    Hud.Render.SetHint(name + desc);
                }
                else
                {
                    if (firstIcon.Exists)
                    {
                        var name = (!info.Rule.DisableName ? (info.Rule.UsePowersName ? info.SnoPower.NameLocalized : firstIcon.TitleLocalized) : null);
                        var desc = (info.Rule.UsePowersDesc ? info.SnoPower.DescriptionLocalized : (firstIcon.DescriptionLocalized != null ? firstIcon.DescriptionLocalized : ""));
                        Hud.Render.SetHint(name + (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(desc) ? "\n\n" : "") + desc);
                    }
                }
            }
            
            //Draw cooldown text            
            DrawTimeLeftNumbers(barRect, getText(info.TimeLeft));            
        }
        
        private void DrawBar( RectangleF rect, IBrush brush, double timeleft, double time) 
        {               
            float width = rect.Width; 
            //resize width according to time left
            width = (float)(width * timeleft / time);   
            //Resize background if SizeMultiplier is set
            BackgroundRect = (SizeMultiplier!= 1 && SizeMultiplier!=0)?ResizeBar(rect):rect;
            //Paint background bar
            BackgroundBrush.DrawRectangle(BackgroundRect);            
            //Paint cooldown bar            
            brush.DrawRectangle(rect.X, rect.Y, width , rect.Height);
        }

        private void DrawName(RectangleF rect, String text) {
            var layout = NameFont.GetTextLayout(text);                                                        
            NameFont.DrawText(layout, rect.X + TextSpacing + (float)Math.Ceiling(layout.Metrics.Height), rect.Y + (rect.Height - layout.Metrics.Height) / 2);            
        }

        private string getText(double timeleft)
        {
            if (timeleft == 0) return "0"; 
            if (!ShowTimeLeftNumbers) return "";
            
            var text = "";
            if (timeleft > 1.0f)
            {
                var mins = Convert.ToInt32(Math.Floor(timeleft / 60.0d));
                var secs = Math.Floor(timeleft - mins * 60.0d);
                if (timeleft >= 60)
                {
                    text = mins.ToString("F0", CultureInfo.InvariantCulture) + ":" + (secs < 10 ? "0" : "") + secs.ToString("F0", CultureInfo.InvariantCulture);
                }
                else text = timeleft.ToString("F0", CultureInfo.InvariantCulture);
            }
            else text = timeleft.ToString("F1", CultureInfo.InvariantCulture);
            return text;
        }

        private void DrawTimeLeftNumbers(RectangleF rect, string text) {
            var layout = TimeLeftFont.GetTextLayout(text);                                                        
            if (TextAlign == TextAlign.Center) TimeLeftFont.DrawText(layout, rect.X + (rect.Width - (float)Math.Ceiling(layout.Metrics.Width)) / 2.0f, rect.Y + (rect.Height - layout.Metrics.Height) / 2);        
            else if (TextAlign == TextAlign.Right) 
            {
                if (text.Length == 1) TimeLeftFont.DrawText(layout, rect.Right + TextSpacing - (float)Math.Ceiling(layout.Metrics.Width) * 2f , rect.Y + (rect.Height - layout.Metrics.Height) / 2);
                else TimeLeftFont.DrawText(layout, rect.Right + TextSpacing - (float)Math.Ceiling(layout.Metrics.Width) * 1.5f , rect.Y + (rect.Height - layout.Metrics.Height) / 2);
            }                        
            else  
            {
                if (text.Length == 1) text = " " + text;
                layout = TimeLeftFont.GetTextLayout(text);
                TimeLeftFont.DrawText(layout, rect.X + TextSpacing + (float)Math.Ceiling(layout.Metrics.Width) / 2.0f, rect.Y + (rect.Height - layout.Metrics.Height) / 2);
            }
        }


        private RectangleF ResizeBar(RectangleF rect) {   
            rect.Width = rect.Width * SizeMultiplier;                   
            rect.Height = rect.Height * SizeMultiplier;                   
            return rect;
        }

        //how to use it for bars ? ->bar
        public IEnumerable<ITransparent> GetTransparents()
        {
            yield return TimeLeftFont;
            yield return NameFont;
            yield return this;
        }
    }

}