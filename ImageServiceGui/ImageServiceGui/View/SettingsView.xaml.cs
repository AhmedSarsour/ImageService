using System;
using System.Windows.Controls;

namespace ImageServiceGui.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.SettingsViewModel();
        }
    }
}
