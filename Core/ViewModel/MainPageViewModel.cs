using Legasy.Core.Servises;
using Legasy.Core.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legasy.Core.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {

        public MainPageViewModel()
        {
            DataBaseServise.DataBaseInit();
            foreach(var item in App.DataBase)
            {
                Debug.WriteLine($"{item.Name} {item.Files.Count}");
            }
        }
    }
}
