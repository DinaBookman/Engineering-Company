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

    private static IDal? s_dal;//stage 2

    private static readonly Random s_rand = new();

    private static void createEngineers()
    {
        string[] engineerNames =
        {
        "Dani Levi" ,
        "Eli Amar",
        "Yair Cohen",
        "Ariela Levin",
        "Diana Klein",
        };
        foreach (var _name in engineerNames)
        {
            //Creates random ids.
            int _id;
            do
                _id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dal!.Engineer!.Read(_id) != null);

            //Creates new Engineer. 
            Engineer newEngineer = new(_id, _name, $"{_id}@gmail.com", (EngineerExperience)1, 200.35);
            s_dal!.Engineer!.Create(newEngineer);
        }
    }

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
            //Date of creation - random date within the recent year.
            int range = s_rand.Next(-365, 0); //1 year
            DateTime _createdAtDate = DateTime.Today.AddDays(range);

            //chooses random task level.
            int _level;
            _level = s_rand.Next(LOW_LEVEL, HIGH_LEVEL);

            //Creates new Task.
            Task newTask = new(0000, _description, null, _createdAtDate, null, null, null, null, null, (EngineerExperience)_level, false,null);
            s_dal!.Task!.Create(newTask);
        }
    }
   
    private static void createDependencies()
    {
        //Create dependencies between Tasks. Every Task with Id 1016 - 1020 dependes of Tasks with Id 1000 - 1010.
        for(int i = 1016; i < 1020; i++)
        {
            for (int j = 1000; j < 1010; j++)
            {
                Dependency newDependency = new(0000, i, j);
                s_dal!.Dependency!.Create(newDependency);
            }
        } 
    }

    public static void Do(IDal dal)
    {
        s_dal = dal ?? throw new NullReferenceException("DAL can not be null!");
        createTasks();
        createEngineers();
        createDependencies();
    }
}