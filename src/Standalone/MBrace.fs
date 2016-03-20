/// Contains helpers around MBrace
module MBraceHelpers

open MBrace.Flow
open MBrace.Thespian
open System.IO
open TextCalculator
open MBrace.Core

let createCluster() =
    ThespianWorker.LocalExecutable <- Path.Combine(@"..\..\..\..\packages\MBrace.Thespian\tools\mbrace.thespian.worker.exe")
    ThespianCluster.InitOnCurrentMachine(4, logger = new ConsoleLogger(), logLevel = LogLevel.Info)

/// Uploads a file
let uploadFile (file:string) =
    let cloudFilename = Path.GetFileName file
    cloud {
        let! exists = CloudFile.Exists cloudFilename
        if not exists then
            do! CloudFile.Upload(file, cloudFilename) |> Cloud.Ignore
    }

/// Performs a word count over the cloud file path supplied.
let getWordCount numberOfResults (file:string) =
    file
    |> CloudFlow.OfCloudFileByLine
    |> CloudFlow.map toLower
    |> CloudFlow.collect splitWords
    |> CloudFlow.filter (not << isShortWord)
    |> CloudFlow.countBy id
    |> CloudFlow.sortByDescending byCount numberOfResults
    |> CloudFlow.toArray

/// Uploads a file and performs a word count on it once uploaded to the store.
let performWordCountFromLocalFile numberOfResults file =
    cloud {
        do! uploadFile file |> Cloud.Ignore
        return! getWordCount numberOfResults (Path.GetFileName file)
    }    