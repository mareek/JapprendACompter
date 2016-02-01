using System;

namespace JapprendACompter
{
    public class Multiplication : ExerciceBase
    {
        protected override double TimeFactor => 1.5;

        protected override Operation GenerateOperation()
        {
            var rnd = new Random();
            var leftOperand = rnd.Next(Config.MultiplicationMax) + 1;
            var rightOperand = rnd.Next(9) + 1;

            return new Operation(leftOperand, rightOperand, leftOperand * rightOperand, "x");
        }
    }
}
