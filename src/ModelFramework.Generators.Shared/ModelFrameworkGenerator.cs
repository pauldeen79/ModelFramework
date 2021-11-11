// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 5.0.11
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ModelFramework.Generators.Shared
{
    using ModelFramework.Common;
    using ModelFramework.Common.Contracts;
    using ModelFramework.Common.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TextTemplateTransformationFramework.Runtime;

    [System.CodeDom.Compiler.GeneratedCode(@"T4PlusCSharpCodeGenerator", @"1.0.0.0")]
    public partial class ModelFrameworkGenerator : ModelFrameworkGeneratorBase
    {
        public virtual void Render(global::System.Text.StringBuilder builder)
        {
            var backup = this.GenerationEnvironment;
            if (builder != null) this.GenerationEnvironment = builder;

            if (builder != null) this.GenerationEnvironment = backup;
        }



        public virtual void Initialize(global::System.Action additionalActionDelegate = null)
        {
            this.Errors.Clear();
            this.GenerationEnvironment.Clear();
            if (Session == null)
            {
                Session = new global::System.Collections.Generic.Dictionary<string, object>();
            }
            if (additionalActionDelegate != null)
            {
                additionalActionDelegate();
            }

        }

    }

    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute(@"T4PlusCSharpCodeGenerator", @"1.0.0.0")]
    public partial class ModelFrameworkGeneratorBase : TextTemplateTransformationFramework.Runtime.T4PlusGeneratedTemplateBase
    {

        public ModelFrameworkGeneratorBase()
        {
            ToStringHelper.AllowNullExpressions = true;
        }
        public bool ResolveFromMetadata(string templateName, string childTemplateName, Type childTemplateModelType, object model)
        {
            return ResolveFromMetadata(templateName, childTemplateName, childTemplateModelType, model, ModelFramework.Common.MetadataNames.CustomTemplateName);
        }

        protected virtual string GetTemplateNameByModelType(object model)
        {
            return null;
        }

        protected bool ResolveFromMetadata(string templateName, string childTemplateName, Type childTemplateModelType, object model, string metadataPropertyName, string defaultTemplateName = null)
        {
            var metadataContainer = model as IMetadataContainer;
            var defaultImplementation = new Func<string, string, Type, object, bool>((tn, ctn, ctmt, m) =>
            {
                var retValue =  string.IsNullOrEmpty(tn) && m != null
                       ? ctmt != null && ctmt == m.GetType()
                       : tn == ctn;

                if (!retValue && string.IsNullOrEmpty(tn) && model != null)
                {
                    retValue = ctmt != null && ctmt.IsAssignableFrom(m.GetType());
                }

                return retValue;
            });

            var customTemplateName = metadataContainer == null || metadataPropertyName == null || metadataContainer.Metadata.GetMetadataValue<object>(metadataPropertyName) == null
                ? null
                : metadataContainer.Metadata.GetMetadataStringValue(metadataPropertyName);

            return customTemplateName == null
                ? defaultTemplateName == null
                    ? defaultImplementation(templateName, childTemplateName, childTemplateModelType, model)
                    : childTemplateName == defaultTemplateName
                : childTemplateName == customTemplateName;
        }

        protected override object GetChildTemplate(string templateName, object model = null, bool silentlyContinueOnError = false, Func<string, string, Type, object, bool> customResolverDelegate = null)
        {
            if (model is IMetadataContainer && base.GetChildTemplate(templateName, model, true, customResolverDelegate) == null)
            {
                return ((IMetadataContainer)model).Metadata.GetMetadataValues<object>(templateName) ?? base.GetChildTemplate(templateName, model, silentlyContinueOnError, customResolverDelegate);
            }
            return base.GetChildTemplate(templateName, model, silentlyContinueOnError, customResolverDelegate);
        }

        public override void RenderTemplate(object template, object model, int? iterationNumber = null, int? iterationCount = null, string resolveTemplateName = null)
        {
            var metadata = template as IEnumerable<object>;
            if (metadata != null)
            {
                var first = true;
                foreach (var item in metadata)
                {
                    var contextualLiteral = item as ContextualLiteral;
                    if (contextualLiteral != null)
                    {
                        var str = contextualLiteral.ToString(template, model);
                        if (str == null) continue;
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            GenerationEnvironment.AppendLine();
                        }
                        GenerationEnvironment.Append(str);
                    }
                    else
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            GenerationEnvironment.AppendLine();
                        }

                        var literal = item as Literal;
                        if (literal != null)
                        {
                            GenerationEnvironment.Append(literal.Value);
                        }
                        else
                        {
                            GenerationEnvironment.Append(item.ToString());
                        }
                    }
                }
                return;
            }
            base.RenderTemplate(template, model, iterationNumber, iterationCount, resolveTemplateName);
        }


    }
    #endregion

}