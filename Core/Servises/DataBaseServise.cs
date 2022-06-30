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

            List<string> subresult = new List<string>();
            foreach (var item in App.DataBase)
            {
                
                if(item.Decsription.Qualification != null)
                {
                    subresult.Add(item.Decsription.Qualification);
                }
            }

            if (subresult.Count > 0)
            {
                subresult = subresult.Distinct().ToList();
                subresult.Sort();
            }

            App.QualificationsSearchPanel.Clear();
            App.QualificationsSearchPanel.AddRange(subresult);
        }
    }
}

