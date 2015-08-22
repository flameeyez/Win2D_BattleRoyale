using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Collections.Generic;
using Windows.UI;
using Windows.Foundation;
using System;
using System.Numerics;

namespace Win2D_BattleRoyale
{
    public class Region
    {
        public int ID { get; set; }
        public Color Color { get; set; }
        public int FailedExpansionCount { get; set; }
        public string Name { get; set; }
        public Leader Leader { get; set; }

        public Region OvertakingRegion { get; set; }
        private int _overtakingframe = 0;

        // list of x,y coordinates for the region
        public List<Tile> Tiles = new List<Tile>();

        public Region(int id, Tile[,] MasterTileList)
        {
            Leader = Leaderboard.Leaders.RandomLeader();
            Name = Statics.RandomRegionName(Leader);

            int TileCountX = MasterTileList.GetLength(0);
            int TileCountY = MasterTileList.GetLength(1);

            FailedExpansionCount = 0;
            ID = id;
            Color = Statics.RandomColor();

            // grab random point as starting tile
            int x = Statics.Random.Next(TileCountX);
            int y = Statics.Random.Next(TileCountY);
            while (!MasterTileList[x, y].Available)
            {
                x = Statics.Random.Next(TileCountX);
                y = Statics.Random.Next(TileCountY);
            }

            // create this tile
            Tiles.Add(MasterTileList[x, y]);
            MasterTileList[x, y].Available = false;
            MasterTileList[x, y].Region = ID;
            
            // if able, grow until minimum size reached, then possibly grow more
            while (((Tiles.Count < Statics.MinimumRegionSize) || (Statics.Random.Next(100) < Statics.ProbabilityOfExpansion)) && HasAvailableNeighboringTile(MasterTileList))
            {
                // pick a random tile from the region set and attempt to grow
                Tile currentTile = Tiles.RandomItem();
                x = (int)currentTile.Coordinates.X;
                y = (int)currentTile.Coordinates.Y;

                int nExpansionDirection = Statics.Random.Next(4);
                bool bExpanded = false;
                int nExpansionAttempts = 0;

                while (!bExpanded && nExpansionAttempts < 4)
                {
                    nExpansionAttempts++;
                    nExpansionDirection = (nExpansionDirection + 1) % 4;
                    switch (nExpansionDirection)
                    {
                        case 0:
                            // try to grow left
                            if (x > 0 && MasterTileList[x - 1, y].Available)
                            {
                                Tiles.Add(MasterTileList[x - 1, y]);
                                MasterTileList[x - 1, y].Available = false;
                                MasterTileList[x - 1, y].Region = ID;
                                bExpanded = true;
                            }
                            break;
                        case 1:
                            // try to grow right
                            if (x < TileCountX - 1 && MasterTileList[x + 1, y].Available)
                            {
                                Tiles.Add(MasterTileList[x + 1, y]);
                                MasterTileList[x + 1, y].Available = false;
                                MasterTileList[x + 1, y].Region = ID;
                                bExpanded = true;
                            }
                            break;
                        case 2:
                            // try to grow up
                            if (y > 0 && MasterTileList[x, y - 1].Available)
                            {
                                Tiles.Add(MasterTileList[x, y - 1]);
                                MasterTileList[x, y - 1].Available = false;
                                MasterTileList[x, y - 1].Region = ID;
                                bExpanded = true;
                            }
                            break;
                        case 3:
                            // try to grow down
                            if (y < TileCountY - 1 && MasterTileList[x, y + 1].Available)
                            {
                                Tiles.Add(MasterTileList[x, y + 1]);
                                MasterTileList[x, y + 1].Available = false;
                                MasterTileList[x, y + 1].Region = ID;
                                bExpanded = true;
                            }
                            break;
                    }
                }

                if (!bExpanded)
                {
                    FailedExpansionCount++;
                }
            }
        }

        private bool HasAvailableNeighboringTile(Tile[,] MasterTileList)
        {
            int TileCountX = MasterTileList.GetLength(0);
            int TileCountY = MasterTileList.GetLength(1);

            foreach (Tile tile in Tiles)
            {
                // check left
                if (tile.Coordinates.X > 0 && MasterTileList[(int)tile.Coordinates.X - 1, (int)tile.Coordinates.Y].Available) { return true; }

                // check right
                if (tile.Coordinates.X < TileCountX - 1 && MasterTileList[(int)tile.Coordinates.X + 1, (int)tile.Coordinates.Y].Available) { return true; }

                // check up
                if (tile.Coordinates.Y > 0 && MasterTileList[(int)tile.Coordinates.X, (int)tile.Coordinates.Y - 1].Available) { return true; }

                // check down
                if (tile.Coordinates.Y < TileCountY - 1 && MasterTileList[(int)tile.Coordinates.X, (int)tile.Coordinates.Y + 1].Available) { return true; }
            }

            return false;
        }

        public void ReindexTiles()
        {
            foreach (Tile tile in Tiles)
            {
                tile.Region = ID;
            }
        }

        public void Draw(Vector2 MapPosition, CanvasAnimatedDrawEventArgs args)
        {
            foreach (Tile tile in Tiles)
            {
                // calculate chances that a tile in an overtaken region is drawn in the conquering color
                // chance increases as region shrinks
                int r = Math.Max(50, Tiles.Count / 200);
                if (OvertakingRegion != null && Statics.Random.Next(r) == 0)
                {
                    args.DrawingSession.FillRectangle(
                        new Rect(MapPosition.X + Statics.LeftColumnPadding + tile.Coordinates.X * Map.PixelScale,
                                 MapPosition.Y + Statics.LeftColumnPadding + tile.Coordinates.Y * Map.PixelScale,
                                 Map.PixelScale,
                                 Map.PixelScale),
                        OvertakingRegion.Color);

                    _overtakingframe++;
                }
                else
                {
                    args.DrawingSession.FillRectangle(
                        new Rect(MapPosition.X + Statics.LeftColumnPadding + tile.Coordinates.X * Map.PixelScale,
                                 MapPosition.Y + Statics.LeftColumnPadding + tile.Coordinates.Y * Map.PixelScale,
                                 Map.PixelScale,
                                 Map.PixelScale),
                        Color);
                }
            }
        }

        public bool Contains(int x, int y)
        {
            foreach (Tile tile in Tiles)
            {
                if (tile.Coordinates.X == x && tile.Coordinates.Y == y) { return true; }
            }

            return false;
        }
    }
}