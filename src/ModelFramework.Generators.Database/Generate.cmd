C:
cd C:\Project\Prive\GenericCodeGen\TextTemplateTransformationFramework.T4.Plus.Cmd\bin\Debug\net5.0
dotnet TextTemplateTransformationFramework.T4.Plus.Cmd.dll source -f "C:\Project\Prive\GenericCodeGen\ModelFramework.Generators.Database\Templates\SqlServerDatabaseSchemaGenerator.template" -o "C:\Project\Prive\GenericCodeGen\ModelFramework.Generators.Database\SqlServerDatabaseSchemaGenerator.cs" -Parameters $T4Plus.BasePath:C:\Project\Prive\GenericCodeGen\
pause
