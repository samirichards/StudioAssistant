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
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.CardView;
using Android.Database;
using Android.Preferences;
using Android.Animation;
using SQLite;
using System.IO;

namespace Studio_Assistant
{
    [Activity(Label = "Studio Assistant", Theme = "@style/AppThemeNoActionBar", MainLauncher = false, WindowSoftInputMode = SoftInput.StateHidden)]
    public class Login : Activity
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            if (prefs.GetBoolean("userLoggedIn", false))
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
            else
            {
                SetContentView(Resource.Layout.Login);
                FindViewById<Button>(Resource.Id.loginButton).Click += LoginButton_Clicked;
                FindViewById<Button>(Resource.Id.btn_register).Click += RegisterButton_Clicked;
            }
        }

        private void RegisterButton_Clicked(object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(Register)));
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            string userN = FindViewById<TextInputEditText>(Resource.Id.txt_login_username).Text;
            string userPassword = FindViewById<TextInputEditText>(Resource.Id.txt_login_password).Text;
            if (ValidateUser(userN, userPassword))
            {
                //Create shared preferences object
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
                ISharedPreferencesEditor editor = prefs.Edit();

                //Get the database from storage
                var db = new SQLiteConnection(Path.Combine(folder, "StudioAssistantData.db"));

                //Save logged in status
                editor.PutBoolean("userLoggedIn", true);
                editor.PutInt("userLoggedInID", db.Query<User>("SELECT * FROM User Where Username =? AND Password =?", userN, userPassword).First<User>().ID);
                editor.Apply();

                //Start main activity and end this one
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
            }
            else
            {
                //If the details fail to validate then inform the user
                Toast.MakeText(Application.Context.ApplicationContext, "Invalid username or password", ToastLength.Short).Show();
            }
        }
        //Intent intent = new Intent(this, typeof(MainActivity));
        //StartActivity(intent);
        private bool ValidateUser(string username, string password)
        {
            var db = new SQLiteConnection(Path.Combine(folder, "StudioAssistantData.db"));
            try
            {
                if (db.Query<User>("SELECT * FROM User Where Username =? AND Password =?", username, password).Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}