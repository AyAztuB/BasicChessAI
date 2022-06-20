using System.Collections.Generic;

namespace BasicChessAI.Game.Pieces
{
    public class King:Piece
    {
        public bool hasMoved { get; set; }

        public King(Tile tile, Color color, Board board, bool hasMoved = false) : base(tile, PieceType.King, color, board)
        {
            this.hasMoved = hasMoved;
        }

        public override List<Tile> GetPossibleMoves(bool checkCheck = true)
        {
            List<Tile> res = new List<Tile>();
            int x = this.Position.X - 1;
            int y;
            while (x <= this.Position.X + 1)
            {
                y = this.Position.Y - 1;
                while (y <= this.Position.Y + 1)
                {
                    if (IsPosValid(x, y) && GetTile(x, y).Piece.Color != this.Color)
                    {
                        Tile tile = GetTile(x, y);
                        if (checkCheck && TestMove(tile))
                            res.Add(tile);
                        else if (!checkCheck)
                            res.Add(tile);
                    }

                    y += 1;
                }
                
                x += 1;
            }

            x = this.Position.X;
            y = this.Position.Y;

            // I'm not really sure about the checkCheck condition here...
            if (checkCheck && !this.hasMoved && IsPosValid(x, y - 4) && GetTile(x, y-4).Piece.Type == PieceType.Rook &&
                !((Rook)GetTile(x, y-4).Piece).hasMoved && GetTile(x, y-1).Piece.Type == PieceType.Empty && GetTile(x, y-2).Piece.Type == PieceType.Empty)
            {
                if (!this.Board.IsCheck(this.Color) && TestMove(GetTile(x, y-1)) && TestMove(GetTile(x, y-2)))
                    res.Add(GetTile(x, y-2));
            }
            if (checkCheck && !this.hasMoved && IsPosValid(x, y + 3) && GetTile(x, y+3).Piece.Type == PieceType.Rook &&
                !((Rook)GetTile(x, y+3).Piece).hasMoved && GetTile(x, y+1).Piece.Type == PieceType.Empty && GetTile(x, y+2).Piece.Type == PieceType.Empty)
            {
                if (!this.Board.IsCheck(this.Color) && TestMove(GetTile(x, y+1)) && TestMove(GetTile(x, y+2)))
                    res.Add(GetTile(x, y+2));
            }

            return res;
        }
    }
}