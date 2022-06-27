using Legasy.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Legasy.Core.Servises
{
    public static class TextServise
    {

        public static DescriptionClass ConvertTextToDescription(string _text)
        {
            DescriptionClass _descriptionClass = new DescriptionClass();
            var array = _text.Split("\t");
            if (array.Length >= 2)
            {
                _descriptionClass.Header = array[0];
                _descriptionClass.Qualification = array[1];
            }
            return _descriptionClass;
        }

        public static string ConvertDescriptionToString(DescriptionClass _description)
        {
            return $"{_description.Header}\t{_description.Qualification}";
        }

        public static bool IsNumberValid(string _value)
        {
            return Regex.IsMatch(_value, @"^\d\d\d\d\d\d\d\d\d\d\d\d\d\d\d\d\d");
        }
    }
}
