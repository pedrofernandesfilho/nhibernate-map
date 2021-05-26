using FluentMigrator;

namespace Migration.Migrations
{
    [Migration(202105261207)]
    public class InsertFieldRecords : FluentMigrator.Migration
    {
        private const string campoNomeTabela = "TB_FIELD";
        private const string campoNomeSequencia = "SEQ_FIELD";

        public override void Up()
        {
            Insert
                .IntoTable(campoNomeTabela)
                .Row(new {
                    ID_FIELD = RawSql.Insert($"nextval('{campoNomeSequencia}')"),
                    CD_FIELD = "Doc",
                    NM_FIELD = "Document",
                    DH_CREATE = RawSql.Insert("NOW()")
                });
            Insert
                .IntoTable(campoNomeTabela)
                .Row(new {
                    ID_FIELD = RawSql.Insert($"nextval('{campoNomeSequencia}')"),
                    CD_FIELD = "Addr",
                    NM_FIELD = "Address",
                    DH_CREATE = RawSql.Insert("NOW()")
                });
        }

        public override void Down()
        {
            Delete
                .FromTable(campoNomeTabela)
                .Row(new { CD_FIELD = "Doc", NM_FIELD = "Document" });
            Delete
                .FromTable(campoNomeTabela)
                .Row(new { CD_FIELD = "Addr", NM_FIELD = "Address" });
        }
    }
}
