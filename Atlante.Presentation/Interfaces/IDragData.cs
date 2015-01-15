
namespace Atlante.Presentation.Interfaces
{
    public interface IDragData
    {
        object GetDragObject( );
        void SetDropObject( object data, int posX, int posY );
    }
}
