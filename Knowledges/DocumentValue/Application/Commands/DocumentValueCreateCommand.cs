namespace DocumentValueWebApi.Application.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using MediatR;
    
    [DataContract]
    public class DocumentValueCreateCommand : IRequest<bool>
    {
        public DocumentValueCreateCommand() => ObjectsIdsWithValuesDictionary = new Dictionary<Guid, Guid>();

        public DocumentValueCreateCommand(Guid documentId, string name, Guid? periodValue, Guid? momentValue, Dictionary<Guid, Guid> objectsIdsWithValuesDictionary)
        {
            Name = name;
            DocumentId = documentId;
            PeriodValue = periodValue;
            MomentValue = momentValue;
            ObjectsIdsWithValuesDictionary = objectsIdsWithValuesDictionary;
        }

        [DataMember]
        public Guid DocumentId { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public Guid? PeriodValue { get; private set; }

        [DataMember]
        public Guid? MomentValue { get; private set; }

        [DataMember]
        public Dictionary<Guid, Guid> ObjectsIdsWithValuesDictionary { get; private set; }
    }
}
