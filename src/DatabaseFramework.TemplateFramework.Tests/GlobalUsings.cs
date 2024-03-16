﻿global using System.Collections.ObjectModel;
global using System.ComponentModel.DataAnnotations;
global using System.Globalization;
global using System.Text;
global using AutoFixture;
global using AutoFixture.AutoNSubstitute;
global using CrossCutting.Common.Extensions;
global using CrossCutting.ProcessingPipeline;
global using CrossCutting.Utilities.Parsers.Extensions;
global using DatabaseFramework.Domain;
global using DatabaseFramework.Domain.Abstractions;
global using DatabaseFramework.Domain.Builders;
global using DatabaseFramework.Domain.Builders.Abstractions;
global using DatabaseFramework.Domain.Builders.Extensions;
global using DatabaseFramework.Domain.Builders.SqlStatements;
global using DatabaseFramework.Domain.Domains;
global using DatabaseFramework.Domain.SqlStatements;
global using DatabaseFramework.TemplateFramework.Builders;
global using DatabaseFramework.TemplateFramework.CodeGenerationProviders;
global using DatabaseFramework.TemplateFramework.Extensions;
global using FluentAssertions;
global using Microsoft.Extensions.DependencyInjection;
global using NSubstitute;
global using TemplateFramework.Abstractions;
global using TemplateFramework.Abstractions.CodeGeneration;
global using TemplateFramework.Core;
global using TemplateFramework.Core.CodeGeneration;
global using TemplateFramework.Core.CodeGeneration.Extensions;
global using TemplateFramework.Core.Extensions;
global using TemplateFramework.Core.GenerationEnvironments;
global using TemplateFramework.TemplateProviders.ChildTemplateProvider.Extensions;
global using Xunit;
