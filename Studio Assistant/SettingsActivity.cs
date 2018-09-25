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
using Android.Database;
using SQLite;
using System.IO;
using Android.Preferences;

namespace Studio_Assistant
{
    [Activity(Label = "SettingsActivity", WindowSoftInputMode = SoftInput.StateUnchanged)]
    public class SettingsActivity : Activity
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SettingsLayout);
            // Create your application here

            FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.SettingsToolBar).NavigationClick += SettingsToolbar_Back;
            FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.SettingsToolBar).InflateMenu(Resource.Menu.SettingsMenu);
            FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.SettingsToolBar).MenuItemClick += MenuItemClick;
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            var db = new SQLiteConnection(Path.Combine(folder, "StudioAssistantData.db"));
            User currentUser = db.Query<User>("SELECT * FROM User Where ID=?", prefs.GetInt("userLoggedInID", 0)).First();

            FindViewById<TextView>(Resource.Id.settingsName).Text = currentUser.FirstName + " " + currentUser.LastName;
            FindViewById<TextView>(Resource.Id.settingsUsername).Text = currentUser.Username;
            FindViewById<Button>(Resource.Id.btn_settings_changePassword).Click += Btn_settings_changPassword_Clicked;

            Android.Net.Uri iconURI = new Android.Net.Uri.Builder().EncodedPath(currentUser.ProfileIconURI).Build();
            if (iconURI.ToString() != "")
            {
                FindViewById<ImageView>(Resource.Id.accIconDisplay).SetImageURI(iconURI);
            }
            FindViewById<ImageView>(Resource.Id.accIconDisplay).Click += AccIconDisplay_Clicked;
        }

        private void AccIconDisplay_Clicked(object sender, EventArgs e)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Icon"), 1);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if ((requestCode == 1) && (resultCode == Result.Ok) && (data != null))
            {
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
                ISharedPreferencesEditor editor = prefs.Edit();
                var db = new SQLiteConnection(Path.Combine(folder, "StudioAssistantData.db"));
                User currentUser = db.Query<User>("SELECT * FROM User Where ID=?", prefs.GetInt("userLoggedInID", 0)).First();

                db.CreateCommand("UPDATE User SET ProfileIconURI=? WHERE ID=?", data.Data.ToString(), currentUser.ID).ExecuteNonQuery();
                Android.Net.Uri uri = data.Data;
                FindViewById<ImageView>(Resource.Id.accIconDisplay).SetImageURI(uri);
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

        private void Btn_settings_changPassword_Clicked(object sender, EventArgs e)
        {
            var oldPassword = FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_settings_oldPassword).Text;
            var newPassword = FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_settings_newPassword).Text;
            var newPasswordConf = FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_settings_newPasswordConf).Text;

            if (newPassword != "")
            {
                if (ChangePassword(oldPassword, newPassword, newPasswordConf))
                {
                    FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_settings_oldPassword).Text = "";
                    FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_settings_newPassword).Text = "";
                    FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.txt_settings_newPasswordConf).Text = "";

                    Toast.MakeText(ApplicationContext, "Password successfully changed", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(ApplicationContext, "New password field cannot be blank", ToastLength.Short).Show();
            }
        }

        private bool ChangePassword(string oldPassword, string newPassword, string confNewPassword)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            var db = new SQLiteConnection(Path.Combine(folder, "StudioAssistantData.db"));
            User currentUser = db.Query<User>("SELECT * FROM User Where ID=?", prefs.GetInt("userLoggedInID", 0)).First();

            if (StringMatch(oldPassword, currentUser.Password))
            {
                if (StringMatch(newPassword, confNewPassword))
                {
                    try
                    {
                        db.CreateCommand("UPDATE User SET Password=? WHERE ID=?", newPassword, currentUser.ID).ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception)
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetIcon(GetDrawable(Resource.Drawable.ic_alert));
                        builder.SetMessage("There was an error changing the password");
                        builder.SetTitle("Could not change password");
                        builder.SetPositiveButton("OK", (s, EventArgs) => { });
                        AlertDialog alert = builder.Create();
                        alert.Show();
                        return false;
                    }
                }
                else
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetIcon(GetDrawable(Resource.Drawable.ic_alert));
                    builder.SetMessage("New password did not match");
                    builder.SetTitle("Could not change password");
                    builder.SetPositiveButton("OK", (s, EventArgs) => { });
                    AlertDialog alert = builder.Create();
                    alert.Show();
                    return false;
                }
            }
            else
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetIcon(GetDrawable(Resource.Drawable.ic_alert));
                builder.SetMessage("Current password is incorrect");
                builder.SetTitle("Could not change password");
                builder.SetPositiveButton("OK", (s, EventArgs) => {});
                AlertDialog alert = builder.Create();
                alert.Show();
                return false;
            }
        }

        private bool StringMatch(string string1, string string2)
        {
            if (string1 == string2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void MenuItemClick(object sender, Android.Support.V7.Widget.Toolbar.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.settings_logout:
                    logoutPrompt();
                    break;
            }
        }

        private void ToolbarMenuItem_Clicked(object sender, Android.Support.V7.Widget.Toolbar.MenuItemClickEventArgs e)
        {
            if (e.Item.ItemId == Resource.Id.settings_logout)
            {

            }
        }

        private void SettingsToolbar_Back(object sender, EventArgs e)
        {
            OnBackPressed();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.SettingsMenu, menu);
            menu.GetItem(Resource.Id.settings_logout).SetShowAsAction(ShowAsAction.CollapseActionView);
            return base.OnCreateOptionsMenu(menu);
        }

        private void logoutPrompt()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetIcon(GetDrawable(Resource.Drawable.ic_alert));
            builder.SetMessage(Resource.String.logout_message);
            builder.SetTitle(Resource.String.logout_message_title);
            builder.SetPositiveButton("Yes", (s, EventArgs) =>
            {
                ISharedPreferencesEditor editor = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext).Edit();
                editor.Clear().Commit();
                StartActivity(new Intent(this, typeof(Login)).SetFlags(ActivityFlags.ClearTop).SetFlags(ActivityFlags.NewTask).SetFlags(ActivityFlags.ClearTask));
                Finish();
                Process.KillProcess(Process.MyPid());
            });
            builder.SetNegativeButton("No", (s, EventArgs) => { Toast.MakeText(ApplicationContext, "Lmao", ToastLength.Short).Show(); });
            AlertDialog alert = builder.Create();
            alert.Show();
        }
    }
}