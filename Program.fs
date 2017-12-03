open System.Diagnostics
open System.Text.RegularExpressions
open System.IO
open System

let search file pattern =
    let r = Regex(pattern, RegexOptions.ExplicitCapture ||| RegexOptions.Compiled)
    let delims = [|';'; '"'; ' '|]
    [| for line in File.ReadLines file |> Seq.take 10_000_000 do
         match r.Match line with
         | m when m.Success -> 
             yield m.Value.Split(delims, StringSplitOptions.RemoveEmptyEntries) |> String.concat "."
         | _ -> () |]

[<EntryPoint>]
let main _ =
    let sw = Stopwatch.StartNew()
    let results = search @"d:\big.txt" @"\{.*(?<name>Microsoft.*)\|\]"
    sw.Stop()
    printfn "Found %d lines, Elapsed %O" results.Length sw.Elapsed
    0