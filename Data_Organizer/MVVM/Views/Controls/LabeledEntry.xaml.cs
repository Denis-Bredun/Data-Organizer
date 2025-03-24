using System.Windows.Input;

namespace Data_Organizer.MVVM.Views.Controls;

public partial class LabeledEntry : ContentView
{
    public static readonly BindableProperty LabelTextProperty =
        BindableProperty.Create(nameof(LabelText), typeof(string), typeof(LabeledEntry), string.Empty);

    public static readonly BindableProperty EntryTextProperty =
        BindableProperty.Create(nameof(EntryText), typeof(string), typeof(LabeledEntry), string.Empty);

    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(LabeledEntry), false);

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public string EntryText
    {
        get => (string)GetValue(EntryTextProperty);
        set => SetValue(EntryTextProperty, value);
    }

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    public LabeledEntry()
    {
        InitializeComponent();
    }
} 