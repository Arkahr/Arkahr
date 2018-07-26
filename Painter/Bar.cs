using SharpDX;
using Turbo.Plugins.Default;

// How to contain IBuff or IPlayerSkill but not both?
// TODO 
// IRechargable ?

namespace Turbo.Plugins.Arkahr
{
        public enum BarType
        {
            Skill,
            Buff
        }

        public class Bar {
            // public int X;
            // public int Y;
            // public int Width;
            // public int Height;
            public string Name;
            private ISnoPower SnoPower;
            public BarType Type;
            private IBuff buff;
            private IPlayerSkill skill;

            public Bar(ISnoPower sno) {
                SnoPower = sno;
                //Checking for being a skill 
                // is it sufficient?
                if (SnoPower.HasKnownRunesValues) Type = BarType.Skill;
                else Type = BarType.Buff;               
            }

            public Bar(IBuff buff) {
                this.buff = buff;
            }

            public Bar(IPlayerSkill skill) {
                this.skill = skill;
            }

            public void Paint(RectangleF rect, IBrush brush, IFont font) {
                //not sure if is it great idea to make painting recurrency style               
                brush.DrawRectangle(rect);  
                DrawName(rect, font, Name);
                //TextLayout GetTextLayoutManualDispose(string text);  //use for cooldowns
                //DrawCooldownText(rect, brush, font);
            }

            protected void DrawName(RectangleF rect, IFont font, string text) {
                var layout = font.GetTextLayout(text);
                font.DrawText(text,rect.Left, rect.Top); //enableLayautCache what it does?       
            }

            protected void DrawCooldownText(RectangleF rect, IFont font, double time) {               

            }

            public ISnoPower getSnoPower() {
                return SnoPower;
            }
        }    
}