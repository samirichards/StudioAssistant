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

namespace Studio_Assistant
{
    [Activity(Label = "Timetables")]
    public class Timetables : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Timetables);


            FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.Timetable_Toolbar).NavigationClick += (s, e) => { OnBackPressed(); };
            // Create your application here
        }
    }
}