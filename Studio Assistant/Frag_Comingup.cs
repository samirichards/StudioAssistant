using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Studio_Assistant
{
    [Activity(Label = "Coming Up")]
    public class Frag_comingup : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Log.Verbose("Frag_Comingup", "Frag_Comingup OnCreate Method ran succesfuly");
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.layout_frag_comingup, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}