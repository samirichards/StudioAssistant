using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Android.Support.V7.Widget;

namespace Studio_Assistant
{
    public class Frag_news : Fragment
    {
        public RecyclerView recycler;
        public RecyclerView.LayoutManager layoutManager;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Log.Verbose("Frag_news", "Frag_news OnCreate Method ran succesfuly");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            //base.OnCreateView(inflater, container, savedInstanceState);

            return inflater.Inflate(Resource.Layout.layout_frag_news, container, false);
        }

        public override void OnStart()
        {
            base.OnStart();
            Article[] articles = GetArticles();
            recycler = View.FindViewById<RecyclerView>(Resource.Id.rcl_NewsRecycler);
            NewsAdapter adapter = new NewsAdapter(articles);
            adapter.ItemClick += NewsItem_Clicked;
            layoutManager = new LinearLayoutManager(this.Context);
            recycler.SetAdapter(adapter);
            recycler.SetLayoutManager(layoutManager);
            recycler.Clickable = true;
        }

        private void NewsItem_Clicked(object sender, NewsItemClickEventArgs e)
        {
            var view = e.view;
            Log.Verbose("Recyclerview click","Click event registered");
        }

        public Article[] GetArticles()
        {
            //Get the news file
            XDocument file = XDocument.Load(Context.Assets.Open("News.xml"));

            //Generate an array of the values of each type of element
            string[] articleTitles = file.Descendants("title").Select(element => element.Value).ToArray();
            Log.Verbose("GetArticles", "Gotten all titles from file");
            string[] articleImageUrls = file.Descendants("headerImage").Select(element => element.Value).ToArray();
            Log.Verbose("GetArticles", "Gotten all header images from file");
            string[] articleContents = file.Descendants("content").Select(e => e.Value).ToArray();
            Log.Verbose("GetArticles", "Gotten all content from file");
            int articlesFound = file.Descendants("article").Count();
            Log.Verbose("GetArticles", "Found " + articlesFound.ToString() + " articles");

            //Create an array of articles with the length of the number of articles
            Article[] articles = new Article[articlesFound];
            for (int i = 0; i < articlesFound; i++)
            {
                //Create article objects with the appropriate values
                Article article = new Article
                {
                    Title = articleTitles[i],
                    HeaderImage = articleImageUrls[i],
                    Content = articleContents[i]

                };
                Log.Verbose("GetArticles", "Sucessfully created article object");
                articles[i] = article;
            }
            //return the array of articles
            return articles;
        }
    }
}