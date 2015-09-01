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

        public HashSet<Region> NeighboringRegions = new HashSet<Region>();
        
        public Faction ControllingFaction { get; set; }
        public Faction OvertakingFaction { get; set; }

        private int _overtakingframe = 0;

        // list of x,y coordinates for the region
        public List<Tile> Tiles = new List<Tile>();

        public Region(int id, Tile[,] MasterTileList)
        {
            Name = Statics.RandomRegionType();

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
            Tile startingTile = MasterTileList[x, y];
            Tiles.Add(startingTile);
            startingTile.Available = false;
            startingTile.Region = ID;

            // if able, grow until minimum size reached, then possibly grow more
            while (((Tiles.Count < Statics.MinimumRegionSize) || (Statics.Random.Next(100) < Statics.ProbabilityOfExpansion)) && HasAvailableNeighboringTile(MasterTileList))
            {
                // pick a random tile from the region set and attempt to grow
                Tile currentTile = Tiles.RandomItem();
                x = (int)currentTile.Coordinates.X;
                y = (int)currentTile.Coordinates.Y;

                int nExpansionDirection = Statics.Random.Next(4);
                int nExpansionAttempts = 0;

                Tile expansionTile = null;
                while (expansionTile == null && nExpansionAttempts < 4)
                {
                    nExpansionAttempts++;
                    nExpansionDirection = (nExpansionDirection + 1) % 4;
                    switch (nExpansionDirection)
                    {
                        case 0:
                            // try to grow left
                            if (x > 0 && MasterTileList[x - 1, y].Available)
                            {
                                expansionTile = MasterTileList[x - 1, y];
                            }
                            break;
                        case 1:
                            // try to grow right
                            if (x < TileCountX - 1 && MasterTileList[x + 1, y].Available)
                            {
                                expansionTile = MasterTileList[x + 1, y];
                            }
                            break;
                        case 2:
                            // try to grow up
                            if (y > 0 && MasterTileList[x, y - 1].Available)
                            {
                                expansionTile = MasterTileList[x, y - 1];
                            }
                            break;
                        case 3:
                            // try to grow down
                            if (y < TileCountY - 1 && MasterTileList[x, y + 1].Available)
                            {
                                expansionTile = MasterTileList[x, y + 1];
                            }
                            break;
                    }
                }

                if (expansionTile != null)
                {
                    Tiles.Add(expansionTile);
                    expansionTile.Available = false;
                    expansionTile.Region = ID;
                }
                else
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
                if (OvertakingFaction != null && Statics.Random.Next(r) == 0)
                {
                    args.DrawingSession.FillRectangle(
                        new Rect(MapPosition.X + Statics.LeftColumnPadding + tile.Coordinates.X * Map.PixelScale,
                                 MapPosition.Y + Statics.LeftColumnPadding + tile.Coordinates.Y * Map.PixelScale,
                                 Map.PixelScale,
                                 Map.PixelScale),
                        OvertakingFaction.Color);

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

        public void FindNeighbors(Tile[,] MasterTileList)
        {
            int TileCountX = MasterTileList.GetLength(0);
            int TileCountY = MasterTileList.GetLength(1);

            foreach (Tile tile in Tiles)
            {
                int x = (int)tile.Coordinates.X;
                int y = (int)tile.Coordinates.Y;

                if (x > 0)
                {
                    // analyze left neighbor
                    Tile neighbor = MasterTileList[x - 1, y];
                    if (neighbor.Region != null && neighbor.Region.ID != ID)
                    {
                        NeighboringRegions.Add(neighbor.Region);
                    }
                }

                if (x < TileCountX - 1)
                {
                    // analyze right neighbor
                    Tile neighbor = MasterTileList[x + 1, y];
                    if (neighbor.Region != null && neighbor.Region.ID != ID)
                    {
                        NeighboringRegions.Add(neighbor.Region);
                    }
                }

                if (y > 0)
                {
                    // analyze up neighbor
                    Tile neighbor = MasterTileList[x, y - 1];
                    if (neighbor.Region != null && neighbor.Region.ID != ID)
                    {
                        NeighboringRegions.Add(neighbor.Region);
                    }
                }

                if (y < TileCountY - 1)
                {
                    // analyze down neighbor
                    Tile neighbor = MasterTileList[x, y + 1];
                    if (neighbor.Region != null && neighbor.Region.ID != ID)
                    {
                        NeighboringRegions.Add(neighbor.Region);
                    }
                }
            }
        }

        public override bool Equals(object obj)
        {
            Region compare = obj as Region;
            return compare.ID == this.ID;
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        public Region RandomNeighbor()
        {
            NeighboringRegions.
        }
    }
}