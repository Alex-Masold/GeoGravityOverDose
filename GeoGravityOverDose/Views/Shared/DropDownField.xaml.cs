using GeoGravityOverDose.Models.Base;
using GeoGravityOverDose.Views.Pages;
using ReactiveUI;
using System.Collections;
using System.Reactive;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GeoGravityOverDose.Views.Shared
{
    /// <summary>
    /// Логика взаимодействия для DropDownField.xaml
    /// </summary>
    public partial class DropDownField : UserControl
    {
        private string _previousValue;
        private string _displayText;
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        public ReactiveCommand<Unit, Unit> GotFocusCommand { get; set; }
        public ReactiveCommand<Unit, Unit> LostFocusCommand { get; set; }

        public string Label
        {
            set => label.Text = value;
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(DropDownField),
                new PropertyMetadata(null));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty SelectedEntityProperty =
            DependencyProperty.Register(
                nameof(SelectedEntity),
                typeof(IdClass),
                typeof(DropDownField),
                new FrameworkPropertyMetadata(
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedEntityChanged)
                );

        public IdClass SelectedEntity
        {
            get => (IdClass)GetValue(SelectedEntityProperty);
            set => SetValue(SelectedEntityProperty, value);
        }
        
        private static void OnSelectedEntityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (DropDownField)d;
            var comboBox = control.comboBox;
            // Синхронизация выбора в ListBox при изменении свойства SelectedEntity
            if (comboBox.SelectedItem != e.NewValue)
            {
                comboBox.SelectedItem = e.NewValue;
            }
        }
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedItem != SelectedEntity)
            {
                SelectedEntity = (IdClass   )comboBox.SelectedItem;
            }
        }

        public static readonly DependencyProperty CallBackCommandProperty = DependencyProperty.Register(
             nameof(CallBackCommand),
             typeof(ICommand),
             typeof(DropDownField),
             new PropertyMetadata(null)
        );

        public ICommand CallBackCommand
        {
            get => (ICommand)GetValue(CallBackCommandProperty);
            set => SetValue(CallBackCommandProperty, value);
        }

       

        public static readonly DependencyProperty ShowSnackBarProperty =
            DependencyProperty.Register(
                nameof(ShowSnackBar),
                typeof(bool),
                typeof(DropDownField),
                new PropertyMetadata(true));
        public bool ShowSnackBar
        {
            get => (bool)GetValue(ShowSnackBarProperty);
            set => SetValue(ShowSnackBarProperty, value);
        }

        private void OnGotFocus()
        {
            _displayText = string.IsNullOrWhiteSpace(SelectedEntity.FullName) ? "Пустая строка" : SelectedEntity.FullName;
            if (_previousValue != SelectedEntity.FullName)
                _previousValue = _displayText;
        }

        private void OnLostFocus()
        {
            _displayText = string.IsNullOrWhiteSpace(SelectedEntity.FullName) ? "Пустая строка" : SelectedEntity.FullName;
            if (_previousValue != SelectedEntity.FullName)
            {
                if (CallBackCommand != null && CallBackCommand.CanExecute(null))
                {
                    CallBackCommand.Execute(null);
                }
                if (ShowSnackBar)
                {
                    mainWindow.GlobalSnackbar.MessageQueue.Enqueue($"{label.Text}: {_previousValue} -> {_displayText}");
                }
            }

            _previousValue = _displayText;
        }

        private void comboBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Проверяем, нажата ли клавиша Enter
            if (e.Key == Key.Enter && comboBox.IsFocused)
            {
                // Принудительно снимаем фокус с TextBox (если необходимо)
                comboBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        public DropDownField()
        {
            InitializeComponent();

            GotFocusCommand = ReactiveCommand.Create(OnGotFocus);
            LostFocusCommand = ReactiveCommand.Create(OnLostFocus);
        }

    }
}
