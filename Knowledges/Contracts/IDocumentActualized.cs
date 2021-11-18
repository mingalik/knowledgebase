using System;

namespace Contracts
{
    public interface IDocumentActualized
    {
        Guid DocumentId { get; set; }

        bool IsActual { get; set; }
    }
}