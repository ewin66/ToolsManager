
namespace Atlante.Presentation.Interfaces
{
    public interface IExpanderContainer
    {
        string Header { get; }

        void AddItem( string Description, object value );
    }
}
