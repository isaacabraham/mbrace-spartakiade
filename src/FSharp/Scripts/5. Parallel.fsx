#load "SetupThespian.fsx"
#load @"..\GeoCoder.fs"

open System
open System.Diagnostics
open MBrace.Core
open MBrace.Thespian
open MBrace.Library.Cloud

let cluster = createLocalCluster(4)

(* F# has a powerful and easy-to-use set of collections modules -
   Seq (lazy, IEnumerable), List (immutable lists), and Array (.NET arrays) *)

let numbers = [ 1 .. 20 ] // create a list 1 -> 20
let numbersArray = [| 1 .. 20 |] // create a .net array 1 -> 20

// Here we "pipe" the numbers array into the Array.map method to get back squares.
let doubleDigits =
    numbersArray
    |> Array.map(fun number -> number * number)

// 1. You can run *simple* tasks in parallel across the cluster using the Balanced module
let balanced =
    numbersArray
    |> Balanced.map(fun i -> i * i) // This code executed on a node.
    |> cluster.Run

// 1.1. You can do multiline lambdas easily. See that the results show in worker windows.
let balancedBig =
    numbersArray
    |> Balanced.map(fun i ->
        printfn "I'm doing work!"
        i * i)
    |> cluster.Run

(* 2. For tasks that have more complex distribution (or pipelines) you can use the
   generalised Cloud.Parallel operator. Here we create 20 cloud computations, and send them
   to the cluster in parallel. *)
let tasks = [ for i in 1 .. 20 -> cloud { return i * i } ]

let results =
    tasks
    |> Cloud.Parallel // go from (array of Cloud<int>) to (Cloud<array of int>)
    |> cluster.Run


// 3. Convert all of these lat / longs into addresses.

let locations =
    [ 51.556021, -0.279519
      52.516275, 13.377704
      38.897676, -77.036530
      52.525084, 13.369402 ]

(* You can use the GeoCoding module to lookup an address e.g.
   GeoCoding.lookupAddress(googleKey, 12.0, -40.0)
   You can use Balanced.map or Parallel, whichever you prefer. *)