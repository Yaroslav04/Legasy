using Legasy.Core.Servises;
using Legasy.Core.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.ConstrainedExecution;

namespace Legasy.Core.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {

        public MainPageViewModel()
        {
            ItemTappedSingle = new Command<CaseClass>(OnSingleItemTapped);
            ItemTappedDouble = new Command<CaseClass>(OnDoubleItemTapped);
            SearchCommand = new Command(Search);
            ClearCommand = new Command(Clear);
            EditCommand = new Command(Edit);
            DeleteCommand = new Command(Delete);
            ConsoleCommand = new Command(ConsoleInput);
            Items = new ObservableCollection<CaseClass>();
            QualificationsSearchPanel = new ObservableCollection<string>();
            WorkFolders = new ObservableCollection<string>(App.WorkFolders);

            Clear();
            FileServise.WorkFolderUpdate();
        }

        private async void ConsoleInput()
        {
            string result = await Shell.Current.DisplayPromptAsync($"Консоль", $"[delete old folders], [open main folder]");
            if (result == "delete old folders")
            {
                try
                {
                    FileServise.CleanOldFolders();
                    await Shell.Current.DisplayAlert("Консоль", "Выполнено", "OK");
                }
                catch
                {
                    await Shell.Current.DisplayAlert("Консоль", "Ошибка", "OK");
                }
            }
            if (result == "open main folder")
            {
                FileServise.OpenFolder(FileSystem.Current.AppDataDirectory);
            }
        }

        public void OnAppearing()
        {

        }

        #region Properties

        public ObservableCollection<CaseClass> Items { get; }

        public ObservableCollection<string> WorkFolders { get; }

        private string workFolder;
        public string WorkFolder
        {
            get => workFolder;
            set
            {
                SetProperty(ref workFolder, value);
            }
        }

        private string searchTextSearchPanel;
        public string SearchTextSearchPanel
        {
            get => searchTextSearchPanel;
            set
            {
                SetProperty(ref searchTextSearchPanel, value);
            }
        }

        public ObservableCollection<string> QualificationsSearchPanel { get; }

        private string selectedQualificationSearchPanel;
        public string SelectedQualificationSearchPanel
        {
            get => selectedQualificationSearchPanel;
            set
            {
                SetProperty(ref selectedQualificationSearchPanel, value);
            }
        }

        //Description

        private CaseClass itemDescription;
        public CaseClass ItemDescription
        {
            get => itemDescription;
            set
            {
                SetProperty(ref itemDescription, value);
            }
        }

        private string qualificationDescription;
        public string QualificationDescription
        {
            get => qualificationDescription;
            set
            {
                SetProperty(ref qualificationDescription, value);
            }
        }

        private string nameDescription;
        public string NameDescription
        {
            get => nameDescription;
            set
            {
                SetProperty(ref nameDescription, value);
            }
        }

        private string descriptionDescription;
        public string DescriptionDescription
        {
            get => descriptionDescription;
            set
            {
                SetProperty(ref descriptionDescription, value);
            }
        }

        #endregion

        #region Commands
        public Command<CaseClass> ItemTappedSingle { get; }
        public Command<CaseClass> ItemTappedDouble { get; }
        public Command SearchCommand { get; }
        public Command ClearCommand { get; }
        public Command EditCommand { get; }
        public Command DeleteCommand { get; }
        public Command ConsoleCommand { get; }

        #endregion

        #region Main

        private void OnDoubleItemTapped(CaseClass _item)
        {
            if (_item == null)
            {
                return;
            }
            FileServise.OpenFolder(_item.Path);
        }

        private void OnSingleItemTapped(CaseClass item)
        {
            if (item != null)
            {
                ItemDescription = item;
                NameDescription = item.Name;
                QualificationDescription = item.Decsription.Qualification;
                DescriptionDescription = item.Decsription.Header;
            }
        }

        private void Clear()
        {
            Items.Clear();
            DataBaseServise.DataBaseUpload();
            QualificationsSearchPanel.Clear();
            foreach (var item in App.QualificationsSearchPanel)
            {
                QualificationsSearchPanel.Add(item);
            }
            LoadAllItems();
            SearchTextSearchPanel = null;
            SelectedQualificationSearchPanel = null;
            NameDescription = null;
            QualificationDescription = null;
            DescriptionDescription = null;
            ItemDescription = null;
            WorkFolder = null;
        }

        private void LoadAllItems()
        {
            Items.Clear();
            var _list = App.DataBase;
            _list = _list.OrderBy(x => x.Decsription.Header).ToList();
            foreach (var item in _list)
            {
                Items.Add(item);
            }
        }

        private async void Search()
        {
            var result = new List<CaseClass>();

            if (App.DataBase.Count > 0)
            {
                result.AddRange(App.DataBase);
            }
            else
            {
                Clear();
                return;
            }

            if (SelectedQualificationSearchPanel != null)
            {
                var subresult = result.Where(x => x.Decsription.Qualification == SelectedQualificationSearchPanel).ToList();
                if (subresult.Count > 0)
                {
                    result.Clear();
                    result.AddRange(subresult);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Поиск", "Не найдено", "OK");
                    Clear();
                    return;
                }

            }

            if (WorkFolder != null)
            {
                var subresult = new List<CaseClass>();
                result = result.Where(x => x.Files.Count > 0).ToList();
                foreach (var item in result)
                {
                    foreach (var file in item.Files)
                    {
                        if (file.Type == WorkFolder)
                        {
                            subresult.Add(item);
                        }
                    }
                }
                if (subresult.Count > 0)
                {
                    subresult = subresult.Distinct().ToList();
                    result.Clear();
                    result.AddRange(subresult);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Поиск", "Не найдено", "OK");
                    Clear();
                    return;
                }
            }

            if (!String.IsNullOrWhiteSpace(SearchTextSearchPanel))
            {
                if (TextServise.IsNumberValid(SearchTextSearchPanel))
                {
                    if (!FileServise.IsFolderExist(Path.Combine(App.GeneralPath, SearchTextSearchPanel)))
                    {
                        var description = await NewElementPromtShell();
                        if (description != null)
                        {
                            FileServise.CreateNewDirectory(Path.Combine(App.GeneralPath, SearchTextSearchPanel));
                            FileServise.CreateNewDescription(SearchTextSearchPanel, description);
                            await Shell.Current.DisplayAlert("Регистрация нового элемента", "Зарегистритровано", "OK");
                            FileServise.OpenFolder(Path.Combine(App.GeneralPath, SearchTextSearchPanel));
                            Clear();
                            return;
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Регистрация нового элемента", "Ошибка", "OK");
                            Clear();
                            return;
                        }
                    }
                    else
                    {
                        result = result.Where(x => x.Name == SearchTextSearchPanel).ToList();
                    }
                }
                else
                {
                    if (SearchTextSearchPanel.Length > 2)
                    {
                        var subresult = result.Where(x => x.Decsription.Header.Contains(SearchTextSearchPanel, StringComparison.OrdinalIgnoreCase)).ToList();
                        Debug.WriteLine(subresult.Count);
                        if (subresult.Count > 0)
                        {
                            result.Clear();
                            result.AddRange(subresult);
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Поиск", "Не найдено", "OK");
                            Clear();
                            return;
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Поиск", "Поиск должен быть минимум 3 буквы", "OK");
                        Clear();
                        return;
                    }
                }
            }
            else
            {
                if (WorkFolder != null | SelectedQualificationSearchPanel != null)
                {
                   
                }
                else
                {
                    await Shell.Current.DisplayAlert("Поиск", "Поиск пуст, параметры не выбраны", "OK");
                    Clear();
                    return;
                }
            }

            if (result.Count > 0)
            {
                Items.Clear();
                foreach (var item in result)
                {
                    Items.Add(item);
                }
                return;
            }
            else
            {
                await Shell.Current.DisplayAlert("Поиск", "Не найдено", "OK");
                Clear();
                return;
            }
        }


        async Task<DescriptionClass> NewElementPromtShell()
        {
            DescriptionClass descriptionClass = new DescriptionClass();
            string _qualification = await Shell.Current.DisplayPromptAsync($"Добавить новое уголовное дело {SearchTextSearchPanel}", $"Введите квалификацию (обязательно):", maxLength: 3);
            string _header = await Shell.Current.DisplayPromptAsync($"Добавить новое уголовное дело {SearchTextSearchPanel}", $"Введите заголовок:", maxLength: 20);
            if (!String.IsNullOrEmpty(_qualification))
            {
                descriptionClass.Qualification = _qualification;
                if (!String.IsNullOrEmpty(_header))
                {
                    descriptionClass.Header = _header;
                }
                else
                {
                    descriptionClass.Header = null;
                }
                return descriptionClass;
            }
            else return null;
        }

        private void Edit()
        {
            if (ItemDescription != null)
            {
                Debug.WriteLine(ItemDescription.Name);
                DescriptionClass descriptionClass = new DescriptionClass();
                descriptionClass.Qualification = QualificationDescription;
                descriptionClass.Header = DescriptionDescription;
                FileServise.UpdateDescription(ItemDescription.Name, descriptionClass);
                Clear();
            }
        }

        private async void Delete()
        {
            if (ItemDescription != null)
            {
                string _ansver = await Shell.Current.DisplayPromptAsync($"Удалить папку {ItemDescription.Name} {ItemDescription.Decsription.Qualification} {ItemDescription.Decsription.Header}", $"Введите [удалить]:");
                if (_ansver == "удалить")
                {
                    FileServise.DeleteDirectory(ItemDescription.Name);
                    Clear();
                }
            }
        }
        #endregion
    }
}
