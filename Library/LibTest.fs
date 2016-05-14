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
    layout.Children.Add(new ScrollView (Content = mainGrid))
    layout

let page title = 
    new ContentPage(Title = title, Content = stackLayout())

(** 
    Table view sample
**)

let tablePage() =
    let tableRoot = new TableRoot("Table root")
    tableRoot.Add [
        let x = new TableSection("Section")
        x.Add(new TextCell(Text = "TextCell text", Detail = "TextCell detail"))
        x.Add(new EntryCell(Label = "EntryCell", Placeholder = "entry text", Keyboard = Keyboard.Default))
        yield x
        let y = new TableSection("Section")
        y.Add(new SwitchCell(Text = "Switch"))
        y.Add(new EntryCell(Label = "Phone", Placeholder = "entry phone", Keyboard = Keyboard.Telephone))
        yield y
    ]
    new ContentPage(
        Title = "Table page", 
        Content = new TableView(Intent = TableIntent.Form, Root = tableRoot))

(**
    List view sample
**)

type Expense = { Amount: decimal; Title: string }

let listPage() =
    let expenses =
        [ { Amount = 10.5m; Title = "Meat" }
          { Amount = 2.5m; Title = "Bread" }
          { Amount = 3.m; Title = "Butter" } ]    

    let viewCell() =
        let label (name: string) = 
            let l = new Label()
            l.SetBinding(Label.TextProperty, name)
            l
    
        let layout = new StackLayout(Padding = new Thickness(5.),
                                     Orientation = StackOrientation.Horizontal)
        [ label "Amount"; label "Title" ] |> List.iter layout.Children.Add
        new ViewCell(View = layout)

    let listview = 
        new ListView(
            ItemsSource = expenses,
            ItemTemplate = new DataTemplate(fun () -> viewCell() |> box))

    new ContentPage(
        Title = "List page", 
        Content = listview)

(** 
    Navigation page
    05-15 00:24:33.391  3201  3201 I MonoDroid: UNHANDLED EXCEPTION:
    05-15 00:24:33.396  3201  3201 I MonoDroid: System.InvalidOperationException: NavigationPage must have a root Page before being used. Either call PushAsync with a valid Page, or pass a Page to the constructor before usage.
**)

let navigationPage() =
    let page = new NavigationPage(Title = "Navigation page")
    let navpage1 = new ContentPage(Title =  "Nav page 1")
    let navpage2 = new ContentPage(Title =  "Nav page 2")
    let navpage3 = new ContentPage(Title =  "Nav page 3", Content = new Label (Text = "Some label 3"))
    
    let btn1 = new Button(Text = "Go 2")
    btn1.Clicked.AddHandler(fun _ _ -> page.PushAsync navpage2 |> ignore)
    navpage1.Content <- btn1

    let btn2 = new Button(Text = "Go 3")
    btn2.Clicked.AddHandler(fun _ _ -> page.PushAsync navpage3 |> ignore)
    navpage2.Content <- btn2

    page.PushAsync navpage1 |> ignore
    page
    

(** 
    Tabbed page
**)

let tabbedPage() = 
    let page = new TabbedPage(Title = "Tabbed page")
    [ tablePage() :> Page; listPage() :> Page; navigationPage() :> Page ] |> List.iter page.Children.Add
    page



let menu() =
    new ListView(ItemsSource = [ "Page 1"; "Page 2" ])

let masterDetailPage() =
    let menu = menu()

    let master =
        let layout = new StackLayout()
        layout.Children.Add(Label(Text = "Master content"))
        layout.Children.Add(menu)
        ContentPage(Title = "Master title", Content = layout)

    let detail =
        new NavigationPage(page "super page")
    
    menu.ItemSelected.AddHandler(fun sender args -> 
        detail.BindingContext <- args.SelectedItem
        ())

    new MasterDetailPage(
        Title = "Master detail page", 
        Master = master,
        Detail = detail)

type App() =
    inherit Application(MainPage = tabbedPage())