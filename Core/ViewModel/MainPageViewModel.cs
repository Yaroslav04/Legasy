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
            Items = new ObservableCollection<CaseClass>();
            QualificationsSearchPanel = new ObservableCollection<string>();
            for (int i = 109; i < 447; i++)
            {
                QualificationsSearchPanel.Add(i.ToString());
            }

            WorkFolders = new ObservableCollection<string>(App.WorkFolders);

            Clear();       
            FileServise.WorkFolderUpdate();
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
            foreach (var item in App.DataBase)
            {
                Items.Add(item);
            }
        }

        private async void Search(object obj)
        {
            var result = new List<CaseClass>();
            result.AddRange(App.DataBase);

            if (!String.IsNullOrEmpty(SearchTextSearchPanel))
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
                            Clear();
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Регистрация нового элемента", "Ошибка", "OK");
                            Clear();
                        }

                        return;
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
                            Clear();
                        }
                    }
                }
            }
            Debug.WriteLine(result.Count);

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
                    Clear();
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
                    Clear();
                }
            }
            Debug.WriteLine(result.Count);
            
            if (result.Count > 0)
            {
                Items.Clear();
                foreach (var item in result)
                {
                    Items.Add(item);
                }
            }
            else
            {
                Clear();
            }
        }


        async Task<DescriptionClass> NewElementPromtShell()
        {
            DescriptionClass descriptionClass = new DescriptionClass();
            string _qualification = await Shell.Current.DisplayPromptAsync($"Добавить новое уголовное дело {SearchTextSearchPanel}", $"Введите квалификацию (обязательно):", maxLength: 3);
            string _header = await Shell.Current.DisplayPromptAsync($"Добавить новое уголовное дело {SearchTextSearchPanel}", $"Введите заголовок:", maxLength: 16);
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
                string _ansver = await Shell.Current.DisplayPromptAsync($"Удалить папку {ItemDescription.Name} {ItemDescription.Decsription.Qualification} {ItemDescription.Decsription.Header}", $"Введите -удалить-:");
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
