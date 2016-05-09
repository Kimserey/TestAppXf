namespace Library

open Xamarin.Forms

module ButtonTest =
    
    let label =
        new Label()
    label.Text <- "Simple"

    let button = 
        new Button()
    button.Text <- "Do this"
    button.Clicked.AddHandler(fun _ _ -> label.Text <- "Clicked")

    let stackLayout =
        new StackLayout()
    stackLayout.Children.Add(label)
    stackLayout.Children.Add(button)

    let page =
        new ContentPage()
    page.Title <- "Some title"
    page.Content <- stackLayout