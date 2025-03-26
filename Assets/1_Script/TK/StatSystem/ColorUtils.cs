using System;
using System.Collections.Generic;

namespace Swift_Blade
{
    public static class ColorUtils
    {
        public static ColorType GetColor(List<ColorType> colors)
        {
            int redColor;
            int greenColor;
            int blueColor;

            foreach (var color in colors)
            {
                switch (color)
                {
                    case ColorType.RED:
                        redColor = 1;
                        continue;
                    case ColorType.GREEN:
                        greenColor = 1;
                        continue;
                    case ColorType.BLUE:
                        blueColor = 1;
                        continue;
                }
            }

            return ColorType.RED;
        }

        public static ColorType GetRGBColorType((int r, int g, int b)rgb)
        {
            return ColorType.RED;
        }
    }
}
