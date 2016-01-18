using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace JapprendACompter
{
    public abstract class ExerciceBase
    {
        private static readonly TimeSpan Duree = TimeSpan.FromMinutes(5);

        private Dictionary<ResponseLevel, int> _responseCountByLevel;

        private int _wrongAnswerCount;

        private Stopwatch _chronoReponse;

        private Stopwatch _chronoTotal;

        public Operation Operation { get; private set; }

        public ExerciceBase()
        {
            Init();
        }

        private void Init()
        {
            _responseCountByLevel = new Dictionary<ResponseLevel, int>
            {
                [ResponseLevel.Fast] = 0,
                [ResponseLevel.Normal] = 0,
                [ResponseLevel.Slow] = 0,
                [ResponseLevel.TooSlow] = 0,
                [ResponseLevel.Wrong] = 0
            };

            _chronoReponse = null;
            _chronoTotal = null;

            Operation = GenerateOperation();
        }

        public ResponseLevel? CheckAnswer(string actualAnswer)
        {
            var expectedAnswer = Operation.ExpectedResult.ToString();

            if (actualAnswer == expectedAnswer)
            {
                return GetResponseLevel();
            }
            else if (actualAnswer.Length >= expectedAnswer.Length || !expectedAnswer.Contains(actualAnswer))
            {
                _wrongAnswerCount++;
            }

            if (_wrongAnswerCount >= 3)
            {
                return ResponseLevel.Wrong;
            }
            else
            {
                return null;
            }
        }

        private ResponseLevel GetResponseLevel()
        {
            if (_chronoReponse == null || _chronoReponse.Elapsed < TimeSpan.FromSeconds(5))
            {
                return ResponseLevel.Fast;
            }
            else if (_chronoReponse.Elapsed < TimeSpan.FromSeconds(10))
            {
                return ResponseLevel.Normal;
            }
            else if (_chronoReponse.Elapsed < TimeSpan.FromSeconds(15))
            {
                return ResponseLevel.Slow;
            }
            else
            {
                return ResponseLevel.TooSlow;
            }
        }

        public Message GetMessage(ResponseLevel responseLevel)
        {
            var expectedAnswer = Operation.ExpectedResult.ToString();
            switch (responseLevel)
            {
                case ResponseLevel.Fast:
                    return new Message("Bravo !", TimeSpan.FromSeconds(2));
                case ResponseLevel.Normal:
                    return new Message("C'est bien.", TimeSpan.FromSeconds(2));
                case ResponseLevel.Slow:
                    return new Message("Juste mais lent...", TimeSpan.FromSeconds(3));
                case ResponseLevel.TooSlow:
                    return new Message("Trop lent !\nLa bonne réponse était\n" + expectedAnswer, TimeSpan.FromSeconds(5));
                case ResponseLevel.Wrong:
                    return new Message("Perdu :-(\nLa bonne réponse était\n" + expectedAnswer, TimeSpan.FromSeconds(5));
                default:
                    throw new ArgumentException($"Unkown value for {nameof(responseLevel)} : {responseLevel}");
            }

        }

        public bool TryShowNextOperation(ResponseLevel previousResponse)
        {
            _responseCountByLevel[previousResponse] += 1;
            _chronoTotal = _chronoTotal ?? Stopwatch.StartNew();

            if (_chronoTotal.Elapsed > Duree)
            {
                return false;
            }
            else
            {
                _wrongAnswerCount = 0;
                _chronoReponse = Stopwatch.StartNew();
                Operation = GenerateOperation();
                return true;
            }
        }

        protected abstract Operation GenerateOperation();

        public Message GetEndMessage()
        {
            var total = _responseCountByLevel.Values.Sum();
            var totalRight = _responseCountByLevel.Where(r => r.Key.IsRight()).Sum(r => r.Value);

            var message = $"{totalRight} réponses justes sur {total}\n"
                        + $"Réponses rapides: {_responseCountByLevel[ResponseLevel.Fast]}\n"
                        + $"Réponses normales: {_responseCountByLevel[ResponseLevel.Normal]}\n"
                        + $"Réponses lentes: {_responseCountByLevel[ResponseLevel.Slow]}\n"
                        + $"Réponses trop lentes: {_responseCountByLevel[ResponseLevel.TooSlow]}\n"
                        + $"Réponses fausses: {_responseCountByLevel[ResponseLevel.Wrong]}\n";

            return new Message(message, TimeSpan.FromMinutes(1));
        }

        public class Message
        {
            public string Text { get; }

            public TimeSpan Duration { get; }

            public Message(string text, TimeSpan duration)
            {
                Text = text;
                Duration = duration;
            }
        }
    }
}
