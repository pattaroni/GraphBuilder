using System;
using System.Globalization;

namespace GraphBuilder
{
    // Розбір чисел і перевірка введених даних
    public class InputHelper
    {
        // Більше точок будувати немає сенсу, програма почне гальмувати
        public const int MaxPoints = 50000;

        // Приймає число і з комою, і з крапкою
        public static bool TryParseNumber(string text, out double value)
        {
            value = 0;
            if (text == null)
            {
                return false;
            }

            string number = text.Trim().Replace(',', '.');
            bool correct = double.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
            return correct && double.IsFinite(value);
        }

        // Перевірка діапазонів. Повертає текст помилки або null, якщо все добре
        public static string CheckRange(double xMin, double xMax, double step, double yMin, double yMax)
        {
            if (xMin >= xMax)
            {
                return "Xmin має бути меншим за Xmax.";
            }
            if (yMin >= yMax)
            {
                return "Ymin має бути меншим за Ymax.";
            }
            if (step <= 0)
            {
                return "Крок має бути більшим за нуль.";
            }
            if (Math.Abs(xMin) > 1e9 || Math.Abs(xMax) > 1e9 || Math.Abs(yMin) > 1e9 || Math.Abs(yMax) > 1e9)
            {
                return "Межі діапазону занадто великі (більше 1000000000 за модулем).";
            }
            if ((xMax - xMin) / step > MaxPoints)
            {
                return "Занадто малий крок: забагато точок. Допустимо не більше " + MaxPoints + " точок.";
            }

            return null;
        }
    }
}
