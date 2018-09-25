using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;
using System.Xml;
using Android.Util;
using System.Xml.Linq;
using System.Net;
using System.IO;
using Android.Support.V7.Widget;

namespace Studio_Assistant
{
    /// <summary>
    /// This activity is what will be used to determine if the user is has previously logged in or not
    /// If not then the user is directed to the login and register pages. If they have then MainActivity is launched.
    /// </summary>
    [Activity(Label = "Studio Assistant", MainLauncher = true)]
    public class Launcher : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (false)
            {

            }
            else
            {
                StartActivity(new Intent (this, typeof(Login)));
                Finish();
            }
        }

        public Article[] GetArticles()
        {
            XDocument file = XDocument.Load(Assets.Open("News.xml"));

            string[] articleTitles = file.Descendants("title").Select(element => element.Value).ToArray();
            string[] articleImageUrls = file.Descendants("headerImage").Select(element => element.Value).ToArray();
            string[] articleContents = file.Descendants("content").Select(e => e.Value).ToArray();
            int articlesFound = file.Descendants("article").Count();

            Article[] articles = new Article[articlesFound];
            for (int i = 0; i < articlesFound; i++)
            {
                Article article = new Article();
                article.Title = articleTitles[i];
                article.HeaderImage = articleImageUrls[i];
                article.Content = articleContents[i];

                articles[i] = article;
            }
            return articles;
        }
    }
}