module Library.LibTest

open System
open Xamarin.Forms

let entryRow txtEntry numericEntry = 
    let grid = new Grid()
    grid.RowDefinitions.Add(new RowDefinition())
    grid.ColumnDefinitions.Add(new ColumnDefinition(Width = GridLength(3., GridUnitType.Star)))
    grid.ColumnDefinitions.Add(new ColumnDefinition(Width = GridLength(1., GridUnitType.Star)))
    grid.Children.Add(txtEntry, 0, 0)
    grid.Children.Add(numericEntry, 1, 0)
    grid

let button handler = 
    let btn = new Button(Text = "Add")
    btn.Clicked.AddHandler handler
    btn

let entry placeholder keyboardType = 
    new Entry(
        Placeholder = placeholder,
        Keyboard = keyboardType)

let entryNumeric() = 
    entry "Price" Keyboard.Numeric

let entryText() = 
    entry "Text" Keyboard.Text

let stackLayout() =
    let mainGrid = new Grid()
    let layout = new StackLayout()
    let entry = entryText()
    let price = entryNumeric()
    let handler =
        new EventHandler (fun _ _ -> 
            let rowIndex = mainGrid.Children.Count / 2
            mainGrid.Children.Add(new Label(Text = entry.Text), 1, rowIndex)
            mainGrid.Children.Add(new Label(Text = price.Text), 2, rowIndex))

    layout.Children.Add(entryRow entry price)
    layout.Children.Add(button handler)
    layout.Children.Add(mainGrid)
    layout

let page title = 
    new ContentPage(Title = title, Content = stackLayout())

let tabbedPage = 
    new TabbedPage(Title = "Tabbed page")
tabbedPage.Children.Add(page "Page 1")
tabbedPage.Children.Add(page "Page 2")
tabbedPage.Children.Add(page "Page 3")
tabbedPage.Children.Add(page "Page 4")

//UNHANDLED EXCEPTION:
//05-14 11:40:33.556  2306  2306 I MonoDroid: System.InvalidOperationException: Master and Detail must be set before adding MasterDetailPage to a container
let masterDetailPage =
    new MasterDetailPage(
        Title = "Master detail page", 
        Master = ContentPage(Title = "Master", Content = Label(Text = "Master content")),
        Detail = ContentPage(Title = "detail", Content = Label(Text = "Detail content")))

type App() =
    inherit Application(MainPage = masterDetailPage)