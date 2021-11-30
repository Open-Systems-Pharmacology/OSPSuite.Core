@echo off
cls
set OSPversion = 12.0.2
if not exist nuget_repo mkdir nuget_repo

dotnet pack src\OSPSuite.Assets\OSPSuite.Assets.csproj -p:PackageVersion=12.0.2 --configuration Debug --include-symbols --output nuget_repo --no-build
dotnet pack src\OSPSuite.Assets.Images\OSPSuite.Assets.Images.csproj -p:PackageVersion=12.0.2 --configuration Debug --include-symbols --output nuget_repo --no-build
dotnet pack src\OSPSuite.Core\OSPSuite.Core.csproj -p:PackageVersion=12.0.2 --configuration Debug --include-symbols --output nuget_repo --no-build
dotnet pack src\OSPSuite.Infrastructure\OSPSuite.Infrastructure.csproj -p:PackageVersion=12.0.2 --configuration Debug --include-symbols --output nuget_repo --no-build
dotnet pack src\OSPSuite.Infrastructure.Castle\OSPSuite.Infrastructure.Castle.csproj -p:PackageVersion=12.0.2 --configuration Debug --include-symbols --output nuget_repo --no-build
dotnet pack src\OSPSuite.Infrastructure.Export\OSPSuite.Infrastructure.Export.csproj -p:PackageVersion=12.0.2 --configuration Debug --include-symbols --output nuget_repo --no-build 
dotnet pack src\OSPSuite.Infrastructure.Import\OSPSuite.Infrastructure.Import.csproj -p:PackageVersion=12.0.2 --configuration Debug --include-symbols --output nuget_repo --no-build
dotnet pack src\OSPSuite.Infrastructure.Reporting\OSPSuite.Infrastructure.Reporting.csproj -p:PackageVersion=12.0.2 --configuration Debug --include-symbols --output nuget_repo --no-build
dotnet pack src\OSPSuite.Infrastructure.Serialization\OSPSuite.Infrastructure.Serialization.csproj -p:PackageVersion=12.0.2 --configuration Debug --include-symbols --output nuget_repo --no-build
dotnet pack src\OSPSuite.Presentation\OSPSuite.Presentation.csproj -p:PackageVersion=12.0.2 --configuration Debug --include-symbols --output nuget_repo --no-build
dotnet pack src\OSPSuite.Presentation.Serialization\OSPSuite.Presentation.Serialization.csproj -p:PackageVersion=12.0.2 --configuration Debug --include-symbols --output nuget_repo --no-build
dotnet pack src\OSPSuite.UI\OSPSuite.UI.csproj -p:PackageVersion=12.0.2 --configuration Debug --include-symbols --output nuget_repo --no-build

Rem move /Y *.nupkg nuget_repo

pause
