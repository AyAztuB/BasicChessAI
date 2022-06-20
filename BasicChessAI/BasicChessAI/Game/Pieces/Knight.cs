using System.Collections.Generic;

namespace BasicChessAI.Game.Pieces
{
    public class Knight:Piece
    {
        public Knight(Tile tile, Color color, Board board) : base(tile, PieceType.Knight, color, board)
        {
        }

        public override List<Tile> GetPossibleMoves(bool checkCheck = true)
        {
            List<Tile> res = new List<Tile>();
            (int k, int l) = (2, 1);
            for (int i = 0; i < 8; i++)
            {
                if (i == 4)
                    (k, l) = (1, 2);
                if (IsPosValid(this.Position.X + k, this.Position.Y + l) &&
                    GetTile(this.Position.X + k, this.Position.Y + l).Piece.Color != this.Color)
                {
                    Tile tile = GetTile(this.Position.X + k, this.Position.Y + l);
                    if (checkCheck && TestMove(tile))
                        res.Add(tile);
                    else if (!checkCheck)
                        res.Add(tile);
                }

                if (i % 2 == 0)
                    k *= -1;
                else
                    l *= -1;
            }
            
            return res;
        }
    }
}