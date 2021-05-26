namespace ConsoleApp1.Domain
{
    public abstract class BaseEntity
    {
        public virtual long Id { get; internal protected set; }
    }
}
