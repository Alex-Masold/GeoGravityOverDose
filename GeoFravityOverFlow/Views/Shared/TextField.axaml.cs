using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GeoGravityOverDose.Views.Shared
{
    public partial class TextField : UserControl
    {
        public TextField()
        {
            InitializeComponent();
            textBox.TextChanged += TextBox_TextChanged;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<TextField, string>(nameof(Text));

        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly StyledProperty<string> NameProperty =
            AvaloniaProperty.Register<TextField, string>(nameof(Name));

        public string Name
        {
            get => GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public static readonly StyledProperty<bool> ShowMessageBoxProperty =
            AvaloniaProperty.Register<TextField, bool>(nameof(ShowMessageBox), true);

        public bool ShowMessageBox
        {
            get => GetValue(ShowMessageBoxProperty);
            set => SetValue(ShowMessageBoxProperty, value);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var oldValue = e.OldText;
            var newValue = textBox.Text;

            if (ShowMessageBox)
            {
                // Вывод MessageBox, если ShowMessageBox равно true
                MessageBox.Show($"Old Value: {oldValue}\nNew Value: {newValue}");
            }
        }
    }
}
