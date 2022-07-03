// Learn more about F# at http://fsharp.org

open System.IO
open System.Text.RegularExpressions

let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
    else None

let win32pattern = @"<p>(\d+)<\/p>.*?<p>(0x[0-9A-F]{8})<\/p>.+?<p>([A-Z_<br \/>]+?)<\/p>.*?<p>(.*?)<\/p>" 

[<EntryPoint>]
let main argv =
    let htmlContent = File.ReadAllText("../../../win32_errors.html")
    Regex(win32pattern, RegexOptions.Singleline).Matches(htmlContent)
    |> Seq.take 3
    |> Seq.map (fun m -> m.Groups |> Seq.tail |> Seq.toArray)
    |> Seq.map (fun x -> $"{x.[2]}")
    |> Seq.iter (printfn "%s")

    0
    
