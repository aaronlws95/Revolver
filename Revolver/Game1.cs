using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
namespace Revolver
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        SpriteFont spriteFont;
        bool startingScreen;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TileWall tile;
        Screen screen;
        int screenHeight;
        int screenWidth;
        float deltaTime;

        KeyboardState newKeyboardState;
        KeyboardState oldKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 510; 
            graphics.PreferredBackBufferHeight = 510;   
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            startingScreen = true;
            
            screenHeight = graphics.GraphicsDevice.Viewport.Height;
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            tile = new TileWall(new Vector2(screenWidth, screenHeight));
            screen = new Screen(tile);
            screen.DoLevelOne();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screen.Player.LoadTexture(this);
            screen.Goal.LoadTexture(this);
            screen.Key.LoadTexture(this);
            spriteFont = this.Content.Load<SpriteFont>("Images/text");
            tile.LoadTexture(this);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            TargetElapsedTime = TimeSpan.FromSeconds(1 / 20.0f);
            // TODO: Add your update logic here
            newKeyboardState = Keyboard.GetState();

            if (newKeyboardState.GetPressedKeys().Length > 0 && startingScreen)
                startingScreen = false;

                if (screen.Player.IsLanded)
            {
                if (newKeyboardState.IsKeyDown(Keys.Right) && oldKeyboardState.IsKeyUp(Keys.Right))
                    screen.RotateScreen90CW();
                
                else if (newKeyboardState.IsKeyDown(Keys.Left) && oldKeyboardState.IsKeyUp(Keys.Left))
                    screen.RotateScreen90CCW();
            }

            if(newKeyboardState.IsKeyDown(Keys.D1) && oldKeyboardState.IsKeyDown(Keys.D1))
            {
                screen.Level = 1;
                screen.Player.HasKey = false;
                screen.ClearTiles();
                screen.DoLevelOne();
            }

            else if (newKeyboardState.IsKeyDown(Keys.D2) && oldKeyboardState.IsKeyDown(Keys.D2))
            {
                screen.Level = 2;
                screen.Player.HasKey = false;
                screen.ClearTiles();
                screen.DoLevelTwo();
            }

            else if (newKeyboardState.IsKeyDown(Keys.D3) && oldKeyboardState.IsKeyDown(Keys.D3))
            {
                screen.Level = 3;
                screen.Player.HasKey = false;
                screen.ClearTiles();
                screen.DoLevelThree();
            }

            else if (newKeyboardState.IsKeyDown(Keys.D4) && oldKeyboardState.IsKeyDown(Keys.D4))
            {
                screen.Level = 4;
                screen.Player.HasKey = false;
                screen.ClearTiles();
                screen.DoLevelFour();
            }

            else if (newKeyboardState.IsKeyDown(Keys.D5) && oldKeyboardState.IsKeyDown(Keys.D5))
            {
                screen.Level = 5;
                screen.Player.HasKey = false;
                screen.ClearTiles();
                screen.DoLevelFive();
            }

            else if (newKeyboardState.IsKeyDown(Keys.D6) && oldKeyboardState.IsKeyDown(Keys.D6))
            {
                screen.Level = 6;
                screen.Player.HasKey = false;
                screen.ClearTiles();
                screen.DoLevelSix();
            }

            else if (newKeyboardState.IsKeyDown(Keys.D7) && oldKeyboardState.IsKeyDown(Keys.D7))
            {
                screen.Level = 7;
                screen.Player.HasKey = false;
                screen.ClearTiles();
                screen.DoLevelSeven();
            }

            else if (newKeyboardState.IsKeyDown(Keys.D8) && oldKeyboardState.IsKeyDown(Keys.D8))
            {
                screen.Level = 8;
                screen.Player.HasKey = false;
                screen.ClearTiles();
                screen.DoLevelEight();
            }

            else if (newKeyboardState.IsKeyDown(Keys.D9) && oldKeyboardState.IsKeyDown(Keys.D9))
            {
                screen.Level = 8;
                screen.Player.HasKey = false;
                screen.ClearTiles();
                screen.DoLevelNine();
            }

            else if (newKeyboardState.IsKeyDown(Keys.D0) && oldKeyboardState.IsKeyDown(Keys.D0))
            {
                screen.Level = 10;
                screen.Player.HasKey = false;
                screen.ClearTiles();
                screen.DoLevelTen();
            }

            oldKeyboardState = newKeyboardState;

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!screen.CheckAllCollisions(screen.Player))
            {
                screen.UpdatePlayerPosition(deltaTime);
                screen.Player.IsLanded = false;
            }
            else
                screen.Player.IsLanded = true;

            if (screen.CheckHasKey())
                screen.Player.HasKey = true;

            if (screen.CheckGoal())
            {
                screen.Level++;
                screen.ClearTiles();
                screen.Player.HasKey = false;
                if (screen.Level == 2)
                    screen.DoLevelTwo();
                else if (screen.Level == 3)
                    screen.DoLevelThree();
                else if (screen.Level == 4)
                    screen.DoLevelFour();
                else if (screen.Level == 5)
                    screen.DoLevelFive();
                else if (screen.Level == 6)
                    screen.DoLevelSix();
                else if (screen.Level == 7)
                    screen.DoLevelSeven();
                else if (screen.Level == 8)
                    screen.DoLevelEight();
                else if (screen.Level == 9)
                    screen.DoLevelNine();
                else if (screen.Level == 10)
                    screen.DoLevelTen();
                else
                {
                    startingScreen = true;
                    screen.DoLevelOne();
                    screen.Level = 1;
                }          
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (startingScreen)
            {
                spriteBatch.DrawString(spriteFont, "Controls: 1,2,3,.. etc to choose and reset levels", new Vector2(100, 100), Color.Black);
                spriteBatch.DrawString(spriteFont, "Controls: Left and Right to play", new Vector2(100, 125), Color.Black);
                spriteBatch.DrawString(spriteFont, "Total levels: 10", new Vector2(100, 150), Color.Black);
                spriteBatch.DrawString(spriteFont, "Press any key to continue..", new Vector2(100, 175), Color.Black);
            }

            else
            {
                screen.DrawTiles(spriteBatch);
                screen.DrawGoal(spriteBatch);
                if (!screen.Player.HasKey)
                    screen.DrawKey(spriteBatch);
                screen.DrawPlayer(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
