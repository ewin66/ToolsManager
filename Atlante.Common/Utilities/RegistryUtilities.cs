using Microsoft.Win32;

namespace Atlante.Common
{
    public static class RegistryUtilities
    {
        public static string ReadDefaultValue( string keyName )
        {
            return Registry.GetValue( keyName, string.Empty, string.Empty ).ToString( );
        }

        public static string ReadDefaultValue( string keyName, string valueName )
        {
            return Registry.GetValue( keyName, valueName, string.Empty ).ToString( );
        }
    }
}
