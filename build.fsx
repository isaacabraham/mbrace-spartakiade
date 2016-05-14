#r @"packages/FAKE/tools/FakeLib.dll"

open Fake

Target "Build" (fun _ ->
    MSBuildHelper.MSBuildRelease "" "Rebuild" [ @"src\MbraceExercises.sln" ]
    |> ignore
  )
  
RunTargetOrDefault "Build"