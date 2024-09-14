using System.Windows;
using System.Windows.Controls;

namespace GeoGravityOverDose.Views.Shared
{
    /// <summary>
    /// Логика взаимодействия для TextField.xaml
    /// </summary>
    public partial class TextField : UserControl
    {
        // Зависимое свойство для текста в TextBox
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text", typeof(string), typeof(TextField),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));

        public string Label
        {
            set => label.Text = value;
        }


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (TextField)d;
            control.textBox.Text = (string)e.NewValue;
        }

        // Обработка изменения текста в TextBox, чтобы обновить привязку
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetValue(TextProperty, textBox.Text);
        }
        public TextField()
        {
            InitializeComponent();
        }
    }
}
