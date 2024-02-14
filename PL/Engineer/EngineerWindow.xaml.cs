using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        // Constructor for EngineerWindow, initializes with default or existing engineer.
        public EngineerWindow(int id = 0)
        {
            InitializeComponent();
            CurrentEngineer = (id == 0) ?
                new BO.Engineer() { Id = 0000, Name = null, Email = null, Level = BO.EngineerExperience.None, Cost = 0, Task = null } :
                s_bl.Engineer.Read(id)!;
        }

        // Dependency property for the current engineer.
        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

        // Event handler for Add/Update button click.
        private void BtnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Adds or updates engineer based on button content.
            if ((sender as Button)?.Content.ToString() == "Add")
            {
                try
                {
                    s_bl.Engineer.Create(CurrentEngineer);
                    Close();
                    MessageBox.Show("The Engineer was successfully added", "success");

                }
                catch
                {
                    MessageBox.Show("Oops! was unable to add Engineer.", "Error to Add Engineer");
                }
            }
            else
            {
                try
                {
                    s_bl.Engineer.Update(CurrentEngineer);
                    Close();
                    MessageBox.Show("The Engineer was successfully updated", "success");
                }
                catch
                {
                    MessageBox.Show("Oops! was unable to update Engineer.", "Error to update Engineer");
                }

            }
        }
    }
}
