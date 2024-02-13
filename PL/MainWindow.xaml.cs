using DalApi;
using PL.Engineer;
using PL.Task;
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

namespace PL
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

        private void btnEngineers_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }
        private void btnTasks_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();
        }

        private void btnInitDB(object sender, RoutedEventArgs e)
        { 
            if (MessageBox.Show("Are you sure you want to initialize the data?", "initialize data",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DalTest.Initialization.Do();
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("If you close the next window without saving, your changes will be lost.",
                        "Configuration",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
        }
    }
}
