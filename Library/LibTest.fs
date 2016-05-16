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
    Content page - Table View
**)

let contentPage() =
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
    Content page - List view
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
            ItemsSource = (List.replicate 100 expenses |> List.concat),
            ItemTemplate = new DataTemplate(fun () -> viewCell() |> box))

    new ContentPage(
        Title = "List page", 
        Content = listview)

(** 
    Navigation page
    05-15 00:24:33.396  3201  3201 I MonoDroid: System.InvalidOperationException: NavigationPage must have a root Page before being used. Either call PushAsync with a valid Page, or pass a Page to the constructor before usage.
**)

let navigationPage() =
    let page = new NavigationPage(Title = "Navigation page")
    page.Icon <- FileImageSource.FromFile("hamburger") :?> FileImageSource

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
    Master detail page
**)

let masterDetailPage() =
    let menu =
        new ListView(ItemsSource = [ "Page 1"; "Page 2" ])
    
    let master =
        let layout = new StackLayout()
        layout.Children.Add(Label(Text = "Master content"))
        layout.Children.Add(menu)
        ContentPage(Title = "Master title", Content = layout)

    let label = new Label(Text = "Nothing selected")
    
    let detail =
        let d = new NavigationPage(new ContentPage(Content = label))
        NavigationPage.SetTitleIcon(d, FileImageSource.FromFile("hamburger.png") :?> FileImageSource)
        d

    detail.BindingContextChanged.AddHandler(fun _ _ ->
        label.Text <- string detail.BindingContext)

    let materDetailPage =
        new MasterDetailPage(
            Title = "Master detail page",
            Master = master,
            Detail = detail)

    menu.ItemSelected.AddHandler(fun _ args -> 
        detail.BindingContext <- args.SelectedItem
        materDetailPage.IsPresented <- false
        ())

    materDetailPage

(** 
    Carousel page
**)

let carouselPage() =
    let expenses =
        [ { Amount = 10.5m; Title = "Meat" }
          { Amount = 2.5m; Title = "Bread" }
          { Amount = 3.m; Title = "Butter" } ]

    let template() =
        let label (name: string) = 
            let l = new Label(HorizontalTextAlignment = TextAlignment.Center)
            l.SetBinding(Label.TextProperty, name)
            l
        let layout = new StackLayout(Padding = new Thickness(5.))
        [ label "Amount"; label "Title" ] |> List.iter layout.Children.Add
        new ContentPage(Content = layout)

    new CarouselPage(
        Title = "Carousel page", 
        ItemsSource = expenses, 
        ItemTemplate = new DataTemplate(fun () -> template() |> box))

(**
    List view with DataTemplateSelector    
    
    When scrolling ->

    05-16 10:26:25.417  1843  1843 I MonoDroid: UNHANDLED EXCEPTION:
    05-16 10:26:25.436  1843  1843 I MonoDroid: Java.Lang.ArrayIndexOutOfBoundsException: length=20; index=20
    05-16 10:26:25.436  1843  1843 I MonoDroid:   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw () <0x9d1bfc70 + 0x0002b> in <filename unknown>:0 
    05-16 10:26:25.436  1843  1843 I MonoDroid:   at Android.Runtime.JNIEnv.CallNonvirtualBooleanMethod (IntPtr jobject, IntPtr jclass, IntPtr jmethod, Android.Runtime.JValue* parms) <0x9c5ceed8 + 0x000b7> in <filename unknown>:0 
    05-16 10:26:25.436  1843  1843 I MonoDroid:   at Android.Views.View.DispatchTouchEvent (Android.Views.MotionEvent e) <0x9d1bc348 + 0x0014f> in <filename unknown>:0 
    05-16 10:26:25.436  1843  1843 I MonoDroid:   at Xamarin.Forms.Platform.Android.PlatformRenderer.DispatchTouchEvent (Android.Views.MotionEvent e) <0x9d1bb7b8 + 0x00133> in <filename unknown>:0 
    05-16 10:26:25.437  1843  1843 I MonoDroid:   at Android.Views.View.n_DispatchTouchEvent_Landroid_view_MotionEvent_ (IntPtr jnienv, IntPtr native__this, IntPtr native_e) <0x9d1bb6f8 + 0x00057> in <filename unknown>:0 
    05-16 10:26:25.437  1843  1843 I MonoDroid:   at (wrapper dynamic-method) System.Object:b6d4097d-ef8d-4b11-aa28-a7d5d0020a20 (intptr,intptr,intptr)
    05-16 10:26:25.437  1843  1843 I MonoDroid:   --- End of managed exception stack trace ---
**)

type ExpenseDataTemplate() =
    inherit DataTemplateSelector()
    
    let label (name: string) = 
        let l = new Label()
        l.SetBinding(Label.TextProperty, name)
        l

    let viewCell1() =
        let layout = new StackLayout(Padding = new Thickness(5.),
                                     Orientation = StackOrientation.Horizontal)
        [ new Label(Text = "Type 1"); label "Amount"; label "Title" ] |> List.iter layout.Children.Add
        new ViewCell(View = layout)

    let viewCell2() =
        let layout = new StackLayout(Padding = new Thickness(5.),
                                     Orientation = StackOrientation.Horizontal)
        [ new Label(Text = "Type 2"); label "Title"; label "Amount"; ] |> List.iter layout.Children.Add
        new ViewCell(View = layout)
    
    override x.OnSelectTemplate(item, container) =
        let exp = unbox<Expense> item
        if exp.Amount < 5.m then new DataTemplate(fun () -> box <| viewCell1())
        else new DataTemplate(fun () -> box <| viewCell2())

let listViewWithDataTemplateSelector() =
    let expenses =
        [ { Amount = 10.5m; Title = "Meat" }
          { Amount = 2.5m; Title = "Bread" }
          { Amount = 3.m; Title = "Butter" } ] |> List.replicate 10 |> List.concat

    let templateSelector = new ExpenseDataTemplate()
    let list = new ListView(ItemsSource = expenses, ItemTemplate = templateSelector)
    new ContentPage(Title = "List view with datatemplate selector", Content = list)

(** 
    Tabbed page
**)

let tabbedPage() = 
    let page = new TabbedPage(Title = "Tabbed page")
    [ contentPage() :> Page
      listPage() :> Page
      navigationPage() :> Page
      masterDetailPage() :> Page
      carouselPage() :> Page
      listViewWithDataTemplateSelector() :> Page ] 
    |> List.iter page.Children.Add
    page

type App() =
    inherit Application(MainPage = tabbedPage())