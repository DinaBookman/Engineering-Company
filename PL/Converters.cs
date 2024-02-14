using System;
using System.Globalization;
using System.Windows.Data;
using System.Collections;
using System.Collections.Generic;

namespace PL
{
    // This class implements the IValueConverter interface to convert an ID value to display content.
    internal class ConvertIdToContent : IValueConverter
    {
        // Converts the input ID value to corresponding display content.
        // In this case, it returns "Add" if the ID is 0, otherwise "Update".
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 0 ? "Add" : "Update";
        }
    }
}

