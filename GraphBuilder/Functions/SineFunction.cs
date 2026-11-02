using System;

namespace GraphBuilder.Functions
{
    // y = a·sin(b·x) + c
    public class SineFunction : MathFunction
    {
        public SineFunction(double a, double b, double c) : base(a, b, c)
        {
            Name = "Синус";
        }

        public override double Calculate(double x)
        {
            return A * Math.Sin(B * x) + C;
        }

        public override string GetFormula()
        {
            return "y = " + Num(A) + "·sin(" + Num(B) + "·x)" + Signed(C);
        }
    }
}
