using System;
using System.Collections.Generic;
using BasicChessAI.Game;
using BasicChessAI.Game.Pieces;

namespace BasicChessAI.Players
{
    public class AI: Player
    {
        private Func<Board, int> heuristic;
        private int maxDepth;

        public AI(Color color, Board board, int difficulty) : base(PlayerType.AI, color, board)
        {
            heuristic = Heuristics.AllIn;
            maxDepth = 4;
        }

        public override (Tile, Tile) PlayOneRound(Tile lastMove, Tile nextPos)
        {
            var minimax = Minimax(this.Board, this.maxDepth, Int32.MinValue, Int32.MaxValue, this.PlayerColor);
            if (minimax.Item1 == null || minimax.Item2 == null)
            {
                Console.Error.WriteLine("ERROR : IMPOSSIBLE TO FOUND A MOVE !!!! " + minimax.Item3);
                Environment.Exit(44);
            }
            
            if (minimax.Item1.Piece.Type == PieceType.King)
                ((King)minimax.Item1.Piece).hasMoved = true;
            if (minimax.Item1.Piece.Type == PieceType.Rook)
                ((Rook) minimax.Item1.Piece).hasMoved = true;
            minimax.Item1.Piece.ApplyMovement(minimax.Item2, true);
            return (minimax.Item1, minimax.Item2);
        }

        private (Tile, Tile, int) Minimax(Board game, int depth, int alpha, int beta, Color colorToPlay)
        {
            if (game.IsEnd(colorToPlay))
            {
                if (game.IsCheck(colorToPlay))
                    return (null, null, colorToPlay == Color.Black ? Int32.MaxValue : Int32.MinValue);
                return (null, null, 0);
            }

            if (depth == 0)
                return (null, null, heuristic(game));
            
            Tile nextLastMove = null;
            Tile nextNextMove = null;
            Tile prevTile;
            Piece prevPiece;
            Piece prevPawnPiece;
            bool isRook;
            Piece rook;
            Piece prevRookPiece;
            int value;
            List<Piece> iterableList = new List<Piece>();
            foreach (var piece in colorToPlay == Color.Black ? game.BlackPieces : game.WhitePieces)
                iterableList.Add(piece);

            if (colorToPlay == Color.White)
            {
                value = Int32.MinValue;
                foreach (var piece in iterableList)
                {
                    prevTile = piece.Position;
                    foreach (var tile in piece.GetPossibleMoves())
                    {
                        isRook = false;
                        rook = null;
                        prevRookPiece = null;
                        prevPawnPiece = null;
                        prevPiece = tile.Piece;
                        if (piece.Type == PieceType.Pawn && tile.X == 0) 
                            prevPawnPiece = piece;
                        if (piece.Type == PieceType.King &&
                            (tile.Y == piece.Position.Y + 2 || tile.Y == piece.Position.Y - 2))
                        {
                            isRook = true;
                            rook = game._Board[piece.Position.X, tile.Y == piece.Position.Y + 2 ? 7 : 0].Piece;
                            prevRookPiece = game._Board[piece.Position.X, tile.Y == 6 ? 5 : 3].Piece;
                        }
                        piece.ApplyMovement(tile, true);
                        var recursivus = Minimax(game, depth - 1, alpha, beta, Color.Black);
                        tile.Piece.ReverseAppliedMove(prevTile, prevPiece, prevPawnPiece, isRook, rook, prevRookPiece);
                        if (recursivus.Item3 > value || (recursivus.Item3 == Int32.MinValue && value == Int32.MinValue))
                        {
                            nextLastMove = piece.Position;
                            nextNextMove = tile;
                            value = recursivus.Item3;
                        }

                        if (value >= beta)
                            return (nextLastMove, nextNextMove, value);
                        if (value > alpha)
                            alpha = value;
                    }
                }

                return (nextLastMove, nextNextMove, value);
            }
            value = Int32.MaxValue;
            foreach (var piece in iterableList)
            {
                prevTile = piece.Position;
                foreach (var tile in piece.GetPossibleMoves())
                {
                    isRook = false;
                    rook = null;
                    prevRookPiece = null;
                    prevPawnPiece = null;
                    prevPiece = tile.Piece;
                    if (piece.Type == PieceType.Pawn && tile.X == 7)
                        prevPawnPiece = piece;
                    if (piece.Type == PieceType.King &&
                        (tile.Y == piece.Position.Y + 2 || tile.Y == piece.Position.Y - 2))
                    {
                        isRook = true;
                        rook = game._Board[piece.Position.X, tile.Y == piece.Position.Y + 2 ? 7 : 0].Piece;
                        prevRookPiece = game._Board[piece.Position.X, tile.Y == 6 ? 5 : 3].Piece;
                    }
                    piece.ApplyMovement(tile, true);
                    var recursivus = Minimax(game, depth - 1, alpha, beta, Color.White);
                    tile.Piece.ReverseAppliedMove(prevTile, prevPiece, prevPawnPiece, isRook, rook, prevRookPiece);
                    if (recursivus.Item3 < value || (recursivus.Item3 == Int32.MaxValue && value == Int32.MaxValue))
                    {
                        nextLastMove = piece.Position;
                        nextNextMove = tile;
                        value = recursivus.Item3;
                    }

                    if (value <= alpha)
                        return (nextLastMove, nextNextMove, value);
                    if (value < beta)
                        beta = value;
                }
            }

            return (nextLastMove, nextNextMove, value);
        }
    }
}