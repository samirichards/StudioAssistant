using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Provider;
using System.Net;
using Android.Net;

namespace Studio_Assistant
{
    public class NewsAdapter : RecyclerView.Adapter
    {
        public Article[] mArticle;
        private readonly Context context;
        public event EventHandler<NewsItemClickEventArgs> ItemClick;
        public NewsAdapter(Context context)
        {
            this.context = context;
        }

        public NewsAdapter(Article[] articles)
        {
            mArticle = articles;
        } 

        public override long GetItemId(int position)
        {
            return position;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            NewsAdapterViewHolder adapterViewHolder = holder as NewsAdapterViewHolder;
            adapterViewHolder.Title.Text = mArticle[position].Title;
            /*
            try
            {
                Android.Graphics.Bitmap image = GetBitmap(mArticle[position].HeaderImage);
                if (image.ByteCount > 10)
                {
                    adapterViewHolder.Image.SetImageBitmap(image);
                    adapterViewHolder.Image.SetScaleType(ImageView.ScaleType.CenterCrop);
                }
            }
            catch (Exception) { }
            */
            
        }

        public override int ItemCount
        {
            get { return mArticle.Count(); }
        }

        void OnClick(NewsItemClickEventArgs args) => ItemClick.Invoke(this, args);

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.NewsArticleItem, parent, false);
            
            NewsAdapterViewHolder adapterViewHolder = new NewsAdapterViewHolder(itemView, OnClick);
            return adapterViewHolder;
        }

        public Android.Graphics.Bitmap GetBitmap(string url)
        {
            WebClient client = new WebClient();
            var data = client.DownloadData(url);
            Android.Graphics.Bitmap bitmap = Android.Graphics.BitmapFactory.DecodeByteArray(data, 0, data.Length);
            return bitmap;
        }
    }

    class NewsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; set; }
        public TextView Title { get; set; }
        public View mMainView { get; set; }

        public NewsAdapterViewHolder (View itemView, Action<NewsItemClickEventArgs> clickListener) : base(itemView)
        {
            Image = itemView.FindViewById<ImageView>(Resource.Id.img_imageBackground);
            Title = itemView.FindViewById<TextView>(Resource.Id.txt_newsTitle);
            mMainView = itemView;
            mMainView.Click += (sender, e) => clickListener(new NewsItemClickEventArgs { view = mMainView, position = AdapterPosition });
            Image.Click += (sender, e) => clickListener(new NewsItemClickEventArgs { view = mMainView, position = AdapterPosition });
            itemView.Click += (sender, e) => clickListener(new NewsItemClickEventArgs { view = mMainView, position = AdapterPosition });
        }
    }
    public class NewsItemClickEventArgs : EventArgs
    {
        public View view { get; set; }
        public int position { get; set; }
    }
}