using BasicChessAI.Game;
using BasicChessAI.Game.Pieces;

namespace BasicChessAI.Players
{
    public static class Heuristics
    {
        #region utils
        
        private static bool IsEndGame(Board game)
        {
            int masterPiecesValue = 0;
            foreach (var pieces in game.BlackPieces)
            {
                if (pieces.Type != PieceType.Pawn && pieces.Type != PieceType.King)
                    masterPiecesValue += (int) pieces.Type;
            }

            foreach (var pieces in game.WhitePieces)
            {
                if (pieces.Type != PieceType.Pawn && pieces.Type != PieceType.King)
                    masterPiecesValue += (int) pieces.Type;
            }

            return masterPiecesValue <= 18;
        }
        
        private static readonly int[,] PawnsPosition =
        {
            {0, 0, 0, 0, 0, 0, 0, 0},
            {50, 50, 50, 50, 50, 50, 50, 50},
            {10, 10, 20, 30, 30, 20, 10, 10},
            {5, 5, 10, 25, 25, 10, 5, 5},
            {0, 0, 0, 20, 20, 0, 0, 0},
            {5, -5, -10, 0, 0, -10, -5, 5},
            {5, 10, 10, -20, -20, 10, 10, 5},
            {0, 0, 0, 0, 0, 0, 0, 0}
        };

        private static readonly int[,] KnightPosition =
        {
            {-50, -40, -30, -30, -30, -30, -40, -50},
            {-40, -20, 0, 0, 0, 0, -20, -40},
            {-30, 0, 10, 15, 15, 10, 0, -30},
            {-30, 5, 15, 20, 20, 15, 5, -30},
            {-30, 0, 15, 20, 20, 15, 0, -30},
            {-30, 5, 10, 15, 15, 10, 5, -30},
            {-40, -20, 0, 5, 5, 0, -20, -40},
            {-50, -40, -30, -30, -30, -30, -40, -50}
        };

        private static readonly int[,] BishopPosition =
        {
            {-20, -10, -10, -10, -10, -10, -10, -20},
            {-10, 0, 0, 0, 0, 0, 0, -10},
            {-10, 0, 5, 10, 10, 5, 0, -10},
            {-10, 5, 5, 10, 10, 5, 5, -10},
            {-10, 0, 10, 10, 10, 10, 0, -10},
            {-10, 10, 10, 10, 10, 10, 10, -10},
            {-10, 5, 0, 0, 0, 0, 5, -10},
            {-20, -10, -10, -10, -10, -10, -10, -20}
        };

        private static readonly int[,] RookPosition =
        {
            {0, 0, 0, 0, 0, 0, 0, 0},
            {5, 10, 10, 10, 10, 10, 10, 5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {0, 0, 0, 5, 5, 0, 0, 0}
        };

        private static readonly int[,] QueenPosition =
        {
            {-20, -10, -10, -5, -5, -10, -10, -20},
            {-10, 0, 0, 0, 0, 0, 0, -10},
            {-10, 0, 5, 5, 5, 5, 0, -10},
            {-5, 0, 5, 5, 5, 5, 0, -5},
            {0, 0, 5, 5, 5, 5, 0, -5},
            {-10, 5, 5, 5, 5, 5, 0, -10},
            {-10, 0, 5, 0, 0, 0, 0, -10},
            {-20, -10, -10, -5, -5, -10, -10, -20}
        };

        private static readonly int[,] KingMiddleGamePosition =
        {
            {-30, -40, -40, -50, -50, -40, -40, -30},
            {-30, -40, -40, -50, -50, -40, -40, -30},
            {-30, -40, -40, -50, -50, -40, -40, -30},
            {-30, -40, -40, -50, -50, -40, -40, -30},
            {-20, -30, -30, -40, -40, -30, -30, -20},
            {-10, -20, -20, -20, -20, -20, -20, -10},
            {20, 20, 0, 0, 0, 0, 20, 20},
            {20, 30, 10, 0, 0, 10, 30, 20}
        };

        private static readonly int[,] KingEndGamePosition =
        {
            {-50, -40, -30, -20, -20, -30, -40, -50},
            {-30, -20, -10, 0, 0, -10, -20, -30},
            {-30, -10, 20, 30, 30, 20, -10, -30},
            {-30, -10, 30, 40, 40, 30, -10, -30},
            {-30, -10, 30, 40, 40, 30, -10, -30},
            {-30, -10, 20, 30, 30, 20, -10, -30},
            {-30, -30, 0, 0, 0, 0, -30, -30},
            {-50, -30, -30, -30, -30, -30, -30, -50}
        };
        
        #endregion

        #region PieceValue
        
        private static int _BoardValue(Board game, Color color)
        {
            int val = 0;
            foreach (var pieces in color == Color.Black ? game.BlackPieces : game.WhitePieces)
                val += (int) pieces.Type * 100;
            return val;
        }

        public static int BoardValue(Board game) =>
            _BoardValue(game, Color.White) - _BoardValue(game, Color.Black);
        
        #endregion
        
        #region PositionValue
        
        private static int _PositionValue(Board board, Color color, bool isEndGame)
        {
            int i = 0;
            foreach (var piece in color == Color.Black ? board.BlackPieces : board.WhitePieces)
            {
                int y = piece.Position.Y;
                int x = color == Color.White ? piece.Position.X : 7 - piece.Position.X;
                switch (piece.Type)
                {
                    case PieceType.Pawn:
                        i += PawnsPosition[x,y];
                        break;
                    case PieceType.Bishop:
                        i += BishopPosition[x,y];
                        break;
                    case PieceType.Rook:
                        i += RookPosition[x,y];
                        break;
                    case PieceType.Knight:
                        i += KnightPosition[x,y];
                        break;
                    case PieceType.Queen:
                        i += QueenPosition[x,y];
                        break;
                    case PieceType.King:
                        if (isEndGame)
                            i += KingEndGamePosition[x,y];
                        else
                            i += KingMiddleGamePosition[x,y];
                        break;
                }
            }

            return i;
        }

        public static int PositionValue(Board board)
        {
            bool isEndGame = IsEndGame(board);
            return _PositionValue(board, Color.White, isEndGame) - _PositionValue(board, Color.Black, isEndGame);
        }
        
        #endregion

        #region PawnStructure

        private static int _PawnStructure(Board game, Color color)
        {
            int res = 0;
            foreach (var piece in color == Color.White ? game.WhitePieces : game.BlackPieces)
            {
                if (piece.Type == PieceType.Pawn)
                {
                    int x = piece.Position.X;
                    int y = piece.Position.Y;
                    if (x != 0)
                    {
                        if (game._Board[x - 1, y].Piece.Type == PieceType.Pawn &&
                            game._Board[x - 1, y].Piece.Color == color)
                            res -= 20;
                        if (y != 0 &&
                            game._Board[x - 1, y - 1].Piece.Type == PieceType.Pawn &&
                            game._Board[x - 1, y - 1].Piece.Color == color)
                            res += 10;
                        if (y != 7 &&
                            game._Board[x - 1, y + 1].Piece.Type == PieceType.Pawn &&
                            game._Board[x - 1, y + 1].Piece.Color == color)
                            res += 10;
                    }

                    if (x != 7)
                    {
                        if (game._Board[x + 1, y].Piece.Type == PieceType.Pawn &&
                            game._Board[x + 1, y].Piece.Color == color)
                            res -= 20;
                        if (y != 0 &&
                            game._Board[x + 1, y - 1].Piece.Type == PieceType.Pawn &&
                            game._Board[x + 1, y - 1].Piece.Color == color)
                            res += 10;
                        if (y != 7 &&
                            game._Board[x + 1, y + 1].Piece.Type == PieceType.Pawn &&
                            game._Board[x + 1, y + 1].Piece.Color == color)
                            res += 10;
                    }
                }
            }

            return res;
        }

        public static int PawnStructure(Board game) =>
            _PawnStructure(game, Color.White) - _PawnStructure(game, Color.Black);

        #endregion

        #region Mobility

        private static int _Mobility(Board game, Color color)
        {
            int res = 0;
            foreach (var piece in color == Color.Black ? game.BlackPieces : game.WhitePieces)
                if (piece.Type != PieceType.Pawn)
                    foreach (var move in piece.GetPossibleMoves())
                    
                        if (move.Piece.Type == PieceType.Empty)
                            res += 10;
            return res;
        }

        public static int Mobility(Board game) => _Mobility(game, Color.White) - _Mobility(game, Color.Black);

        #endregion

        #region CenterControl

        public static int CenterControl(Board board)
        {
            int res = 0;
            for (int i = 3; i <= 4; i++)
            {
                for (int j = 3; j <= 4; j++)
                {
                    if (board._Board[i, j].Piece.Color == Color.White)
                        res += 5;
                    else if (board._Board[i, j].Piece.Color == Color.Black)
                        res -= 5;
                }
            }

            return res;
        }

        #endregion

        #region KingSafety

        // TODO

        #endregion

        #region AllIn

        private static int _AllIn(Board game, Color color, bool isEndGame)
        {
            int i = 0;
            foreach (var piece in color == Color.Black ? game.BlackPieces : game.WhitePieces)
            {
                int y = piece.Position.Y;
                int x = color == Color.White ? piece.Position.X : 7 - piece.Position.X;
                switch (piece.Type)
                {
                    case PieceType.Pawn:
                        i += PawnsPosition[x, y];
                        int _x = piece.Position.X;
                        int _y = piece.Position.Y;
                        if (_x != 0)
                        {
                            if (game._Board[_x - 1, _y].Piece.Type == PieceType.Pawn &&
                                game._Board[_x - 1, _y].Piece.Color == color)
                                i -= 10;
                            if (_y != 0 &&
                                game._Board[_x - 1, _y - 1].Piece.Type == PieceType.Pawn &&
                                game._Board[_x - 1, _y - 1].Piece.Color == color)
                                i += 2;
                            if (_y != 7 &&
                                game._Board[_x - 1, _y + 1].Piece.Type == PieceType.Pawn &&
                                game._Board[_x - 1, _y + 1].Piece.Color == color)
                                i += 2;
                        }

                        if (_x != 7)
                        {
                            if (game._Board[_x + 1, _y].Piece.Type == PieceType.Pawn &&
                                game._Board[_x + 1, _y].Piece.Color == color)
                                i -= 10;
                            if (_y != 0 &&
                                game._Board[_x + 1, _y - 1].Piece.Type == PieceType.Pawn &&
                                game._Board[_x + 1, _y - 1].Piece.Color == color)
                                i += 2;
                            if (_y != 7 &&
                                game._Board[_x + 1, _y + 1].Piece.Type == PieceType.Pawn &&
                                game._Board[_x + 1, _y + 1].Piece.Color == color)
                                i += 2;
                        }
                        break;
                    case PieceType.Bishop:
                        i += BishopPosition[x, y];
                        break;
                    case PieceType.Rook:
                        i += RookPosition[x, y];
                        break;
                    case PieceType.Knight:
                        i += KnightPosition[x, y];
                        break;
                    case PieceType.Queen:
                        i += QueenPosition[x, y];
                        break;
                    case PieceType.King:
                        if (isEndGame)
                            i += KingEndGamePosition[x, y];
                        else
                            i += KingMiddleGamePosition[x, y];
                        break;
                }
                if (piece.Type != PieceType.Pawn)
                    foreach (var move in piece.GetPossibleMoves())
                        if (move.Piece.Type == PieceType.Empty)
                            i += 4;
            }

            return i;
        }

        public static int AllIn(Board game)
        {
            bool isEndGame = IsEndGame(game);
            return _AllIn(game, Color.White, isEndGame) - _AllIn(game, Color.Black, isEndGame) + CenterControl(game);
        }

        #endregion
    }
}