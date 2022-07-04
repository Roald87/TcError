// Learn more about F# at http://fsharp.org

open System.IO
open System.Text.RegularExpressions

let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
    else None

let win32pattern = @"<p>(?<decimal>\d+)<\/p>.*?<p>(?<hex>0x[0-9A-F]{8})<\/p>.+?<p>(?<name>[A-Z][\w<br \/>]+)[.]?<\/p>.*?<p>(?<description>.*?)<\/p>" 

let writeAllLines filename contents = File.WriteAllLines(filename, contents)

let rereplace (pattern: string) (replacement: string) (input: string) =  Regex.Replace(input, pattern, replacement)

let replace (oldString: string) (newString: string) (input: string) = input.Replace(oldString, newString)

let getErrorDetails html =
    Regex(win32pattern, RegexOptions.Singleline).Matches(html)
    |> Seq.map (fun m -> m.Groups)

let toEnum groups = 
    groups
    |> Seq.map (fun (g: GroupCollection) -> $"""    {g.["name"]} := {g.["decimal"]},""")
    |> Seq.toArray
    
let toDescription groups =
    groups 
    |> Seq.map (
        fun (g: GroupCollection) -> 
            let description = g.["description"].ToString() |> replace "'" ""
            $"""    Win32ErrorCodes.{g.["name"]}: Win32ErrorCodeDescription := '{description}';""")
    |> Seq.toArray

[<EntryPoint>]
let main argv =
    let errors = 
        File.ReadAllText("../../../win32_errors.html") 
        |> rereplace @"<br />" ""
        |> getErrorDetails
    
    errors
    |> toEnum
    |> writeAllLines "enum_win32error.txt"

    errors
    |> toDescription
    |> writeAllLines "description_win32error.txt"

    0
    
