using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace JapprendACompter.Stats
{
    public class Session
    {
        public Session(string exercice)
        {
            Exercice = exercice;
            Date = DateTime.Now;
            Questions = new List<Question>();
        }

        public Session(XElement sessionElement)
        {
            Exercice = sessionElement.Attribute("Exercice")?.Value ?? "";
            Date = XmlConvert.ToDateTime(sessionElement.Attribute("Date").Value, XmlDateTimeSerializationMode.Local);
            Questions = sessionElement.Elements("Questions").Elements("Question").Select(e => new Question(e)).ToList();
        }

        public string Exercice { get; }
        public DateTime Date { get; }
        public List<Question> Questions { get; }

        public XElement ToXml() => new XElement("Session",
                                       new XAttribute("Exercice", Exercice),
                                       new XAttribute("Date", Date),
                                       new XElement("Questions", Questions.Select(q => q.ToXml())));

        public void AddQuestion(string text, bool correct, TimeSpan duration) => Questions.Add(new Question(text, correct, duration));
    }
}
