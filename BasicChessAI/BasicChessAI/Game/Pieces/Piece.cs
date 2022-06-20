using System;
using System.Collections.Generic;

namespace BasicChessAI.Game.Pieces
{
    public enum Color
    {
        Null = 0,
        Black = 1,
        White = -1,
    }
    
    public enum PieceType
    {
        Empty = 0,
        Pawn = 1,
        Knight = 2,
        Bishop = 3,
        Rook = 5,
        Queen = 9,
        King = 1000,
    }
    
    public class Piece
    {
        private PieceType type;
        private Tile position;
        private Color color;
        private Board board;

        public PieceType Type => this.type;
        public Tile Position => this.position;
        public Color Color => this.color;
        public Board Board => this.board;

        protected Piece(Tile tile, PieceType type, Color color, Board board)
        {
            this.board = board;
            this.position = tile;
            this.type = type;
            this.color = color;
            board.AddPieces(this, color);
        }

        private bool Move(Tile newTile)
        {
            Tile previousTile = this.position;
            Piece previousPiece = newTile.Piece;
            this.position.ChangePiece(new Empty(this.position, this.board));
            //if (newTile.Piece.Type != PieceType.Empty)
            //    this.board.RemovePieces(newTile.Piece, newTile.Piece.Color);
            this.position = newTile;
            this.position.ChangePiece(this);
            if (this.board.IsCheck(this.color, previousPiece))
            {
                ReverseMove(previousTile, previousPiece);
                return false;
            }

            return true;
        }


        protected bool TestMove(Tile newTile)
        {
            Tile prevTile = this.position;
            Piece prevPiece = newTile.Piece;
            bool res = Move(newTile);
            if (res)
                ReverseMove(prevTile,prevPiece);
            return res;
        }
        
        
        private void ReverseMove(Tile previousTile, Piece previousPiece)
        {
            //if (previousPiece.type != PieceType.Empty)
            //    this.board.AddPieces(previousPiece, previousPiece.Color);
            this.position.ChangePiece(previousPiece);
            previousTile.ChangePiece(this);
            this.position = previousTile;
        }


        public void ReverseAppliedMove(Tile prevTile, Piece prevPiece, Piece prevPawnPiece = null)
        {
            if (prevPawnPiece != null)
            {
                this.Board.RemovePieces(this, this.Color);
                this.Board.AddPieces(prevPawnPiece, prevPawnPiece.Color);
                prevPawnPiece.ReverseAppliedMove(prevTile, prevPiece);
            }
            else
            {
                if (prevPiece.Type != PieceType.Empty)
                    this.Board.AddPieces(prevPiece, prevPiece.Color);
                this.Position.ChangePiece(prevPiece);
                prevTile.ChangePiece(this);
                this.position = prevTile;
            }
        }


        public void ApplyMovement(Tile newTile, bool isAI)
        {
            if (this.Type == PieceType.King && (this.Position.Y - newTile.Y) is 2 or -2)
            {
                if (this.Position.Y - newTile.Y == 2)
                    GetTile(this.Position.X, 0).Piece.ApplyMovement(GetTile(this.Position.X, 3), isAI);
                else
                    GetTile(this.Position.X, 7).Piece.ApplyMovement(GetTile(this.Position.X, 5), isAI);
            }

            if (this.Type == PieceType.King)
                ((King) this).hasMoved = true;
            if (this.Type == PieceType.Rook)
                ((Rook) this).hasMoved = true;
            
            this.position.ChangePiece(new Empty(this.position, this.board));
            if (newTile.Piece.Type != PieceType.Empty)
                this.board.RemovePieces(newTile.Piece, newTile.Piece.Color);
            this.position = newTile;
            this.position.ChangePiece(this);
            if (this.Type == PieceType.Pawn && this.Position.X is 0 or 7)
            {
                this.board.RemovePieces(this, this.color);
                if (isAI)
                    ((Pawn) this).ChangeToQueen();
                else
                    ((Pawn) this).ChangeTo(AskToChange());
            }
        }

        private char AskToChange()
        {
            string s;
            string possible = "QqBbRrNn";
            do
            {
                Console.WriteLine("Choose a piece for the pawn : (q: queen, b: bishop, r: rook, n: knight)");
                s = Console.ReadLine();
            } while (String.IsNullOrEmpty(s) || s.Length != 1 || !possible.Contains(s[0]));

            return s[0];
        }
        
        public virtual List<Tile> GetPossibleMoves(bool checkCheck = true) => new List<Tile>();

        protected bool IsPosValid(int x, int y) => x is >= 0 and < 8 && y is >= 0 and < 8;

        protected Tile GetTile(int x, int y) => this.board._Board[x, y];

        public override string ToString()
        {
            switch (this.type)
            {
                case PieceType.Bishop:
                    return this.color == Color.Black ? "B" : "b";
                case PieceType.King:
                    return this.color == Color.Black ? "K" : "k";
                case PieceType.Knight:
                    return this.color == Color.Black ? "N" : "n";
                case PieceType.Pawn:
                    return this.color == Color.Black ? "P" : "p";
                case PieceType.Queen:
                    return this.color == Color.Black ? "Q" : "q";
                case PieceType.Rook:
                    return this.color == Color.Black ? "R" : "r";
            }

            return "0";
        }

        protected List<Tile> RookMoves(bool checkCheck = true)
        {
            List<Tile> res = new List<Tile>();
            Tile tile;
            int x = this.Position.X + 1;
            int y = this.Position.Y;
            while (x < 8 && GetTile(x, y).Piece.Type == PieceType.Empty)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
                x += 1;
            }

            if (x < 8 && GetTile(x, y).Piece.Color != this.Color)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
            }
            x = this.Position.X - 1;
            while (x >= 0 && GetTile(x, y).Piece.Type == PieceType.Empty)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
                x -= 1;
            }
            if (x >= 0 && GetTile(x, y).Piece.Color != this.Color)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
            }
            x = this.Position.X;
            y = this.Position.Y + 1;
            while (y < 8 && GetTile(x, y).Piece.Type == PieceType.Empty)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
                y += 1;
            }
            if (y < 8 && GetTile(x, y).Piece.Color != this.Color)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
            }
            y = this.Position.Y - 1;
            while (y >= 0 && GetTile(x, y).Piece.Type == PieceType.Empty)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
                y -= 1;
            }
            if (y >= 0 && GetTile(x, y).Piece.Color != this.Color)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
            }

            return res;
        }

        protected List<Tile> BishopMoves(bool checkCheck = true)
        {
            List<Tile> res = new List<Tile>();
            Tile tile;
            int x = this.Position.X + 1;
            int y = this.Position.Y + 1;
            while (x < 8 && y < 8 && GetTile(x, y).Piece.Type == PieceType.Empty)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
                x += 1;
                y += 1;
            }
            if (x < 8 && y < 8 && GetTile(x, y).Piece.Color != this.Color)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
            }
            x = this.Position.X - 1;
            y = this.Position.Y - 1;
            while (x >= 0 && y >= 0 && GetTile(x, y).Piece.Type == PieceType.Empty)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
                x -= 1;
                y -= 1;
            }
            if (x >= 0 && y >= 0 && GetTile(x, y).Piece.Color != this.Color)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
            }
            x = this.Position.X - 1;
            y = this.Position.Y + 1;
            while (y < 8 && x >= 0 && GetTile(x, y).Piece.Type == PieceType.Empty)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
                y += 1;
                x -= 1;
            }
            if (y < 8 && x >= 0 && GetTile(x, y).Piece.Color != this.Color)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
            }
            x = this.Position.X + 1;
            y = this.Position.Y - 1;
            while (y >= 0 && x < 8 && GetTile(x, y).Piece.Type == PieceType.Empty)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
                y -= 1;
                x += 1;
            }
            if (y >= 0 && x < 8 && GetTile(x, y).Piece.Color != this.Color)
            {
                tile = GetTile(x, y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
            }
            return res;
        }
    }
}