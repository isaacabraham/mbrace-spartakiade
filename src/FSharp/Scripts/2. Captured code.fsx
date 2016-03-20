#load "SetupThespian.fsx"

open System
open System.Diagnostics
open MBrace.Core
open MBrace.Thespian

let cluster = createLocalCluster(4)

// 1. Here's a simple function. We want to carry this out on the cluster.
let addNumbers(a, b) =
    printfn "Adding %d and %d" a b
    a + b

(* Easy - wrap it in cloud { }. MBrace will send across the function to the node
   as a compiled .NET assembly automatically. *)
let answer =
    cloud {
        return addNumbers(5, 10)
    } |> cluster.Run

// Where is the Console output? Check the worker output windows!





// 2. MBrace will also automatically capture objects and send across to the cluster
let firstNumber = 10
let otherAnswer = cloud { return addNumbers(firstNumber, 5) } |> cluster.Run