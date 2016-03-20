#load "SetupThespian.fsx"
#load @"..\GeoCoder.fs"

open System
open MBrace.Core
open MBrace.Thespian
open MBrace.Library.Cloud
open System.IO
open System.Net
open MBrace.Flow

let cluster = createLocalCluster(4)

(* In this lab we'll look at concurrent operations.

1. Let's see how to send the same work item to all nodes. *)

let clearConsole = cloud {
    // TODO: Clear the Console!
    return ()
    }

clearConsole
|> Cloud.ParallelEverywhere // Submit the same computation to all nodes.
|> cluster.Run

// Let's make a helper function for later...
let clearAllConsoles() = clearConsole |> Cloud.ParallelEverywhere |> cluster.Run |> ignore

(* 1.2 You can also use this to send the same code that gets executed on each node and
   get results back locally before collating them. This can be useful if e.g.
   you want to perform some calculation multiple times and take the average of all the
   results. This could be used for stress testing for example. Notice you go from just
   one Cloud<float> to a array of floats. *)
let results =
    cloud {
        // do to: return a random Double between 1 and 10
        return 0.
    }
    |> Cloud.ParallelEverywhere
    |> cluster.Run

let average = results |> Seq.average

(* 2. Let's now consider sending the same job to all nodes but only getting a single
   result back. To do this, we have to also use the Option type in F#. The first node
   to return "Some" result will win; any that return "None" are discarded.

   You could have a need for this if e.g. you are connecting to a service that times
   out often or is unreliable *)

let workflow =
    let workerCount = cluster.Workers.Length
    cloud {
        if Random().Next(1, 5) = 1 then
            printfn "Yes!"
            return Some "Hello"
        else
            printfn "No!" // check console output of all workers.
            return None
    }

// Notice that the result is also an "option" - it's possible none of the workers
// returned a result so you get nothing back.
let result =
    clearAllConsoles()
    workflow
    |> Cloud.ChoiceEverywhere
    |> cluster.Run
    
(*

First let's consider the situation where you want to execute multiple jobs simultaneously
(like Parallel) but only want to get a *single* result back. This is useful for e.g.
connect to multiple replicated data sources and just get the quickest result back.

*)

// Simulate multiple cloud flows that will get some data back at different times
let multipleDataSources =
    [ 1000 .. 500 .. 5000 ]
    |> List.mapi(fun index duration ->
        cloud {
            let printText = printfn "[Data Source %d / %dms]: %s" index duration
            printText "Started!"
            do! Cloud.Sleep duration
            printText "Completed!"
            return Some(index, duration)
        })

// Use Cloud.Choose to get the first result back and discard the rest
clearAllConsoles()
let firstResult = multipleDataSources |> Cloud.Choice |> cluster.Run

(*

Question: Why does only one cloud task in the last example print "Completed!"

*)