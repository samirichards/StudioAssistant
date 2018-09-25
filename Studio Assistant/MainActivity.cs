using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Support.V7.CardView;
using Android.Support.V4.Content;
using Android.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.Runtime;
using Android.Preferences;
using Android.Animation;
using SQLite;
using System.IO;

namespace Studio_Assistant
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppThemeNoActionBar")]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                //If item.ItemId equals this then change the subtitle and call SwitchToFrag()
                case Resource.Id.navigation_news:
                    SwitchToFrag(new Frag_news());
                    FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.MainToolBar).Subtitle = "News";
                    return true;
                case Resource.Id.navigation_comingup:
                    SwitchToFrag(new Frag_comingup());
                    FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.MainToolBar).Subtitle = "Coming Up";
                    return true;
                case Resource.Id.navigation_termdates:
                    SwitchToFrag(new Frag_termdates());
                    FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.MainToolBar).Subtitle = "Termdates";
                    return true;
            }
            return false;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Setup so the database can be accessed
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            var db = new SQLiteConnection(Path.Combine(folder, "StudioAssistantData.db"));

            //Get the currently logged on user and then set the content view
            User currentUser = db.Query<User>("SELECT * FROM User Where ID=?", prefs.GetInt("userLoggedInID", 0)).First();
            SetContentView(Resource.Layout.activity_main);

            //Set the listener for the Bottom Navigation View
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            //Change display width based on device width
            if (Resources.DisplayMetrics.Xdpi > 260)
            {
                FindViewById<FrameLayout>(Resource.Id.frag_mainContainer).LayoutParameters.Width = 640 * (int)Resources.DisplayMetrics.Density;
            }

            //Set the toolbar as the default actionbar for this activity
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.MainToolBar);
            SetSupportActionBar(toolbar);

            //Create event handlers for the various triggers 
            toolbar.NavigationClick += Toolbar_NavigationClick;
            FindViewById<NavigationView>(Resource.Id.NavView).NavigationItemSelected += NavView_NavigationItemSelected;
            FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawer).DrawerOpened += MainActivity_DrawerOpened;

            //Switch to news by default on app launch
            SwitchToFrag(new Frag_news());
        }

        private void MainActivity_DrawerOpened(object sender, Android.Support.V4.Widget.DrawerLayout.DrawerOpenedEventArgs e)
        {
            Android.Support.V4.Widget.DrawerLayout drawer = sender as Android.Support.V4.Widget.DrawerLayout;
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            var db = new SQLiteConnection(Path.Combine(folder, "StudioAssistantData.db"));

            User currentUser = db.Query<User>("SELECT * FROM User Where ID=?", prefs.GetInt("userLoggedInID", 0)).First();
            drawer.FindViewById<TextView>(Resource.Id.navDrawer_txtUsername).Text = currentUser.Username;
        }

        //Event handler for the Navigation Drawer 
        private void NavView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            //Switch case to perform different actions based on what called it
            switch (e.MenuItem.ItemId)
            {
                //Start the settings activity n the event that the settings button called this
                case Resource.Id.nav_settings:
                    StartActivity(new Android.Content.Intent(this, typeof(SettingsActivity)));
                    break;
                //Close the nav drawer in the event home is clicked
                //This is because the user must be home
                case Resource.Id.nav_home:
                    FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawer).CloseDrawer(Resource.Id.drawer);
                    break;
                //Do nothing as of yet when the timetable button is clicked
                case Resource.Id.nav_timetable:
                    StartActivity(new Intent(this, typeof(Timetables)));
                    break;
                //Do nothing as of yet when the about button is selected
                case Resource.Id.nav_about:
                    Toast.MakeText(Application, "Will be implemented soon", ToastLength.Short).Show();
                    break;
            }
        }

        private void Toolbar_NavigationClick(object sender, Android.Support.V7.Widget.Toolbar.NavigationClickEventArgs e)
        {
            var drawer = FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawer);
            drawer.OpenDrawer(Resource.Id.drawer);
        }

        protected void SwitchToFrag(Fragment frag)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.frag_mainContainer, frag).SetTransition(FragmentTransit.FragmentFade).Commit();
        }
    }
}