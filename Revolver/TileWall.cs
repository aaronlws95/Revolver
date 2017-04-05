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
    class TileWall
    {
        public Texture2D Texture { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ScreenDivision { get; private set; }

        public TileWall(Vector2 screenDimension)
        {
           ScreenDivision = 51;
           Width = (int)screenDimension.X / ScreenDivision;
           Height = (int)screenDimension.Y / ScreenDivision;   
        }

        public void LoadTexture(Game1 game)
        {
            Texture = game.Content.Load<Texture2D>("Images\\TileTexture");           
        }
    }
}
