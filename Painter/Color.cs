using  System.Globalization;

namespace Turbo.Plugins.Arkahr {

    public class Color
    {
        public int Alpha { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }        

        public Color(int alpha, int red, int green, int blue) 
        {
            Alpha = alpha;
            Red = red;
            Green = green;
            Blue = blue;
            
        }

        public Color(int alpha, string hexColor) 
        //====================================================
        //| Downloaded From                                  |
        //| Visual C# Kicks - http://www.vcskicks.com/       |
        //| License - http://www.vcskicks.com/license.html   |
        //====================================================
        {
            //Remove # if present
            if (hexColor.IndexOf('#') != -1)
                hexColor = hexColor.Replace("#", "");

            int red = 0;
            int green = 0;
            int blue = 0;

            if (hexColor.Length == 6)
            {
                //#RRGGBB
                red = int.Parse(hexColor.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                green = int.Parse(hexColor.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                blue = int.Parse(hexColor.Substring(4, 2), NumberStyles.AllowHexSpecifier);


            }
            else if (hexColor.Length == 3)
            {
                //#RGB
                red = int.Parse(hexColor[0].ToString() + hexColor[0].ToString(), NumberStyles.AllowHexSpecifier);
                green = int.Parse(hexColor[1].ToString() + hexColor[1].ToString(), NumberStyles.AllowHexSpecifier);
                blue = int.Parse(hexColor[2].ToString() + hexColor[2].ToString(), NumberStyles.AllowHexSpecifier);
            }

            Alpha = alpha;
            Red = red;
            Green = green;
            Blue = blue;
        }                       
    } 
}