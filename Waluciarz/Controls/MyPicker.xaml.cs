using System.Windows.Input;
using Waluciarz.MVVM.Models;

namespace Waluciarz.Controls;

public partial class MyPicker : ContentView
{
    public MyPicker()
    {
        InitializeComponent();

        Loaded += (s, e) =>
        {
            searchEntry.Text = SelectedItem?.Symbol;
            scrollView.IsVisible = false;
        };
    }

    private void FilterList(string text)
    {
        if (text is not null)
            list.ItemsSource = ItemsSource
                .Where(x => x.FullName.Contains(text, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
    }

    private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e?.SelectedItem != null)
        {
            var currency = (Currency)e.SelectedItem;
            searchEntry.Text = currency.Symbol;
            scrollView.IsVisible = false;
            SelectedItem = currency;

            ItemChangedCommand.Execute(null);
        }
    }

    private void Entry_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (e?.NewTextValue is not null) FilterList(e.NewTextValue);
    }

    private void SearchEntry_OnFocused(object sender, FocusEventArgs e)
    {
        scrollView.IsVisible = true;
    }

    private void SearchEntry_OnUnfocused(object sender, FocusEventArgs e)
    {
        scrollView.IsVisible = false;
        searchEntry.Text = SelectedItem?.Symbol;
    }

    #region Bindable Properties

    public List<Currency> ItemsSource
    {
        get => (List<Currency>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty
        .CreateAttached(nameof(ItemsSource), typeof(List<Currency>), typeof(MyPicker), new List<Currency>());

    public Currency SelectedItem
    {
        get => (Currency)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly BindableProperty SelectedItemProperty = BindableProperty
        .CreateAttached(nameof(SelectedItem), typeof(Currency), typeof(MyPicker), null);


    public ICommand ItemChangedCommand
    {
        get => (ICommand)GetValue(ItemChangedCommandProperty);
        set => SetValue(ItemChangedCommandProperty, value);
    }

    public static readonly BindableProperty ItemChangedCommandProperty = BindableProperty
        .CreateAttached(nameof(ItemChangedCommand), typeof(ICommand), typeof(MyPicker), null);

    #endregion
}