using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Data
{
    /// <summary>
    /// Implements the basic service method with a repository that derives from <see cref="IRepositoryBase{TId, TModel}"/>
    /// </summary>
    /// <typeparam name="TId">A key which represents the primary key in the database</typeparam>
    /// <typeparam name="TModel">A model that contains the necessary information</typeparam>
    public abstract class SqlServiceBase<TId, TModel> : IRepositoryBase<TId, TModel>
    {
        private readonly IRepositoryBase<TId, TModel> repositoryBase;

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="repositoryBase">A repository that handles the data acess layer</param>
        public SqlServiceBase(IRepositoryBase<TId, TModel> repositoryBase)
        {
            this.repositoryBase = repositoryBase;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="obj"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public virtual bool Delete(TModel obj) => repositoryBase.Delete(obj);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="id"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public virtual bool Delete(TId id) => repositoryBase.Delete(id);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="id"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public virtual TModel Get(TId id) => repositoryBase.Get(id);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public virtual IEnumerable<TModel> GetAll() => repositoryBase.GetAll();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="obj"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public virtual bool Save(TModel obj) => repositoryBase.Save(obj);
    }


}
