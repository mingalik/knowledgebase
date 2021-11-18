using System;

namespace Contracts
{
    public class DocumentActualized : IDocumentActualized
    {
        public Guid DocumentId { get; set; }

        public bool IsActual { get; set; }
    }
}
