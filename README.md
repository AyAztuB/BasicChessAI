# BasicChessAI
This is a basic AI (in c# 9.0 using .NET 5.0) that can play chess in a terminal.\
This game doesn't support the _en passant_ capture move.
## Display
The board is displayed really simply. There are no colors for the diagonals.\
Below is a summary table of the different symbols:

|symbol|signification|
|------|-------------|
|p|pawn|
|r|rook|
|n|knight|
|b|bishop|
|q|queen|
|k|king|

The uppercase letters are for the black player and the lowercase ones are for the white player.

You will also see different colors. The table below explain each color in importance order.

|color|signification|
|-----|-------------|
|red|king is check|
|green|possible moves of the selected piece|
|purple|last move (piece and previous tile)|
|blue|black pieces|

If a black piece move, it will be colored in purple (purple is more important than blue).

## How to use it

After compiling the program, you can execute it with arguments or not.\
The following table will show you the available arguments.

|argument|signification|
|--------|-------------|
|`--vs`|you can play against an another player|
|`--vsBot`|you can play against the AI|
|`--bot`|the AI will play against itself|
|`--withoutEnter`|You don't need to press enter before playing your turn|

The 3 first arguments can't be used simultaneously. If you used 2 arguments, the order isn't important. If you launch it without argument, the program will be launched as you used the first argument.


## Basic modifications

Some basic modifications can upgrade the AI.

You can add some heuristic functions or modify the values (in the `heuristics.cs` file).

You also can increment the `maxDepth` value in `AI.cs`.

You also can add the _en passant_ capture move or add the possibility for the AI to choose between a queen and a knight when a pawn is promoted.

Finally, you can add some options using the difficulty argument in the AI constructor (like change `maxDepth` or `heuristic` attributes).

## Contribution

Everyone can contribute to this project. However, I will ask every contributors to use explicit variable and function name, add comment if it's not simple to know what is done, and to precisely explain what modification was done in the commit section (what is added, deleted, changed, where it is, how it's use and how to use it, potentially why).

To become a contributor, please contact me explaining me what you are going to do on the project and giving me your github account name or email.

## License

This project is free to use !

## Contact me

If you have a trouble (after research obviously) or for anything else about this project, you can contact me at ayaztub@gmail.com or on discord via private message at `AyAztuB#2419`. When you contact me, please use a specific subject (like _basic chess AI_) and precisely explain what your problem with.
