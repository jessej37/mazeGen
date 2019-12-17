using MazeGeneration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace MazeGenProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle pixel = new Rectangle(0, 0, 1, 1);
        static int mapWidth = 45, mapHeight = 27;
        Chunk[,] chunks = new Chunk[mapWidth, mapHeight];
        MazeGenerator mazeGen;
        Texture2D blkPixel;
        bool spaceReset = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            System.Diagnostics.Debug.WriteLine("Starting");
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            mazeGen = new MazeGenerator(mapWidth, mapHeight);
            mazeGen.addRoom(new CornerRoom());
            mazeGen.addRoom(new CornerRoom2());
            mazeGen.addRoom(new CornerRoom3());
            mazeGen.addRoom(new CornerRoom4());
            mazeGen.init();
            chunks = mazeGen.getMap();

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

            FileStream fstream = new FileStream("Content/blkPixel.png", FileMode.Open);
            blkPixel = Texture2D.FromStream(GraphicsDevice, fstream);

            fstream.Dispose();
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

            if(Keyboard.GetState().IsKeyDown(Keys.Space) && spaceReset)
            {
                mazeGen = new MazeGenerator(mapWidth, mapHeight);

                mazeGen.init();
                chunks = mazeGen.getMap();
            }
            else if(!Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                spaceReset = true;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            for (int w = 0; w < mapWidth; w++)
            {
                for(int h = 0; h < mapHeight; h++)
                {
                    for (int x = 0; x < mazeGen.chunkWidth; x++)
                    {
                        for (int y = 0; y < mazeGen.chunkHeight; y++)
                        {
                            if(!chunks[w, h].tiles[x, y].traversible)
                            {
                                Rectangle thisPixel = new Rectangle(chunks[w, h].tiles[x, y].x + 5, chunks[w, h].tiles[x, y].y + 5, 1, 1);
                                spriteBatch.Draw(blkPixel, thisPixel, Color.Black);
                            }
                        }
                    }
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
