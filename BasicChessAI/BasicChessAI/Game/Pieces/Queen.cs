using System.Collections.Generic;

namespace BasicChessAI.Game.Pieces
{
    public class Queen:Piece
    {
        public Queen(Tile tile, Color color, Board board) : base(tile, PieceType.Queen, color, board)
        {
        }

        public override List<Tile> GetPossibleMoves(bool checkCheck = true)
        {
            List<Tile> res = this.RookMoves(checkCheck);
            foreach (var tile in this.BishopMoves(checkCheck))
                res.Add(tile);
            return res;
        }
    }
}