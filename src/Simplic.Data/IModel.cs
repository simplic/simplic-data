namespace Simplic.Data
{
    /// <summary>
    /// Basic interfacce for models with a generic type id.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public interface IModel<TId>
    {
        /// <summary>
        /// Gets or sets the identifier of the model.
        /// </summary>
        TId Id { get; set; }
    }
}
