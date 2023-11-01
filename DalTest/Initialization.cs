namespace DalTest;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Xml.Linq;

public static class Initialization
{
    //Used to create random id.
    const int MIN_ID = 200000000, MAX_ID = 400000000;
    //Used to create random level.
    const int LOW_LEVEL = 0, HIGH_LEVEL = 4;

    private static ITask? s_dalTask;
    private static IEngineer? s_dalEngineer;
    private static IDependency? s_dalDependency;
    private static readonly Random s_rand = new();
    private static void createTasks() 
    {
        string[] TaskDescriptions =
        {
        "Create Ilustration for Project 87364",
        "Create Projects indentation",
        "Finalize Project",
        "Create DataBase Project",
        "Create css files for Project 83746",
        "Add Enum to Project i3764",
        "Add Implementation to file gfyeu.cs",
        "Create Ilustration for Project 87364",
        "Create Project 82674hdi indentation",
        "Upload project to web",
        "Create History Project",
        "Create css files for Project mduh36",
        "Add Enum to File fdg.fdg",
        "Add Implementation to file osdbf.cs",
        "Create Ilustration for Project 87364",
        "Create Projects indentation",
        "Finalize Project",
        "Update Project fcgh546",
        "Update Data in file fhu8476.txt",
        "Add Enum to Project i3764"
        };

        foreach (var _description in TaskDescriptions)
        {
            //All tasks are repeated 5 times with different details.
            for (int i = 0; i < 5; i++)
            { 
                //Date of creation - random date within the recent year.
                int range = s_rand.Next(-365, 0); //1 year
                DateTime _createdAtDate = DateTime.Today.AddDays(range);

                //chooses random date in the next five years range for scheduledDate and then the forecastDate is 20 days after.
                range = s_rand.Next(0, 365 * 2); //5 years  
                DateTime _scheduledDate = DateTime.Today.AddDays(range);
                DateTime _forecastDate = _scheduledDate.AddDays(20);

                //chooses random task level.
                int _level;
                _level = s_rand.Next(LOW_LEVEL, HIGH_LEVEL);

                Task newTask = new(0000, _description, null, _createdAtDate, DateTime.Now, _scheduledDate, _forecastDate, null, (EngineerExperience)_level, false);
                s_dalTask!.Create(newTask);
            }
        }

    }
    private static void createEngineers()
    {
        string[] engineerNames =
        {
        "Dani Levi" ,
        "Eli Amar",
        "Yair Cohen",
        "Ariela Levin",
        "Dina Klein",
        "Shira Pliskin",
        "Sara Cohen",
        "Naama Polak",
        "Dina Levinson",
        "Ayala Meir",
        "Avi Berlin",
        "Yael Berlin",
        "Shira Gray",
        "Chavi Safra",
        "Chavi Naftali",
        "Chara Shlomo",
        "Shlomo Ellinson",
        "Dani Lev",
        "Eli Amarti",
        "Yair Plisk",
        "Ariela Dissin",
        "Dina Klowy",
        "Shira Henner",
        "Sara Elner",
        "Rivky Laofer",
        "Malkiel Dash",
        "BatTzion Ray",
        "Avi Hofman",
        "Yael Malkiel",
        "Dassi Bruan",
        "Leah shitrit",
        "Sari Chefetz",
        "Esti Shpigel",
        "Tzvi Kaplan",
        "Lally Shprintza",
        "Shoshanat Ha'amakim",
        "Chavatzelet Ha'sharon",
        "Yaeli Ellinson",
        "Yossi Adler",
        "Plonit Almonit"
        };
        foreach (var _name in engineerNames)
        {
            //creates random ids.
            int _id;
            do
               _id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dalEngineer!.Read(_id) != null);

            Engineer newEngineer = new(_id, _name, $"{_id}@gmail.com", (EngineerExperience)1, 12000);
            s_dalEngineer!.Create(newEngineer);
        } 
    }
    private static void createDependencies()
    {
        Dependency newDependency = new(0000, null, null);
        s_dalDependency!.Create(newDependency);
    }

    public static void Do(IEngineer? dalEngineer, ITask? dalTask, IDependency? dalDependency)
    {
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        createTasks();
        createEngineers();
        createDependencies();
    }
}