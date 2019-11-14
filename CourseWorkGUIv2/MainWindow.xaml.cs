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

namespace CourseWorkGUIv2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method to Exit Program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemExit_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }

        /// <summary>
        /// Method to Import Json File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importFileMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Method to Display About Pop-up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
