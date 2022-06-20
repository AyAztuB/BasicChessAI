using System;
using System.Collections.Generic;
using BasicChessAI.Game.Pieces;

namespace BasicChessAI.Game
{
    public class Board
    {
        private Tile[,] board;
        private List<Piece> blackPieces;
        private List<Piece> whitePieces;

        public Tile[,] _Board => this.board;
        public List<Piece> BlackPieces => this.blackPieces;
        public List<Piece> WhitePieces => this.whitePieces;

        public Board()
        {
            this.blackPieces = new List<Piece>();
            this.whitePieces = new List<Piece>();
            this.board = new Tile[8, 8];
            string start = "RNBQKBNR/PPPPPPPP/8/8/8/8/pppppppp/rnbqkbnr";
            DecodeStringRepresentation(start);
        }

        public Board(string s)
        {
            this.blackPieces = new List<Piece>();
            this.whitePieces = new List<Piece>();
            this.board = new Tile[8, 8];
            DecodeStringRepresentation(s);
        }

        public Board(Board copy)
        {
            this.blackPieces = new List<Piece>();
            this.whitePieces = new List<Piece>();
            this.board = new Tile[8, 8];
            DecodeStringRepresentation(copy.ToString());
        }

        public void AddPieces(Piece piece, Color color)
        {
            if (color == Color.Black)
                this.blackPieces.Add(piece);
            else if (color == Color.White)
                this.whitePieces.Add(piece);
        }

        public void RemovePieces(Piece piece, Color color)
        {
            List<Piece> pieces = color == Color.Black ? this.blackPieces : this.whitePieces;
            foreach (var p in pieces)
            {
                if (p.Type == piece.Type && p.Position == piece.Position)
                {
                    pieces.Remove(p);
                    return;
                }
            }
        }


        public bool IsCheck(Color color, Piece piece = null)
        {
            foreach (var pieces in color == Color.Black ? this.whitePieces : this.blackPieces)
            {
                if (piece == null || pieces != piece)
                {
                    foreach (var uwu in pieces.GetPossibleMoves(false))
                    {
                        if (uwu.Piece.Type == PieceType.King)
                            return true;
                    }
                }
            }

            return false;
        }


        public bool IsMat(Color color) => IsCheck(color) && IsEnd(color);
        

        public bool IsPat(Color color) => !IsCheck(color) && IsEnd(color);


        public bool IsEnd(Color color)
        {
            if (this.blackPieces.Count == 1 && this.whitePieces.Count == 1)
                return true;
            foreach (var piece in color == Color.Black ? this.blackPieces : this.whitePieces)
            {
                if (piece.GetPossibleMoves().Count != 0)
                    return false;
            }
                
            return true;
        }


        public bool IsMoveValid(Piece piece, int x, int y)
        {
            foreach (var tile in piece.GetPossibleMoves())
            {
                if (tile.X == x && tile.Y == y)
                { return true; }
            }
            
            return false;
        }


        public void Print(Tile lastMove = null, Tile newPieceTile = null, Color colorToPlay = Color.White, Piece pieceToMove = null)
        {
            bool isCheck = IsCheck(colorToPlay);
            List<Tile> moves = new List<Tile>();
            if (pieceToMove != null)
                moves = pieceToMove.GetPossibleMoves();
            Console.Write("   |");
            for (int i = 0; i < 8; i++)
                Console.Write($" {i+1} |");
            Console.WriteLine();
            Console.WriteLine("------------------------------------");
            for (int i = 0; i < 8; i++)
            {
                Console.Write($"{(char) (i + 'a')} ||");
                for (int j = 0; j < 8; j++)
                {
                    if (this._Board[i, j].Piece.Type != PieceType.Empty)
                    {
                        if (pieceToMove != null && moves.Contains(this._Board[i, j]))
                            Console.ForegroundColor = ConsoleColor.Green;
                        else if (newPieceTile != null && this._Board[i, j] == newPieceTile)
                            Console.ForegroundColor = ConsoleColor.Magenta;
                        else if (isCheck && this._Board[i, j].Piece.Type == PieceType.King &&
                                 this._Board[i, j].Piece.Color == colorToPlay)
                            Console.ForegroundColor = ConsoleColor.Red;
                        else if (this._Board[i, j].Piece.Color == Color.Black)
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write(" " + this._Board[i,j].Piece.ToString());
                    }
                    else
                    {
                        if (pieceToMove != null && moves.Contains(this._Board[i, j]))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write($" {'\u2218'}");
                        }
                        else if (lastMove != null && this._Board[i, j] == lastMove)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write($" {'\u2218'}");
                        }
                        else
                            Console.Write("  ");
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" |");
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                Console.WriteLine("------------------------------------");
            }
        }


        private void DecodeStringRepresentation(string s)
        {
            int i = 0;
            int j = 0;
            foreach (var t in s)
            {
                switch (t)
                {
                    case > '0' and <= '9':
                        int h = t - '0';
                        while (h > 0)
                        {
                            this.board[i, j] = new Tile(i, j, this, '0');
                            j += 1;
                            h -= 1;
                        }
                        break;
                    case '/':
                        i += 1;
                        j = 0;
                        break;
                    default:
                        this.board[i, j] = new Tile(i, j, this, t);
                        j += 1;
                        break;
                }
            }
        }

        public override string ToString()
        {
            string res = "";
            int nbNull;
            for (int i = 0; i < 8; i++)
            {
                nbNull = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (this.board[i, j].Piece.Type == PieceType.Empty)
                        nbNull += 1;
                    else
                    {
                        if (nbNull != 0)
                        {
                            res += nbNull.ToString();
                            nbNull = 0;
                        }

                        switch (this.board[i, j].Piece.Type)
                        {
                            case PieceType.Pawn:
                                if (this.board[i, j].Piece.Color == Color.Black)
                                    res += 'P';
                                else
                                    res += 'p';
                                break;
                            case PieceType.Knight:
                                if (this.board[i, j].Piece.Color == Color.Black)
                                    res += 'N';
                                else
                                    res += 'n';
                                break;
                            case PieceType.Bishop:
                                if (this.board[i, j].Piece.Color == Color.Black)
                                    res += 'B';
                                else
                                    res += 'b';
                                break;
                            case PieceType.Rook:
                                if (this.board[i, j].Piece.Color == Color.Black)
                                    res += 'R';
                                else
                                    res += 'r';
                                break;
                            case PieceType.Queen:
                                if (this.board[i, j].Piece.Color == Color.Black)
                                    res += 'Q';
                                else
                                    res += 'q';
                                break;
                            case PieceType.King:
                                if (this.board[i, j].Piece.Color == Color.Black)
                                    res += 'K';
                                else
                                    res += 'k';
                                break;
                        }
                    }
                }
                if (nbNull != 0)
                    res += nbNull.ToString();
                res += '/';
            }

            return res;
        }
    }
}