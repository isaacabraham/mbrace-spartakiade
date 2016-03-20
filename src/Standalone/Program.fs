open System
open MBraceHelpers

let getTopFiftyResults = performWordCountFromLocalFile 50

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
        let path = Console.ReadLine()
        try
            let results = getTopFiftyResults path |> cluster.Run
            results |> Seq.iter (printfn "%A")
        with ex -> printfn "Something went wrong: %O" ex

        Console.ReadLine() |> ignore

    0
