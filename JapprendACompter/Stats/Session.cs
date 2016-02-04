using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JapprendACompter.Stats
{
    public class Session
    {
        public Session(XElement sessionElement)
        {
            Questions = sessionElement.Elements("Questions").Elements("Question").Select(e => new Question(e)).ToList();
        }

        public List<Question> Questions { get; }
    }
}
