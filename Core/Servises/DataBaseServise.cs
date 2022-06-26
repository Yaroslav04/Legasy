using Legasy.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legasy.Core.Servises
{
    public static class DataBaseServise
    {

        public static void DataBaseUpload()
        {
            App.DataBase.Clear();
            foreach (var folder in Directory.GetDirectories(App.GeneralPath))
            {
                CaseClass caseClass = new CaseClass();
                caseClass.Name = folder.Replace(App.GeneralPath, "").Replace("\\", "");
                caseClass.Path = folder;
                caseClass.Decsription = FileServise.GetDescriptionFromCase(folder);
                caseClass.Files = FileServise.GetFilesFromCase(folder);
                App.DataBase.Add(caseClass);
            }
        }
    }
}

