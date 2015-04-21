// Include libraries
#r "packages/FAKE/tools/FakeLib.dll"
open Fake 

// Properties
let outputDir = "./output/"
let buildDir = outputDir + "build/"
let testDir = outputDir + "tests/"

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

Target "Deploy" (fun _ ->
    trace "Deploying the application..."
)

// Target dependencies
"Clean"
   ==> "BuildApp"
   ==> "BuildTest"
   ==> "Deploy"

// Start the build process
Run "Deploy"