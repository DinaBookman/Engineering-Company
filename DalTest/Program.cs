using Dal;
using DalApi;
using DO;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace DalTest
{
    internal class Program
    {
        static readonly IDal s_dal = new DalList(); //stage 2

        //function to create an engineer.
        private static Engineer CreateEngineer()
        {
            Console.WriteLine("Please enter Engineers Id:");
            int _id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please enter Engineers Name:");
            string _name = Console.ReadLine()!;
            Console.WriteLine("Please enter Engineers Email:");
            string _email = Console.ReadLine()!;
            Console.WriteLine("Please enter Engineer Experience (0 - 4):");
            int _level = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please enter Engineers cost:");
            int _cost = Convert.ToInt32(Console.ReadLine());
            Engineer newEngineer = new(_id, _name, _email, (EngineerExperience)_level, _cost);
            return newEngineer;
        }

        //function to create an task.
        private static DO.Task CreateTask(int _id = 0000)
        {
            Console.WriteLine("Please enter a description of the task:");
            string _description = Console.ReadLine()!;
            Console.WriteLine("Please enter tasks alias:");
            string _alias = Console.ReadLine()!;
            Console.WriteLine("Please enter Engineers id:");
            int _engineerId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please enter complexity level (0 - 4):");
            int _level = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please enter a comment:");
            string _comment = Console.ReadLine()!;
            DateTime createdDate = DateTime.Now;
            DateTime startDate = createdDate.Add(TimeSpan.FromDays(1));
            DateTime endDate = createdDate.Add(TimeSpan.FromDays(30));
            DateTime Deadline = createdDate.Add(TimeSpan.FromDays(15));
            DateTime estDateCompletion = endDate.Add(TimeSpan.FromDays(-3));
            DO.Task newTask = new(_id, _description, _alias, createdDate, startDate, endDate, Deadline, estDateCompletion, _engineerId, (EngineerExperience)_level, false,_comment);
            return newTask;
        }

        //function to create an Dependency.
        private static Dependency CreateDependency()
        {
            int _dependentTask;
            int _dependsOnTask;
            Console.WriteLine("Enter dependent Task:");
            _dependentTask = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter depends on Task id:");
            _dependsOnTask = Convert.ToInt32(Console.ReadLine());
            Dependency newDpendency = new(0000,_dependentTask, _dependsOnTask);
            return newDpendency;
        }

        //engineer CRUD functions
        private static void engineerCRUD()
        {
            int choice;
            do
            {
                Console.WriteLine("Please Enter your choice: 0 -To Exit, 1 -Create Engineer, 2 -Find Engineer," +
                             " 3 -Get list of Engineers, 4 -Update engineers information, 5 -Delete Engineer");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 0: return;
                    case 1:
                        s_dal!.Engineer.Create(CreateEngineer());
                        break;
                    case 2:
                        Console.WriteLine("Please enter Engineers Id");
                        Console.WriteLine(s_dal!.Engineer.Read(Convert.ToInt32(Console.ReadLine())));
                        break;
                    case 3:
                        foreach (var item in s_dal!.Engineer.ReadAll())
                            Console.WriteLine(item);
                        break;
                    case 4:
                        Console.WriteLine("Enter Engineer's id:");
                        int _id = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(s_dal!.Engineer.Read(_id));
                        s_dal!.Engineer.Update(CreateEngineer());
                        break;
                    case 5:
                        Console.WriteLine("Enter Engineer's id:");
                        s_dal!.Engineer.Delete(Convert.ToInt32(Console.ReadLine()));
                        break;
                    default:
                        Console.WriteLine("Wrong choice:");
                        break;
                }
            } while (choice != 0);
        }

        //Task CRUD functions
        private static void taskCRUD()
        {
            int choice;
            do
            {
                Console.WriteLine("Please Enter your choice: 0 -To Exit, 1 -Create Task, 2 -Find Task, 3 -Get list of Tasks, 4 -Update Tasks information, 5 -Delete Task");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 0: return;
                    case 1:
                        s_dal!.Task.Create(CreateTask()); 
                        break;
                    case 2:
                        Console.WriteLine("Enter task's id:");
                        Console.WriteLine(s_dal!.Task.Read(Convert.ToInt32(Console.ReadLine())));
                        break;
                    case 3:
                        foreach (var item in s_dal!.Task.ReadAll())
                            Console.WriteLine(item);
                        break;
                    case 4:
                        Console.WriteLine("Enter task's id:");
                        int _id = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(s_dal!.Task.Read(_id));
                        s_dal!.Task.Update(CreateTask(_id));
                        break;
                    case 5:
                        Console.WriteLine("Enter task's id:");
                        s_dal!.Task.Delete(Convert.ToInt32(Console.ReadLine()));
                        break;
                    default:
                        Console.WriteLine("Wrong choice:");
                        break;
                }
            } while (choice != 0);
        }

        //dependency CRUD functions
        private static void dependencyCRUD()
        {
            int choice;
            do
            {
                Console.WriteLine("Please Enter your choice: 0 -To Exit, 1 -Create dependency, 2 -Find dependency," +
                             " 3 -Get list of dependencies, 4 -Update dependencies information, 5 -Delete dependency");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 0: return;
                    case 1:
                        s_dal!.Dependency.Create(CreateDependency());
                        break;
                    case 2:
                        Console.WriteLine("Enter Dependency id:");
                        s_dal!.Dependency.Read(Convert.ToInt32(Console.ReadLine()));
                        break;
                    case 3:
                        foreach (var item in s_dal!.Dependency.ReadAll())
                             Console.WriteLine($"id: {item.Id}  task id: {item.DependentTask}  pervious task id: {item.DependsOnTask}");
                        break;
                    case 4:
                        s_dal!.Dependency.Update(CreateDependency());
                        break;
                    case 5:
                        Console.WriteLine("Enter Dependency id:");
                        s_dal!.Dependency.Delete(Convert.ToInt32(Console.ReadLine()));
                        break;
                    default:
                        Console.WriteLine("Wrong choice");
                        break;
                }
            } while (choice != 0);
        }

        //Main function
        static void Main(string[] args)
        {
            try
            {
                int choice;
                Initialization.Do(s_dal);
                do
                {
                    Console.WriteLine("Please enter your choice: 0 -Exit, 1 -Engineer, 2 -Task, 3 -Dependency");
                    choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 0: return;
                        case 1:
                            engineerCRUD();
                            break;
                        case 2:
                            taskCRUD();
                            break;
                        case 3:
                            dependencyCRUD();
                            break;
                        default:
                            Console.WriteLine("Wrong choice");
                            break;
                    }
                } while (choice != 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
         }
            Main(args);
        }  
    }
}