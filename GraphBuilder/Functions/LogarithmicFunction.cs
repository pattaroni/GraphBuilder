using System;

namespace GraphBuilder.Functions
{
    // y = a·ln(b·x) + c
    public class LogarithmicFunction : MathFunction
    {
        public LogarithmicFunction(double a, double b, double c) : base(a, b, c)
        {
            Name = "Логарифмічна";
        }

        // Для b·x <= 0 Math.Log поверне NaN або -Infinity, такі точки не малюються
        public override double Calculate(double x)
        {
            return A * Math.Log(B * x) + C;
        }

        public override string GetFormula()
        {
            return "y = " + Num(A) + "·ln(" + Num(B) + "·x)" + Signed(C);
        }
    }
}
