using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Windows.UI.Xaml.Data;
namespace App5
{
    class IntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string input = value as string;
            int retValue;
            bool result = int.TryParse(input, out retValue);
            Debug.WriteLine("result is:" + result);

            if (!result)
            {
                retValue = -1;
            }
            return retValue;
        }
    }
}
