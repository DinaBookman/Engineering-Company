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
        private static void EngineerCRUD()
        {
            int choice;
            do
            {
                Console.WriteLine("Please Enter your choice: 0 -To Exit, 1 -Create Engineer, 2 -Find Engineer," +
                             " 3 -Get list of Engineers, 4 -Update engineers information, 5 -Delete Engineer\n");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Please enter Engineers Id\n");
                        int _id = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Please enter Engineers Name\n");
                        string _name = Console.ReadLine()!;
                        Console.WriteLine("Please enter Engineers Email\n");
                        string _email = Console.ReadLine()!;
                        Console.WriteLine("Please enter Engineers Engineer Experience\n");
                        int _level = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Please enter Engineers wage\n");
                        int _wage = Convert.ToInt32(Console.ReadLine());
                        Engineer newEngineer = new(_id, _name, _email, (EngineerExperience)_level, _wage);
                        s_dalEngineer!.Create(newEngineer);
                        break;
                    case 2:
                        Console.WriteLine("Please enter Engineers Id\n");
                        _id = int.TryParse(Console.ReadLine());
                        newEngineer = s_dalEngineer!.Read(_id);
                        
                        break;
                    case 3:
                        
                        break;
                        break;
                    default: break;
                }
            } while (choice != 0);
           
        }
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
                            EngineerCRUD();
                            break;
                        case 2: break;
                        case 3: break;
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
       
     
