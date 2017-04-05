using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Revolver
{
    enum Side { Top, Bottom, Right, Left }
    enum Rotation90 {CW, CCW }
    enum Alignment { Horizontal, Vertical}

    class Screen
    {
        public int Level { get; set; }
        public List<Vector2> TileIndexes { get; private set; }
        public TileWall TileRef { get; private set; }
        public TilePlayer Player { get; set; }
        public TileGoal Goal { get; private set; }
        public TileKey Key { get; private set; }
        public Screen(TileWall tile)
        {
            Player = new TilePlayer(new Vector2(0, 0));
            Goal = new TileGoal(new Vector2(0, 0));
            Key = new TileKey(new Vector2(0, 0));
            Level = 1;
            TileIndexes = new List<Vector2>();
            TileRef = tile;
        }

        public void UpdatePlayerPosition(float deltaTime)
        {
            Vector2 tmpVec = new Vector2();
            tmpVec.Y = Player.Index.Y + 1f;
            tmpVec.X = Player.Index.X;
            Player.Index = tmpVec;
        }

        public void DrawPlayer(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(Player.Index.X * TileRef.Width, Player.Index.Y * TileRef.Height);
            spriteBatch.Draw(Player.Texture, position);
        }

        public void DrawKey(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(Key.Index.X * TileRef.Width, Key.Index.Y * TileRef.Height);
            spriteBatch.Draw(Key.Texture, position);
        }

        public void DrawGoal(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(Goal.Index.X * TileRef.Width, Goal.Index.Y * TileRef.Height);
            spriteBatch.Draw(Goal.Texture, position);
        }
        public void AddTile(int xIndex, int yIndex)
        {
            TileIndexes.Add(new Vector2(xIndex, yIndex));
        }

        public void ClearTiles()
        {
            TileIndexes.Clear();
        }

        public Vector2 Rotate90(Vector2 index, Rotation90 direction)
        {
            float centerX = (TileRef.ScreenDivision - 1) / 2;
            float centerY = (TileRef.ScreenDivision - 1) / 2;
            float tmp;
            Vector2 tmpVec = new Vector2();
     
            //convert to Cartesian
            tmpVec.X = index.X - centerX;
            tmpVec.Y = centerY - index.Y;

            //do rotation
            if (direction == Rotation90.CW)
            {
                tmp = tmpVec.X;
                tmpVec.X = tmpVec.Y;
                tmpVec.Y = -tmp;
            }

            else if(direction == Rotation90.CCW)
            {
                tmp = tmpVec.X;
                tmpVec.X = -tmpVec.Y;
                tmpVec.Y = tmp;
            }

            //convert back
            tmpVec.X += centerX;
            tmpVec.Y = centerY - tmpVec.Y;

            return tmpVec;
        }

        public void DrawTiles(SpriteBatch spriteBatch)
        {
            foreach (Vector2 index in TileIndexes)
            {
                Vector2 position = new Vector2(index.X * TileRef.Width, index.Y * TileRef.Height);
                spriteBatch.Draw(TileRef.Texture, position);
            }
        }

        public void AddLine(Vector2 start, int length, Alignment alignment)
        {
            if (alignment == Alignment.Horizontal)
            {
                for (int i = (int)start.X; i < (int)start.X + length; i++)
                {
                    AddTile(i, (int)start.Y);
                }
            }
            else if(alignment == Alignment.Vertical)
            {
                for (int i = (int)start.Y; i < (int)start.Y + length ; i++)
                {
                    AddTile((int)start.X, i);
                }
            }
        }

        public void AddBorder(int distFromEdge, Side side)
        {
            int screenWidth = TileRef.Width * TileRef.ScreenDivision;
            int screenHeight = TileRef.Height * TileRef.ScreenDivision;
            switch (side)
            {
                case Side.Top:
                    for (int i = 0; i < screenWidth / TileRef.Width; i++)
                        AddTile(i , distFromEdge);
                    break;

                case Side.Bottom:
                     for (int i = 0; i < screenWidth / TileRef.Width; i++)
                        AddTile(i, screenHeight / TileRef.Height - 1 - distFromEdge);
                    break;

                case Side.Left:
                    for (int i = 0; i < screenHeight / TileRef.Height; i++)
                        AddTile(distFromEdge, i);
                    break;

                case Side.Right:
                    for (int i = 0; i < screenHeight / TileRef.Height; i++)
                        AddTile(screenWidth / TileRef.Width - 1 - distFromEdge, i);
                    break;

                default:
                    break;
            }
        }

        public void RotateScreen90CW()
        {
            for (int i =0;i<TileIndexes.Count;i++)
            {
                TileIndexes[i] = Rotate90(TileIndexes[i], Rotation90.CW);
            }
            Player.Index = Rotate90(Player.Index, Rotation90.CW);
            Goal.Index = Rotate90(Goal.Index, Rotation90.CW);
            Key.Index = Rotate90(Key.Index, Rotation90.CW);
        }

        public void RotateScreen90CCW()
        {
            for (int i = 0; i < TileIndexes.Count; i++)
            {
                TileIndexes[i] = Rotate90(TileIndexes[i], Rotation90.CCW);
            }
            Player.Index = Rotate90(Player.Index, Rotation90.CCW);
            Goal.Index = Rotate90(Goal.Index, Rotation90.CCW);
            Key.Index = Rotate90(Key.Index, Rotation90.CCW);
        }

        public bool CheckCollision(Vector2 tileIndex, TilePlayer player)
        {
            Vector2 playerPosition = new Vector2(player.Index.X * TileRef.Width, player.Index.Y * TileRef.Height);
            Vector2 tilePosition = new Vector2(tileIndex.X * TileRef.Width, tileIndex.Y * TileRef.Height);

            if (player.Index.Y == tileIndex.Y - 1 && player.Index.X == tileIndex.X)
                return true;
            else
                return false;
        }

        public bool CheckAllCollisions(TilePlayer player)
        {
            foreach(Vector2 index in TileIndexes)
            {
                if (CheckCollision(index, player))
                    return true;
            }
            return false;
        }

        public bool CheckHasKey()
        {
            if (Player.Index == Key.Index)
                return true;
            else
                return false;
        }

        public bool CheckGoal()
        {
            if (Player.HasKey && Player.Index == Goal.Index)
                return true;
            else
                return false;
        }

        public void DoLevelOne()
        {
            Player.Index = new Vector2(20, 20);
            Goal.Index = new Vector2(21, 20);
            Key.Index = new Vector2(24, 26);

            for (int i = 0; i < 20; i++)
            {
                AddBorder(i, Side.Top);
                AddBorder(i, Side.Bottom);
                AddBorder(i, Side.Left);
                AddBorder(i, Side.Right);
            }

            AddLine(new Vector2(20, 21), 10, Alignment.Horizontal);
            AddLine(new Vector2(29, 21), 9, Alignment.Vertical);
            AddLine(new Vector2(21, 29), 8, Alignment.Horizontal);
            AddLine(new Vector2(21, 23), 7, Alignment.Vertical);
            AddLine(new Vector2(22, 23), 6, Alignment.Horizontal);
            AddLine(new Vector2(27, 24), 4, Alignment.Vertical);
            AddLine(new Vector2(23, 27), 4, Alignment.Horizontal);
            AddLine(new Vector2(23, 25), 3, Alignment.Vertical);
            AddLine(new Vector2(24, 25), 2, Alignment.Horizontal);
        }

        public void AddTilesToRow(int[] columns, int row)
        {
            foreach (int column in columns)
            {
                AddTile(column, row);
            }
        }

        public void DoLevelTwo()
        {
            Player.Index = new Vector2(20, 20);
            Goal.Index = new Vector2(21, 20);
            Key.Index = new Vector2(24, 26);

            for (int i = 0; i < 20; i++)
            {
                AddBorder(i, Side.Top);
                AddBorder(i, Side.Bottom);
                AddBorder(i, Side.Left);
                AddBorder(i, Side.Right);
            }

            AddTile(20, 21);
            AddTile(22, 20);
            AddTile(21, 23);
            AddTile(29, 20);
            AddTile(30, 22);
            AddTile(30, 24);
            AddTile(21, 25);
            AddTile(22, 28);
            AddTile(25, 27);
            AddTile(29, 29);
            AddTile(27, 27);
        }

        public void DoLevelThree()
        {
            Player.Index = new Vector2(24, 20);
            Goal.Index = new Vector2(25, 26);
            Key.Index = new Vector2(22, 25);

            for (int i = 0; i < 20; i++)
            {
                AddBorder(i, Side.Top);
                AddBorder(i, Side.Bottom);
                AddBorder(i, Side.Left);
                AddBorder(i, Side.Right);
            }

            AddTile(20, 20);
            AddTile(22, 20);
            AddTile(27, 20);
            AddTile(24, 21);
            AddTile(29, 21);
            AddTile(20, 22);
            AddTile(24, 22);
            AddTile(26, 22);
            AddTile(23, 23);
            AddTile(28, 23);
            AddTile(23, 24);
            AddTile(27, 24);
            AddTile(21, 25);
            AddTile(24, 25);
            AddTile(29, 25);
            AddTile(30, 25);
            AddTile(22, 26);
            AddTile(24, 27);
            AddTile(25, 27);
            AddTile(20, 28);
            AddTile(23, 28);
            AddTile(28, 28);
            AddTile(20, 29);
            AddTile(21, 29);
            AddTile(30, 30);
            AddTile(25, 24);
            AddLine(new Vector2(23, 30), 5,Alignment.Horizontal);
        }

        public void DoLevelFour()
        {
            Player.Index = new Vector2(25, 24);
            Goal.Index = new Vector2(26, 21);
            Key.Index = new Vector2(24, 22);

            for (int i = 0; i < 20; i++)
            {
                AddBorder(i, Side.Top);
                AddBorder(i, Side.Bottom);
                AddBorder(i, Side.Left);
                AddBorder(i, Side.Right);
            }

            AddTile(23, 21);
            AddTile(27, 21);
            AddTile(20, 22);
            AddTile(30, 22);
            AddTile(22, 24);
            AddTile(28, 24);
            AddTile(25, 25);
            AddTile(20, 26);
            AddTile(30, 26);
            AddTile(23, 27);
            AddTile(27, 27);
            AddTile(25, 27);
            AddTile(24, 29);
            AddTile(26, 29);

        }

        public void DoLevelFive()
        {
            Player.Index = new Vector2(24, 25);
            Goal.Index = new Vector2(25, 25);
            Key.Index = new Vector2(20, 20);

            for (int i = 0; i < 20; i++)
            {
                AddBorder(i, Side.Top);
                AddBorder(i, Side.Bottom);
                AddBorder(i, Side.Left);
                AddBorder(i, Side.Right);
            }

            AddLine(new Vector2(20, 21), 4, Alignment.Horizontal);
            AddLine(new Vector2(27, 20), 2, Alignment.Horizontal);
            AddTile(27, 21);
            AddTile(25, 22);
            AddTile(22, 23);
            AddTile(29, 22);
            AddTile(27, 23);
            AddTile(21, 24);
            AddTile(27, 24);
            AddTile(23, 25);
            AddTile(26, 25);
            AddLine(new Vector2(23, 26), 3, Alignment.Horizontal);
            AddTile(21, 27);
            AddTile(24, 27);
            AddTile(29, 27);
            AddTile(22, 28);
            AddTile(26, 28);
            AddTile(29, 28);
            AddTile(23, 29);
            AddTile(25, 29);
            AddLine(new Vector2(27, 29), 2, Alignment.Horizontal);
        }

        public void DoLevelSix()
        {
            Player.Index = new Vector2(20, 30);
            Goal.Index = new Vector2(27, 22);
            Key.Index = new Vector2(30, 21);

            for (int i = 0; i < 20; i++)
            {
                AddBorder(i, Side.Top);
                AddBorder(i, Side.Bottom);
                AddBorder(i, Side.Left);
                AddBorder(i, Side.Right);
            }

            AddTile(21, 20);
            AddTile(26, 20);
            AddTile(30, 20);
            AddTile(24, 21);
            AddTile(29, 21);
            AddTile(22, 22);
            AddTile(25, 22);
            AddTile(28, 22);
            AddTile(20, 23);
            AddTile(23, 23);
            AddTile(27, 23);
            AddTile(30, 23);
            AddTile(23, 24);
            AddTile(26, 24);
            AddTile(29, 24);
            AddTile(30, 24);
            AddTile(21, 25);
            AddTile(22, 25);
            AddTile(25, 25);
            AddTile(22, 26);
            AddTile(24, 26);
            AddTile(20, 27);
            AddTile(23, 27);
            AddTile(24, 27);
            AddTile(29,27);
            AddTile(22, 28);
            AddTile(26, 28);
            AddTile(21, 29);
            AddTile(25, 29);
            AddTile(23, 30);
            AddTile(30, 30);
        }

        public void DoLevelSeven()
        {
            Player.Index = new Vector2(23, 22);
            Goal.Index = new Vector2(24, 26);
            Key.Index = new Vector2(24, 20);

            for (int i = 0; i < 20; i++)
            {
                AddBorder(i, Side.Top);
                AddBorder(i, Side.Bottom);
                AddBorder(i, Side.Left);
                AddBorder(i, Side.Right);
            }

            AddTile(23, 20);
            AddTile(28, 20);
            AddTile(20, 21);
            AddTile(26, 21);
            AddTile(30, 31);
            AddTile(22, 22);
            AddTile(29, 22);
            AddTile(23, 23);
            AddTile(25, 23);
            AddTile(21, 24);
            AddTile(30, 24);
            AddTile(24, 25);
            AddTile(27, 25);
            AddTile(29, 25);
            AddTile(30, 25);
            AddTile(23, 26);
            AddTile(20, 28);
            AddTile(29, 28);
            AddTile(22, 29);
            AddTile(26, 25);
            AddTile(25, 30);
            AddTile(24, 28);
            AddTile(29, 29);
            AddTile(20, 30);
        }

        public void DoLevelEight()
        {
            Player.Index = new Vector2(20, 20);
            Goal.Index = new Vector2(21, 20);
            Key.Index = new Vector2(21, 29);

            for (int i = 0; i < 20; i++)
            {
                AddBorder(i, Side.Top);
                AddBorder(i, Side.Bottom);
                AddBorder(i, Side.Left);
                AddBorder(i, Side.Right);
            }

            int[] columns = { 22, 23, 24, 28 };
            AddTilesToRow(columns, 20);
            columns = new int [] { 20, 25};
            AddTilesToRow(columns, 21);
            columns = new int [] { 21, 25, 26, 29};
            AddTilesToRow(columns, 22);
            columns = new int [] { 20, 25, 27};
            AddTilesToRow(columns, 23);
            columns = new int [] { 24, 30};
            AddTilesToRow(columns, 24);
            columns = new int [] { 20, 23, 27, 29};
            AddTilesToRow(columns, 25);
            columns = new int[] { 26 };
            AddTilesToRow(columns, 26);
            columns = new int[] { 23, 26, 29, 30 };
            AddTilesToRow(columns, 27);
            columns = new int[] { 21, 25, 28 };
            AddTilesToRow(columns, 28);

            columns = new int[] { 23, 25, 29 };
            AddTilesToRow(columns, 30);
        }

        public void DoLevelNine()
        {
            Player.Index = new Vector2(23, 23);
            Goal.Index = new Vector2(25, 23);
            Key.Index = new Vector2(25, 29);

            for (int i = 0; i < 20; i++)
            {
                AddBorder(i, Side.Top);
                AddBorder(i, Side.Bottom);
                AddBorder(i, Side.Left);
                AddBorder(i, Side.Right);
            }

            int[] columns = { 20,24,29 };
            AddTilesToRow(columns, 20);
            columns = new int[] { 22,27 };
            AddTilesToRow(columns, 21);
            columns = new int[] { 25,30 };
            AddTilesToRow(columns, 22);
            columns = new int[] { 21 };
            AddTilesToRow(columns, 23);
            columns = new int[] { 20, 23,28,30 };
            AddTilesToRow(columns, 24);
            columns = new int[] { 22,26,30 };
            AddTilesToRow(columns, 25);
            columns = new int[] { 21,23,24 };
            AddTilesToRow(columns, 26);
            columns = new int[] { 20,29,30 };
            AddTilesToRow(columns, 27);
            columns = new int[] {23,25,30};
            AddTilesToRow(columns, 28);
            columns = new int[] { 26,27 };
            AddTilesToRow(columns, 29);
            columns = new int[] { 22,23 };
            AddTilesToRow(columns, 30);
        }

        public void DoLevelTen()
        {
            Player.Index = new Vector2(24, 24);
            Goal.Index = new Vector2(20, 30);
            Key.Index = new Vector2(23, 21);

            for (int i = 0; i < 20; i++)
            {
                AddBorder(i, Side.Top);
                AddBorder(i, Side.Bottom);
                AddBorder(i, Side.Left);
                AddBorder(i, Side.Right);
            }

            int[] columns = { 27,30 };
            AddTilesToRow(columns, 20);
            columns = new int[] { 22,25,28 };
            AddTilesToRow(columns, 21);
            columns = new int[] { 21,26};
            AddTilesToRow(columns, 22);
            columns = new int[] { 29};
            AddTilesToRow(columns, 23);
            columns = new int[] { 21,25,27 };
            AddTilesToRow(columns, 24);
            columns = new int[] { 22, 24 };
            AddTilesToRow(columns, 25);
            columns = new int[] { 21,26,29 };
            AddTilesToRow(columns, 26);
            columns = new int[] { 25,30 };
            AddTilesToRow(columns, 27);
            columns = new int[] { 21,28 };
            AddTilesToRow(columns, 28);
            columns = new int[] { 20,25,30 };
            AddTilesToRow(columns, 29);
            columns = new int[] { 24,26 };
            AddTilesToRow(columns, 30);
        }
    }
}
