using Legasy.Core.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legasy.Core.Servises
{
    public static class FileServise
    {
        public static List<FileClass> GetFilesFromCase(string _path)
        {
            List<FileClass> files = new List<FileClass>();

            foreach (var directory in Directory.GetDirectories(_path))
            {
                if (directory.Replace(_path, "") == @"\Data")
                {
                    foreach (var headerDirectory in Directory.GetDirectories(directory))
                    {
                        foreach (var file in Directory.GetFiles(headerDirectory))
                        {
                            var fileInfo = new FileInfo(file);
                            FileClass fileClass = new FileClass();
                            fileClass.Path = file;
                            fileClass.Name = fileInfo.Name;
                            fileClass.Size = Convert.ToInt32(fileInfo.Length / 1000);
                            fileClass.UpdateTime = Convert.ToDateTime(fileInfo.LastWriteTime);
                            fileClass.Type = headerDirectory.Replace(directory, "").Replace("\\", "");
                            files.Add(fileClass);
                        }
                    }
                }
                else
                {
                    foreach (var file in Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories))
                    {
                        var fileInfo = new FileInfo(file);
                        FileClass fileClass = new FileClass();
                        fileClass.Path = file;
                        fileClass.Name = fileInfo.Name;
                        fileClass.Size = Convert.ToInt32(fileInfo.Length / 1000000);
                        fileClass.UpdateTime = Convert.ToDateTime(fileInfo.LastWriteTime);
                        fileClass.Type = "Другое";
                        files.Add(fileClass);
                    }
                }
            }

            foreach (var file in Directory.GetFiles(_path))
            {
                var fileInfo = new FileInfo(file);
                FileClass fileClass = new FileClass();
                fileClass.Path = file;
                fileClass.Name = fileInfo.Name;
                fileClass.Size = Convert.ToInt32(fileInfo.Length / 1000000);
                fileClass.UpdateTime = Convert.ToDateTime(fileInfo.LastWriteTime);
                fileClass.Type = "Другое";
                files.Add(fileClass);
            }

            return files;
        }

        public static DescriptionClass GetDescriptionFromCase(string _path)
        {
            DescriptionClass descriptionClass = new DescriptionClass();

            foreach (var directory in Directory.GetDirectories(_path))
            {
                if (directory.Replace(_path, "") == @"\Data")
                {
                    if (File.Exists(Path.Combine(_path, "Data", "description.ini")))
                    {
                        using (StreamReader sr = new StreamReader(Path.Combine(_path, "Data", "description.ini")))
                        {
                            return TextServise.ConvertTextToDescription(sr.ReadToEnd());
                        }
                    }
                }
                else
                {
                    return descriptionClass;
                }
            }

            return null;
        }

        public static void OpenFolder(string _path)
        {
            Process.Start("explorer.exe", _path);
        }

        public static bool IsFolderExist(string _name)
        {
            return Directory.Exists(Path.Combine(App.GeneralPath, _name));
        }

        public static void CreateNewDirectory(string _name)
        {
            if (!Directory.Exists(Path.Combine(App.GeneralPath, _name)))
            {
                try
                {
                    Directory.CreateDirectory(Path.Combine(App.GeneralPath, _name));
                    Directory.CreateDirectory(Path.Combine(App.GeneralPath, _name, "Data"));
                    foreach (var workFolder in App.WorkFolders)
                    {
                        Directory.CreateDirectory(Path.Combine(App.GeneralPath, _name, "Data", workFolder));
                    }
                }
                catch { }
            }
        }

        public static void CreateNewDescription(string _name, DescriptionClass _description)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(App.GeneralPath, _name, @"Data\description.ini")))
                {
                    sw.Write(TextServise.ConvertDescriptionToString(_description));
                }
            }
            catch { }
        }

        public static bool UpdateDescription(string _name, DescriptionClass _description)
        {
            
            try
            {
                File.Delete(Path.Combine(App.GeneralPath, _name, @"Data\description.ini"));
                using (StreamWriter sw = new StreamWriter(Path.Combine(App.GeneralPath, _name, @"Data\description.ini")))
                {
                    sw.Write(TextServise.ConvertDescriptionToString(_description));
                }
                return true;
            }
            catch 
            { 
            return false;
            }
        }

        public static void WorkFolderUpdate()
        {
            foreach (var item in Directory.GetDirectories(App.GeneralPath))
            {
                foreach (var directory in Directory.GetDirectories(item))
                {             
                    if (directory.Replace(item, "") == @"\Data")
                    {                   
                        foreach (var workFolder in App.WorkFolders)
                        {
                            if (!Directory.Exists(Path.Combine(directory, workFolder)))
                            {                              
                                Directory.CreateDirectory(Path.Combine(directory, workFolder));
                            }
                        }                     
                    }
                }
            }
        }

        public static void Clean()
        {
            //удалить пустые папки
            //foreach (var item in Directory.GetDirectories(App.GeneralPath))
            //{
            //    foreach (var directory in Directory.GetDirectories(item))
            //    {
            //        if (Directory.GetFiles(directory).Length == 0)
            //        {
            //            Directory.Delete(directory, true);
            //        }
            //    }
            //}



            
        }
    }
}

