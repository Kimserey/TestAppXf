using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppAndroidCSharp
{
    [Activity(Theme = "@android:style/Theme.Material.Light.DarkActionBar", 
              Label = "AppAndroidCSharp", 
              Icon = "@drawable/icon", 
              MainLauncher = true, 
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Forms.Init(this, bundle);
            LoadApplication(new Library.ButtonTest.App());
        }
    }
}

