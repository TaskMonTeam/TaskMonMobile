using Microsoft.Maui.Controls.Shapes;

namespace TaskMonAdmin.Controls;

public class CustomChip : ContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(CustomChip), string.Empty, propertyChanged: OnTextChanged);

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected), typeof(bool), typeof(CustomChip), false, BindingMode.TwoWay, propertyChanged: OnIsSelectedChanged);
        
    public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(
        nameof(IconSource), typeof(string), typeof(CustomChip), "check.png", propertyChanged: OnIconSourceChanged);

    public event EventHandler<bool> SelectedChanged;

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }
    
    public string IconSource
    {
        get => (string)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    private readonly Border _container;
    private readonly Label _label;
    private readonly Image _checkImage;
    private readonly StackLayout _contentStack;

    public CustomChip()
    {
        _label = new Label
        {
            TextColor = Colors.Black,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(5, 0)
        };

        _checkImage = new Image
        {
            Source = IconSource,
            HeightRequest = 16,
            WidthRequest = 16,
            IsVisible = false,
            VerticalOptions = LayoutOptions.Center
        };

        _contentStack = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = 5,
            Children = { _checkImage, _label }
        };

        _container = new Border
        {
            Padding = new Thickness(8, 5),
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(8)
            },
            Stroke = Colors.LightGray,
            BackgroundColor = Colors.White,
            Content = _contentStack
        };

        Content = _container;
        
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += OnChipTapped;
        GestureRecognizers.Add(tapGestureRecognizer);

        UpdateVisualState();
    }

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomChip)bindable;
        control._label.Text = newValue?.ToString();
    }

    private static void OnIsSelectedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomChip)bindable;
        control.UpdateVisualState();
    }
    
    private static void OnIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomChip)bindable;
        control._checkImage.Source = (string)newValue;
    }

    private void OnChipTapped(object sender, EventArgs e)
    {
        IsSelected = !IsSelected;
        SelectedChanged?.Invoke(this, IsSelected);
    }

    private void UpdateVisualState()
    {
        if (IsSelected)
        {
            _container.BackgroundColor = Color.FromArgb("#E8DEF8");
            _container.Stroke = Colors.Transparent;
            _label.TextColor = Color.FromArgb("#6750A4");
            _checkImage.IsVisible = true;
        }
        else
        {
            _container.BackgroundColor = Colors.White;
            _container.Stroke = Colors.LightGray;
            _label.TextColor = Colors.Black;
            _checkImage.IsVisible = false;
        }
    }
}