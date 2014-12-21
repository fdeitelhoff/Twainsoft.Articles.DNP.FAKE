#r "packages/FAKE/tools/FakeLib.dll"
open Fake 

Target "Build" (fun _ ->
    trace "Building the application..."
)

Target "Deploy" (fun _ ->
    trace "Deploying the application..."
)

"Build"
   ==> "Deploy"

Run "Deploy"