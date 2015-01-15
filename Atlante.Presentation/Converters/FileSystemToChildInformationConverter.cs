using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.IO;

namespace Atlante.Presentation.Converters
{
    public class FileSystemToChildInformationConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            var directories = new List<DirectoryInfo>( );

            if ( value == null )
                return directories;

            try
            {
                DirectoryInfo directoryInfo;

                if ( value is DriveInfo )
                    directoryInfo = new DirectoryInfo( ( value as DriveInfo ).RootDirectory.Name );
                else
                    directoryInfo = value as DirectoryInfo;

                foreach ( var directory in directoryInfo.GetDirectories( ) )
                    directories.Add( directory );
            }
            catch ( Exception ) { }

            return directories;
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException( );
        }
    }
}
