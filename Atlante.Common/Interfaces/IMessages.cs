using System;
using System.Collections;

namespace Atlante.Common
{
    public interface IMessages : IEnumerable
    {
        bool HasErrors { get; }

        void AddException( Exception e );
        void AddWarning( string description );
        void AddInformation( string description );
    }
}
