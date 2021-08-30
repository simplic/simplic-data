using System;

namespace Simplic.Data
{
    /// <summary>
    /// Interface for repositories with external database access
    /// </summary>
    public interface IExternalDataBase
    {
        /// <summary>
        /// Gets the name of the repository group 
        /// </summary>
        Guid? GroupId { get; }
    }
}
