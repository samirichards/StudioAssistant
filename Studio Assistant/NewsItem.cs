using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Xml.Serialization;

namespace Studio_Assistant
{
    [XmlRoot(ElementName = "article")]
    public class Article
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        [XmlAttribute(AttributeName = "title")]
        public string _Title { get; set; }
        [XmlElement(ElementName = "headerImage")]
        public string HeaderImage { get; set; }
        [XmlElement(ElementName = "content")]
        public string Content { get; set; }
        [XmlAttribute(AttributeName = "date")]
        public string Date { get; set; }
    }

    [XmlRoot(ElementName = "news")]
    public class News
    {
        [XmlElement(ElementName = "article")]
        public List<Article> Article { get; set; }
    }

}