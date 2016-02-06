using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace JapprendACompter.Stats
{
    public class Session
    {
        public Session()
        {
            Date = DateTime.Now;
            Questions = new List<Question>();
        }

        public Session(XElement sessionElement)
        {
            Date = XmlConvert.ToDateTime(sessionElement.Attribute("Date").Value, XmlDateTimeSerializationMode.Local);

            Questions = sessionElement.Elements("Questions").Elements("Question").Select(e => new Question(e)).ToList();
        }

        public DateTime Date { get; }
        public List<Question> Questions { get; }

        public XElement ToXml() => new XElement("Session",
                                       new XAttribute("Date", Date),
                                       new XElement("Questions", Questions.Select(q => q.ToXml())));

        public void AddQuestion(string text, bool correct, TimeSpan duration) => Questions.Add(new Question(text, correct, duration));
    }
}
