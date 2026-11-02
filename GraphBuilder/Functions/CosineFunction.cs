using System;

namespace GraphBuilder.Functions
{
    // y = a·cos(b·x) + c
    public class CosineFunction : MathFunction
    {
        public CosineFunction(double a, double b, double c) : base(a, b, c)
        {
            Name = "Косинус";
        }

        public override double Calculate(double x)
        {
            return A * Math.Cos(B * x) + C;
        }

        public override string GetFormula()
        {
            return "y = " + Num(A) + "·cos(" + Num(B) + "·x)" + Signed(C);
        }
    }
}
