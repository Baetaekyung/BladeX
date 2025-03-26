using System.Collections.Generic;

namespace Swift_Blade
{
    public static class ColorUtils
    {
        public ColorType GetColor(List<ColorType> colors)
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

            return 
        }

        public ColorType GetRGBColorType(int r, int g, int b)
        {

        }
    }
}
