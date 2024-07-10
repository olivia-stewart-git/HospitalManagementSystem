namespace HMS.Common;

public interface IChangePropagator<T>
{
    public EventHandler<T> OnChange { get; set; }
    public void DoChange();
}