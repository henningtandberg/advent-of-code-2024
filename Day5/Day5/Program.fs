let input = System.IO.File.ReadAllLines "puzzle-part1.txt"

let rules =
    input
    |> Array.filter _.Contains("|")
    |> Array.map _.Split("|")
    |> Array.groupBy (fun x -> x.[0])
    |> Array.map (fun (key, value) -> (key, value |> Array.map (fun x -> x[1])))
    
let listOfUpdates =
    input
    |> Array.toList
    |> List.filter _.Contains(",")
    |> List.map (fun x ->
        x.Split(",")
        |> Array.toList
        |> List.rev) // Reverse the list to make it easier to check the rules using head

let rec checkRule (rule : string array) (update : string list) : bool =
    match update with
    | head :: tail ->
        let doesNotExist = rule |> Array.exists (fun x -> x = head) |> not
        if doesNotExist then checkRule rule tail
        else false
    | [] -> true

let rec checkRules (rules : (string * string array) array) (update : string list) : bool =
    match update with
    | head :: tail ->
        let rule = rules |> Array.tryFind (fun (key, _) -> key = head) |> Option.map snd
        
        let ruleApplies =
            match rule with
            | Some rule -> checkRule rule tail
            | None -> true
        
        if ruleApplies then checkRules rules tail
        else false
    | [] -> true

let result =
    listOfUpdates
    |> List.map (checkRules rules)
    |> List.zip listOfUpdates
    |> List.filter snd
    |> List.map fst
    |> List.map (fun x -> x |> List.item (List.length x / 2) |> int)
    |> List.sum
    
printfn $"Sum: %A{result}"