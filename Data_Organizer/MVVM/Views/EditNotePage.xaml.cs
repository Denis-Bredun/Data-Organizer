using Data_Organizer.MVVM.ViewModels;

namespace Data_Organizer.MVVM.Views;

public partial class EditNotePage : ContentPage
{
    private EditNotePageViewModel _editNotePageViewModel;

    public EditNotePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        if (_editNotePageViewModel == null)
            _editNotePageViewModel = (EditNotePageViewModel)BindingContext;

        _editNotePageViewModel.IsLoading = false;

        base.OnAppearing();
    }
}