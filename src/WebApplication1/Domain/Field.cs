namespace ConsoleApp1.Domain
{
    public class Field : BaseEntity
    {
        public virtual string Name { get; protected set; } = default!;
        public virtual string Code { get; protected set; } = default!;
        public virtual short DisplayOrder { get; set; }

        protected Field() { }

        public Field(string name, string code) => (Name, Code) = (name, code);
    }
}
