//let input = System.IO.File.ReadAllLines "puzzle-test.txt"
let input = System.IO.File.ReadAllLines "puzzle.txt"

let rules =
    input
    |> Array.filter _.Contains("|")
    |> Array.map _.Split("|")
    |> Array.groupBy (fun x -> x.[0])
    |> Array.map (fun (key, value) -> (key, value |> Array.map (fun x -> x[1]) |> Array.toList))
    
let listOfUpdates =
    input
    |> Array.toList
    |> List.filter _.Contains(",")
    |> List.map (fun x ->
        x.Split(",")
        |> Array.toList
        |> List.rev) // Reverse the list to make it easier to check the rules using head

let rec checkRule (rule : string list) (update : string list) : bool =
    match update with
    | head :: tail ->
        let doesNotExist = rule |> List.exists (fun x -> x = head) |> not
        if doesNotExist then checkRule rule tail
        else false
    | [] -> true

let rec isValidUpdate (rules : (string * string list) array) (update : string list) : bool =
    match update with
    | head :: tail ->
        let rule = rules |> Array.tryFind (fun (key, _) -> key = head) |> Option.map snd
        
        let ruleApplies =
            match rule with
            | Some rule -> checkRule rule tail
            | None -> true
        
        if ruleApplies then isValidUpdate rules tail
        else false
    | [] -> true

let rec getMiddleElement (update : string list) : int =
    update
    |> List.item (List.length update / 2)
    |> int

let resultPart1 =
    listOfUpdates
    |> List.filter (isValidUpdate rules)
    |> List.map getMiddleElement
    |> List.sum
    
printfn $"Result part1: %A{resultPart1}"
    
let isNotValidUpdate (rules : (string * string list) array) (update : string list) : bool =
    isValidUpdate rules update |> not
    
let rec getMaxIndex (rule : string list) (update : string list) : int =
    match rule with
    | head :: tail ->
        let index = update |> List.tryFindIndex (fun x -> x = head) |> Option.defaultValue 0
        let maxIndex = getMaxIndex tail update
        
        if index > maxIndex then index
        else maxIndex
    | [] -> 0

let rec insert v i l =
    match i, l with
    | 0, xs -> v::xs
    | i, x::xs -> x::insert v (i - 1) xs
    | i, [] -> failwith "index out of range"
    
let rec sortUpdateByRules (rules : (string * string list) array) (i : int) (update : string list) : string list =
    match i, update with
    | 0, head :: tail -> head :: tail
    | i, head :: tail ->
        let rule = rules |> Array.tryFind (fun (key, _) -> key = head) |> Option.map snd
        
        if isValidUpdate rules (head::tail) then head::tail
        else 
            let indexToPlaceHead =
                match rule with
                | Some rule -> getMaxIndex rule (head::tail)
                | None -> (head::tail) |> List.tryFindIndex (fun x -> x = head) |> Option.defaultValue 0
        
            if indexToPlaceHead = 0 then head :: sortUpdateByRules rules (i - 1) tail
            else sortUpdateByRules rules (i - 1) (insert head indexToPlaceHead tail)
    | i, [] -> []

let rec sortUntilValid (rules : (string * string list) array) (update : string list) : string list =
    if isValidUpdate rules update then update
    else sortUntilValid rules (sortUpdateByRules rules (List.length update - 1) update)

let resultPart2 =
    listOfUpdates
    |> List.filter (isNotValidUpdate rules)
    |> List.map (sortUntilValid rules)
    |> List.map getMiddleElement 
    |> List.sum
    
printfn $"Result part2: %A{resultPart2}"
