﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.12
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Objects.Builders
{
#nullable enable
    public partial class ClassMethodBuilder
    {
        public bool Partial
        {
            get;
            set;
        }

        public bool ExtensionMethod
        {
            get;
            set;
        }

        public bool Operator
        {
            get;
            set;
        }

        public bool Async
        {
            get;
            set;
        }

        public System.Collections.Generic.List<string> GenericTypeArguments
        {
            get;
            set;
        }

        public System.Collections.Generic.List<string> GenericTypeArgumentConstraints
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public bool Static
        {
            get;
            set;
        }

        public bool Virtual
        {
            get;
            set;
        }

        public bool Abstract
        {
            get;
            set;
        }

        public bool Protected
        {
            get;
            set;
        }

        public bool Override
        {
            get;
            set;
        }

        public ModelFramework.Objects.Contracts.Visibility Visibility
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder> Attributes
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder> CodeStatements
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.ParameterBuilder> Parameters
        {
            get;
            set;
        }

        public string TypeName
        {
            get;
            set;
        }

        public bool IsNullable
        {
            get;
            set;
        }

        public bool IsValueType
        {
            get;
            set;
        }

        public string ExplicitInterfaceName
        {
            get;
            set;
        }

        public string ParentTypeFullName
        {
            get;
            set;
        }

        public ModelFramework.Objects.Contracts.IClassMethod Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Objects.ClassMethod(Partial, ExtensionMethod, Operator, Async, new CrossCutting.Common.ValueCollection<System.String>(GenericTypeArguments), new CrossCutting.Common.ValueCollection<System.String>(GenericTypeArgumentConstraints), Metadata.Select(x => x.Build()), Static, Virtual, Abstract, Protected, Override, Visibility, Name, Attributes.Select(x => x.Build()), CodeStatements.Select(x => x.Build()), Parameters.Select(x => x.Build()), TypeName, IsNullable, IsValueType, ExplicitInterfaceName, ParentTypeFullName);
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ClassMethodBuilder WithPartial(bool partial = true)
        {
            Partial = partial;
            return this;
        }

        public ClassMethodBuilder WithExtensionMethod(bool extensionMethod = true)
        {
            ExtensionMethod = extensionMethod;
            return this;
        }

        public ClassMethodBuilder WithOperator(bool @operator = true)
        {
            Operator = @operator;
            return this;
        }

        public ClassMethodBuilder WithAsync(bool async = true)
        {
            Async = async;
            return this;
        }

        public ClassMethodBuilder AddGenericTypeArguments(System.Collections.Generic.IEnumerable<string> genericTypeArguments)
        {
            return AddGenericTypeArguments(genericTypeArguments.ToArray());
        }

        public ClassMethodBuilder AddGenericTypeArguments(params string[] genericTypeArguments)
        {
            GenericTypeArguments.AddRange(genericTypeArguments);
            return this;
        }

        public ClassMethodBuilder AddGenericTypeArgumentConstraints(System.Collections.Generic.IEnumerable<string> genericTypeArgumentConstraints)
        {
            return AddGenericTypeArgumentConstraints(genericTypeArgumentConstraints.ToArray());
        }

        public ClassMethodBuilder AddGenericTypeArgumentConstraints(params string[] genericTypeArgumentConstraints)
        {
            GenericTypeArgumentConstraints.AddRange(genericTypeArgumentConstraints);
            return this;
        }

        public ClassMethodBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ClassMethodBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ClassMethodBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ClassMethodBuilder WithStatic(bool @static = true)
        {
            Static = @static;
            return this;
        }

        public ClassMethodBuilder WithVirtual(bool @virtual = true)
        {
            Virtual = @virtual;
            return this;
        }

        public ClassMethodBuilder WithAbstract(bool @abstract = true)
        {
            Abstract = @abstract;
            return this;
        }

        public ClassMethodBuilder WithProtected(bool @protected = true)
        {
            Protected = @protected;
            return this;
        }

        public ClassMethodBuilder WithOverride(bool @override = true)
        {
            Override = @override;
            return this;
        }

        public ClassMethodBuilder WithVisibility(ModelFramework.Objects.Contracts.Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }

        public ClassMethodBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public ClassMethodBuilder AddAttributes(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }

        public ClassMethodBuilder AddAttributes(params ModelFramework.Objects.Builders.AttributeBuilder[] attributes)
        {
            Attributes.AddRange(attributes);
            return this;
        }

        public ClassMethodBuilder AddCodeStatements(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatementBuilder> codeStatements)
        {
            return AddCodeStatements(codeStatements.ToArray());
        }

        public ClassMethodBuilder AddCodeStatements(params ModelFramework.Objects.Contracts.ICodeStatementBuilder[] codeStatements)
        {
            CodeStatements.AddRange(codeStatements);
            return this;
        }

        public ClassMethodBuilder AddLiteralCodeStatements(params string[] statements)
        {
            AddCodeStatements(ModelFramework.Objects.Extensions.EnumerableOfStringExtensions.ToLiteralCodeStatementBuilders(statements));
            return this;
        }

        public ClassMethodBuilder AddLiteralCodeStatements(System.Collections.Generic.IEnumerable<string> statements)
        {
            AddLiteralCodeStatements(statements.ToArray());
            return this;
        }

        public ClassMethodBuilder AddParameters(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.ParameterBuilder> parameters)
        {
            return AddParameters(parameters.ToArray());
        }

        public ClassMethodBuilder AddParameters(params ModelFramework.Objects.Builders.ParameterBuilder[] parameters)
        {
            Parameters.AddRange(parameters);
            return this;
        }

        public ClassMethodBuilder AddParameter(string name, System.Type type)
        {
            AddParameters(new ModelFramework.Objects.Builders.ParameterBuilder().WithName(name).WithType(type));
            return this;
        }

        public ClassMethodBuilder AddParameter(string name, System.Type type, bool isNullable)
        {
            AddParameters(new ModelFramework.Objects.Builders.ParameterBuilder().WithName(name).WithType(type).WithIsNullable(isNullable));
            return this;
        }

        public ClassMethodBuilder AddParameter(string name, string typeName)
        {
            AddParameters(new ModelFramework.Objects.Builders.ParameterBuilder().WithName(name).WithTypeName(typeName));
            return this;
        }

        public ClassMethodBuilder AddParameter(string name, string typeName, bool isNullable)
        {
            AddParameters(new ModelFramework.Objects.Builders.ParameterBuilder().WithName(name).WithTypeName(typeName).WithIsNullable(isNullable));
            return this;
        }

        public ClassMethodBuilder WithTypeName(string typeName)
        {
            TypeName = typeName;
            return this;
        }

        public ClassMethodBuilder WithType(System.Type type)
        {
            TypeName = type.AssemblyQualifiedName.FixTypeName(); IsValueType = type.IsValueType || type.IsEnum;
            return this;
        }

        public ClassMethodBuilder WithIsNullable(bool isNullable = true)
        {
            IsNullable = isNullable;
            return this;
        }

        public ClassMethodBuilder WithIsValueType(bool isValueType = true)
        {
            IsValueType = isValueType;
            return this;
        }

        public ClassMethodBuilder WithExplicitInterfaceName(string explicitInterfaceName)
        {
            ExplicitInterfaceName = explicitInterfaceName;
            return this;
        }

        public ClassMethodBuilder WithParentTypeFullName(string parentTypeFullName)
        {
            ParentTypeFullName = parentTypeFullName;
            return this;
        }

        public ClassMethodBuilder()
        {
            GenericTypeArguments = new System.Collections.Generic.List<string>();
            GenericTypeArgumentConstraints = new System.Collections.Generic.List<string>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            CodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ParameterBuilder>();
            Visibility = ModelFramework.Objects.Contracts.Visibility.Public;
            Name = string.Empty;
            TypeName = string.Empty;
            ExplicitInterfaceName = string.Empty;
            ParentTypeFullName = string.Empty;
        }

        public ClassMethodBuilder(ModelFramework.Objects.Contracts.IClassMethod source)
        {
            GenericTypeArguments = new System.Collections.Generic.List<string>();
            GenericTypeArgumentConstraints = new System.Collections.Generic.List<string>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            CodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ParameterBuilder>();
            Partial = source.Partial;
            ExtensionMethod = source.ExtensionMethod;
            Operator = source.Operator;
            Async = source.Async;
            GenericTypeArguments.AddRange(source.GenericTypeArguments);
            GenericTypeArgumentConstraints.AddRange(source.GenericTypeArgumentConstraints);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            Static = source.Static;
            Virtual = source.Virtual;
            Abstract = source.Abstract;
            Protected = source.Protected;
            Override = source.Override;
            Visibility = source.Visibility;
            Name = source.Name;
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Builders.AttributeBuilder(x)));
            CodeStatements.AddRange(source.CodeStatements.Select(x => x.CreateBuilder()));
            Parameters.AddRange(source.Parameters.Select(x => new ModelFramework.Objects.Builders.ParameterBuilder(x)));
            TypeName = source.TypeName;
            IsNullable = source.IsNullable;
            IsValueType = source.IsValueType;
            ExplicitInterfaceName = source.ExplicitInterfaceName;
            ParentTypeFullName = source.ParentTypeFullName;
        }
    }
#nullable restore
}

