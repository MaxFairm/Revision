using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TabControlExperiments
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        BlockChain TheBlocks = new BlockChain();

        public MainWindow()
        {                     
           
            foreach (int i in Enumerable.Range(0, 5))
            {
                TheBlocks.Add(new Block(i.ToString()));
            }
            InitializeComponent();
            this.DataContext = TheBlocks;
            TBC.SelectedIndex = 0;
        }

        private void Button_Click(object sender,RoutedEventArgs e)
        {
            if (TBC.SelectedContent is Block B)
            {
                Task.Factory.StartNew(B.Mine);
            }
        }
    }

    /// <summary>
    /// Code for colours of background and updates when value is signed and unsigned
    /// </summary>
    public class ConvertStuff : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush)) throw new ArgumentException();

            bool IsSigned = (bool)value;

            return IsSigned == true ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}


    
