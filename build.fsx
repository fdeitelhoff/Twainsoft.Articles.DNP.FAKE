// Include FAKE libraries
#r "packages/FAKE/tools/FakeLib.dll"
open Fake 
open Fake.AssemblyInfoFile

// Include custom tasks
#r @"tools\tasks\Twainsoft.Articles.DNP.FAKE.RandomTask.dll"
open Twainsoft.Articles.DNP.FAKE.RandomTask

// Get all external references via NuGet
RestorePackages()

// Properties
let outputDir = "./output/"
let buildDir = outputDir + "build/"
let testDir = outputDir + "tests/"
let deployDir = outputDir + "deploy/"
let packagingDir = outputDir + "nuget-packaging/"
let testResults = testDir + "TestResults.xml"

let version = RandomVersionTask.RandomVersion(4, 18)

// Target definitions
Description "Cleans the complete output."
Target "Clean" (fun _ ->
    CleanDirs [outputDir; testDir; deployDir]
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

Description "Creates a NuGet package without publishing it."
Target "CreateNuGetPackage" (fun _ ->
    CopyFiles (packagingDir) !! (buildDir + "/**/*.*")

    NuGet (fun p -> 
        {p with
            Authors = ["Fabian Deitelhoff"]
            Project = "Calculator"
            Description = "Sample Project for the dotnetpro FAKE Article"
            OutputPath = deployDir
            Summary = "Sample Project for the dotnetpro FAKE Article"
            WorkingDir = packagingDir
            Version = version
            AccessKey = "ABCDEFG"
            Files = ["*.*", Some "lib/net45", Some "*.pdb;*.exe;*.config"]
            Publish = false }) 
            "./resources/calculator.nuspec"
)

Target "FxCopCheck" (fun () ->  
    !! (buildDir + @"\**\*.dll") 
    ++ (buildDir + @"\**\*.exe") 
    |> FxCop 
        (fun p -> 
            {p with
              ReportFileName = testDir + "FXCopResults.xml"
              //FailOnError = FxCopErrorLevel.CriticalWarning
              ToolPath = @"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Team Tools\Static Analysis Tools\FxCop\FxCopCmd.exe"})
)

// Target dependencies
"Clean"
   ==> "CreateAssemblyInfos"
   ==> "BuildApp"
   ==> "FxCopCheck"
   ==> "BuildTest"
   ==> "RunTest"
   ==> "CreateNuGetPackage"
   ==> "DeployZip"

// Start the build process
RunTargetOrDefault "DeployZip"