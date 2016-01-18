using System;

namespace JapprendACompter
{
    public class ExerciceAddition : ExerciceBase
    {
        protected override Operation GenerateOperation()
        {
            var rnd = new Random();
            var leftOperand = rnd.Next(9) + 1;
            var rightOperand = rnd.Next(9) + 1;

            return new Operation(leftOperand, rightOperand, leftOperand + rightOperand, "+");
        }
    }
}
