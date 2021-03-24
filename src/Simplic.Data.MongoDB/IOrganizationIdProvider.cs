using System;
using Simplic.Data.NoSql;

namespace Simplic.Data.MongoDB
{
    public interface IOrganizationIdProvider
    {
        Guid? GetOrganizationId();
    }
}
