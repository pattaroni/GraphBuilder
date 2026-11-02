using System;
using System.Globalization;

namespace GraphBuilder
{
    // Розбір чисел і перевірка введених даних
    public class InputHelper
    {
        public const double MaxValue = 1e9;
        public const double MinRange = 1e-6;
        public const int MaxPoints = 50000;

        // Приймає число і з комою, і з крапкою
        public static bool TryParseNumber(string text, out double value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            string number = text.Trim().Replace(',', '.');
            if (!double.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                return false;
            }

            return double.IsFinite(value);
        }

        // Повертає текст помилки або null, якщо все добре
        public static string CheckNumber(string text, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "Поле «" + fieldName + "» порожнє. Введіть число.";
            }

            double value;
            if (!TryParseNumber(text, out value))
            {
                return "У полі «" + fieldName + "» має бути число.";
            }

            return null;
        }

        // Перевірка діапазонів. Повертає текст помилки або null
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
            if (Math.Abs(xMin) > MaxValue || Math.Abs(xMax) > MaxValue
                || Math.Abs(yMin) > MaxValue || Math.Abs(yMax) > MaxValue)
            {
                return "Межі діапазону занадто великі. Допустимо не більше 1000000000 за модулем.";
            }
            if (xMax - xMin < MinRange || yMax - yMin < MinRange)
            {
                return "Діапазон занадто малий. Найменша ширина діапазону 0.000001.";
            }
            if (step > xMax - xMin)
            {
                return "Крок не може бути більшим за діапазон по X.";
            }
            if ((xMax - xMin) / step > MaxPoints)
            {
                return "Занадто малий крок: забагато точок. Допустимо не більше " + MaxPoints + " точок.";
            }

            return null;
        }

        // Запис числа для таблиці, файлу і рядка стану
        public static string FormatNumber(double value)
        {
            if (!double.IsFinite(value))
            {
                return "не визначено";
            }

            double rounded = Math.Round(value, 6);
            if (rounded == 0)
            {
                return "0";
            }

            return rounded.ToString("G10", CultureInfo.InvariantCulture);
        }
    }
}
