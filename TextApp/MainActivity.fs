namespace TextApp

open System
open Xamarin.Forms


open Android.App
open Android.Content
open Android.OS
open Android.Runtime
open Android.Views
open Android.Widget

[<Activity (Label = "TextApp", MainLauncher = true)>]
type MainActivity () =
    inherit Xamarin.Forms.Platform.Android.FormsApplicationActivity()

    override this.OnCreate (bundle) =
        base.OnCreate (bundle)

        Xamarin.Forms.Forms.Init(this, bundle)
        base.LoadApplication(new Library.ButtonTest.App())



