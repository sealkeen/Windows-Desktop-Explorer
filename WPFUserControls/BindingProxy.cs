using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExplorerLibrary;

namespace UserControls
{
    /// <summary>
    /// Add Proxy <ut:BindingProxy x:Key="Proxy" Data="{Binding}" /> to Resources
    /// Bind like <Element Property="{Binding Data.MyValue, Source={StaticResource Proxy}}" />   
    /// </summary>
    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public FileIconInfo Data
        {
            get { return (FileIconInfo)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(FileIconInfo), typeof(BindingProxy));
    }
}
