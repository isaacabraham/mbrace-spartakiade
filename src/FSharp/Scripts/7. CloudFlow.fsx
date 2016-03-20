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

(*

CloudFlow is a distributed data flow library - think LINQ but scalable across machines.
To get "into" a CloudFlow, use one of the following CloudFlow.Of combinators e.g.

* CloudFlow.OfArray (from a plain array)
* CloudFlow.OfCloudFileByLine (from one or many CloudFiles)
* CloudFlow.OfHttpFileByLine (from one or many HTTP urls)

Then you can perform any number of operations on the flow, piped together. These operators
follow the set of operations you can do in Seq / List / Array closely, so they will be
familiar to any F# developer. They also have LINQ equivalents e.g.

* CloudFlow.map (LINQ Select)
* CloudFlow.filter (LINQ Where)
* CloudFlow.distinct (LINQ Distinct?)
* CloudFlow.groupBy (LINQ GroupBy) etc. etc.

Where you are done, you must "exit" the CloudFlow by calling either a function that
accumulates data into a single value, or starts with "to" e.g

* CloudFlow.toArray (to a normal Array)
* CloudFlow.toTextCloudFiles (to a set of CloudFiles)
* CloudFlow.sum (LINQ Sum)
* CloudFlow.average (LINQ Average?)
* CloudFlow.exists (LINQ Any)

You will end up with a Cloud<T> e.g. Cloud<string array> or Cloud<int> etc. which you
can then run against the cluster as normal.

*)

(* 1. Start by opening a handle to a book we already inserted into the cluster as a
   CloudFlow *)

let huckleberryFinn = CloudFlow.OfCloudFileByLine("books/huckleberryfinn.txt")

(* With this we can write distributed queries against the book.

   2. Count the number of lines in the book
   All of the "combinators" live inside the CloudFlow module. *)

let lines =
    huckleberryFinn
    |> CloudFlow.length // number of rows. Accumulator, so ends the flow.
    |> cluster.Run

// 3. We can chain up CloudFlow calls, just like LINQ.
let linesLongerThanFiftyCharacters =
    huckleberryFinn
    |> CloudFlow.filter(fun line -> line.Length > 50) // only lines with more than 50 characters.
    |> CloudFlow.length
    |> cluster.Run

(* 4. Convert the book to uppercase and get back the first 50 rows of the book.
   Use CloudFlow.map to convert each line to uppercase
   Then use Take to restrict to 50 rows
   Finally use toArray to get "out" of the CloudFlow<string []> and back to Cloud<string []>. *)

(* 5. Find out the number of total words in the book.
   a. You need to split each line into words
   b. For each line, get back the length of the array of words
   c. Call sum to add them. *)

(* 6. Get the ten most popular words in all books
   You will need to use either groupBy, countBy or sumByKey.
   Use sortByDescending to get the "top ten" words
   CloudFlow.ofCloudFile can take in a list [ ] of books e.g. [ "book1"; "book2"; "book3"; ] *)

let wordFrequency = [ "word", 10; "otherWord", 20 ]

// You can chart the results easily: 
open XPlot.GoogleCharts

wordFrequency
|> Chart.Column
|> Chart.Show