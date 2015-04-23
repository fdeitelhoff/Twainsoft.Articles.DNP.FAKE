// Include libraries
#r "packages/FAKE/tools/FakeLib.dll"
open Fake 

// Get all external references via NuGet
RestorePackages()

// Properties
let outputDir = "./output/"
let buildDir = outputDir + "build/"
let testDir = outputDir + "tests/"
let testResults = testDir + "TestResults.xml"

// Target definitions
Target "Clean" (fun _ ->
    CleanDir outputDir
)

Target "BuildApp" (fun _ ->
    !! "src/**/*.csproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
    !! "tests/**/*.csproj"
      |> MSBuildDebug testDir "Build"
      |> Log "TestBuild-Output: "
)

Target "RunTest" (fun _ ->
    !! (testDir + "/*.Tests.dll")
      |> NUnit (fun p ->
          {p with
             DisableShadowCopy = true;
             OutputFile = testResults })
)

Target "Deploy" (fun _ ->
    trace "Deploying the application..."
)

// Target dependencies
"Clean"
   ==> "BuildApp"
   ==> "BuildTest"
   ==> "RunTest"
   ==> "Deploy"

// Start the build process
Run "Deploy"