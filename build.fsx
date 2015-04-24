// Include libraries
#r "packages/FAKE/tools/FakeLib.dll"
open Fake 
open Fake.AssemblyInfoFile

// Get all external references via NuGet
RestorePackages()

// Properties
let outputDir = "./output/"
let buildDir = outputDir + "build/"
let testDir = outputDir + "tests/"
let deployDir = outputDir + "deploy/"
let testResults = testDir + "TestResults.xml"

let version = "0.1"

// Target definitions
Description "Cleans the complete output."
Target "Clean" (fun _ ->
    CleanDirs [outputDir; deployDir]
)

Description "Creates assembly info files for all three projects."
Target "CreateAssemblyInfos" (fun _ ->
    CreateCSharpAssemblyInfo "./src/Twainsoft.SimpleApplication/Properties/AssemblyInfo.cs"
        [Attribute.Title "Calculator Command Line Application"
         Attribute.Description "Sample Project for the dotnetpro FAKE Article"
         Attribute.Guid "96CAFDED-19CC-4409-AD99-454311AC76C8"
         Attribute.Product "Calculator"
         Attribute.Version version
         Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./src/Twainsoft.SimpleApplication.Lib/Properties/AssemblyInfo.cs"
       [Attribute.Title "Calculator Library"
        Attribute.Description "Sample Project for the dotnetpro FAKE Article"
        Attribute.Guid "8617DA9B-055D-4B27-8D74-D36BA9C821E2"
        Attribute.Product "Calculator"
        Attribute.Version version
        Attribute.FileVersion version]

    CreateCSharpAssemblyInfo "./tests/Twainsoft.SimpleApplication.Tests/Properties/AssemblyInfo.cs"
       [Attribute.Title "Calculator Library Tests"
        Attribute.Description "Sample Project for the dotnetpro FAKE Article"
        Attribute.Guid "DC26E96E-23D5-45D0-B19C-FFDD0432402F"
        Attribute.Product "Calculator"
        Attribute.Version version
        Attribute.FileVersion version]
)

Description "Builds all C# projects located in the src folder."
Target "BuildApp" (fun _ ->
    !! "src/**/*.csproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)

Description "Builds all C# test projects located in the tests folder."
Target "BuildTest" (fun _ ->
    !! "tests/**/*.csproj"
      |> MSBuildDebug testDir "Build"
      |> Log "TestBuild-Output: "
)

Description "Runs all tests located within the *.Tests.dll assembly."
Target "RunTest" (fun _ ->
    !! (testDir + "/*.Tests.dll")
      |> NUnit (fun p ->
          {p with
             DisableShadowCopy = true;
             OutputFile = testResults })
)

Description "Deploys the application as a zip file."
Target "DeployZip" (fun _ ->
    !! (buildDir + "/**/*.*")
    -- "*.zip"
    |> Zip buildDir (deployDir + "Calculator." + version + ".zip")
)

// Target dependencies
"Clean"
   ==> "CreateAssemblyInfos"
   ==> "BuildApp"
   ==> "BuildTest"
   ==> "RunTest"
   ==> "DeployZip"

// Start the build process
RunTargetOrDefault "DeployZip"