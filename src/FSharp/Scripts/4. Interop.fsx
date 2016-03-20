#load "SetupThespian.fsx"

open System
open System.Diagnostics
open MBrace.Core
open MBrace.Thespian

let cluster = createLocalCluster(4)

// 1. F# can natively call out to any other NET assembly e.g.
let fifteen = Helpers.Calculator().Add(10, 5)

// We can also do this within a cloud { }!
let answerFromCSharp =
    cloud {
        //TODO: Call the Helpers.Calculator from here and return the result...
        return 99
    } |> cluster.Run

(* 2. One for you. Make a cloud computation to get the current geolocation. Helpers.GeoLocation
   has the geolocation code already.

   Remember, you can only let! on a Cloud<T>. To go from a Task<T> to Cloud<T>, you need
   to use Cloud.AwaitTask (as we did previously with Cloud.AwaitProcess). *)

let location =
    cloud {
        // CSharp.GeoLocation.GetLongLat("google API key")
        /// ??
        return()
    } |> cluster.Run

