param(
    [Parameter(Mandatory = $true)]
    [string]$ProjectRoot
)

# Extract project name from folder name
$ProjectName = Split-Path $ProjectRoot -Leaf
$SrcDir = Join-Path $ProjectRoot "src"
$TestsDir = Join-Path $ProjectRoot "tests"

$ApiProjectName = "$ProjectName.App"
$TestProjectName = "$ProjectName.Tests"

$ApiProjectPath = Join-Path $SrcDir $ApiProjectName
$TestProjectPath = Join-Path $TestsDir $TestProjectName

# Ensure project root exists
if (-Not (Test-Path $ProjectRoot)) {
    Write-Host "Creating project root at $ProjectRoot..."
    New-Item -Path $ProjectRoot -ItemType Directory | Out-Null
}

Set-Location $ProjectRoot
Write-Host "Project root detected: $ProjectRoot"
Write-Host "Project name: $ProjectName"

# Create solution
dotnet new sln -n $ProjectName

# Create global.json for SDK pinning
dotnet new globaljson --sdk-version 8.0.416

# Create src and tests folders
New-Item -Path $SrcDir -ItemType Directory -Force | Out-Null
New-Item -Path $TestsDir -ItemType Directory -Force | Out-Null

# ===== Create Web API project =====
Set-Location $SrcDir
dotnet new webapi -n $ApiProjectName

# ===== Create Test project =====
Set-Location $TestsDir
dotnet new xunit -n $TestProjectName

# Back to root
Set-Location $ProjectRoot

# Add projects to solution
dotnet sln add "$ApiProjectPath/$ApiProjectName.csproj"
dotnet sln add "$TestProjectPath/$TestProjectName.csproj"

# Add reference: tests → app
dotnet add "$TestProjectPath/$TestProjectName.csproj" reference "$ApiProjectPath/$ApiProjectName.csproj"

# ===== Install test dependencies =====
Set-Location $TestProjectPath

dotnet add package Microsoft.NET.Test.Sdk
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package xunit
dotnet add package Moq
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector

# Back to root directory
Set-Location $ProjectRoot

# ===== Run tests =====
dotnet test

Write-Host ""
Write-Host "Setup complete!"