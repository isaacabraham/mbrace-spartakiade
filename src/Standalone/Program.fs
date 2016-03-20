open System
open MBraceHelpers
open MBrace.Thespian

let getTopFiftyResults = performWordCountFromLocalFile 50

let quit (cluster:ThespianCluster) =
    Console.Clear()
    printf "Shutting down..."
    cluster.KillAllWorkers()
    cluster.ClearSystemLogs()
    printfn "All done!"
    Environment.Exit(0)

let doWordCount path (cluster:ThespianCluster) =
    printfn ""
    try
        let results = getTopFiftyResults path |> cluster.Run
        results |> Seq.iter (printfn "%A")
    with ex -> printfn "Something went wrong: %O" ex
    Console.ReadLine() |> ignore

[<EntryPoint>]
let main argv =
    printf "Spinning up local cluster..."
    let cluster = MBraceHelpers.createCluster()
    printfn "Done!"

    // loop indefinitely...
    while true do
        Console.Clear()
        printfn "ULTIMATE WORD COUNT PROGRAMME"
        printfn "============================="
        printfn ""
        printf "Please provide the path to a text file to do an analysis on: "

        match Console.ReadLine() with
        | "exit" -> cluster |> quit
        | path -> cluster |> doWordCount path
    0
