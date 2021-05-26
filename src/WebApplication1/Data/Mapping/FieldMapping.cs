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

            Id(x => x.Id, model =>
            {
                model.Column("ID_FIELD");
                model.Generator(Generators.Sequence, m => m.Params(new { sequence = "SEQ_FIELD" }));
            });

            Property(x => x.Name, model =>
            {
                model.Column("NM_FIELD");
                model.NotNullable(true);
            });

            Property(x => x.Code, model =>
            {
                model.Column("CD_FIELD");
                model.NotNullable(true);
            });
        }
    }
}
