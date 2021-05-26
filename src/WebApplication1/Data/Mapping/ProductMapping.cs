using ConsoleApp1.Domain;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace WebApplication1.Data.Mapping
{
    public class ProductMapping : ClassMapping<Product>
    {
        private const string tableName = "TB_PRODUCT";

        public ProductMapping()
        {
            Table(tableName);

            Id(x => x.Id, model =>
            {
                model.Column("ID_PRODUCT");
                model.Generator(Generators.Sequence, m => m.Params(new { sequence = "SEQ_PRODUCT" }));
            });

            Property(x => x.Name, model =>
            {
                model.Column("NM_PRODUCT");
                model.NotNullable(true);
            });

            Bag<Field>("fields", mapping =>
            {
                mapping.Table("TB_PRODUCT_FIELD");
                mapping.Key(k => k.Column("ID_PRODUCT"));
            },
            mapField => mapField
                .ManyToMany(mapping => mapping.Column("ID_FIELD")));
        }
    }
}
