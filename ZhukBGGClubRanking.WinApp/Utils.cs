using System;
using System.Collections.Generic;
using System.Drawing;

namespace ZhukBGGClubRanking.WinApp
{
    public static class Utils
    {
        public static List<Color> GetGradientColors(Color start, Color end, int steps)
        {
            return GetGradientColors(start, end, steps, 0, steps - 1);
        }

        public static List<Color> GetGradientColors(Color start, Color end, int steps, int firstStep, int lastStep)
        {
            var colorList = new List<Color>();
            if (steps <= 0 || firstStep < 0 || lastStep > steps - 1)
                return colorList;

            double aStep = (end.A - start.A) / (double)steps;
            double rStep = (end.R - start.R) / (double)steps;
            double gStep = (end.G - start.G) / (double)steps;
            double bStep = (end.B - start.B) / (double)steps;

            for (int i = firstStep + 1; i <= lastStep; i++)
            {
                var a = start.A + (aStep * i);
                var r = start.R + (rStep * i);
                var g = start.G + (gStep * i);
                var b = start.B + (bStep * i);
                colorList.Add(Color.FromArgb((int)a, (int)r, (int)g, (int)b));
            }

            return colorList;
        }

        public static double GetCompliancePercent(double rating1, double rating2, double maxRatingSize)
        {
            var diff = Math.Abs(rating1 - rating2);
            var coeff = 5;
            var res = (maxRatingSize - diff * coeff) / maxRatingSize * 100;
            if (res < 0) return 0;
            if (res > 100) return 100;
            return res;
        }
    }
}
