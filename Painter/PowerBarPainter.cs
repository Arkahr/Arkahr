using Turbo.Plugins;
using Turbo.Plugins.Default;
using SharpDX;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Turbo.Plugins.Arkahr 
{    
    interface IPaintable
    {
        IBrush BackgroundBrush {get;set;}
        IBrush ActivePowerBrush {get;set;}
        IBrush CooldownBrush {get;set;}
        IFont TimeLeftFont {get;set;}
    }
    
    interface IP
    {
        IBrush BackgroundBrush {get;set;}
        IBrush ActivePowerBrush {get;set;}
        IBrush CooldownBrush {get;set;}
        IFont Font {get;set;}

        ITexture BackgroundTexture {get; set;}        
        ITexture ForegroundTexture {get;set;}
        
    }

    public class Bar 
    {
        private RectangleF _rectangle;
        public float X {get; set;} 
        public float Y {get; set;}
        public float Width {get; set;}
        public float Height {get; set;}
        public string Text {get; set;}

        public IBrush ActiveColorBrush {get; set;}
        public IBrush BackgroundBrush {get; set;}

        //THINK how can i disable it, or dont include if this class will be used also for DataPainter dor resources etc
        public IBrush CooldownBrush {get; set;}
        public ITexture BackgroundTexture {get; set;}
        public ITexture ForegroundTexture {get; set;}
        

        public Bar(float x, float y, float width, float height )
        {
            _rectangle.X = X = x;
            _rectangle.Y = Y = y;
            _rectangle.Width = Width = width;
            _rectangle.Height = Height = height;
        }
        public Bar(RectangleF rect) {
            X = rect.X;
            Y = rect.Y;
            Width = rect.Width;
            Height = rect.Height;
        }
        public RectangleF getRectangle() {
            return new RectangleF(X, Y, Width, Height);
        }
    }

   
    public class PowerBarPainter : PowerPainter<Bar>, IPaintable
    {
        //TODO make PowerBar width overall width, when with icon and without.

        //TODO .... other fields, properties and methods //
        public IController Hud; //why it is set to public in classes :BasePlugin?
        
        //private IBrush _backgroundBrush;
        private PowerInfoCalculator _powerInfoCalculator;
        private BuffPaintInfo _buffPaintInfo; //for painting Buffs
        private IPlayerSkill _playerSkill;
        private PowerTimeInfo _timeInfo;

        //public IBrush ... do I need other brushes for Buff, Skill, Charge ?
        public IBrush BackgroundBrush {get;set;}
        public IBrush ActivePowerBrush {get;set;} //generic color if other not set
        public IBrush CooldownBrush {get;set;}

        public IFont TextFont {get;set;}
        public IFont TimeLeftFont {get;set;}
        public IFont ChargesFont {get;set;}
        public IFont StackFont {get;set;} //THINK when to use it                

        public PowerBarPainter(IController hud, bool setDefaultStyle) : base(hud, setDefaultStyle) 
        {
           Hud = hud;
           _powerInfoCalculator = new PowerInfoCalculator(hud);                
            if (setDefaultStyle)
            {
                // BuffBarBrush = Hud.Render.CreateBrush(255, 218, 165, 32, 0);
                // CooldownBarBrush = Hud.Render.CreateBrush(255, 112, 128, 144, 0);
                // SkillBarBrush = Hud.Render.CreateBrush(255, 0, 204, 153, 0);

                BackgroundBrush = Hud.Render.CreateBrush(225, 0, 0, 0, 0);
                ActivePowerBrush = Hud.Render.CreateBrush(255, 0, 89, 183, 0);
                CooldownBrush = Hud.Render.CreateBrush(255, 112, 128, 144, 0);
                TimeLeftFont = Hud.Render.CreateFont("tahoma", 8, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
                ChargesFont = StackFont = Hud.Render.CreateFont("tahoma", 9, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
                
            }
        }
        

        /// <summary>
        /// Creates customizable Bar for painting purpose.
        /// <param name="active">Used to indicate Brush of bar when power (Buff, Skill, Power) / resource (class resource, health etc.) value is greater than 0.</param>
        /// <param name="cooldown">Used to indicate Brush of bar when power ((Buff, Skill, Power) is on cooldown.</param>
        /// <param name="background">Used to indicate Brush of bar background.</param>
        // TODO Texture defined by user -> link to Jacks Textur List when he finish it
        // /// <param name="backgroundTexture">Used to indicate Brush of bar background texture.</param>
        // TODO check if you can paint rectangle Foregoround texture, on top of active rectangle,  and on top of background texture, make like 15 bars with full plugins use, and see if lag like hell
        // /// <param name="foregroundTexture">Used to indicate Brush of bar background texture.</param>
        /// </summary>  
        public Bar CreateBar(float x, float y, float width, float height, IBrush background, ITexture backgroundTexture, IBrush cooldown, IBrush active)
        {
            var bar = new Bar(x, y, width, height );
            bar.BackgroundTexture = backgroundTexture;
            bar.BackgroundBrush = background;
            bar.ActiveColorBrush = active;
            
            return bar;
        }

        public override void CalculatePowerInfo(BuffPaintInfo paintInfo)
        {
            _timeInfo = _powerInfoCalculator.GetPowerTimeInfo(paintInfo);   
            //_timeInfo = PowerInfoCalculator.GetPowerTimeInfo(paintInfo);   
        }

        public override void CalculatePowerInfo(IPlayerSkill playerSkill)
        {
            _timeInfo = _powerInfoCalculator.GetPowerTimeInfo(playerSkill);   
            //_timeInfo = PowerInfoCalculator.GetPowerTimeInfo(Hud, playerSkill);   
        }

        public override void CalculatePowerInfo(ISnoPower snoPower)
        {
            _timeInfo = _powerInfoCalculator.GetPowerTimeInfo(snoPower); 
           // _timeInfo = PowerInfoCalculator.GetPowerTimeInfo(Hud, snoPower); 

        }

        protected void PaintBackground(Bar bar)
        {           
            //BackgroundBrush.DrawRectangle() //tu juz mialem martwice mozgu ;) i poszedlem spac
            if (bar.BackgroundBrush != null)
                bar.BackgroundBrush.DrawRectangle(bar.getRectangle());
            else
                BackgroundBrush.DrawRectangle(bar.getRectangle());                        
        }

        public void PaintCooldown(Bar bar, PowerTimeInfo timeInfo)         
        {

        }

        public void PaintActive(Bar bar, PowerTimeInfo timeInfo)
        {

        }        

        public void PaintTimeLeftNumbers(Bar bar, PowerTimeInfo timeInfo) 
        {

        }    

        public void PaintText(Bar bar)         
        {
            var text = bar.Text;
            var textLayout = TextFont.GetTextLayout(text);
            var x = bar.X + (float)Math.Ceiling(textLayout.Metrics.Height/2);
            var y = bar.Y +  (bar.Height - textLayout.Metrics.Height) / 2;           
            TextFont.DrawText(textLayout, x, y);
        }        
        // Maybe public void Paint(Bar bar) ? //what it gives?

        public void Paint(RectangleF rect, List<BuffPaintInfo> listBuffPaintInfo) 
        {
            foreach (var buffPaintInfo in listBuffPaintInfo)
            {
                Paint(rect, buffPaintInfo);
            }
        }
        

        //TODO resolve issue that you need to get rectangle from user, to make it work
        //THINK better when instead of rectangles it would be Bar but, is PowerBarPainter.Paint(PowerBarPainter.CreateBar(x,y,w,h,bg,bgTex,active),skill) not over complicated ? how to chain metods ?
        public void Paint(RectangleF rect, BuffPaintInfo buffPaintInfo) 
        {
            //_bar = new Bar(rect);            
            CalculatePowerInfo(buffPaintInfo);
            PowerTimeInfo timeInfo =  _powerInfoCalculator.GetPowerTimeInfo(buffPaintInfo);
            //Paint();           
        }

        public void Paint(RectangleF rect, IPlayerSkill playerSkill) 
        {
            //_bar = new Bar(rect);            
            //CalculatePowerInfo(playerSkill);
            PowerTimeInfo timeInfo =  _powerInfoCalculator.GetPowerTimeInfo(playerSkill);
            //Paint();            
        }


        public void Paint(RectangleF rect, ISnoPower snoPower) 
        //skill charges painting
        // we should check what kind of power we will be painting,
        // we we will need another one  
        {
            //CalculatePowerInfo(snoPower);
            PowerTimeInfo timeInfo =  _powerInfoCalculator.GetPowerTimeInfo(snoPower);
            if (_timeInfo == null) return; // there are no charges to paint anymore
            Bar bar = new Bar(rect);            
            Paint(bar);
            
        }


        public override void Paint(Bar bar)
        //Baking our bar
        {                       
            PaintBackground(bar); 
            PaintCooldown(bar, _timeInfo); 
            PaintActive(bar, _timeInfo); 
            PaintTimeLeftNumbers(bar, _timeInfo);
            PaintText(bar); 
            //Baked!
        }       
    }
}