using System;
using MongoDB.Bson.Serialization.Attributes;
using Simplic.Data.NoSql;

namespace Simplic.Data.MongoDB
{
    [BsonIgnoreExtraElements]
    public class BaseDocument : IDocument<Guid>
    {
        [BsonId]
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
