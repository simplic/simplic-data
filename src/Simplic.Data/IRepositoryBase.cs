﻿using System.Collections.Generic;

namespace Simplic.Data
{
    /// <summary>
    /// Simplic repository data base
    /// </summary>
    /// <typeparam name="TModel">Entity type</typeparam>
    /// <typeparam name="TId">Entity key type</typeparam>
    public interface IRepositoryBase<TId, TModel>
    {
        /// <summary>
        /// Get data by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Instance of <typeparamref name="TModel"/> if exists</returns>
        TModel Get(TId id);

        /// <summary>
        /// Get all objects
        /// </summary>
        /// <returns>Enumerable of <typeparamref name="TModel"/> </returns>
        IEnumerable<TModel> GetAll();

        /// <summary>
        /// Create or update data
        /// </summary>
        /// <param name="obj">Object to save</param>
        /// <returns>True if successful</returns>
        bool Save(TModel obj);

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="obj">Object to delete</param>
        /// <returns>True if successful</returns>
        bool Delete(TModel obj);

        /// <summary>
        /// Delete data by id
        /// </summary>
        /// <param name="id">Id of an object to delete</param>
        /// <returns>True if successful</returns>
        bool Delete(TId id);
    }
}
