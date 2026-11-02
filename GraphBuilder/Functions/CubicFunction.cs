namespace GraphBuilder.Functions
{
    // y = a·x³ + b·x² + c
    public class CubicFunction : MathFunction
    {
        public CubicFunction(double a, double b, double c) : base(a, b, c)
        {
            Name = "Кубічна";
        }

        public override double Calculate(double x)
        {
            return A * x * x * x + B * x * x + C;
        }

        public override string GetFormula()
        {
            string bPart = "";
            if (B > 0)
            {
                bPart = " + " + Num(B) + "·x²";
            }
            else if (B < 0)
            {
                bPart = " − " + Num(-B) + "·x²";
            }

            return "y = " + Num(A) + "·x³" + bPart + Signed(C);
        }
    }
}
