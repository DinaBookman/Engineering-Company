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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public TaskWindow(int id = 0)
        {
            InitializeComponent();
            CurrentTask = (id == 0) ?
                 new BO.Task()
                 {
                     Id = 0000,
                     Description = null,
                     Alias = null,
                     Status = BO.Status.None,
                     DependenciesList = null,
                     CreatedAtDate = DateTime.Now,
                     StartDate = null,
                     ScheduledStartDate = null,
                     DeadlineDate = null,
                     ForecastAtDate = null,
                     CompletedAtDate = null,
                     Milestone = null,
                     Engineer = null,
                     ComplexityLevel = BO.EngineerExperience.None,
                     Deliverables = null,
                     Remarks = null
                 } :
                s_bl.Task.Read(id)!;
        }

        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }

        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        private void BtnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Content.ToString() == "Add")
            {
                try
                {
                    s_bl.Task.Create(CurrentTask);
                    Close();
                    MessageBox.Show("The Task was successfully added", "success");

                }
                catch
                {
                    MessageBox.Show("Oops! was unable to add Task.", "Error to Add Task");
                }
            }
            else
            {
                try
                {
                    s_bl.Task.Update(CurrentTask);
                    Close();
                    MessageBox.Show("The Task was successfully updated", "success");
                }
                catch
                {
                    MessageBox.Show("Oops! was unable to update Task.", "Error to update Task");
                }

            }
        }
    }
}
