﻿global using System.ComponentModel.DataAnnotations;
global using System.Globalization;
global using System.Text;
global using AutoFixture;
global using AutoFixture.AutoNSubstitute;
global using ClassFramework.CsharpExpressionCreator.Extensions;
global using ClassFramework.Domain;
global using ClassFramework.Domain.Abstractions;
global using ClassFramework.Domain.Builders;
global using ClassFramework.Domain.Builders.Abstractions;
global using ClassFramework.Domain.Builders.CodeStatements;
global using ClassFramework.Domain.Builders.Extensions;
global using ClassFramework.Domain.Builders.Types;
global using ClassFramework.Domain.Domains;
global using ClassFramework.Domain.Types;
global using ClassFramework.Pipelines.Builder;
global using ClassFramework.Pipelines.Entity;
global using ClassFramework.Pipelines.Extensions;
global using ClassFramework.Pipelines.Interface;
global using ClassFramework.Pipelines.Reflection;
global using ClassFramework.TemplateFramework.Builders;
global using ClassFramework.TemplateFramework.Extensions;
global using ClassFramework.TemplateFramework.Models;
global using ClassFramework.TemplateFramework.ViewModels;
global using CrossCutting.ProcessingPipeline;
global using CrossCutting.Utilities.Parsers.Extensions;
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
