using System.Windows.Media;

namespace Atlante.Mef.Interfaces
{
    public interface IApplicationInfo
    {
        string ApplicationVersion { get; }
        string ApplicationName { get; }
        string ApplicationDescription { get; }
        string ApplicationCopyright { get; }

        ImageSource AppIcon { get; }        
    }
}
