using System;

namespace Simplic.Data
{
    /// <summary>
    /// Interface for the properties of instancedata.
    /// </summary>
    public interface IInstanceData
    {
        /// <summary>
        /// Gets or sets the createDateTime.
        /// </summary>
        DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Gets or sets the updatedDateTime.
        /// </summary>
        DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// Gets or Sets the user that created the rating.
        /// </summary>
        int CreateUserId { get; set; }

        /// <summary>
        /// Gets or sets the user that last updated the rating.
        /// </summary>
        int? UpdateUserId { get; set; }
    }
}
