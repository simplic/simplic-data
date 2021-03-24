using System;
using System.Collections.Generic;
using Simplic.Data.NoSql;

namespace Simplic.Data.MongoDB
{
    public class BaseFilter : IFilter<Guid>
    {
        public Guid Id { get; set; }
        public Guid? OrganizationId { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public bool QueryAllOrganizations { get; set; } = false;
        public IList<Guid> IncludeIds { get; set; }
        public Guid? ExcludeId { get; set; }
    }
}
