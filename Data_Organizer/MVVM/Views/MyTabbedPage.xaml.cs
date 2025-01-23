namespace Data_Organizer.MVVM.Views;

public partial class MyTabbedPage : TabbedPage
{
    public MyTabbedPage(MainPage mainPage, SavedNotesPage savedNotesPage, SettingsPage settingsPage)
    {
        InitializeComponent();

        Children.Add(mainPage);
        Children.Add(savedNotesPage);
        Children.Add(settingsPage);
    }
}