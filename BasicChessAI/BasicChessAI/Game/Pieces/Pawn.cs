using System.Collections.Generic;

namespace BasicChessAI.Game.Pieces
{
    public class Pawn:Piece
    {
        public Pawn(Tile tile, Color color, Board board) : base(tile, PieceType.Pawn, color, board)
        {
        }

        public void ChangeTo(char c)
        {
            switch (c)
            {
                case 'q':
                case 'Q':
                    ChangeToQueen();
                    break;
                case 'b':
                case 'B':
                    this.Position.ChangePiece(new Bishop(this.Position, this.Color, this.Board));
                    this.Board.RemovePieces(this, this.Color);
                    this.Board.AddPieces(this.Position.Piece, this.Color);
                    break;
                case 'r':
                case 'R':
                    this.Position.ChangePiece(new Rook(this.Position, this.Color, this.Board, true));
                    this.Board.RemovePieces(this, this.Color);
                    this.Board.AddPieces(this.Position.Piece, this.Color);
                    break;
                case 'n':
                case 'N':
                    this.Position.ChangePiece(new Knight(this.Position, this.Color, this.Board));
                    this.Board.RemovePieces(this, this.Color);
                    this.Board.AddPieces(this.Position.Piece, this.Color);
                    break;
            }
        }
        
        
        // to the AI
        public void ChangeToQueen()
        {
            this.Position.ChangePiece(new Queen(this.Position, this.Color, this.Board));
            this.Board.RemovePieces(this, this.Color);
        }

        public override List<Tile> GetPossibleMoves(bool checkCheck = true)
        {
            List<Tile> res = new List<Tile>();
            Tile tile;
            if (this.Color == Color.Black && this.Position.X == 1 &&
                GetTile(2, this.Position.Y).Piece.Type == PieceType.Empty &&
                GetTile(3, this.Position.Y).Piece.Type == PieceType.Empty)
            {
                tile = GetTile(3, this.Position.Y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
            }
            else if (this.Color == Color.White && this.Position.X == 6 &&
                     GetTile(5, this.Position.Y).Piece.Type == PieceType.Empty &&
                     GetTile(4, this.Position.Y).Piece.Type == PieceType.Empty)
            {
                tile = GetTile(4, this.Position.Y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
            }

            if (IsPosValid(this.Position.X + (int) this.Color, this.Position.Y) &&
                GetTile(this.Position.X + (int) this.Color, this.Position.Y).Piece.Type == PieceType.Empty)
            {
                tile = GetTile(this.Position.X + (int) this.Color, this.Position.Y);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
            }

            if (IsPosValid(this.Position.X + (int) this.Color, this.Position.Y + 1) &&
                GetTile(this.Position.X + (int) this.Color, this.Position.Y + 1).Piece.Color == (Color)((int)this.Color * -1))
            {
                tile = GetTile(this.Position.X + (int) this.Color, this.Position.Y + 1);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
            }

            if (IsPosValid(this.Position.X + (int) this.Color, this.Position.Y - 1) &&
                GetTile(this.Position.X + (int) this.Color, this.Position.Y - 1).Piece.Color == (Color)((int)this.Color * -1))
            {
                tile = GetTile(this.Position.X + (int) this.Color, this.Position.Y - 1);
                if (checkCheck && TestMove(tile))
                    res.Add(tile);
                else if (!checkCheck)
                    res.Add(tile);
            }

            return res;
        }
    }
}