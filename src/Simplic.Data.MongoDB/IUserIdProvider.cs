using System;
using Simplic.Data.NoSql;

namespace Simplic.Data.MongoDB
{
    public interface IUserIdProvider
    {
        Guid? GetUserId();
    }
}
