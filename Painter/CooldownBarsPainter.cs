using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Turbo.Plugins.Default;
using SharpDX.DirectWrite;


//Turbo.Plugins.Arkahr.Color class is ambigous with System.Drawing.Color
//so if you want to use CooldownBarsPainter you will also need the class Color

//TODO
//show cooldowns last activated on the bottom of bar list bars
//now it is in order adds to Rules list

namespace Turbo.Plugins.Arkahr
{   

    public class CooldownBarsPainter :  ITransparentCollection, ITransparent
    {

        public bool Enabled { get; set; }
        public IController Hud { get; set; }

        //Text
        //TODO opisac <Summary> reszte wlasciwosci i metod
        public IFont TimeLeftFont { get; set; }

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

        public IFont StackFont { get; set; }

        public IFont NameFont { get; set; }

        public IBrush TimeLeftClockBrush { get; set; }

        //TODO  rzwiaz problem miedzy przekazywaniem coloru cooldownu a go ustalaniem w plluginie
        public Color CooldownTextColor { get; set;}

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
        //public bool ShowBarTimeLeftNumbers { get; set; }
        //public bool ShowIconTimeLeftNumbers { get; set; }
        public bool HasIconBorder { get; set; }
        
        //Bar
        //TODO ADD PROPERTY
        public string BarName { get; set; }
        //TODO ADD PROPERTY
        
        //TODO ADD PROPERTY
        public Color BuffBarColor { get; set;} //when there is no activebuffcolor set  actualy it should be per buff ?
        // or probably palyer shoould do it themselves in plugin designed to show specific buff!        
        public Color SkillBarColor { get; set;}
        public Color CooldownBarColor { get; set;}

        //TODO ADD PROPERTY
        public float Width {get; set; }
        //TODO ADD PROPERTY
        public string AciveBuffSound {get; set; }
        //TODO ADD PROPERTY
        public int BarsLocation {get; set;}
        //TODO ADD PROPERTY
        public Shrink ShrinkingDirection {get; set;}

        /// <summary>
        /// Changes size of bar, and buff icon altogether.
        /// Icon get size of bar height.
        /// Negative value moves TimeLeftFont to he left.
        /// </summary>        
        public float SizeMultiplier { get; set; }
        public float StrokeWidth { get; set; }  

        /// <summary>
        /// Vertical spacing between bars.
        /// </summary>           
        public float VerticalSpacing { get; set; }  

        //TODO czy chcesz tu miec ?     
        public ITexture BackgroundTexture { get; set; }

        /// <summary>
        /// Bar background color.
        /// </summary>        
        public Color BackgroundColor { get; set; }

        public RectangleF BackgroundRect {get; set;}

        //Icon
        /// <summary>
        /// Space between bar and icon.
        /// Positive values push icon further away from bar edge. 
        ///  Negative values pull icon to the bar edge and beyond. 
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

        private TextDebug TextDebug;           

        public CooldownBarsPainter(IController hud, bool setDefaultStyle)
        {  

            Enabled = true;
            Hud = hud; 

            if (setDefaultStyle)
            {
                TimeLeftFont = Hud.Render.CreateFont("tahoma", 8, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
                StackFont = Hud.Render.CreateFont("tahoma", 9, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
                NameFont = Hud.Render.CreateFont("tahoma", 9, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
                TimeLeftClockBrush = Hud.Render.CreateBrush(220, 0, 0, 0, 0);
            }
            Opacity = 1f;

            ShowTooltips = true;
            ShowTimeLeftNumbers = true;
            //ShowActiveBuffTimeLeft = false;
            SizeMultiplier = 1;
            StrokeWidth = 0; 
            BuffBarColor = new Color(255, "daa520");
            BackgroundColor = new Color(170,"000");
            CooldownBarColor = new Color(200,"708090");
            //IconSpacing = 0;    
            ShowIcon = true;      
            //HasIconBorder = true;                        
            //IconAlign = IconAlign.Left;  
            TextAlign = TextAlign.Right;  
            //TextSpacing = 0f;              
            //VerticalSpacing = 5;  

            TextDebug = new TextDebug(100,200,hud);                 
        }

        public void PaintBuffs(List<BuffPaintInfo> infoList, float x, float y, float w, float h, float s, Color color)
        {
            foreach (var info in infoList)
            {
                PaintBuff(info, x, y, w, h, color);
                y += s + h;
            }
        }    
                   
        public void PaintBuffs(List<BuffPaintInfo> infoList, float x, float y, float w, float h, float s)        
        {
            PaintBuffs(infoList, x, y, w, h, s, BuffBarColor);            
        }

        public void PaintBuff(BuffPaintInfo info, float x, float y, float w, float h, Color color) 
        {
            var rect =  new RectangleF(x, y , w, h);
            BackgroundRect = rect;
            Paint(info, rect, color);
        }

        public void PaintSkills(List<IPlayerSkill> skillList, float x, float y, float w, float h, float s, Color color)
        {
            //var d = new TextDebug(500,500,Hud);
            foreach (var skill in skillList)
            {               
                var rect =  new RectangleF(x, y , w, h);
                BackgroundRect = rect;
               // d.addText(skill.SnoPower.NameLocalized + "\n");
               // d.Print();
                PaintSkill(skill, rect, color);
                y += s + h;
            }
        }               
        


// painting method for skills  ------------------------------------------------------------------------------------------------//
        public void PaintSkill(IPlayerSkill skill, RectangleF rect,  Color color)
        {    
            if (skill == null) return;
            if (Opacity == 0) return;
            double activeBuffTimeleft = 0;
            double activeBuffTime = 0;
            double cooldownTimeleft = 0;
            double cooldownTime = 0;

            
            var barRect = (SizeMultiplier!=0 && SizeMultiplier!=1)?ResizeBar(rect):rect;
            //icon new coords            
            var iconSize = barRect.Height; 
            var iconRect = new SharpDX.RectangleF(barRect.Left - IconSpacing - iconSize, barRect.Y, iconSize, iconSize);            
            var texture = Hud.Texture.GetTexture(skill.SnoPower.NormalIconTextureId);
            if (IconAlign == IconAlign.Right) iconRect.X = barRect.Right + iconRect.Width + IconSpacing;  
            else iconRect.X = barRect.Left - iconRect.Width - IconSpacing;                 

               
            if (skill.BuffIsActive) {
                activeBuffTimeleft = skill.Buff.TimeLeftSeconds.Max(); //skill.Buff.TimeLeftSeconds[0];
                activeBuffTime = activeBuffTimeleft + skill.Buff.TimeElapsedSeconds.Max();                
                DrawBar(barRect, color, activeBuffTimeleft, activeBuffTime);
                if (texture != null) texture.Draw(iconRect.X, iconRect.Y, iconRect.Width, iconRect.Height , Opacity);
                DrawTimeLeftNumbers(rect, activeBuffTimeleft);
                //Draw skill name                
                DrawName(barRect,skill.SnoPower.NameLocalized);                
            } 
            if (!skill.BuffIsActive)
                if (skill.IsOnCooldown && (skill.CooldownFinishTick > Hud.Game.CurrentGameTick))
                {
                    cooldownTimeleft = (skill.CooldownFinishTick - Hud.Game.CurrentGameTick) / 60.0d;
                    cooldownTime = (skill.CooldownFinishTick - skill.CooldownStartTick) / 60.0d;                
                    DrawBar(rect, CooldownBarColor, cooldownTimeleft, cooldownTime); 
                    if (texture != null) texture.Draw(iconRect.X, iconRect.Y, iconRect.Width, iconRect.Height , Opacity);
                    DrawTimeLeftNumbers(rect, cooldownTimeleft); 
                    //Draw skill name
                    DrawName(barRect,skill.SnoPower.NameLocalized);                    
                }


            /// Hint rectangle detection
/*             if (Hud.Window.CursorInsideRect(iconRect.X, iconRect.Y, ShowIcon==true?iconRect.Width + IconSpacing:0 + barRect.Width, barRect.Height ) && ShowTooltips)
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
            } */
            
        }

// painting method for Buffs --------------------------------------------------------------------------------------------//
        protected void Paint(BuffPaintInfo info, RectangleF rect,  Color color)
        {                        
            var barRect = (SizeMultiplier!=0 && SizeMultiplier!=1)?ResizeBar(rect):rect;
            var firstIcon = info.Icons[0];
            var isDebuff = firstIcon.Harmful;

            //icon new coords            
            var iconSize = barRect.Height;
            var iconRect = new SharpDX.RectangleF(barRect.Left - IconSpacing - iconSize, barRect.Y, iconSize, iconSize);            

            //Drawing icon
            if (ShowIcon) 
            {
                if (IconAlign == IconAlign.Right) iconRect.X = barRect.Right + IconSpacing;                                                  
                    if (info.BackgroundTexture != null)
                    {
                        info.BackgroundTexture.Draw(iconRect.X, iconRect.Y, iconRect.Width, iconRect.Height , Opacity);
                    }
                    info.Texture.Draw(iconRect.X, iconRect.Y, iconRect.Width, iconRect.Height , Opacity);            

                    if (HasIconBorder)
                    {
                        (isDebuff ? Hud.Texture.DebuffFrameTexture : Hud.Texture.BuffFrameTexture).Draw(iconRect.X, iconRect.Y, iconRect.Width, iconRect.Height, Opacity);
                    }                               
            }
            //Paint bar
            DrawBar(info, barRect, color);

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
            DrawTimeLeftNumbers(barRect, iconRect, info.TimeLeft);
        }
        
        private void DrawBar(BuffPaintInfo info, RectangleF rect, Color color) 
        {               
            float width = rect.Width;                        
            double tl = info.TimeLeft;
            double te = info.Elapsed;                       
            width = (float)(width * tl/ (tl + te ));   
            //Resize background if SizeMultiplier is set
            BackgroundRect = (SizeMultiplier!= 1 && SizeMultiplier!=0)?ResizeBar(rect):rect;
            //Paint background bar
            Hud.Render.CreateBrush(BackgroundColor.Alpha, BackgroundColor.Red, BackgroundColor.Green, BackgroundColor.Blue, StrokeWidth).DrawRectangle(BackgroundRect.X, BackgroundRect.Y, BackgroundRect.Width , BackgroundRect.Height);            
            //Paint time bar
            
            Hud.Render.CreateBrush(color.Alpha, color.Red, color.Green, color.Blue, StrokeWidth).DrawRectangle(rect.X, rect.Y, width , rect.Height);
        }

        private void DrawBar( RectangleF rect, Color color, double timeleft, double time) 
        {               
            float width = rect.Width;            
            if (timeleft == time)  { 
       

                width = (float)(BackgroundRect.Width / timeleft);   
            } else                              
                width = (float)(width * timeleft / time);   
            //Resize background if SizeMultiplier is set
            BackgroundRect = (SizeMultiplier!= 1 && SizeMultiplier!=0)?ResizeBar(rect):rect;
            //Paint background bar
            Hud.Render.CreateBrush(BackgroundColor.Alpha, BackgroundColor.Red, BackgroundColor.Green, BackgroundColor.Blue, StrokeWidth).DrawRectangle(BackgroundRect.X, BackgroundRect.Y, BackgroundRect.Width , BackgroundRect.Height);            
            //Paint cooldown bar            
            Hud.Render.CreateBrush(color.Alpha, color.Red, color.Green, color.Blue, StrokeWidth).DrawRectangle(rect.X, rect.Y, width , rect.Height);
        }

        private void DrawName(RectangleF rect, String text) {
            var layout = NameFont.GetTextLayout(text);                                                        
            NameFont.DrawText(layout, rect.X + TextSpacing + (float)Math.Ceiling(layout.Metrics.Height), rect.Y + (rect.Height - layout.Metrics.Height) / 2);            
        }



        private void DrawTimeLeftNumbers(RectangleF barRect, RectangleF iconRect, double timeleft)
        {
            if (timeleft == 0) return; //dlatego nie rysowal liczb?
            if (!ShowTimeLeftNumbers) return;
            //if (info.TimeLeftNumbersOverride != null && info.TimeLeftNumbersOverride.Value == false) return;
            
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
      
            DrawBarTimeLeftNumbers(barRect, text);
            //DrawIconTimeLeftNumbers(iconRect, text);
        }

        private double getTimeLeft(double timeleft) 
        {
            if (timeleft == 0) return 0; //dlatego nie rysowal liczb?
            if (!ShowTimeLeftNumbers) return 0;
            //if (info.TimeLeftNumbersOverride != null && info.TimeLeftNumbersOverride.Value == false) return;
            
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

            return Convert.ToDouble(text);
        }


        private void DrawTimeLeftNumbers(RectangleF barRect, double timeleft)
        {
            if (timeleft == 0) return; //dlatego nie rysowal liczb?
            if (!ShowTimeLeftNumbers) return;
            //if (info.TimeLeftNumbersOverride != null && info.TimeLeftNumbersOverride.Value == false) return;
            
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
      
            DrawBarTimeLeftNumbers(barRect, text);
        }

/*         private void IconTimeLeftNumbers(RectangleF rect, String text) {
            var layout = TimeLeftFont.GetTextLayout(text);
            TimeLeftFont.DrawText(layout, rect.X + (rect.Width - (float)Math.Ceiling(layout.Metrics.Width)) / 2.0f, rect.Y + (rect.Height - layout.Metrics.Height) / 2);
        } */

        private void DrawBarTimeLeftNumbers(RectangleF rect, String text) {
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
                                 
            return new RectangleF(rect.X, rect.Y, rect.Width * SizeMultiplier, rect.Height * SizeMultiplier);            
        }

        public IEnumerable<ITransparent> GetTransparents()
        {
            yield return TimeLeftFont;
            yield return StackFont;
            yield return TimeLeftClockBrush;
            yield return this;
        }
    }

}