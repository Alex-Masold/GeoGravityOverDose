using GeoGravityOverDose.Views.Pages;
using ReactiveUI;
using System.Reactive;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GeoGravityOverDose.Views.Shared
{
    /// <summary>
    /// Логика взаимодействия для TextField.xaml
    /// </summary>
    public partial class TextField : UserControl
    {
        private string _previousValue;
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

        public ReactiveCommand<Unit, Unit> GotFocusCommand { get; set; }
        public ReactiveCommand<Unit, Unit> LostFocusCommand { get; set; }

        public string Label
        {
            set => label.Text = value;
        }

        public static readonly DependencyProperty MaxLengthProperty =
           DependencyProperty.Register(
               nameof(MaxLength),
               typeof(int),
               typeof(TextField),
               new PropertyMetadata(50)
               );
        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public static readonly DependencyProperty PrefixTextProperty =
            DependencyProperty.Register(
                nameof(PrefixText),
                typeof(string),
                typeof(TextField),
                new PropertyMetadata("")
            );
        public string PrefixText
        {
            get => (string)GetValue(PrefixTextProperty);
            set => SetValue(PrefixTextProperty, value);
        }

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
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (TextField)d;
            control.textBox.Text = (string)e.NewValue;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetValue(TextProperty, textBox.Text);
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

        private void OnGotFocus()
        {
            _previousValue = Text;
        }

        private void OnLostFocus()
        {
            string displayText = string.IsNullOrWhiteSpace(Text) ? "Пустая строка" : Text;
            if (_previousValue != Text)
            {
                if (CallBackCommand != null && CallBackCommand.CanExecute(null))
                {
                    CallBackCommand.Execute(null);
                }
                if (ShowSnackBar)
                {
                    mainWindow.GlobalSnackbar.MessageQueue.Enqueue($"{label.Text}: {_previousValue} -> {displayText}");
                }
            }

            _previousValue = displayText;


        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Проверяем, нажата ли клавиша Enter
            if (e.Key == Key.Enter && textBox.IsFocused)
            {
                // Принудительно снимаем фокус с TextBox (если необходимо)
                textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        public TextField()
        {
            InitializeComponent();

            GotFocusCommand = ReactiveCommand.Create(OnGotFocus);
            LostFocusCommand = ReactiveCommand.Create(OnLostFocus);
        }
    }
}
