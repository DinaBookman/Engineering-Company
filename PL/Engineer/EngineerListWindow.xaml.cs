using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
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
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public EngineerListWindow()
        {
            InitializeComponent();
            // Retrieves engineers from the business logic layer and populates EngineerList.
            var temp = s_bl?.Engineer.ReadAll();
            var list = from engineer in temp
                       select new BO.EngineerInList() { Id = engineer.Id, Name = engineer.Name };
            EngineerList = list == null ? new() : new(list);
        }

        // Dependency property for the list of engineers.
        public ObservableCollection<BO.EngineerInList> EngineerList
        {
            get { return (ObservableCollection<BO.EngineerInList>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(ObservableCollection<BO.EngineerInList>), typeof(EngineerListWindow), new PropertyMetadata(null));

        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;
        // Event handler for changing the level selector.
        private void CbLevelSelector_selectorChanged(object sender, SelectionChangedEventArgs e)
        {
            var temp = Level == BO.EngineerExperience.None ?
            s_bl?.Engineer.ReadAll() :
            s_bl?.Engineer.ReadAll(item => item.Level == Level);
            var list = from engineer in temp
                       select new BO.EngineerInList() { Id = engineer.Id, Name = engineer.Name };
            EngineerList = list == null ? new() : new(list);
        }

        // Event handler for the Add button click.
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Opens EngineerWindow for adding a new engineer.
            EngineerWindow ew = new EngineerWindow();
            ew.ShowDialog();
            var temp = s_bl?.Engineer.ReadAll();
            var list = from engineer in temp
                       select new BO.EngineerInList() { Id = engineer.Id, Name = engineer.Name };
            EngineerList = list == null ? new() : new(list);
        }

        // Event handler for updating a single engineer.
        private void SingleEngineer_update(object sender, MouseButtonEventArgs e)
        {
            // Opens EngineerWindow for updating a selected engineer.
            BO.EngineerInList? EngineerInList = (sender as ListView)?.SelectedItem as BO.EngineerInList;
            EngineerWindow ew = new EngineerWindow(EngineerInList!.Id);
            ew.ShowDialog();
            var temp = s_bl?.Engineer.ReadAll();
            var list = from engineer in temp
                       select new BO.EngineerInList() { Id = engineer.Id, Name = engineer.Name };
            EngineerList = list == null ? new() : new(list);
        }
    }
}
