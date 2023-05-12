﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.5
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Database.Models
{
#nullable enable
    public partial class StoredProcedureModel
    {
        public System.Collections.Generic.List<ModelFramework.Database.Contracts.ISqlStatementModel> Statements
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Models.StoredProcedureParameterModel> Parameters
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel> Metadata
        {
            get;
            set;
        }

        public ModelFramework.Database.Contracts.IStoredProcedure ToEntity()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Database.StoredProcedure(Statements.Select(x => x.ToEntity()), Parameters.Select(x => x.ToEntity()), Name, Metadata.Select(x => x.ToEntity()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public StoredProcedureModel()
        {
            Statements = new System.Collections.Generic.List<ModelFramework.Database.Contracts.ISqlStatementModel>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Database.Models.StoredProcedureParameterModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            #pragma warning disable CS8603 // Possible null reference return.
            Name = string.Empty;
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public StoredProcedureModel(ModelFramework.Database.Contracts.IStoredProcedure source)
        {
            Statements = new System.Collections.Generic.List<ModelFramework.Database.Contracts.ISqlStatementModel>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Database.Models.StoredProcedureParameterModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Statements.AddRange(source.Statements.Select(x => x.CreateModel()));
            Parameters.AddRange(source.Parameters.Select(x => new ModelFramework.Database.Models.StoredProcedureParameterModel(x)));
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Models.MetadataModel(x)));
        }
    }
#nullable restore
}
