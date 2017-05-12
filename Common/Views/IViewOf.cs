namespace Common.Views
{
    public interface IViewOf<T>
    {
        T Model { get; }
    }
}
