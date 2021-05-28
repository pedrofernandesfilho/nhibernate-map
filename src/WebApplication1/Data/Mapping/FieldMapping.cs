using ConsoleApp1.Domain;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace WebApplication1.Data.Mapping
{
    public class FieldMapping : ClassMapping<Field>
    {
        private const string tableName = "TB_FIELD";

        public FieldMapping()
        {
            Table(tableName);

            Id(field => field.Id, mapping =>
            {
                mapping.Column("ID_FIELD");
                mapping.Generator(Generators.Sequence, m => m.Params(new { sequence = "SEQ_FIELD" }));
            });

            Property(field => field.Name, mapping =>
            {
                mapping.Column("NM_FIELD");
                mapping.NotNullable(true);
            });

            Property(field => field.Code, mapping =>
            {
                mapping.Column("CD_FIELD");
                mapping.NotNullable(true);
            });

            //Property(field => field.DisplayOrder, mapping =>
            //{
            //    mapping.Formula("SELECT NU_DISPORDER FROM TB_PRODUCT_FIELD WHERE ID_PRODUCT = ? AND ID_FIELD = ?");
            //});

            //Join("TB_FIELD", mapping =>
            //{
            //    mapping.Key(km =>
            //    {
            //        // km.Column(colMapper => colMapper.)
            //        km.Column("ID_FIELD");
            //    });

            //    mapping.Property(field => field.Code, mapper =>
            //    {
            //        mapper.Column("CD_FIELD");
            //        mapper.NotNullable(true);
            //    });

            //    mapping.Property(field => field.Name, mapper =>
            //    {
            //        mapper.Column("NM_FIELD");
            //        mapper.NotNullable(true);
            //    });
            //});
        }
    }
}
