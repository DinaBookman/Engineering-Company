using Dal;
using DalApi;
using DO;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace DalTest
{
    internal class Program
    {
        private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
        private static ITask? s_dalTask = new TaskImplementation(); //stage 1
        private static IDependency? s_dalDependency = new DependencyImplementation();

        //function to create an engineer.
        private Engineer createEngineer()
        {
            Console.WriteLine("Please enter Engineers Id\n");
            int _id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please enter Engineers Name\n");
            string _name = Console.ReadLine()!;
            Console.WriteLine("Please enter Engineers Email\n");
            string _email = Console.ReadLine()!;
            Console.WriteLine("Please enter Engineer Experience (0 - 4)\n");
            int _level = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please enter Engineers cost\n");
            int _cost = Convert.ToInt32(Console.ReadLine());
            Engineer newEngineer = new(_id, _name, _email, (EngineerExperience)_level, _cost);
            return newEngineer;
        }

        //function to create an task.
        private Task createTask(int _id = 0000)
        {
            Console.WriteLine("Please enter a description of the task");
            string _description = Console.ReadLine()!;
            Console.WriteLine("Please enter tasks alias");
            string _alias = Console.ReadLine()!;
            Console.WriteLine("Please enter Engineers id");
            int _engineerId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please enter complexity level (0 - 4)\n");
            int _level = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please enter a comment");
            string _comment = Console.ReadLine()!;
            DateTime createdDate = DateTime.Now;
            DateTime startDate = createdDate.Add(TimeSpan.FromDays(1));
            DateTime endDate = createdDate.Add(TimeSpan.FromDays(30));
            DateTime Deadline = createdDate.Add(TimeSpan.FromDays(15));
            DateTime estDateCompletion = endDate.Add(TimeSpan.FromDays(-3));
            Engineer newEngineer = new(_id, _description, _alias, createdDate, startDate, endDate, Deadline, estDateCompletion, _engineerId, (EngineerExperience)_level, false);
            newEngineer.Remark = _comment;
            return newEngineer;
        }

        //function to create an Dependency.
        private Dependency createDependency()
        {
            int _dependentTask;
            int _dependsOnTask;
            Console.WriteLine("Enter dependent Task:");
            task_id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter depends on Task id:");
            prev_task_id = Convert.ToInt32(Console.ReadLine());
            Dependency newDpendency = new(_dependentTask, _dependsOnTask);
            return newDpendency
        }

        //engineer CRUD functions
        private void engineerCRUD()
        {
            int choice;
            do
            {
                Console.WriteLine("Please Enter your choice: 0 -To Exit, 1 -Create Engineer, 2 -Find Engineer," +
                             " 3 -Get list of Engineers, 4 -Update engineers information, 5 -Delete Engineer\n");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 0: break;
                    case 1:
                        createEngineer();
                        break;
                    case 2:
                        Console.WriteLine("Please enter Engineers Id\n");
                        Console.WriteLine(s_dalEngineer!.Read(Convert.ToInt32(Console.ReadLine())));
                        break;
                    case 3:
                        foreach (var item in s_dalEngineer!.ReadAll())
                            Console.WriteLine(item);
                        break;
                    case 4:
                        Console.WriteLine("Enter Engineer's id");
                        int _id = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(s_dalEngineer!.Read(_id)));
                        s_dalEngineer!.Update(createEngineer());
                        break;
                    case 5:
                        s_dalEngineer!.Delete(Convert.ToInt32(Console.ReadLine()));
                        break;
                    default:
                        Console.WriteLine("Wrong choice");
                        break;
                }
            } while (choice != 0);
        }
        //Task CRUD functions
        private void taskCRUD()
        {
            int choice;
            do
            {
                Console.WriteLine("Please Enter your choice: 0 -To Exit, 1 -Create Task, 2 -Find Task," +
                             " 3 -Get list of Tasks, 4 -Update Tasks information, 5 -Delete Task\n");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        break;
                    case 1:
                        s_dalEngineer!.Create(createTask()); 
                        break;
                    case 2:
                        Console.WriteLine("Enter task's id");
                        Console.WriteLine(s_dalTask!.Read(Convert.ToInt32(Console.ReadLine())));
                        break;
                    case 3:
                        foreach (var item in s_dalTask!.ReadAll())
                            Console.WriteLine(item);
                        break;
                    case 4:
                        Console.WriteLine("Enter task's id");
                        int _id = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(s_dalTask!.Read(_id)));
                        s_dalTask!.Update(createTask(_id));
                        break;
                    case 5:
                        s_dalTask!.Delete(Convert.ToInt32(Console.ReadLine()));
                        break;
                    default:
                        Console.WriteLine("Wrong choice");
                        break;
                }
            } while (choice != 0);
        }
        //dependency CRUD functions
        private void dependencyCRUD()
        {
            int choice;
            do
            {
                Console.WriteLine("Please Enter your choice: 0 -To Exit, 1 -Create dependency, 2 -Find dependency," +
                             " 3 -Get list of dependencies, 4 -Update dependencies information, 5 -Delete dependency\n");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        break;
                    case 1:
                        s_dalDependency!.Create(createDependency());
                        break;
                    case 2:
                        Console.WriteLine("Enter Dependency id");
                        s_dalDependence!.Read(Convert.ToInt32(Console.ReadLine()));
                        break;
                    case 3:
                        foreach (var item in s_dalDependency!.ReadAll())
                             Console.WriteLine($"id: {item.Id}  task id: {item.DependentTask}  pervious task id: {item.DependsOnTask}");
                        break;
                    case 4:
                        s_dalDependency!.Update(createDependency());
                        break;
                    case 5:
                        Console.WriteLine("Enter Dependency id");
                        s_dalDependency!.Delete(Convert.ToInt32(Console.ReadLine()));
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
                Initialization.Do(s_dalEngineer, s_dalTask, s_dalDependency);
                do
                {
                    Console.WriteLine("Please enter your choice: 0 -Exit, 1 -Engineer, 2 -Task, 3 -Dependency\n");
                    choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            engineerCRUD();
                            break;
                        case 2:
                            taskCRUD();
                            break;
                        case 3:
                            dependencyCRUD();
                            break;
                        default:break;
                    }
                }
                while (choice != 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}