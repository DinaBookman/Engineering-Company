namespace DalTest;
using DalApi;
using DO;
using Dal;
using System.Globalization;
using System.Transactions;
using System.Runtime.InteropServices;
internal class Program
{
    static readonly IDal? s_dal = new DalList(); //stage 2

    /// <summary>
    /// function to create an engineer.
    /// </summary>
    /// <returns></returns>
    private static Engineer CreateEngineer()
    {
        Console.WriteLine("Please enter Engineers Id:");
        int id = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Please enter Engineers Name:");
        string name = Console.ReadLine()!;
        Console.WriteLine("Please enter Engineers Email:");
        string email = Console.ReadLine()!;
        Console.WriteLine("Please enter Engineer Experience (0 - 4):");
        int level = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Please enter Engineers cost:");
        int cost = Convert.ToInt32(Console.ReadLine());
        Engineer newEngineer = new(id, name, email, (EngineerExperience)level, cost);
        return newEngineer;
    }

    /// <summary>
    /// function to create an task.
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    private static DO.Task CreateTask(int id = 0000)
    {
        Console.WriteLine("Please enter a description of the task:");
        string description = Console.ReadLine()!;
        Console.WriteLine("Please enter tasks alias:");
        string alias = Console.ReadLine()!;
        Console.WriteLine("Please enter Engineers id:");
        int engineerId = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Please enter complexity level (0 - 4):");
        int level = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Please enter a comment:");
        string comment = Console.ReadLine()!;
        DateTime createdDate = DateTime.Now;
        DateTime startDate = createdDate.Add(TimeSpan.FromDays(1));
        DateTime endDate = createdDate.Add(TimeSpan.FromDays(30));
        DateTime Deadline = createdDate.Add(TimeSpan.FromDays(15));
        DateTime estDateCompletion = endDate.Add(TimeSpan.FromDays(-3));
        DO.Task newTask = new(id, description, alias, createdDate, startDate, endDate, Deadline, estDateCompletion, engineerId, (EngineerExperience)level, false, comment);
        return newTask;
    }

    /// <summary>
    /// function to create an Dependency.
    /// </summary>
    /// <returns></returns>
    private static Dependency CreateDependency()
    {
        int dependentTask;
        int dependsOnTask;
        Console.WriteLine("Enter dependent Task:");
        dependentTask = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter depends on Task id:");
        dependsOnTask = Convert.ToInt32(Console.ReadLine());
        Dependency newDpendency = new(0000, dependentTask, dependsOnTask);
        return newDpendency;
    }

    /// <summary>
    /// Updating the details of an existing task according to its id number
    /// </summary>
    private static void updateTask()
    {
        Console.WriteLine("Enter task's id:");
        int id = Convert.ToInt32(Console.ReadLine());
        Task? returnTask = s_dal!.Task.Read(x => x.Id == id);
        if (returnTask is null)
            throw new DalDoesNotExistException("Task with such id does not exist");
        Console.WriteLine(returnTask);
        Console.WriteLine("Enter task's details to update, if you don't want to change the detail press enter");
        string? description, alias, comment, engineerIdString, timeString, level;
        int? engineerId;
        DateTime? startTime, estimatedEndTime, deadLine;
        EngineerExperience difficultyLevel;
        Console.WriteLine("Enter task's description:");
        description = Console.ReadLine();
        if (string.IsNullOrEmpty(description))
            description = returnTask.Description;
        Console.WriteLine("Enter task's alias:");
        alias = Console.ReadLine();
        if (string.IsNullOrEmpty(alias))
            alias = returnTask.Alias;
        Console.WriteLine("Enter task's comments (if there are any) :");
        comment = Console.ReadLine();
        if (string.IsNullOrEmpty(comment))
            comment = returnTask.Remarks;
        Console.WriteLine("Enter the ID number of the engineer responsible for the task:");
        engineerIdString = Console.ReadLine();
        if (string.IsNullOrEmpty(engineerIdString))
            engineerId = returnTask.EngineerId;
        else
            engineerId = int.Parse(engineerIdString);
        Console.WriteLine("Enter task's start time in dd/mm/yyyy format:");
        timeString = Console.ReadLine();
        if (string.IsNullOrEmpty(timeString))
            startTime = returnTask.StartDate;
        else
            startTime = DateTime.Parse(timeString);
        Console.WriteLine("Enter the estimated time for finishing the task in dd/mm/yyyy format:");
        timeString = Console.ReadLine();
        if (string.IsNullOrEmpty(timeString))
            estimatedEndTime = returnTask.ScheduledDate;
        else
            estimatedEndTime = DateTime.Parse(timeString);
        Console.WriteLine("Enter the deadline for the task:");
        timeString = Console.ReadLine();
        if (string.IsNullOrEmpty(timeString))
            deadLine = returnTask.DeadlineDate;
        else
            deadLine = DateTime.Parse(timeString);
        Console.WriteLine("Please enter complexity level (0 - 4):");
        level = Console.ReadLine();
        if (string.IsNullOrEmpty(level))
            difficultyLevel = returnTask.CopmlexityLevel;
        else
            difficultyLevel = Enum.Parse<EngineerExperience>(level);
        Task newTask = new(id, description, alias, returnTask.CreatedAtDate, startTime, estimatedEndTime, deadLine, null, engineerId, difficultyLevel, returnTask.IsMilestone, comment);
        s_dal!.Task.Update(newTask);
    }

    /// <summary>
    /// Updating the details of an existing engineer according to its id number
    /// </summary>
    private static void updateEngineer()
    {
        string? name, email, rank, costString;
        int id;
        double cost;
        EngineerExperience engineerLevel;
        Console.WriteLine("Enter engineer's id:");
        id = Convert.ToInt32(Console.ReadLine());
        Engineer? returnEngineer = s_dal!.Engineer.Read(x => x.Id == id);
        if (returnEngineer is null)
            throw new DalDoesNotExistException("Engineer with such id does not exist");
        Console.WriteLine(returnEngineer);
        Console.WriteLine("Enter engineer's details you want to update, if you don't want to change the detail press enter:");
        Console.WriteLine("Enter engineer's name:");
        name = Console.ReadLine();
        if (string.IsNullOrEmpty(name))
            name = returnEngineer.Name;
        Console.WriteLine("Enter engineer's email:");
        email = Console.ReadLine();
        if (string.IsNullOrEmpty(email))
            email = returnEngineer.Email;
        Console.WriteLine("Enter engineer's salary:");
        costString = Console.ReadLine();
        if (string.IsNullOrEmpty(costString))
            cost = returnEngineer.Cost;
        else
            cost = double.Parse(costString);
        Console.WriteLine("Please enter complexity level (0 - 4):");
        rank = Console.ReadLine();
        if (string.IsNullOrEmpty(rank))
            engineerLevel = returnEngineer.Level;
        else
            engineerLevel = Enum.Parse<EngineerExperience>(rank);
        Engineer newEngineer = new(id, name, email, engineerLevel, cost);
        s_dal!.Engineer.Update(newEngineer);
    }

    /// <summary>
    /// Updating the details of an existing dependency according to its id number
    /// </summary>
    private static void updateDependency()
    {
        string? sDependentTask, sDependsOnTask;
        int id;
        int? dependentTask, dependsOnTask;
        Console.WriteLine("Enter the dependency's id:");
        id = Convert.ToInt32(Console.ReadLine());
        Dependency? returnDependency = s_dal!.Dependency.Read(x => x.Id == id);
        if (returnDependency == null)
            throw new DalDoesNotExistException("Engineer with such id does not exist");
        Console.WriteLine(returnDependency);
        Console.WriteLine("Enter the dependency's details, if you don't want to change the detail press enter:");
        Console.WriteLine("Enter the dependent task's id:");
        sDependentTask = Console.ReadLine();
        if (string.IsNullOrEmpty(sDependentTask))
            dependentTask = returnDependency.DependentTask;
        else
            dependentTask = int.Parse(sDependentTask);
        Console.WriteLine("Enter the dependent-on task's id:");
        sDependsOnTask = Console.ReadLine();
        if (string.IsNullOrEmpty(sDependsOnTask))
            dependsOnTask = returnDependency.DependsOnTask;
        else
            dependsOnTask = int.Parse(sDependsOnTask);
        Dependency newDependency = new(id, dependentTask, dependsOnTask);
        s_dal!.Dependency.Update(newDependency);
    }

    /// <summary>
    ///engineer CRUD functions
    /// </summary>
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
                    updateEngineer();
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

    /// <summary>
    /// Task CRUD functions
    /// </summary>
    private static void taskCRUD()
    {
        int choice;
        do
        {
            Console.WriteLine("Please Enter your choice: 0 -To Exit, 1 -Create Task, 2 -Find Task," +
                " 3 -Get list of Tasks, 4 -Update Tasks information, 5 -Delete Task");
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
                    updateTask();
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

    /// <summary>
    /// dependency CRUD functions
    /// </summary>
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
                    foreach (var dependency in s_dal!.Dependency.ReadAll())
                        Console.WriteLine($"id: {dependency!.Id}  task id: {dependency.DependentTask}  pervious task id: {dependency.DependsOnTask}");
                    break;
                case 4:
                    updateDependency();
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

    /// <summary>
    /// Main function
    /// </summary>
    /// <param name="args"></param>
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