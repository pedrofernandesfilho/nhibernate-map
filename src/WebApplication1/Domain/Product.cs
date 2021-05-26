using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ConsoleApp1.Domain
{
    public class Product : BaseEntity
    {
        public virtual string Name { get; protected set; } = default!;
        private readonly IList<Field> fields = new List<Field>();
        public virtual IReadOnlyCollection<Field> Fields => new ReadOnlyCollection<Field>(fields);

        protected Product() { }

        public Product(string name) => Name = name;

        public virtual void AddField(Field field) => fields.Add(field);
    }
}
