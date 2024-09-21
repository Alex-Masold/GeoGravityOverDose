using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GeoGravityOverDose.Views.Shared
{
    /// <summary>
    /// Логика взаимодействия для TextField.xaml
    /// </summary>
    public partial class TextField : UserControl
    {
        private string _previosValue;
        private DispatcherTimer _inputTimer;

        // Зависимое свойство для текста в TextBox
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text), 
                typeof(string), 
                typeof(TextField),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnTextChanged)
                );

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

            _inputTimer.Stop();
            _inputTimer.Start();
        }

        public static readonly DependencyProperty ShowSnackBarProperty =
            DependencyProperty.Register(
                nameof(ShowSnackBar),
                typeof(bool),
                typeof(TextField),
                new PropertyMetadata(true));
        public bool ShowSnackBar
        {
            get => (bool)GetValue(ShowSnackBarProperty);
            set => SetValue(ShowSnackBarProperty, value);
        }

        public SnackbarMessageQueue SnackbarMessageQueue { get; } = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

        private void InputTimer_Tick (object sender, EventArgs e)
        {
            _inputTimer.Stop();

            if (ShowSnackBar && _previosValue != Text)
            {
                SnackbarMessageQueue.Enqueue($"{label.Text} {_previosValue} -> {Text}");
                _previosValue = Text;
            }
        }
        public TextField()
        {
            InitializeComponent();
            _previosValue = Text;

            _inputTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _inputTimer.Tick += InputTimer_Tick;
        }
    }
}
