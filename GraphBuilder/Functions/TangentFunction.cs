using System;

namespace GraphBuilder.Functions
{
    // y = a·tg(b·x) + c
    public class TangentFunction : MathFunction
    {
        public TangentFunction(double a, double b, double c) : base(a, b, c)
        {
            Name = "Тангенс";
        }

        public override double Calculate(double x)
        {
            return A * Math.Tan(B * x) + C;
        }

        public override string GetFormula()
        {
            return "y = " + Num(A) + "·tg(" + Num(B) + "·x)" + Signed(C);
        }

        // Асимптоти там, де b·x = π/2 + π·k. Рахуємо номер k для обох точок:
        // якщо номери різні, то між точками є асимптота
        public override bool HasBreakBetween(double x1, double x2)
        {
            double k1 = Math.Floor((B * x1 - Math.PI / 2) / Math.PI);
            double k2 = Math.Floor((B * x2 - Math.PI / 2) / Math.PI);
            return k1 != k2;
        }
    }
}
