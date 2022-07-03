// Learn more about F# at http://fsharp.org

open System.IO
open System.Text.RegularExpressions

let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
    else None

let win32pattern = @"<p>(?<decimal>\d+)<\/p>.*?<p>(?<hex>0x[0-9A-F]{8})<\/p>.+?<p>(?<name>[A-Z_<br \/>]+)<\/p>.*?<p>(?<description>.*?)<\/p>" 

let writeAllLines filename contents = File.WriteAllLines(filename, contents)

[<EntryPoint>]
let main argv =
    let htmlContent = File.ReadAllText("../../../win32_errors.html")
    Regex(win32pattern, RegexOptions.Singleline).Matches(htmlContent)
    |> Seq.map (fun m -> m.Groups)
    |> Seq.map (fun g -> $"""    {g.["name"]}:={g.["decimal"]},""")
    |> Seq.toArray
    |> writeAllLines "enum_win32error.txt"

    0
    
