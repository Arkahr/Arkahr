using System;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.Arkahr {

    public class TextDebug {
        private string debugText;
        public IFont DebugFont;
        private SharpDX.DirectWrite.TextLayout layout;
        public float X;
        public float Y;

        public TextDebug(float x, float y, IController Hud) 
        {
            X = x;
            Y = y;

            DebugFont = Hud.Render.CreateFont("tahoma", 10, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
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
    }
}


/* 
         debugText += string.Format("{0} info[{1}] TimeLeft {3:F} Elapsed {4:F}\n", _zywioly[(int)info.Rule.IconIndex], orderIndex, info.Rule.IconIndex, info.TimeLeft, info.Elapsed );
debugText = "1st line: Paint() iconVisible: " + iconVisible.ToString() + "\nShowActiveBuffTimeLeft:" + ShowActiveBuffTimeLeft.ToString(); */