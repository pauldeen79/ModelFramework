using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class SchemaBuilder
    {
        public List<TableBuilder> Tables { get; set; }
        public List<StoredProcedureBuilder> StoredProcedures { get; set; }
        public List<ViewBuilder> Views { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public ISchema Build()
        {
            return new Schema(Name,
                              Tables.Select(x => x.Build()),
                              StoredProcedures.Select(x => x.Build()),
                              Views.Select(x => x.Build()),
                              Metadata.Select(x => x.Build()));
        }
        public SchemaBuilder Clear()
        {
            Tables.Clear();
            StoredProcedures.Clear();
            Views.Clear();
            Name = default;
            Metadata.Clear();
            return this;
        }
        public SchemaBuilder ClearTables()
        {
            Tables.Clear();
            return this;
        }
        public SchemaBuilder AddTables(IEnumerable<TableBuilder> tables)
        {
            return AddTables(tables.ToArray());
        }
        public SchemaBuilder AddTables(params TableBuilder[] tables)
        {
            if (tables != null)
            {
                foreach (var itemToAdd in tables)
                {
                    Tables.Add(itemToAdd);
                }
            }
            return this;
        }
        public SchemaBuilder AddTables(IEnumerable<ITable> tables)
        {
            return AddTables(tables.ToArray());
        }
        public SchemaBuilder AddTables(params ITable[] tables)
        {
            if (tables != null)
            {
                foreach (var itemToAdd in tables)
                {
                    Tables.Add(new TableBuilder(itemToAdd));
                }
            }
            return this;
        }
        public SchemaBuilder ClearStoredProcedures()
        {
            StoredProcedures.Clear();
            return this;
        }
        public SchemaBuilder AddStoredProcedures(IEnumerable<StoredProcedureBuilder> storedProcedures)
        {
            return AddStoredProcedures(storedProcedures.ToArray());
        }
        public SchemaBuilder AddStoredProcedures(params StoredProcedureBuilder[] storedProcedures)
        {
            if (storedProcedures != null)
            {
                foreach (var itemToAdd in storedProcedures)
                {
                    StoredProcedures.Add(itemToAdd);
                }
            }
            return this;
        }
        public SchemaBuilder AddStoredProcedures(IEnumerable<IStoredProcedure> storedProcedures)
        {
            return AddStoredProcedures(storedProcedures.ToArray());
        }
        public SchemaBuilder AddStoredProcedures(params IStoredProcedure[] storedProcedures)
        {
            if (storedProcedures != null)
            {
                foreach (var itemToAdd in storedProcedures)
                {
                    StoredProcedures.Add(new StoredProcedureBuilder(itemToAdd));
                }
            }
            return this;
        }
        public SchemaBuilder ClearViews()
        {
            Views.Clear();
            return this;
        }
        public SchemaBuilder AddViews(IEnumerable<ViewBuilder> views)
        {
            return AddViews(views.ToArray());
        }
        public SchemaBuilder AddViews(params ViewBuilder[] views)
        {
            if (views != null)
            {
                foreach (var itemToAdd in views)
                {
                    Views.Add(itemToAdd);
                }
            }
            return this;
        }
        public SchemaBuilder AddViews(IEnumerable<IView> views)
        {
            return AddViews(views.ToArray());
        }
        public SchemaBuilder AddViews(params IView[] views)
        {
            if (views != null)
            {
                foreach (var itemToAdd in views)
                {
                    Views.Add(new ViewBuilder(itemToAdd));
                }
            }
            return this;
        }
        public SchemaBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public SchemaBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public SchemaBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public SchemaBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            if (metadata != null)
            {
                foreach (var itemToAdd in metadata)
                {
                    Metadata.Add(itemToAdd);
                }
            }
            return this;
        }
        public SchemaBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public SchemaBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public SchemaBuilder()
        {
            Tables = new List<TableBuilder>();
            StoredProcedures = new List<StoredProcedureBuilder>();
            Views = new List<ViewBuilder>();
            Metadata = new List<MetadataBuilder>();
        }
        public SchemaBuilder(ISchema source)
        {
            Tables = new List<TableBuilder>();
            StoredProcedures = new List<StoredProcedureBuilder>();
            Views = new List<ViewBuilder>();
            Metadata = new List<MetadataBuilder>();

            if (source.Tables != null) foreach (var x in source.Tables) Tables.Add(new TableBuilder(x));
            if (source.StoredProcedures != null) foreach (var x in source.StoredProcedures) StoredProcedures.Add(new StoredProcedureBuilder(x));
            if (source.Views != null) foreach (var x in source.Views) Views.Add(new ViewBuilder(x));
            Name = source.Name;
            if (source.Metadata != null) foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
        }
    }
}
