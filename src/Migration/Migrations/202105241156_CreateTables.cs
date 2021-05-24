using FluentMigrator;

namespace Migration.Migrations
{
    [Migration(202105241156)]
    public class CreateTables : FluentMigrator.Migration
    {
        private const string productTableName = "TB_PRODUCT";
        private const string productSequenceName = "SEQ_PRODUCT";

        private const string fieldTableName = "TB_FIELD";
        private const string fieldSequenceName = "SEQ_FIELD";

        private const string productFieldTableName = "TB_PRODUCT_FIELD";
        private const string productFieldFKNameProduct = "FK_PRODUCTFIELD_PRODUCT";
        private const string productFieldFKNameField = "FK_PRODUCTFIELD_FIELD";

        public override void Up()
        {
            Create.Sequence(productSequenceName).IncrementBy(1).MinValue(1).MaxValue(long.MaxValue);
            Create.Table(productTableName)
                .WithColumn("ID_PRODUCT").AsInt64().PrimaryKey()
                .WithColumn("NM_PRODUCT").AsString(100).NotNullable()
                .WithColumn("DH_CREATE").AsDateTime2().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime);

            Create.Sequence(fieldSequenceName).IncrementBy(1).MinValue(1).MaxValue(long.MaxValue);
            Create.Table(fieldTableName)
                .WithColumn("ID_FIELD").AsInt64().PrimaryKey()
                .WithColumn("CD_FIELD").AsString(5).NotNullable().Unique()
                .WithColumn("NM_FIELD").AsString(100).NotNullable()
                .WithColumn("DH_CREATE").AsDateTime2().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime);

            Create.Table(productFieldTableName)
                .WithColumn("ID_PRODUCT").AsInt64()
                .WithColumn("ID_FIELD").AsInt64();

            Create.ForeignKey(productFieldFKNameProduct)
                .FromTable(productFieldTableName).ForeignColumn("ID_PRODUCT")
                .ToTable(productTableName).PrimaryColumn("ID_PRODUCT");

            Create.ForeignKey(productFieldFKNameField)
                .FromTable(productFieldTableName).ForeignColumn("ID_FIELD")
                .ToTable(fieldTableName).PrimaryColumn("ID_FIELD");
        }

        public override void Down()
        {
            Delete.Table(productFieldTableName);

            Delete.Table(productTableName);
            Delete.Sequence(productSequenceName);

            Delete.Table(fieldTableName);
            Delete.Sequence(fieldSequenceName);
        }
    }
}
