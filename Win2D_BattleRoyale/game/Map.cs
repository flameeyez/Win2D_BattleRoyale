using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace Win2D_BattleRoyale
{
    class Map
    {
        #region Debug
        public int MergeCount { get; set; }
        public int FailedExpansionCount
        {
            get
            {
                return Regions.Select(x => x.FailedExpansionCount).Sum();
            }
        }
        #endregion

        private int nWinningImageFrame = 0;
        private int nWinningStringFrame = 0;
        private string strWinnerString = string.Empty;
        private float fWinnerStringPositionX;
        private float fWinnerStringPositionY;

        public List<Faction> Factions = new List<Faction>();
        public List<Region> Regions = new List<Region>();

        private Tile[,] MasterTileList;

        private TimeSpan UpdateDelta { get; set; }
        public BattleEventArgs LastBattle { get; set; }
        public bool Finished { get { return State == MAPSTATE.FINISHED; } }

        #region Layout
        public Vector2 Position { get; set; }
        public int WidthInPixels { get; set; }
        public int HeightInPixels { get; set; }
        public static int PixelScale = 3;

        public int WidthInTiles { get { return WidthInPixels / PixelScale; } }
        public int HeightInTiles { get { return HeightInPixels / PixelScale; } }
        #endregion

        enum MAPSTATE
        {
            READY_FOR_BATTLE,
            TAKEOVER_IN_PROGRESS,
            PAUSE_BETWEEN_BATTLES,
            WIN_DRAW_IMAGE,
            WIN_DRAW_STRING,
            FINISHED
        }
        private MAPSTATE State { get; set; }

        public Map(Vector2 position)
        {
            // layout
            Position = position;
            WidthInPixels = Statics.LeftColumnWidth - (int)Position.X * 2;
            HeightInPixels = Statics.CanvasHeight - Statics.LeftColumnPadding * 2 - (int)Position.Y * 2;

            UpdateDelta = TimeSpan.Zero;
            State = MAPSTATE.READY_FOR_BATTLE;

            // DEBUG
            MergeCount = 0;
            // END DEBUG

            // MapTiles holds only whether a map tile coordinate is still available
            MasterTileList = new Tile[WidthInTiles, HeightInTiles];

            for (int x = 0; x < WidthInTiles; x++)
            {
                for (int y = 0; y < HeightInTiles; y++)
                {
                    MasterTileList[x, y] = new Tile(new Point(x, y));
                }
            }

            // now we have a grid of tiles, each of which states its availability
            // loop, creating regions, each of which will switch off a series of tiles
            int AvailableTileCount = WidthInTiles * HeightInTiles;
            int nCurrentRegionId = 0;
            while (AvailableTileCount > 0)
            {
                Region region = new Region(nCurrentRegionId++, MasterTileList);
                Regions.Add(region);

                AvailableTileCount -= region.Tiles.Count;
            }

            MergeRegions(MasterTileList);
            ReindexRegions();
        }

        #region Draw
        public void Draw(CanvasAnimatedDrawEventArgs args)
        {
            switch (State)
            {
                case MAPSTATE.READY_FOR_BATTLE:
                case MAPSTATE.TAKEOVER_IN_PROGRESS:
                case MAPSTATE.PAUSE_BETWEEN_BATTLES:
                    DrawMap(args);
                    break;
                case MAPSTATE.WIN_DRAW_IMAGE:
                    DrawMap(args);
                    DrawWinImage(args);
                    break;
                case MAPSTATE.WIN_DRAW_STRING:
                case MAPSTATE.FINISHED:
                    DrawMap(args);
                    DrawWinImage(args);
                    DrawWinString(args);
                    break;
            }
        }
        private void DrawMap(CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawRectangle(new Rect(Position.X + Statics.LeftColumnPadding, Position.Y + Statics.LeftColumnPadding, WidthInPixels, HeightInPixels), Colors.White);
            foreach(Faction faction in Factions)
            {
                faction.Draw(Position, args);
            }
            //foreach (Region region in Regions)
            //{
            //    region.Draw(Position, args);
            //}
        }
        private void DrawWinImage(CanvasAnimatedDrawEventArgs args)
        {
            // draw partial image over time
            args.DrawingSession.DrawImage(Statics.CrownImage, 
                new Rect(Position.X + Statics.LeftColumnPadding + (WidthInPixels - Statics.CrownImage.Bounds.Width) / 2, 
                         Position.Y + Statics.LeftColumnPadding + (HeightInPixels - Statics.CrownImage.Bounds.Height) / 2,
                         Statics.CrownImage.Bounds.Width,
                         Statics.CrownImage.Bounds.Height * nWinningImageFrame / 100), 
                new Rect(0, 0, Statics.CrownImage.Bounds.Width, Statics.CrownImage.Bounds.Height * nWinningImageFrame / 100));
        }
        private void DrawWinString(CanvasAnimatedDrawEventArgs args)
        {
            // initialize (need drawing args for text width)
            if (strWinnerString == string.Empty)
            {
                // initialize win environment
                strWinnerString = "Congratulations, " + Regions[0].Name + "!";
                CanvasTextLayout winningTextLayout = new CanvasTextLayout(args.DrawingSession, strWinnerString, Statics.FontExtraLarge, 0.0f, 0.0f);
                double dWinningStringWidth = winningTextLayout.DrawBounds.Width;
                fWinnerStringPositionX = (float)(WidthInPixels - dWinningStringWidth) / 2 + Position.X + Statics.LeftColumnPadding;
                fWinnerStringPositionY = 40.0f + Position.Y + Statics.LeftColumnPadding;
            }

            args.DrawingSession.DrawText(strWinnerString.Substring(0, nWinningStringFrame / 5), new Vector2(fWinnerStringPositionX, fWinnerStringPositionY), Colors.White, Statics.FontExtraLarge);
        }
        #endregion

        #region Update
        public void Update(RichListBoxProminent rlb, CanvasAnimatedUpdateEventArgs args)
        {
            UpdateDelta += args.Timing.ElapsedTime;
            if (UpdateDelta.TotalMilliseconds < Statics.MapUpdateThreshold) { return; }

            //switch (State)
            //{
            //    case MAPSTATE.READY_FOR_BATTLE:
            //        // reset update timer
            //        UpdateDelta = TimeSpan.Zero;

            //        // fight a battle
            //        //Region winner = Regions[Statics.Random.Next(Regions.Count)];
            //        //Region loser = RandomNeighbor(winner.ID);

            //        //winner.Leader.BattleWins++;
            //        //loser.Leader.BattleLosses++;
            //        //string strRandomDefeatString = Statics.DefeatWords.RandomString();

            //        //loser.OvertakingRegion = winner;

            //        //LastBattle = new BattleEventArgs(winner, strRandomDefeatString, loser);
            //        ////StringManager.Add(LastBattle.ToRichString());
            //        //if (rlb != null) { rlb.Add(LastBattle.ToRichString()); }

            //        //State = MAPSTATE.TAKEOVER_IN_PROGRESS;
            //        break;
            //    case MAPSTATE.TAKEOVER_IN_PROGRESS:
            //        // reset update timer
            //        UpdateDelta = TimeSpan.Zero;
            //        //MergeRegions(winner.ID, loser.ID);

            //        // update one tile per frame
            //        if (!MiniMerge())
            //        {
            //            State = MAPSTATE.PAUSE_BETWEEN_BATTLES;
            //        }
            //        break;
            //    case MAPSTATE.PAUSE_BETWEEN_BATTLES:
            //        // don't update timer unless threshold reached
            //        // check for win condition
            //        if (Regions.Count == 1)
            //        {
            //            Leaderboard.DeclareWinner(Regions[0].Leader.ToString());
            //            State = MAPSTATE.WIN_DRAW_IMAGE;
            //        }
            //        else if (UpdateDelta.TotalMilliseconds > Statics.PauseBetweenBattlesMilliseconds)
            //        {
            //            UpdateDelta = TimeSpan.Zero;
            //            State = MAPSTATE.READY_FOR_BATTLE;
            //        }
            //        break;
            //    case MAPSTATE.WIN_DRAW_IMAGE:
            //        // reset update timer
            //        UpdateDelta = TimeSpan.Zero;

            //        // partial image draw
            //        if (nWinningImageFrame < 100)
            //        {
            //            nWinningImageFrame += 1;
            //        }
            //        else
            //        {
            //            State = MAPSTATE.WIN_DRAW_STRING;
            //        }
            //        break;
            //    case MAPSTATE.WIN_DRAW_STRING:
            //        // partial string draw
            //        if (nWinningStringFrame < strWinnerString.Length * 5)
            //        {
            //            nWinningStringFrame++;
            //        }
            //        else if (UpdateDelta.TotalSeconds >= 5)
            //        {
            //            State = MAPSTATE.FINISHED;
            //        }
            //        break;
            //}
        }
        #endregion

        #region Region Operations
        private void MergeRegions(Tile[,] MasterTileList)
        {
            bool bRestart = true;
            while (bRestart)
            {
                bRestart = false;

                foreach(Region region in Regions)
                {
                    if(region.Tiles.Count <= Statics.MergeThreshold)
                    {
                        Region neighbor = region.RandomNeighbor();
                        MergeCount++;
                        MergeRegions(region, neighbor);
                        bRestart = true;
                    }
                }
            }

            //for (int i = Regions.Count - 1; i >= 0; i--)
            //{
            //    if (Regions[i].Tiles.Count <= Statics.MergeThreshold)
            //    {
            //        // determine merge direction
            //        // pick random tile and check neighbors for a new region
            //        int nMergeRegion = RandomNeighbor(i).ID;
            //        MergeCount++;
            //        MergeRegions(nMergeRegion, i);
            //    }
            //}
        }

        private void MergeRegions(Region r1, Region r2)
        {
            foreach(Tile tile in r2.Tiles)
            {
                r1.Tiles.Add(tile);
            }

            Regions.Remove(r2);
        }

        //private void MergeRegions(int nRegion1, int nRegion2)
        //{
        //    foreach (Tile tile in Regions[nRegion2].Tiles)
        //    {
        //        Regions[nRegion1].Tiles.Add(tile);
        //    }

        //    Regions.RemoveAt(nRegion2);
        //    ReindexRegions();
        //}
        private bool MiniMerge()
        {
            //MergeRegions(winner.ID, loser.ID);
            if (Regions[LastBattle.Loser.ID].Tiles.Count > 0)
            {
                for (int i = 0; i < LastBattle.OvertakeStep; i++)
                {
                    if (Regions[LastBattle.Loser.ID].Tiles.Count > 0)
                    {
                        Tile currentTile = Regions[LastBattle.Loser.ID].Tiles[Regions[LastBattle.Loser.ID].Tiles.Count - 1];
                        currentTile.Region = LastBattle.Winner.ID;
                        Regions[LastBattle.Winner.ID].Tiles.Add(currentTile);

                        Regions[LastBattle.Loser.ID].Tiles.RemoveAt(Regions[LastBattle.Loser.ID].Tiles.Count - 1);
                    }
                }

                return true;
            }
            else
            {
                Regions.RemoveAt(LastBattle.Loser.ID);
                ReindexRegions();
                return false;
            }
        }

        private void ReindexRegions()
        {
            for (int i = 0; i < Regions.Count; i++)
            {
                Regions[i].ID = i;
                Regions[i].ReindexTiles();
            }
        }

        public Region GetRegion(int x, int y)
        {
            return MasterTileList[x, y].Region;
        }

        public Region RandomNeighbor(int nRegionID)
        {
            return Regions[nRegionID].RandomNeighbor();

            //Region region = Regions[nRegionID];
            //while (region == Regions[nRegionID])
            //{
            //    Tile tileRandom = Regions[nRegionID].Tiles.RandomItem();
            //    int tileRandomX = (int)tileRandom.Coordinates.X;
            //    int tileRandomY = (int)tileRandom.Coordinates.Y;

            //    switch (Statics.Random.Next(4))
            //    {
            //        case 0:
            //            // left
            //            if (tileRandomX > 0)
            //            {
            //                region = MasterTileList[tileRandomX - 1, tileRandomY].Region;
            //            }
            //            break;
            //        case 1:
            //            // right
            //            if (tileRandomX < WidthInTiles - 1)
            //            {
            //                region = MasterTileList[tileRandomX + 1, tileRandomY].Region;
            //            }
            //            break;
            //        case 2:
            //            // up
            //            if (tileRandomY > 0)
            //            {
            //                region = MasterTileList[tileRandomX, tileRandomY - 1].Region;
            //            }
            //            break;
            //        case 3:
            //            // down
            //            if (tileRandomY < HeightInTiles - 1)
            //            {
            //                region = MasterTileList[tileRandomX, tileRandomY + 1].Region;
            //            }
            //            break;
            //    }
            //}

            //return region;
        }
        #endregion
    }
}