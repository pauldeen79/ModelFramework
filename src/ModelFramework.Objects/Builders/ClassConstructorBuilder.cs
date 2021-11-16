using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;

namespace ModelFramework.Objects.Builders
{
    public class ClassConstructorBuilder
    {
        public List<MetadataBuilder> Metadata { get; set; }
        public bool Static { get; set; }
        public bool Virtual { get; set; }
        public bool Abstract { get; set; }
        public bool Protected { get; set; }
        public bool Override { get; set; }
        public Visibility Visibility { get; set; }
        public List<AttributeBuilder> Attributes { get; set; }
        public string Body { get; set; }
        public string ChainCall { get; set; }
        public List<ParameterBuilder> Parameters { get; set; }
        public List<ICodeStatementBuilder> CodeStatements { get; set; }
        public IClassConstructor Build()
        {
            return new ClassConstructor(Visibility,
                                        Static,
                                        Virtual,
                                        Abstract,
                                        Protected,
                                        Override,
                                        Body,
                                        ChainCall,
                                        Parameters.Select(x => x.Build()),
                                        Attributes.Select(x => x.Build()),
                                        CodeStatements.Select(x => x.Build()),
                                        Metadata.Select(x => x.Build()));
        }
        public ClassConstructorBuilder Clear()
        {
            Metadata.Clear();
            Static = default;
            Virtual = default;
            Abstract = default;
            Protected = default;
            Override = default;
            Visibility = default;
            Attributes.Clear();
            Body = default;
            ChainCall = default;
            Parameters.Clear();
            CodeStatements.Clear();
            return this;
        }
        public ClassConstructorBuilder Update(IClassConstructor source)
        {
            Metadata = new List<MetadataBuilder>();
            Attributes = new List<AttributeBuilder>();
            Parameters = new List<ParameterBuilder>();
            CodeStatements = new List<ICodeStatementBuilder>();
            Metadata.AddRange(source.Metadata?.Select(x => new MetadataBuilder(x)) ?? Enumerable.Empty<MetadataBuilder>());
            Static = source.Static;
            Virtual = source.Virtual;
            Abstract = source.Abstract;
            Protected = source.Protected;
            Override = source.Override;
            Visibility = source.Visibility;
            Attributes.AddRange(source.Attributes?.Select(x => new AttributeBuilder(x)) ?? Enumerable.Empty<AttributeBuilder>());
            Body = source.Body;
            ChainCall = source.ChainCall;
            Parameters.AddRange(source.Parameters?.Select(x => new ParameterBuilder(x)) ?? Enumerable.Empty<ParameterBuilder>());
            CodeStatements.AddRange(source.CodeStatements?.Select(x => x.CreateBuilder()) ?? Enumerable.Empty<ICodeStatementBuilder>());
            return this;
        }
        public ClassConstructorBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public ClassConstructorBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ClassConstructorBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata);
            }
            return this;
        }
        public ClassConstructorBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ClassConstructorBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public ClassConstructorBuilder WithStatic(bool @static = true)
        {
            Static = @static;
            return this;
        }
        public ClassConstructorBuilder WithVirtual(bool @virtual = true)
        {
            Virtual = @virtual;
            return this;
        }
        public ClassConstructorBuilder WithAbstract(bool @abstract = true)
        {
            Abstract = @abstract;
            return this;
        }
        public ClassConstructorBuilder WithProtected(bool @protected = true)
        {
            Protected = @protected;
            return this;
        }
        public ClassConstructorBuilder WithOverride(bool @override = true)
        {
            Override = @override;
            return this;
        }
        public ClassConstructorBuilder WithVisibility(Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }
        public ClassConstructorBuilder ClearAttributes()
        {
            Attributes.Clear();
            return this;
        }
        public ClassConstructorBuilder AddAttributes(IEnumerable<AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public ClassConstructorBuilder AddAttributes(params AttributeBuilder[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes);
            }
            return this;
        }
        public ClassConstructorBuilder AddAttributes(IEnumerable<IAttribute> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public ClassConstructorBuilder AddAttributes(params IAttribute[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes.Select(x => new AttributeBuilder(x)));
            }
            return this;
        }
        public ClassConstructorBuilder WithBody(string body)
        {
            Body = body;
            return this;
        }
        public ClassConstructorBuilder WithChainCall(string chainCall)
        {
            ChainCall = chainCall;
            return this;
        }
        public ClassConstructorBuilder ClearParameters()
        {
            Parameters.Clear();
            return this;
        }
        public ClassConstructorBuilder AddParameters(IEnumerable<ParameterBuilder> parameters)
        {
            return AddParameters(parameters.ToArray());
        }
        public ClassConstructorBuilder AddParameters(params ParameterBuilder[] parameters)
        {
            if (parameters != null)
            {
                Parameters.AddRange(parameters);
            }
            return this;
        }
        public ClassConstructorBuilder AddParameters(IEnumerable<IParameter> parameters)
        {
            return AddParameters(parameters.ToArray());
        }
        public ClassConstructorBuilder AddParameters(params IParameter[] parameters)
        {
            if (parameters != null)
            {
                Parameters.AddRange(parameters.Select(x => new ParameterBuilder(x)));
            }
            return this;
        }
        public ClassConstructorBuilder ClearCodeStatements()
        {
            CodeStatements.Clear();
            return this;
        }
        public ClassConstructorBuilder AddCodeStatements(IEnumerable<ICodeStatementBuilder> codeStatements)
        {
            return AddCodeStatements(codeStatements.ToArray());
        }
        public ClassConstructorBuilder AddCodeStatements(params ICodeStatementBuilder[] codeStatements)
        {
            if (codeStatements != null)
            {
                CodeStatements.AddRange(codeStatements);
            }
            return this;
        }
        public ClassConstructorBuilder AddCodeStatements(IEnumerable<ICodeStatement> codeStatements)
        {
            return AddCodeStatements(codeStatements.ToArray());
        }
        public ClassConstructorBuilder AddCodeStatements(params ICodeStatement[] codeStatements)
        {
            if (codeStatements != null)
            {
                CodeStatements.AddRange(codeStatements.Select(x => x.CreateBuilder()));
            }
            return this;
        }
        public ClassConstructorBuilder()
        {
            Metadata = new List<MetadataBuilder>();
            Attributes = new List<AttributeBuilder>();
            Parameters = new List<ParameterBuilder>();
            CodeStatements = new List<ICodeStatementBuilder>();
        }
        public ClassConstructorBuilder(IClassConstructor source)
        {
            Metadata = new List<MetadataBuilder>(source.Metadata?.Select(x => new MetadataBuilder(x)) ?? Enumerable.Empty<MetadataBuilder>());
            Static = source.Static;
            Virtual = source.Virtual;
            Abstract = source.Abstract;
            Protected = source.Protected;
            Override = source.Override;
            Visibility = source.Visibility;
            Attributes = new List<AttributeBuilder>(source.Attributes?.Select(x => new AttributeBuilder(x)) ?? Enumerable.Empty<AttributeBuilder>());
            Body = source.Body;
            ChainCall = source.ChainCall;
            Parameters = new List<ParameterBuilder>(source.Parameters?.Select(x => new ParameterBuilder(x)) ?? Enumerable.Empty<ParameterBuilder>());
            CodeStatements = new List<ICodeStatementBuilder>(source.CodeStatements?.Select(x => x.CreateBuilder()) ?? Enumerable.Empty<ICodeStatementBuilder>());
        }
    }
}
