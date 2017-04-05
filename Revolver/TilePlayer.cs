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
    class TilePlayer 
    {
        public Texture2D Texture { get; set; }
        public Vector2 Index { get; set; }
        public float FallVelocity { get; set; }
        public bool IsLanded { get; set; }
        public bool HasKey { get; set; }

        public TilePlayer(Vector2 initialIndex)
        {
            FallVelocity = 10f;
            Index = initialIndex;
            IsLanded = false;
            HasKey = false;
        }

        public void LoadTexture(Game1 game)
        {
            Texture = game.Content.Load<Texture2D>("Images\\PlayerTexture");
        }


    }
}
