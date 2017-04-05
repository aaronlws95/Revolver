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
    class TileKey
    {
        public Texture2D Texture { get; set; }
        public Vector2 Index { get; set; }

        public TileKey(Vector2 index)
        {
            Index = index;
        }

        public void LoadTexture(Game1 game)
        {
            Texture = game.Content.Load<Texture2D>("Images\\KeyTexture");
        }
    }
}
