﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 4.0.30319.42000
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ModelFramework.Generators.Tests.POC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [System.CodeDom.Compiler.GeneratedCode(@"T4PlusCSharpCodeGenerator", @"1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class MultipleOutputExample : MultipleOutputExampleBase
    {
        public virtual void Render(global::System.Text.StringBuilder builder)
        {
            var backup = this.GenerationEnvironment;
            if (builder != null) this.GenerationEnvironment = builder;
            Write(this.ToStringHelper.ToStringWithCulture(@"Hello, "));
            Write(this.ToStringHelper.ToStringWithCulture(Model));

            if (builder != null) this.GenerationEnvironment = backup;
        }


        protected System.String _ModelField;

        /// <summary>
        /// Access the Model parameter of the template.
        /// </summary>
        public System.String Model
        {
            get
            {
                return this._ModelField;
            }
        }

        public virtual void Initialize()
        {
            this.Errors.Clear();
            this.GenerationEnvironment.Clear();
            if (Session == null)
            {
                Session = new global::System.Collections.Generic.Dictionary<string, object>();
            }
            bool ModelValueAcquired = false;
            if (this.Session != null && this.Session.ContainsKey("Model") && this.Session["Model"] != null)
            {
                if ((typeof(System.String).IsAssignableFrom(this.Session["Model"].GetType()) == false))
                {
                    this.Error("The type \'System.String\' of the parameter \'Model\' did not match the type of the data passed to the template.");
                }
                else
                {
                    this._ModelField = ((System.String)(this.Session["Model"]));
                    ModelValueAcquired = true;
                }
            }
            if ((ModelValueAcquired == false))
            {
                this._ModelField = default(System.String);
                ModelValueAcquired = true;
            }

        }

    }

    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute(@"T4PlusCSharpCodeGenerator", @"1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class MultipleOutputExampleBase
    {
        #region CompilerError
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public class CompilerError
        {
            public CompilerError(int column, string errorNumber, string errorText, string fileName, bool isWarning, int line)
            {
                Column = column;
                ErrorNumber = errorNumber;
                ErrorText = errorText;
                FileName = fileName;
                IsWarning = isWarning;
                Line = line;
            }

            public int Column { get; private set; }
            public string ErrorNumber { get; private set; }
            public string ErrorText { get; private set; }
            public string FileName { get; private set; }
            public bool IsWarning { get; private set; }
            public int Line { get; private set; }

            public override string ToString()
            {
                return string.Format("{0}({1},{2}): {3}{4}: {5}"
                    , FileName
                    , Line
                    , Column
                    , IsWarning
                        ? "warning"
                        : "error"
                    , string.IsNullOrEmpty(ErrorNumber)
                        ? string.Empty
                        : " " + ErrorNumber
                    , ErrorText);
            }
        }
        #endregion
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.Collections.Generic.List<CompilerError> errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.Collections.Generic.List<CompilerError> Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.Collections.Generic.List<CompilerError>();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public virtual void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            CompilerError error = new CompilerError(1, "TemplateError", message, "T4GeneratedTemplateBase.cs", false, 1);
            Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            CompilerError error = new CompilerError(1, "TemplateWarning", message, "T4GeneratedTemplateBase.cs", true, 1);
            Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField = new global::System.Globalization.CultureInfo(@"en-US");
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    return null;
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] { typeof(System.IFormatProvider) });
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] { this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion


        #region Template context
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public class TemplateInstanceContext
        {
            public object Template { get; private set; }
            public object Model { get; set; }
            public string ResolveTemplateName { get; private set; }
            public TemplateInstanceContext ParentContext { get; private set; }
            public TemplateInstanceContext RootContext
            {
                get
                {
                    var p = this;
                    while (p != null && p.ParentContext != null) p = p.ParentContext;
                    return p;
                }
            }
            public T GetModelFromContextByType<T>(Func<TemplateInstanceContext, bool> predicate = null)
            {
                var p = this;
                while (p != null)
                {
                    if (p.Model != null && typeof(T).IsAssignableFrom(p.Model.GetType()))
                    {
                        return (T)p.Model;
                    }
                    p = p.ParentContext;
                }
                return default(T);
            }
            public T GetContextByType<T>(Func<TemplateInstanceContext, bool> predicate = null)
            {
                var p = this;
                while (p != null)
                {
                    if (p.Template != null && typeof(T).IsAssignableFrom(p.Template.GetType()))
                    {
                        return (T)p.Template;
                    }
                    p = p.ParentContext;
                }
                return default(T);
            }
            public bool IsRootContext
            {
                get
                {
                    return ParentContext == null;
                }
            }
            public TemplateInstanceContext CreateChildContext(object template, object model, int? iterationNumber = null, int? iterationCount = null, string resolveTemplateName = null)
            {
                return new TemplateInstanceContext
                {
                    Template = template,
                    Model = model,
                    ParentContext = this,
                    IterationNumber = iterationNumber,
                    IterationCount = iterationCount,
                    ResolveTemplateName = resolveTemplateName
                };
            }
            public static TemplateInstanceContext CreateRootContext(object template)
            {
                return new TemplateInstanceContext
                {
                    Template = template
                };
            }
            public int? IterationNumber { get; set; }
            public int? IterationCount { get; set; }
            public bool IsFirstIteration
            {
                get
                {
                    return IterationNumber.HasValue
                        && IterationCount.HasValue
                        && IterationNumber.Value == 0;
                }
            }
            public bool IsLastIteration
            {
                get
                {
                    return IterationNumber.HasValue
                        && IterationCount.HasValue
                        && IterationNumber.Value + 1 == IterationCount.Value;
                }
            }
        }
        protected TemplateInstanceContext _templateContextField;
        protected TemplateInstanceContext TemplateContext
        {
            get
            {
                if (_templateContextField == null)
                {
                    _templateContextField = TemplateInstanceContext.CreateRootContext(this);
                }
                return _templateContextField;
            }
            set { _templateContextField = value; }
        }
        #endregion
        #region Template file manager
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public class TemplateFileManager
        {
            private readonly MultipleOutputExampleBase _outputContainer;
            private readonly global::System.Text.StringBuilder _originalStringBuilder;
            private readonly MultipleContentBuilder _builder;
            private Content _currentContent;

            public TemplateFileManager(MultipleOutputExampleBase outputContainer, string basePath = null)
            {
                _outputContainer = outputContainer;
                _originalStringBuilder = outputContainer.GenerationEnvironment;
                _builder = new MultipleContentBuilder(basePath);
            }

            public StringBuilder StartNewFile(string fileName = null, bool skipWhenFileExists = false)
            {
                var builder = new global::System.Text.StringBuilder();
                _currentContent = _builder.AddContent(fileName, skipWhenFileExists, builder);
                _outputContainer.GenerationEnvironment = builder;
                return builder;
            }

            public void ResetToDefaultOutput()
            {
                _outputContainer.GenerationEnvironment = _originalStringBuilder;
            }

            public override string ToString()
            {
                return _builder.ToString();
            }

            public void Process(bool split = true, bool silentOutput = false)
            {
                ResetToDefaultOutput();

                if (split)
                {
                    _originalStringBuilder.Clear();
                    if (!silentOutput) _originalStringBuilder.Append(_builder.ToString());
                }
                else if (!silentOutput)
                {
                    foreach (var item in _builder.Contents)
                    {
                        _originalStringBuilder.Append(item.Builder.ToString());
                    }
                }
            }

            public void SaveAll()
            {
                _builder.SaveAll();
            }

            public void SaveLastGeneratedFiles(string lastGeneratedFilesPath)
            {
                _builder.SaveLastGeneratedFiles(lastGeneratedFilesPath);
            }

            public void DeleteLastGeneratedFiles(string lastGeneratedFilesPath)
            {
                _builder.DeleteLastGeneratedFiles(lastGeneratedFilesPath);
            }

            protected MultipleContentBuilder MultipleContentBuilder { get { return _builder; } }
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public class MultipleContentBuilder
        {
            private readonly global::System.Collections.Generic.List<Content> _contentList;

            public MultipleContentBuilder(string basePath = null)
            {
                _contentList = new global::System.Collections.Generic.List<Content>();
                BasePath = basePath;
            }

            public string BasePath { get; set; }

            public void SaveAll(Func<string, string> filenameTransformFunc = null, Func<string, string> contentTransformFunc = null)
            {
                foreach (var content in _contentList)
                {
                    var path = string.IsNullOrEmpty(BasePath) || global::System.IO.Path.IsPathRooted(content.FileName)
                        ? content.FileName
                        : global::System.IO.Path.Combine(BasePath, content.FileName);

                    if (filenameTransformFunc != null)
                    {
                        path = filenameTransformFunc(path);
                    }
                    var contents = content.Builder.ToString();
                    if (contentTransformFunc != null)
                    {
                        contents = contentTransformFunc(contents);
                    }
                    var dir = global::System.IO.Path.GetDirectoryName(path);
                    if (!global::System.IO.Directory.Exists(dir))
                    {
                        global::System.IO.Directory.CreateDirectory(dir);
                    }
                    global::System.IO.File.WriteAllText(path, contents, global::System.Text.Encoding.UTF8);
                }
            }

            public void SaveLastGeneratedFiles(string lastGeneratedFilesPath)
            {
                var fullPath = string.IsNullOrEmpty(BasePath) || global::System.IO.Path.IsPathRooted(lastGeneratedFilesPath)
                    ? lastGeneratedFilesPath
                    : global::System.IO.Path.Combine(BasePath, lastGeneratedFilesPath);

                var dir = global::System.IO.Path.GetDirectoryName(fullPath);
                if (!global::System.IO.Directory.Exists(dir))
                {
                    global::System.IO.Directory.CreateDirectory(dir);
                }

                global::System.IO.File.WriteAllLines(fullPath, _contentList.OrderBy(c => c.FileName).Select(c => c.FileName));
            }

            public void DeleteLastGeneratedFiles(string lastGeneratedFilesPath)
            {
                var fullPath = string.IsNullOrEmpty(BasePath) || global::System.IO.Path.IsPathRooted(lastGeneratedFilesPath)
                    ? lastGeneratedFilesPath
                    : global::System.IO.Path.Combine(BasePath, lastGeneratedFilesPath);

                if (!global::System.IO.File.Exists(fullPath))
                {
                    // No previously generated files to delete
                    return;
                }

                foreach (var fileName in global::System.IO.File.ReadAllLines(fullPath))
                {
                    var fileFullPath = string.IsNullOrEmpty(BasePath) || global::System.IO.Path.IsPathRooted(fileName)
                        ? fileName
                        : global::System.IO.Path.Combine(BasePath, fileName);

                    if (global::System.IO.File.Exists(fileFullPath))
                    {
                        global::System.IO.File.Delete(fileFullPath);
                    }
                }
            }

            public Content AddContent(string fileName = null, bool skipWhenFileExists = false, global::System.Text.StringBuilder builder = null)
            {
                var content = builder == null
                ? new Content
                {
                    FileName = fileName,
                    SkipWhenFileExists = skipWhenFileExists
                }
                : new Content(builder)
                {
                    FileName = fileName,
                    SkipWhenFileExists = skipWhenFileExists
                };
                _contentList.Add(content);
                return content;
            }

            public override string ToString()
            {
                var mc = new MultipleContents
                {
                    BasePath = BasePath,
                    Contents = _contentList.Select(c => new Contents
                    {
                        FileName = c.FileName,
                        Lines = c.Builder.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                        SkipWhenFileExists = c.SkipWhenFileExists
                    }).ToList()
                };

                var serializer = new global::System.Runtime.Serialization.DataContractSerializer(typeof(MultipleContents));
                var sb = new global::System.Text.StringBuilder();

                using (var writer = global::System.Xml.XmlWriter.Create(sb, new global::System.Xml.XmlWriterSettings { Indent = true }))
                {
                    serializer.WriteObject(writer, mc);
                }

                return sb.ToString();
            }

            public static MultipleContentBuilder FromString(string xml)
            {
                if (string.IsNullOrEmpty(xml))
                {
                    return null;
                }

                var result = new MultipleContentBuilder();

                MultipleContents mc;

                // Cope with CA2202 analysis warning by using this unusual statement with try.finally instead of nested usings
                var stringReader = new global::System.IO.StringReader(xml);
                try
                {
                    using (var reader = global::System.Xml.XmlReader.Create(stringReader))
                    {
                        // Set reference to null because the reader will destroy it
                        stringReader = null;

                        var serializer = new global::System.Runtime.Serialization.DataContractSerializer(typeof(MultipleContents));
                        mc = serializer.ReadObject(reader) as MultipleContents;
                    }
                }
                finally
                {
                    if (stringReader != null)
                    {
                        stringReader.Dispose();
                    }
                }

                if (mc == null)
                {
                    return null;
                }

                result.BasePath = mc.BasePath;
                foreach (var item in mc.Contents)
                {
                    var c = result.AddContent(item.FileName, item.SkipWhenFileExists);
                    foreach (var line in item.Lines)
                    {
                        c.Builder.AppendLine(line);
                    }
                }

                return result;
            }

            public global::System.Collections.Generic.IEnumerable<Content> Contents { get { return _contentList.AsReadOnly(); } }
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public class Content
        {
            private readonly global::System.Text.StringBuilder _builder;

            public Content()
            {
                _builder = new global::System.Text.StringBuilder();
            }

            public Content(global::System.Text.StringBuilder builder)
            {
                _builder = builder;
            }

            public string FileName { get; set; }

            public bool SkipWhenFileExists { get; set; }

            public global::System.Text.StringBuilder Builder { get { return _builder; } }
        }

        [global::System.Runtime.Serialization.DataContract(Name = "MultipleContents", Namespace = "http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework")]
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public class MultipleContents
        {
            public static MultipleContents FromXmlString(string xml)
            {
                if (string.IsNullOrEmpty(xml))
                {
                    return null;
                }

                if (xml.IndexOf(XmlStringFragment) == -1)
                {
                    return null;
                }

                MultipleContents mc;

                // Cope with CA2202 analysis warning by using this unusual statement with try.finally instead of nested usings
                var stringReader = new global::System.IO.StringReader(xml);
                try
                {
                    using (var reader = global::System.Xml.XmlReader.Create(stringReader))
                    {
                        // Set reference to null because the reader will destroy it
                        stringReader = null;

                        var serializer = new global::System.Runtime.Serialization.DataContractSerializer(typeof(MultipleContents));
                        mc = serializer.ReadObject(reader) as MultipleContents;
                    }
                }
                finally
                {
                    if (stringReader != null)
                    {
                        stringReader.Dispose();
                    }
                }

                return mc;
            }

            public static MultipleContents FromDelimitedString(string delimitedString)
            {
                if (string.IsNullOrEmpty(delimitedString))
                {
                    return null;
                }

                if (delimitedString.IndexOf(DelimitedStringFragment) == -1)
                {
                    return null;
                }

                var result = new MultipleContents { Contents = new global::System.Collections.Generic.List<Contents>() };
                var split = delimitedString.Split(new[] { DelimitedStringFragment }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < split.Length; i += 2)
                {
                    var fileName = Normalize(split[i]);
                    bool skipWhenFileExists = false;
                    if (fileName.StartsWith("(") && fileName.EndsWith(")"))
                    {
                        fileName = fileName.Substring(1, fileName.Length - 2);
                        skipWhenFileExists = true;
                    }
                    if (split.Length <= i + 1)
                    {
                        continue;
                    }
                    var lines = split[i + 1].Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
                    var c = new Contents
                    {
                        FileName = fileName,
                        SkipWhenFileExists = skipWhenFileExists,
                        Lines = lines
                    };
                    result.Contents.Add(c);
                }

                return result;
            }

            [global::System.Runtime.Serialization.DataMember]
            public string BasePath { get; set; }

            [global::System.Runtime.Serialization.DataMember]
            public global::System.Collections.Generic.List<Contents> Contents { get; set; }

            public const string XmlStringFragment = @"<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformation"">";

            public const string DelimitedStringFragment = "!@#$";

            private static string Normalize(string input)
            {
                return input
                    .Replace("\r", "")
                    .Replace("\n", "")
                    .Trim();
            }

            public void SaveAll(Func<string, string> filenameTransformFunc = null, Func<string, string> contentTransformFunc = null)
            {
                foreach (var content in Contents)
                {
                    var path = string.IsNullOrEmpty(BasePath) || global::System.IO.Path.IsPathRooted(content.FileName)
                        ? content.FileName
                        : global::System.IO.Path.Combine(BasePath, content.FileName);

                    if (filenameTransformFunc != null)
                    {
                        path = filenameTransformFunc(path);
                    }
                    var contents = string.Join("\r\n", content.Lines);
                    if (contentTransformFunc != null)
                    {
                        contents = contentTransformFunc(contents);
                    }
                    global::System.IO.File.WriteAllText(path, contents, global::System.Text.Encoding.UTF8);
                }
            }
        }

        [global::System.Runtime.Serialization.DataContract(Name = "Contents", Namespace = "http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework")]
        public class Contents
        {
            [global::System.Runtime.Serialization.DataMember]
            public string FileName { get; set; }
            [global::System.Runtime.Serialization.DataMember]
            public global::System.Collections.Generic.List<string> Lines { get; set; }
            [global::System.Runtime.Serialization.DataMember]
            public bool SkipWhenFileExists { get; set; }
        }
        #endregion

    }
    #endregion

}
