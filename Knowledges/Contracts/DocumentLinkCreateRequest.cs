namespace Contracts
{
    using System;

    public class DocumentLinkCreateRequest
    {
        public Guid DocumentId { get; set; }

        public Guid LyfeCycleTemplateId { get; set; }
    }
}
