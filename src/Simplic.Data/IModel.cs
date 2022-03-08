namespace Simplic.Data
{
    /// <summary>
    /// Basic interfacce for models with a generic type id.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public interface IModel<TId>
    {
        /// <summary>
        /// Identifer of the model.
        /// </summary>
        TId Id { get; set; }
    }
}
