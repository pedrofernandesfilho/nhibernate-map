using FluentMigrator;

namespace Migration.Migrations
{
    [Migration(202105281633)]
    public class AddDisplayOrderColumn : FluentMigrator.Migration
    {
        private const string productFieldTableName = "TB_PRODUCT_FIELD";
        private const string displayOrderColumnName = "NU_DISPORDER";

        public override void Up() =>
            Alter.Table(productFieldTableName).AddColumn(displayOrderColumnName).AsInt16();

        public override void Down() =>
            Delete.Column(displayOrderColumnName).FromTable(productFieldTableName);
    }
}
