using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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
using ExplorerLibrary;
using ObjectModelExtensions;
using System.IO;
using WPFUserControls;
using System.Threading;
using System.Threading.Tasks;

namespace UserControls
{
    /// <summary>
    /// Логика взаимодействия для FileStackPanel.xaml
    /// </summary>
    public partial class FileStackPanel : UserControl
    {
        public static ViewModelBase ViewModel { get; set; }
        public FileStackPanel()
        {
            InitializeComponent();
            //Binding binding = new Binding("ItemsSource");
            //binding.Source = this;
            //icFiles.SetBinding(ComboBox.ItemsSourceProperty, binding);
            //icFiles.GetBindingExpression(System.Windows.Controls.ListView.ItemsSourceProperty).UpdateTarget();
        }

        public void UpdateViewModelDataContext(string directory = "")
        {
            if (directory == "" || !Directory.Exists(directory))
                directory = Environment.SpecialFolder.DesktopDirectory.ToString();
            FileControl.MaxIndex = 0;

            var task = Task.Factory.StartNew( delegate
                {
                    this.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        ViewModel = new ViewModelBase();
                        ViewModel.DirectoryFiles = new DirectoryFiles(directory);
                        DataContext = ViewModel.DirectoryFiles;
                        var fii = ViewModel.DirectoryFiles.FileIconInfos;
                        ViewModel.DirectoryFiles.List(directory);
                    }));
                }
            );
            task.Wait();
            task.ContinueWith(delegate
                {
                    this.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        this.OnItemsSourceChanged(ViewModel.DirectoryFiles.FileIconInfos, ViewModel.DirectoryFiles.FileIconInfos);
                    }));
                }
            );//icFiles.GetBindingExpression(ItemsControl.ItemsSourceProperty).UpdateTarget();
            //task.Wait();
        }

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
            var oldValueINotifyCollectionChanged = oldValue as ConcurrentObservableCollection<FileIconInfo>;

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

        public FileIconInfo ClickedItem
        {
            get { return (FileIconInfo)GetValue(ClickedItemProperty); }
            set { value.OpenFile(); }
        }
        public static readonly DependencyProperty ClickedItemProperty =
            DependencyProperty.Register("ClickedItem", typeof(FileIconInfo), 
            typeof(FileStackPanel), new PropertyMetadata(null));

        private void ucFileControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataContext = ((DirectoryFiles)this.GetValue(DataContextProperty));
            dataContext.SelectedIndex = (sender as FileControl).ItemIndex;
            //if (dataContext.CurrentFileIconInfo.FileInfo.Attributes.HasFlag(FileAttributes.Directory))
            //Process.Start(dataContext.CurrentFileIconInfo.FileInfo.FullName);

            if (dataContext.CurrentFileIconInfo.FileInfo.Attributes.HasFlag(FileAttributes.Directory))
            {
                UpdateViewModelDataContext(dataContext.CurrentFileIconInfo.FileInfo.FullName);
            }
            else
                Process.Start(dataContext.CurrentFileIconInfo.FileInfo.FullName);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo newDirectory = new DirectoryInfo(ViewModel.DirectoryFiles.DirectoryInfo.FullName);
            if (newDirectory.Parent == null)
                return;
            string parent = newDirectory.Parent.FullName;
            if(Directory.Exists(parent))
                UpdateViewModelDataContext(parent);
        }
    }
}
