using Common.Events;
using System.Runtime.Serialization;

namespace Common.Identity
{
    /// <summary>
    /// <see cref="IIdentity{T}"/> base.
    /// </summary>
    /// <typeparam name="T">Identifier type.</typeparam>
    [DataContract]
    public abstract class Identity<T> :
        NotifyPropertyChange,
        IIdentity<T>
    {
        public Identity(T id)
        {
            Id = id;
        }

        #region Id

        [DataMember(IsRequired = true)]
        public T Id { get; protected set; }

        #endregion

        #region Info

        public virtual string Name => Id.ToString();
        public virtual string Description => null;

        protected virtual void OnNameChanged() => OnPropertyChanged(nameof(Name));
        protected virtual void OnDescriptionChanged() => OnPropertyChanged(nameof(Description));

        #endregion

        public override string ToString() => $"{Name} ({Id})";
    }
}
