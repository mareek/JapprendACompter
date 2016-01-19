﻿namespace JapprendACompter
{
    public class Operation
    {
        public int LeftOperand { get; }

        public int RightOperand { get; }

        public int ExpectedResult { get; }

        public string OperationSign { get; }

        public Operation(int leftOperand, int rightOperand, int expectedResult, string operationSign)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
            ExpectedResult = expectedResult;
            OperationSign = operationSign;
        }
    }
}