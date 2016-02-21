namespace JapprendACompter
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

        public string GetQuestionLabel() => $"{LeftOperand} {OperationSign} {RightOperand} = ?";

        public string GetQuestionAndAnswerLabel() => $"{LeftOperand} {OperationSign} {RightOperand} = {ExpectedResult}";
    }
}
