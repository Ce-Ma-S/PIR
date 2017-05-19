namespace Common.Identity
{
    /// <summary>
    /// Information about <see cref="IIdentity{T}"/> to be presented in UI etc.
    /// </summary>
    public interface IIdentityInfo
    {
        /// <summary>
        /// Name.
        /// </summary>
        /// <value>non-empty</value>
        string Name { get; }
        /// <summary>
        /// Optional description.
        /// </summary>
        string Description { get; }
    }
}
