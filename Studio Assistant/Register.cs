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
using SQLite;
using System.IO;
using Android.Preferences;


namespace Studio_Assistant
{
    [Activity(Label = "Register", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class Register : Activity
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Register);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.RegToolBar);
            toolbar.NavigationClick += Toolbar_NavigationClick;
            FindViewById<Button>(Resource.Id.btn_regSubmit).Click += btn_Register_Clicked;
            FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_username).TextChanged += ValidateRegForm;
            FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_password).TextChanged += ValidateRegForm;
            FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_conf_password).TextChanged += ValidateRegForm;
            FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_email).TextChanged += ValidateRegForm;
            FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_firstName).TextChanged += ValidateRegForm;
            FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_lastName).TextChanged += ValidateRegForm;
        }

        private void btn_Register_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(Path.Combine(folder, "StudioAssistantData.db"));
            db.CreateTable<User>();
            User user = new User
            {
                Username = FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_username).Text,
                Password = FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_password).Text,
                Email = FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_email).Text,
                FirstName = FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_firstName).Text,
                LastName = FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_lastName).Text
            };
            db.Insert(user);
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutBoolean("userLoggedIn", true);
            editor.PutInt("userLoggedInID", user.ID);
            editor.Apply();
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        private void Toolbar_NavigationClick(object sender, Android.Support.V7.Widget.Toolbar.NavigationClickEventArgs e)
        {
            OnBackPressed();
            Finish();
        }

        private void ValidateRegForm(object sender, EventArgs e)
        {
            bool isValid = true;

            if (FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_username).Text == "")
            {
                isValid = false;
            }
            if (FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_password).Text == "")
            {
                isValid = false;
            }
            if (FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_conf_password).Text != FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_password).Text)
            {
                isValid = false;
            }
            if (FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_email).Text == "")
            {
                isValid = false;
            }
            else if (!Android.Util.Patterns.EmailAddress.Matcher(FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_email).Text).Matches())
            {
                isValid = false;
            }
            if (FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_firstName).Text == "" || FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_lastName).Text == "")
            {
                isValid = false;
            }
            FindViewById<Button>(Resource.Id.btn_regSubmit).Enabled = isValid;
        }
    }
}