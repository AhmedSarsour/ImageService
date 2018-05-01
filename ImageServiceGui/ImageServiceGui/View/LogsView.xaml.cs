using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageServiceGui.View
{
    /// <summary>
    /// Interaction logic for LogsView.xaml
    /// </summary>
    public partial class LogsView : TabItem
    {
        public LogsView()
        {
            InitializeComponent();
            StackPanel stack = this.FindName("mainVertical") as StackPanel;
            StackPanel stack2 = new StackPanel();
            stack2.Orientation = Orientation.Horizontal;
            Label infoText = new Label();
            infoText.Content = "INFO";
            infoText.Width = 80;
            Label fullInfo = new Label();
            fullInfo.Content = "hello evsgfsgkslgkdl;fkslf;gksdl;\ngksldgksd;lgk;lkfsl;gksdg;lskgl;sdk;gdlk;gkery1";
            fullInfo.Width = 422;
            infoText.Background = Brushes.Yellow;
            infoText.BorderThickness = new Thickness(1);
            infoText.BorderBrush = Brushes.Black;
            fullInfo.BorderThickness = new Thickness(1);
            fullInfo.BorderBrush = Brushes.Black;

            Label warning = new Label();
            warning.Content = "WARNING DUDE~~ Something BAD IS ABOUT To HAPPEN!!!";
            warning.Width = 422;
            warning.BorderBrush = Brushes.Black;
            warning.BorderThickness = new Thickness(1);

            Label infoAg = new Label();
            infoAg.Content = "WARNING";
            infoAg.Width = 80;
            infoAg.Background = Brushes.Red;
            infoAg.BorderThickness = new Thickness(1);
            infoAg.BorderBrush = Brushes.Black;

            StackPanel stack3 = new StackPanel();
            stack3.Orientation = Orientation.Horizontal;
            stack3.Children.Add(infoAg);
            stack3.Children.Add(warning);
            stack.Children.Add(stack2);
            stack.Children.Add(stack3);
            stack2.Children.Add(infoText);
            stack2.Children.Add(fullInfo);
        }
    }
}
