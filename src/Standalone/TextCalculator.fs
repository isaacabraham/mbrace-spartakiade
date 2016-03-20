/// Contains functionality regarding text operations.
module TextCalculator

open System

/// Splits a line into words.
let splitWords (line:string) = line.Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
/// Converts a word to lower case.
let toLower (line:string) = line.ToLower() 
/// Tests if a word is less than or equal to 3 characters.
let isShortWord (word:string) = word.Length <= 3
let byCount = snd