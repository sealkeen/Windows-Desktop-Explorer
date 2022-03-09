using System;
using System.Collections;
using System.Collections.Generic;
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
using ExplorerLibrary;

namespace UserControls
{
    /// <summary>
    /// Логика взаимодействия для FileControl.xaml
    /// </summary>
    public partial class FileControl : UserControl
    {
        public FileControl()
        {
            InitializeComponent();
            ItemIndex = MaxIndex++;
        }

        public static int MaxIndex;

        public int ItemIndex { get; set; }
        public static readonly DependencyProperty ItemIndexProperty =
            DependencyProperty.Register("ItemIndex", typeof(int), typeof(FileControl), new UIPropertyMetadata(0));   

        public FileIconInfo FileIconInfo { get; set; }
        public static readonly DependencyProperty FileIconInfoProperty =
            DependencyProperty.Register("FileIconInfo", typeof(FileIconInfo), typeof(FileControl), new UIPropertyMetadata(null));

        public IEnumerable ItemsSource { get { return (IEnumerable)GetValue(ItemsSourceProperty); } set { SetValue(ItemsSourceProperty, value); } }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(DirectoryFiles), typeof(FileControl), new PropertyMetadata(null));

        private void btnTakeOwnership_Click(object sender, RoutedEventArgs e)
        {
            AccessController.TakeOwnership(FileIconInfo.FileInfo.FullName);
        }

        private void btnIconButton_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            AccessController.TakeOwnership(FileIconInfo.FileInfo.FullName);
        }

        private void fileControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            btnIconButton.Height = ExplorerLibrary.FileIconInfo._iconHeight;
            btnIconButton.Width = ExplorerLibrary.FileIconInfo._iconWidth;
            //btnIconButton.GetBindingExpression(Button.HeightProperty).UpdateTarget();
            //btnIconButton.GetBindingExpression(Button.WidthProperty).UpdateTarget();
        }
    }
}
