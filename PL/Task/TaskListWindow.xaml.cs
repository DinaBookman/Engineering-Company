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
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        // Constructor for TaskListWindow, populates TaskList with all tasks.
        public TaskListWindow()
        {
            InitializeComponent();
            var temp = s_bl?.Task.ReadAll();
            var list = from task in temp
                       select new BO.TaskInList() { Id = task.Id, Alias = task.Alias, Description = task.Description, Status = task.Status };
            TaskList = list == null ? new() : new(list);
        }

        // Dependency property for the list of tasks.
        public ObservableCollection<BO.TaskInList> TaskList
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));

        public BO.Status Status { get; set; } = BO.Status.None;

        // Event handler for changing the status selector.
        private void CbStatusSelector_selectorChanged(object sender, SelectionChangedEventArgs e)
        {
            var temp = Status == BO.Status.None ?
            s_bl?.Task.ReadAll() :
            s_bl?.Task.ReadAll(item => item.Status == Status);
            var list = from task in temp
                       select new BO.TaskInList() { Id = task.Id, Alias = task.Alias, Description = task.Description, Status = task.Status };
            TaskList = list == null ? new() : new(list);
        }

        // Event handler for the Add button click.
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Opens TaskWindow for adding a new task.
            TaskWindow ew = new TaskWindow();
            ew.ShowDialog();
            var temp = s_bl?.Task.ReadAll();
            var list = from task in temp
                       select new BO.TaskInList() { Id = task.Id, Alias = task.Alias, Description = task.Description, Status = task.Status };
            TaskList = list == null ? new() : new(list);
        }

        // Event handler for updating a single task.
        private void SingleTask_update(object sender, MouseButtonEventArgs e)
        {
            // Opens TaskWindow for updating a selected task.
            BO.TaskInList? TaskInList = (sender as ListView)?.SelectedItem as BO.TaskInList;
            TaskWindow ew = new TaskWindow(TaskInList!.Id);
            ew.ShowDialog();
            var temp = s_bl?.Task.ReadAll();
            var list = from task in temp
                       select new BO.TaskInList() { Id = task.Id, Alias = task.Alias, Description = task.Description, Status = task.Status };
            TaskList = list == null ? new() : new(list);
        }
    }
}
