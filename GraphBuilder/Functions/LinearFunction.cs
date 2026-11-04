namespace GraphBuilder.Functions
{
    // y = a·x + b
    public class LinearFunction : MathFunction
    {
        public LinearFunction(double a, double b) : base(a, b, 0)
        {
            Name = "Лінійна";
        }

        public override double Calculate(double x)
        {
            return A * x + B;
        }

        public override string GetFormula()
        {
            return "y = " + A + "·x" + Signed(B);
        }
    }
}
