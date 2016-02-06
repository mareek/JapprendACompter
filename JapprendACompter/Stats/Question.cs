using System;
using System.Xml;
using System.Xml.Linq;

namespace JapprendACompter.Stats
{
    public class Question
    {
        public Question(string text, bool correct, TimeSpan duration)
        {
            Text = text;
            Correct = correct;
            Duration = duration;
        }

        public Question(XElement questionElement)
        {
            Text = questionElement.Value;
            Correct = XmlConvert.ToBoolean(questionElement.Attribute("Correct").Value);
            Duration = XmlConvert.ToTimeSpan(questionElement.Attribute("Duration").Value);
        }

        public string Text { get; }
        public bool Correct { get; }
        public TimeSpan Duration { get; }

        public XElement ToXml() => new XElement("Question",
                                       new XAttribute("Correct", Correct),
                                       new XAttribute("Duration", Duration),
                                       Text);
    }
}
