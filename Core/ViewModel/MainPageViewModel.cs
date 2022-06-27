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
            Items = new ObservableCollection<CaseClass>();
            DataBaseServise.DataBaseUpload();
            LoadAllItems();
            QualificationsSearchPanel = new ObservableCollection<string>();
            for (int i = 109; i < 447; i++)
            {
                QualificationsSearchPanel.Add(i.ToString());
            }

            FileServise.WorkFolderUpdate();
        }

        public void OnAppearing()
        {

        }

        #region Properties

        public ObservableCollection<CaseClass> Items { get; }

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
        #endregion

        #region Commands
        public Command<CaseClass> ItemTappedSingle { get; }
        public Command<CaseClass> ItemTappedDouble { get; }
        public Command SearchCommand { get; }
        public Command ClearCommand { get; }

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

        private void OnSingleItemTapped(CaseClass obj)
        {

        }

        private void Clear()
        {
            Items.Clear();
            DataBaseServise.DataBaseUpload();
            LoadAllItems();
            SearchTextSearchPanel = null;
            SelectedQualificationSearchPanel = null;
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
                        result = result.Where(x => x.Decsription.Header == SearchTextSearchPanel).ToList();
                    }
                }
            }
       
            Items.Clear();
            foreach (var item in result)
            {
                Items.Add(item);
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

        #endregion
    }
}
