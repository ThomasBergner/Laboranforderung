using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Laboranforderung.Controls;

public class DoubleImageButton : Button
{

    private Grid m_Grid;
    public DoubleImageButton()
    {
        m_Grid = new Grid();
        this.HorizontalContentAlignment = HorizontalAlignment.Stretch;
        this.VerticalContentAlignment = VerticalAlignment.Stretch;
        this.Content = m_Grid;
        this.MinHeight = 23;
        this.MinWidth = 23;
    }


    public static readonly DependencyProperty EnabledBrushProperty =
       DependencyProperty.Register("EnabledBrush", typeof(Brush), typeof(DoubleImageButton), new PropertyMetadata(Brushes.Transparent, OnChanged));

    private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        DoubleImageButton btn = (DoubleImageButton)d;
        btn.OnBrushChanged();
    }

    public static readonly DependencyProperty DisabledBrushProperty =
        DependencyProperty.Register("DisabledBrush", typeof(Brush), typeof(DoubleImageButton), new PropertyMetadata(Brushes.Transparent, OnChanged));

    public Brush EnabledBrush
    {
        get { return (Brush)GetValue(EnabledBrushProperty); }
        set { SetValue(EnabledBrushProperty, value); }
    }

    public Brush DisabledBrush
    {
        get { return (Brush)GetValue(DisabledBrushProperty); }
        set { SetValue(DisabledBrushProperty, value); }
    }


    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property.Name == "IsEnabled")
            OnBrushChanged();
    }

    private void OnBrushChanged()
    {
       m_Grid.Background = this.IsEnabled ? EnabledBrush : DisabledBrush;
    }


}
