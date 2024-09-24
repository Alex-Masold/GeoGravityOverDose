using GeoGravityOverDose.Views.Pages;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace GeoGravityOverDose.Views.Shared
{
    /// <summary>
    /// Логика взаимодействия для TextField.xaml
    /// </summary>
    public partial class TextField : UserControl
    {
        private string _previousValue;
        private bool _isInternalChange;
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
            set
            {
                if (!_isInternalChange)
                {
                    SetValue(TextProperty, value);
                }
            }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (TextField)d;
            control._isInternalChange = true;
            control.textBox.Text = (string)e.NewValue;
            control._isInternalChange = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isInternalChange)
            {
                SetValue(TextProperty, textBox.Text);

                //_inputTimer.Stop();
                //_inputTimer.Start();
            }
        }

        public static readonly DependencyProperty CallBackCommandProperty = DependencyProperty.Register(
              nameof(CallBackCommand),
              typeof(ICommand),
              typeof(TextField),
              new PropertyMetadata(null));

        public ICommand CallBackCommand
        {
            get => (ICommand)GetValue(CallBackCommandProperty);
            set => SetValue(CallBackCommandProperty, value);
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

        public void GotFocus()
        {
            _previousValue = Text;
        }

        public void LostFocus()
        {
            string displayText = string.IsNullOrWhiteSpace(Text) ? "Пустая строка" : Text;
            if (ShowSnackBar && _previousValue != Text)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.GlobalSnackbar.MessageQueue.Enqueue($"{label.Text}: {_previousValue} -> {displayText}");
            }

            _previousValue = displayText;

            if (CallBackCommand != null && CallBackCommand.CanExecute(null))
            {
                CallBackCommand.Execute(null);
            }
        }

        private void InputTimer_Tick(object sender, EventArgs e)
        {
            _inputTimer.Stop();

            if (ShowSnackBar &&_previousValue != Text)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                string displayText = string.IsNullOrWhiteSpace(Text) ? "Пустая строка" : Text;
                mainWindow.GlobalSnackbar.MessageQueue.Enqueue($"{label.Text}: {_previousValue} -> {displayText}");

                _previousValue = displayText;

                if (CallBackCommand != null && CallBackCommand.CanExecute(null))
                {
                    CallBackCommand.Execute(null);
                }
            }
        }
        public TextField()
        {
            InitializeComponent();


            //_inputTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            //_inputTimer.Tick += InputTimer_Tick;

        }
    }
}
