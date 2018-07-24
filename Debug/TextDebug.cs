using System;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.Arkahr {

    public class TextDebug {
        private string debugText;
        public IFont DebugFont;
        private SharpDX.DirectWrite.TextLayout layout;
        public float X;
        public float Y;
        private IController hud;

        public TextDebug(float x, float y, IController hud) 
        {
            X = x;
            Y = y;
            this.hud = hud;

            DebugFont = hud.Render.CreateFont("tahoma", 7, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
            debugText = "";
        }

        public void addText(string text) 
        {            
            debugText += text;
        }        
        
        public void Clear() 
        {
            debugText = "";
        }

        public void Print() 
        {            
            layout = DebugFont.GetTextLayout(debugText.ToString());
            DebugFont.DrawText(layout, X , Y);      
        }
        public void setTextSize(int i) {
            DebugFont = hud.Render.CreateFont("tahoma", i, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
        }
    }
}


/* 
         debugText += string.Format("{0} info[{1}] TimeLeft {3:F} Elapsed {4:F}\n", _zywioly[(int)info.Rule.IconIndex], orderIndex, info.Rule.IconIndex, info.TimeLeft, info.Elapsed );
debugText = "1st line: Paint() iconVisible: " + iconVisible.ToString() + "\nShowActiveBuffTimeLeft:" + ShowActiveBuffTimeLeft.ToString(); */