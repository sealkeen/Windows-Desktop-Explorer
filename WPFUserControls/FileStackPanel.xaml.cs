using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;
using System.Collections.ObjectModel;
using ExplorerLibrary;

namespace UserControls
{
    /// <summary>
    /// Логика взаимодействия для FileStackPanel.xaml
    /// </summary>
    public partial class FileStackPanel : UserControl
    {
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(DirectoryFiles), typeof(FileStackPanel), new PropertyMetadata(new PropertyChangedCallback(OnItemsSourcePropertyChanged)));

        private static void OnItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as FileStackPanel;
            if (control != null)
                control.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        }
        public void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            // Remove handler for oldValue.CollectionChanged
            var oldValueINotifyCollectionChanged = oldValue as ObservableCollection<FileIconInfo>;

            if (null != oldValueINotifyCollectionChanged)
            {
                oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
            }
            // Add handler for newValue.CollectionChanged (if possible)
            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if (null != newValueINotifyCollectionChanged)
            {
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
            }
            icFiles.ItemsSource = newValue;
        }

        void newValueINotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Do your stuff here.
            //icFiles.ItemsSource = newValue;

            this.GetBindingExpression(System.Windows.Controls.ListView.ItemsSourceProperty).UpdateTarget();
        }
        public FileStackPanel()
        {
            InitializeComponent();
            //Binding binding = new Binding("ItemsSource");
            //binding.Source = this;
            //icFiles.SetBinding(ComboBox.ItemsSourceProperty, binding);
            //icFiles.GetBindingExpression(System.Windows.Controls.ListView.ItemsSourceProperty).UpdateTarget();
        }

        public FileIconInfo ClickedItem
        {
            get { return (FileIconInfo)GetValue(ClickedItemProperty); }
            set { value.OpenFile(); }
        }
        public static readonly DependencyProperty ClickedItemProperty =
            DependencyProperty.Register("ClickedItem", typeof(FileIconInfo), 
            typeof(FileStackPanel), new PropertyMetadata(null));

    }
}
