// Include libraries
#r "packages/FAKE/tools/FakeLib.dll"
open Fake 

// Properties
let outputDir = "./output/"
let buildDir = outputDir + "build/"

// Target definitions
Target "BuildApp" (fun _ ->
    !! "src/**/*.csproj"
      |> MSBuildRelease buildDir "Build"
      |> Log "AppBuild-Output: "
)

Target "Deploy" (fun _ ->
    trace "Deploying the application..."
)

// Target dependencies
"BuildApp"
   ==> "Deploy"

// Start the build process
Run "Deploy"