using FluentMigrator.Runner.VersionTableInfo;

namespace Migration
{
    class VersionTable : IVersionTableMetaData
    {
        public object? ApplicationContext { get; set; }

        public bool OwnsSchema { get; } = false;

        public string SchemaName { get; } = default!;

        public string TableName { get; } = "version_info";

        public string ColumnName { get; } = "version";

        public string DescriptionColumnName { get; } = "description";

        public string UniqueIndexName { get; } = "uc_version";

        public string AppliedOnColumnName { get; } = "applied_on";
    }
}
