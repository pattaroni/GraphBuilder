using System;

namespace GraphBuilder.Functions
{
    // y = a·e^(b·x) + c
    public class ExponentialFunction : MathFunction
    {
        public ExponentialFunction(double a, double b, double c) : base(a, b, c)
        {
            Name = "Показникова";
        }

        public override double Calculate(double x)
        {
            return A * Math.Exp(B * x) + C;
        }

        public override string GetFormula()
        {
            return "y = " + A + "·e^(" + B + "·x)" + Signed(C);
        }
    }
}
