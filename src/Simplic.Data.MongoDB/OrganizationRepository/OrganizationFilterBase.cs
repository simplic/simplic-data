using System;
using System.Collections.Generic;
using Simplic.Data.NoSql;

namespace Simplic.Data.MongoDB
{
    /// <summary>
    /// Organization data filter
    /// </summary>
    public class OrganizationFilterBase : IFilter<Guid>
    {
        /// <summary>
        /// Gets or sets the data id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the organization id
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets whether to filter deleted data
        /// </summary>
        public bool? IsDeleted { get; set; } = false;

        /// <summary>
        /// Gets or sets wether all organisations will be queried.
        /// </summary>
        public bool QueryAllOrganizations { get; set; } = false;

        /// <summary>
        /// Gets or sets a list of included ids.
        /// </summary>
        public IList<Guid> IncludeIds { get; set; }

        /// <summary>
        /// Gets or sets an exclude id.
        /// </summary>
        public Guid? ExcludeId { get; set; }
    }
}
