namespace GraphBuilder.Functions
{
    // y = a·x² + b·x + c
    public class QuadraticFunction : MathFunction
    {
        public QuadraticFunction(double a, double b, double c) : base(a, b, c)
        {
            Name = "Квадратична";
        }

        public override double Calculate(double x)
        {
            return A * x * x + B * x + C;
        }

        public override string GetFormula()
        {
            string bPart = "";
            if (B > 0)
            {
                bPart = " + " + Num(B) + "·x";
            }
            else if (B < 0)
            {
                bPart = " − " + Num(-B) + "·x";
            }

            return "y = " + Num(A) + "·x²" + bPart + Signed(C);
        }
    }
}
