using System;

namespace GraphBuilder.Functions
{
    // y = a·√(b·x) + c
    public class RootFunction : MathFunction
    {
        public RootFunction(double a, double b, double c) : base(a, b, c)
        {
            Name = "Корінь";
        }

        // Для b·x < 0 Math.Sqrt поверне NaN
        public override double Calculate(double x)
        {
            return A * Math.Sqrt(B * x) + C;
        }

        public override string GetFormula()
        {
            return "y = " + A + "·√(" + B + "·x)" + Signed(C);
        }
    }
}
