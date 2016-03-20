[<AutoOpen>]
module SetupThespian

#load @"..\..\..\packages\MBrace.Thespian\MBrace.Thespian.fsx"
#load @"load-references-debug.fsx"

// Remember, highlight multiple lines and hit LALT + ENTER to send them to F# Interactive
open System.IO
open MBrace.Thespian

ThespianWorker.LocalExecutable <- Path.Combine(__SOURCE_DIRECTORY__, @"..\..\..\packages\MBrace.Thespian\tools\mbrace.thespian.worker.exe")

let createLocalCluster nodes =
    ThespianCluster.InitOnCurrentMachine(nodes,
                                         logger = new ConsoleLogger(), 
                                         logLevel = LogLevel.Info)

