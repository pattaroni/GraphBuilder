namespace GraphBuilder.Functions
{
    // Базовий клас для всіх функцій
    public abstract class MathFunction
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
        public string Name { get; protected set; }

        public MathFunction(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
        }

        public abstract double Calculate(double x);

        public abstract string GetFormula();

        // Чи є між двома точками розрив функції (асимптота).
        // У більшості функцій розривів немає, тому за замовчуванням false
        public virtual bool HasBreakBetween(double x1, double x2)
        {
            return false;
        }

        // Доданок зі знаком: " + 3" або " − 3". Нульовий доданок не показуємо
        protected string Signed(double value)
        {
            if (value > 0)
            {
                return " + " + value;
            }
            if (value < 0)
            {
                return " − " + (-value);
            }
            return "";
        }
    }
}
