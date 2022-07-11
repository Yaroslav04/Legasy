using Legasy.Core.Model;
using Legasy.Core.Servises;

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
        try
        {
            using (StreamReader sr = new StreamReader(Path.Combine(FileSystem.Current.AppDataDirectory, "work_folders.ini")))
            {
                text = sr.ReadToEnd();
            }
        }
        catch
        {
            FileServise.OpenFolder(FileSystem.Current.AppDataDirectory);
            return null;
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


    //public static string GeneralPath = @"D:\LegasyDB\CriminalCases\";
    private static string generalPath = null;
    public static string GeneralPath
    {
        get
        {
            if (generalPath == null)
            {
                generalPath = GetGeneralPath();
            }
            return generalPath;
        }
    }

    private static string GetGeneralPath()
    {
        string path = null;
        try
        {
            using (StreamReader sr = new StreamReader(Path.Combine(FileSystem.Current.AppDataDirectory, "path.ini")))
            {
                path = sr.ReadToEnd();
            }
            return path;
        }
        catch
        {
            FileServise.OpenFolder(FileSystem.Current.AppDataDirectory);
            return null;
        }
        
    }


    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
