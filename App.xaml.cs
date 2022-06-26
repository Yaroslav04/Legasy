﻿using Legasy.Core.Model;

namespace Legasy;

public partial class App : Application
{
    static List<CaseClass> dataBase;
    public static List<CaseClass> DataBase
    {
        get
        {
            if (dataBase == null)
            {
                dataBase = new List<CaseClass>();
            }
            return dataBase;
        }
    }

    static List<string> workFolders;
    public static List<string> WorkFolders
    {
        get
        {
            if (workFolders == null)
            {
                workFolders = new List<string>
                {
                    "Вказівки",
                    "Постанови про скасування закриття",
                    "Підслідність"
                };
            }
            return workFolders;
        }
    }

    public static string GeneralPath = @"D:\LegasyDB\CriminalCases\";

    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}