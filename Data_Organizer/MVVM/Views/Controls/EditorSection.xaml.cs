namespace Data_Organizer.MVVM.Views.Controls
{
    public partial class EditorSection : ContentView
    {
        public EditorSection()
        {
            InitializeComponent();
        }

        private async void OnEditorTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.OldTextValue != e.NewTextValue)
                await MyScrollView.ScrollToAsync(MyEditor, ScrollToPosition.End, true);
        }
    }
}