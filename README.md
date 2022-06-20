# BasicChessAI
This is a basic AI (in c#) that can play chess in a terminal.\
The board representation is mine and doesn't respect any convention.\
You can change it directly in the Print function in `Board.cs` and in the IsValid function in `Human.cs`.\
This game isn't support the _en passant_ capture move.
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

## Basic modifications

Some basic modifications can upgrade the AI.

You can add some heuristic functions or modify the values (in the `heuristics.cs` file).

You also can increment the `maxDepth` value in `AI.cs`.

You also can add the _en passant_ capture move or add the possibility for the AI to choose between a queen and a knight when a pawn is promoted.

Finally, you can add some options using the difficulty argument in the AI constructor (like change `maxDepth` or `heuristic` attributes).

## Contribution

Everyone can contribute to this project. However, I will ask every contributors to use explicit variable and function name, add comment if it's not simple to know what is done, and to precisely explain what modification was done in the commit section (what is added, deleted, changed, where it is, how it's use and how to use it, potentially why).

## License

This project is free to use !

## Contact me

If you have any problem or if you don't understand any thing (after research obviously), you can contact me at ayaztub@gmail.com or on discord via pm at `AyAztuB#2419`. When you contact me, please use a specific subject (like _basic chess AI_) and precisely explain what your problem with.