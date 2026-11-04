namespace GraphBuilder.Functions
{
    // y = a / (x − b) + c
    public class HyperbolaFunction : MathFunction
    {
        public HyperbolaFunction(double a, double b, double c) : base(a, b, c)
        {
            Name = "Гіпербола";
        }

        public override double Calculate(double x)
        {
            return A / (x - B) + C;
        }

        public override string GetFormula()
        {
            return "y = " + A + " / (x" + Signed(-B) + ")" + Signed(C);
        }

        // Асимптота у точці x = b
        public override bool HasBreakBetween(double x1, double x2)
        {
            return x1 < B && x2 > B;
        }
    }
}
