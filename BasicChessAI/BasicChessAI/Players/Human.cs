using System;
using BasicChessAI.Game;
using BasicChessAI.Game.Pieces;

namespace BasicChessAI.Players
{
    public class Human : Player
    {
        public Human(Color color, Board board) : base(PlayerType.Human, color, board)
        {
        }

        public override (Tile, Tile) PlayOneRound(Tile lastMove, Tile nextPos)
        {
            string input;
            Tile nextLastMove;
            Tile nextNextPos;
            int x;
            int y;
            bool isFirst;
            do
            {
                isFirst = true;
                do
                {
                    Console.Clear();
                    if (!isFirst)
                        Console.WriteLine("Invalid Board Piece : please enter position with 'a1' format for example");
                    isFirst = false;
                    Console.WriteLine("It's your turn " + (this.PlayerColor == Color.Black ? "black" : "white") + " !");
                    this.Board.Print(lastMove, nextPos, this.PlayerColor);
                    Console.Write("\nChoose a piece to move : ");
                    input = Console.ReadLine();
                } while (String.IsNullOrEmpty(input) || !IsValid(input, out x, out y));

                nextLastMove = this.Board._Board[x, y];
                Piece pieceToMove = nextLastMove.Piece;
                isFirst = true;

                do
                {
                    Console.Clear();
                    if (!isFirst)
                        Console.WriteLine(
                            "Invalid Board Piece : please enter position with 'a1' format for example / enter 'back' to choose an another piece");
                    isFirst = false;
                    Console.WriteLine("It's your turn " + (this.PlayerColor == Color.Black ? "black" : "white") + " !");
                    this.Board.Print(lastMove, nextPos, this.PlayerColor, pieceToMove);
                    Console.Write("\nChoose where to move : ");
                    input = Console.ReadLine();
                } while (String.IsNullOrEmpty(input) || input != "back" && (!IsValid(input, out x, out y, false) ||
                         !this.Board.IsMoveValid(pieceToMove, x, y)));
            } while (input == "back");

            nextNextPos = this.Board._Board[x, y];
            nextLastMove.Piece.ApplyMovement(nextNextPos, false);
            
            return (nextLastMove, nextNextPos);
        }

        private bool IsValid(string s, out int x, out int y, bool MyPiece = true)
        {
            x = 0;
            y = 0;
            if (s.Length != 2 || s[0] is < 'a' or > 'h' || s[1] is < '1' or > '8')
                return false;
            x = s[0] - 'a';
            y = s[1] - '1';
            if (MyPiece)
                return this.Board._Board[x, y].Piece.Color == this.PlayerColor;
            return this.Board._Board[x, y].Piece.Color != this.PlayerColor;
        }
    }
}