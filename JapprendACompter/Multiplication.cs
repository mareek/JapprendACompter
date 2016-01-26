using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapprendACompter
{
    public class Multiplication : ExerciceBase
    {
        protected override Operation GenerateOperation()
        {
            var rnd = new Random();
            var leftOperand = rnd.Next(4) + 1;
            var rightOperand = rnd.Next(9) + 1;

            return new Operation(leftOperand, rightOperand, leftOperand * rightOperand, "x");
        }
    }
}
