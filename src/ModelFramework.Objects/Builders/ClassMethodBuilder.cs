using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;

namespace ModelFramework.Objects.Builders
{
    public class ClassMethodBuilder
    {
        public bool Partial { get; set; }
        public bool ExtensionMethod { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public bool Static { get; set; }
        public bool Virtual { get; set; }
        public bool Abstract { get; set; }
        public bool Protected { get; set; }
        public bool Override { get; set; }
        public bool Operator { get; set; }
        public bool IsNullable { get; set; }
        public Visibility Visibility { get; set; }
        public string Name { get; set; }
        public List<AttributeBuilder> Attributes { get; set; }
        public string Body { get; set; }
        public List<ParameterBuilder> Parameters { get; set; }
        public string TypeName { get; set; }
        public string ExplicitInterfaceName { get; set; }
        public List<ICodeStatementBuilder> CodeStatements { get; set; }
        public IClassMethod Build()
        {
            return new ClassMethod(Name,
                                   TypeName,
                                   Visibility,
                                   Static,
                                   Virtual,
                                   Abstract,
                                   Protected,
                                   Partial,
                                   Override,
                                   ExtensionMethod,
                                   Operator,
                                   IsNullable,
                                   Body,
                                   ExplicitInterfaceName,
                                   Parameters.Select(x => x.Build()),
                                   Attributes.Select(x => x.Build()),
                                   CodeStatements.Select(x => x.Build()),
                                   Metadata.Select(x => x.Build()));
        }
        public ClassMethodBuilder Clear()
        {
            Partial = default;
            ExtensionMethod = default;
            Metadata.Clear();
            Static = default;
            Virtual = default;
            Abstract = default;
            Protected = default;
            Override = default;
            Operator = default;
            IsNullable = default;
            Visibility = default;
            Name = default;
            Attributes.Clear();
            Body = default;
            Parameters.Clear();
            TypeName = default;
            ExplicitInterfaceName = default;
            CodeStatements.Clear();
            return this;
        }
        public ClassMethodBuilder Update(IClassMethod source)
        {
            Partial = default;
            ExtensionMethod = default;
            Metadata = new List<MetadataBuilder>();
            Static = default;
            Virtual = default;
            Abstract = default;
            Protected = default;
            Override = default;
            IsNullable = default;
            Operator = default;
            Visibility = default;
            Name = default;
            Attributes = new List<AttributeBuilder>();
            Body = default;
            Parameters = new List<ParameterBuilder>();
            TypeName = default;
            ExplicitInterfaceName = default;
            CodeStatements = new List<ICodeStatementBuilder>();
            if (source != null)
            {
                Partial = source.Partial;
                ExtensionMethod = source.ExtensionMethod;
                if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
                Static = source.Static;
                Virtual = source.Virtual;
                Abstract = source.Abstract;
                Protected = source.Protected;
                Override = source.Override;
                Operator = source.Operator;
                IsNullable = source.IsNullable;
                Visibility = source.Visibility;
                Name = source.Name;
                if (source.Attributes != null) Attributes.AddRange(source.Attributes.Select(x => new AttributeBuilder(x)));
                Body = source.Body;
                if (source.Parameters != null) Parameters.AddRange(source.Parameters.Select(x => new ParameterBuilder(x)));
                TypeName = source.TypeName;
                ExplicitInterfaceName = source.ExplicitInterfaceName;
                if (source.CodeStatements != null) CodeStatements.AddRange(source.CodeStatements.Select(x => x.CreateBuilder()));
            }
            return this;
        }
        public ClassMethodBuilder WithPartial(bool partial)
        {
            Partial = partial;
            return this;
        }
        public ClassMethodBuilder WithExtensionMethod(bool extensionMethod)
        {
            ExtensionMethod = extensionMethod;
            return this;
        }
        public ClassMethodBuilder WithOperator(bool @operator)
        {
            Operator = @operator;
            return this;
        }
        public ClassMethodBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public ClassMethodBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ClassMethodBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata);
            }
            return this;
        }
        public ClassMethodBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ClassMethodBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public ClassMethodBuilder WithStatic(bool @static)
        {
            Static = @static;
            return this;
        }
        public ClassMethodBuilder WithVirtual(bool @virtual)
        {
            Virtual = @virtual;
            return this;
        }
        public ClassMethodBuilder WithAbstract(bool @abstract)
        {
            Abstract = @abstract;
            return this;
        }
        public ClassMethodBuilder WithProtected(bool @protected)
        {
            Protected = @protected;
            return this;
        }
        public ClassMethodBuilder WithOverride(bool @override)
        {
            Override = @override;
            return this;
        }
        public ClassMethodBuilder WithIsNullable(bool isNullable)
        {
            IsNullable = isNullable;
            return this;
        }
        public ClassMethodBuilder WithVisibility(Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }
        public ClassMethodBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public ClassMethodBuilder ClearAttributes()
        {
            Attributes.Clear();
            return this;
        }
        public ClassMethodBuilder AddAttributes(IEnumerable<AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public ClassMethodBuilder AddAttributes(params AttributeBuilder[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes);
            }
            return this;
        }
        public ClassMethodBuilder AddAttributes(IEnumerable<IAttribute> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public ClassMethodBuilder AddAttributes(params IAttribute[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes.Select(x => new AttributeBuilder(x)));
            }
            return this;
        }
        public ClassMethodBuilder WithBody(string body)
        {
            Body = body;
            return this;
        }
        public ClassMethodBuilder ClearParameters()
        {
            Parameters.Clear();
            return this;
        }
        public ClassMethodBuilder AddParameters(IEnumerable<ParameterBuilder> parameters)
        {
            return AddParameters(parameters.ToArray());
        }
        public ClassMethodBuilder AddParameters(params ParameterBuilder[] parameters)
        {
            Parameters.AddRange(parameters);
            return this;
        }
        public ClassMethodBuilder AddParameters(IEnumerable<IParameter> parameters)
        {
            return AddParameters(parameters.ToArray());
        }
        public ClassMethodBuilder AddParameters(params IParameter[] parameters)
        {
            if (parameters != null)
            {
                foreach (var itemToAdd in parameters)
                {
                    Parameters.Add(new ParameterBuilder(itemToAdd));
                }
            }
            return this;
        }
        public ClassMethodBuilder WithTypeName(string typeName)
        {
            TypeName = typeName;
            return this;
        }
        public ClassMethodBuilder WithExplicitInterfaceName(string explicitInterfaceName)
        {
            ExplicitInterfaceName = explicitInterfaceName;
            return this;
        }
        public ClassMethodBuilder ClearCodeStatements()
        {
            CodeStatements.Clear();
            return this;
        }
        public ClassMethodBuilder AddCodeStatements(IEnumerable<ICodeStatementBuilder> codeStatements)
        {
            return AddCodeStatements(codeStatements.ToArray());
        }
        public ClassMethodBuilder AddCodeStatements(params ICodeStatementBuilder[] codeStatements)
        {
            if (codeStatements != null)
            {
                CodeStatements.AddRange(codeStatements);
            }
            return this;
        }
        public ClassMethodBuilder AddCodeStatements(IEnumerable<ICodeStatement> codeStatements)
        {
            return AddCodeStatements(codeStatements.ToArray());
        }
        public ClassMethodBuilder AddCodeStatements(params ICodeStatement[] codeStatements)
        {
            if (codeStatements != null)
            {
                CodeStatements.AddRange(codeStatements.Select(x => x.CreateBuilder()));
            }
            return this;
        }
        public ClassMethodBuilder(IClassMethod source = null)
        {
            if (source != null)
            {
                Partial = source.Partial;
                ExtensionMethod = source.ExtensionMethod;
                Metadata = new List<MetadataBuilder>(source.Metadata?.Select(x => new MetadataBuilder(x)) ?? Enumerable.Empty<MetadataBuilder>());
                Static = source.Static;
                Virtual = source.Virtual;
                Abstract = source.Abstract;
                Protected = source.Protected;
                Override = source.Override;
                Operator = source.Operator;
                IsNullable = source.IsNullable;
                Visibility = source.Visibility;
                Name = source.Name;
                Attributes = new List<AttributeBuilder>(source.Attributes?.Select(x => new AttributeBuilder(x)) ?? Enumerable.Empty<AttributeBuilder>());
                Body = source.Body;
                Parameters = new List<ParameterBuilder>(source.Parameters?.Select(x => new ParameterBuilder(x)) ?? Enumerable.Empty<ParameterBuilder>());
                TypeName = source.TypeName;
                ExplicitInterfaceName = source.ExplicitInterfaceName;
                CodeStatements = new List<ICodeStatementBuilder>(source.CodeStatements?.Select(x => x.CreateBuilder()) ?? Enumerable.Empty<ICodeStatementBuilder>());
            }
            else
            {
                Metadata = new List<MetadataBuilder>();
                Attributes = new List<AttributeBuilder>();
                Parameters = new List<ParameterBuilder>();
                CodeStatements = new List<ICodeStatementBuilder>();
            }
        }
#pragma warning disable S107 // Methods should not have too many parameters
        public ClassMethodBuilder(string name,
                                  string typeName,
                                  Visibility visibility = Visibility.Public,
                                  bool @static = false,
                                  bool @virtual = false,
                                  bool @abstract = false,
                                  bool @protected = false,
                                  bool partial = false,
                                  bool @override = false,
                                  bool extensionMethod = false,
                                  bool @operator = false,
                                  bool isNullable = false,
                                  string body = null,
                                  string explicitInterfaceName = null,
                                  IEnumerable<IParameter> parameters = null,
                                  IEnumerable<IAttribute> attributes = null,
                                  IEnumerable<ICodeStatement> codeStatements = null,
                                  IEnumerable<IMetadata> metadata = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            Metadata = new List<MetadataBuilder>();
            Attributes = new List<AttributeBuilder>();
            Parameters = new List<ParameterBuilder>();
            CodeStatements = new List<ICodeStatementBuilder>();
            Name = name;
            TypeName = typeName;
            Visibility = visibility;
            Static = @static;
            Virtual = @virtual;
            Abstract = @abstract;
            Protected = @protected;
            Partial = partial;
            Override = @override;
            ExtensionMethod = extensionMethod;
            Operator = @operator;
            IsNullable = isNullable;
            Body = body;
            ExplicitInterfaceName = explicitInterfaceName;
            if (parameters != null) Parameters.AddRange(parameters.Select(x => new ParameterBuilder(x)));
            if (attributes != null) Attributes.AddRange(attributes.Select(x => new AttributeBuilder(x)));
            if (codeStatements != null) CodeStatements.AddRange(codeStatements.Select(x => x.CreateBuilder()));
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
