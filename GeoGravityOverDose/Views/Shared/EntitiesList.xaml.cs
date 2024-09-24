using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GeoGravityOverDose.Views.Shared
{
    /// <summary>
    /// Логика взаимодействия для EntitiesList.xaml
    /// </summary>
    public partial class EntitiesList : UserControl
    {
        public EntitiesList()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(Entities),
                typeof(ICollection),
                typeof(EntitiesList),
                new PropertyMetadata(null));

        public ICollection Entities
        {
            get => (ICollection)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty SelectedEntityProperty =
            DependencyProperty.Register(
                nameof(SelectedEntity),
                typeof(object),
                typeof(EntitiesList),
                new FrameworkPropertyMetadata(
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedEntityChanged)
                );

        public object SelectedEntity
        {
            get => GetValue(SelectedEntityProperty);
            set => SetValue(SelectedEntityProperty, value);
        }

        // Метод, вызываемый при изменении SelectedEntity
        // Метод для обработки изменения SelectedEntity
        private static void OnSelectedEntityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (EntitiesList)d;
            var listBox = control.List;
            // Синхронизация выбора в ListBox при изменении свойства SelectedEntity
            if (listBox.SelectedItem != e.NewValue)
            {
                listBox.SelectedItem = e.NewValue;
            }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (List.SelectedItem != SelectedEntity)
            {
                SelectedEntity = List.SelectedItem;
            }
        }

        public static readonly DependencyProperty AddEntityCommandProperty = DependencyProperty.Register(
              nameof(AddEntityCommand),
              typeof(ICommand),
              typeof(EntitiesList),
              new PropertyMetadata(null));

        public ICommand AddEntityCommand
        {
            get => (ICommand)GetValue(AddEntityCommandProperty);
            set => SetValue(AddEntityCommandProperty, value);
        }

        public static readonly DependencyProperty DeleteEntityCommandProperty =
            DependencyProperty.Register(
                nameof(DeleteEntityCommand),
                typeof(ICommand),
                typeof(EntitiesList),
                new PropertyMetadata(null));

        public ICommand DeleteEntityCommand
        {
            get => (ICommand)(GetValue(DeleteEntityCommandProperty));
            set => SetValue(DeleteEntityCommandProperty, value);
        }
    }
}
