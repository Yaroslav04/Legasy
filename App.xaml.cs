using Legasy.Core.Model;

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

    static List<string> qualificationsSearchPanel;

    public static List<string> QualificationsSearchPanel
    {
        get
        {
            if (qualificationsSearchPanel == null)
            {
                qualificationsSearchPanel = new List<string>();
            }
            return qualificationsSearchPanel;
        }
    }

    static List<string> workFolders;
    public static List<string> WorkFolders
    {
        get
        {
            if (workFolders == null)
            {
                workFolders = GetWorkFolders();
            }

            return workFolders;
        }
    }

    private static List<string> GetWorkFolders()
    {
        List<string> result = new List<string>();
        string text = "";
        using (StreamReader sr = new StreamReader(Path.Combine(GeneralPath, "work_folders.ini")))
        {
            text = sr.ReadToEnd();
        }

        foreach (var item in text.Split("\n"))
        {
            if (!String.IsNullOrWhiteSpace(item))
            {
                result.Add(item.Replace("\r", ""));
            }
        }

        return result;
    }

    public static string GeneralPath = @"D:\LegasyDB\CriminalCases\";


    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
