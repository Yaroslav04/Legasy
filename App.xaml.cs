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
                workFolders = new List<string>
                {
                    "Вказівки",
                    "Постанови про скасування закриття",
                    "Підслідність",
                    "Допит",
                    "Обшук",
                    "Продовження строку",
                    "Апеляція",
                    "Апеляція досудове розслідування",
                    "Службове розслідування",
                    "Привід",
                    "Вирок",
                    "Тимчасовий доступ",
                    "Арешт майна",
                    "Судовий розгляд",
                    "Запобіжний захід"
                };
            }
            workFolders.Sort();
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
