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
            string formula = "y = " + A + "·x²";
            if (B != 0)
            {
                formula += Signed(B) + "·x";
            }
            return formula + Signed(C);
        }
    }
}
