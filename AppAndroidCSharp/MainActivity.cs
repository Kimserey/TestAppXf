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

/*
 *  Wondering why a csproj vs a fsproj?
 *  I added a csproj because at the moment the fsproj template is messed up.
 *  It does not allow me to change the target version of Android which messes up the deployment to the VM.
 *  Also it keeps showing errors on the auto-generated Resource.Designer saying 'end' is a reserved keyword.
 */
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

