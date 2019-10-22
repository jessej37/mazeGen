using MazeGenProject.MazeGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAndSlash.MazeGeneration
{
    class MazeGenerator
    {
        SeedGen seed;
        public List<Tile> availableTiles = new List<Tile>();
        List<Tile> traversible = new List<Tile>(), nonTraversible = new List<Tile>();
        int width, height;
        Chunk[,] chunks;
        
        public MazeGenerator()
        {
            seed = new SeedGen();
            width = 16;
            height = 16;
            init();
        }

        public MazeGenerator(int w, int h)
        {
            seed = new SeedGen();
            width = w;
            height = h;
            init();
        }

        public MazeGenerator(string s)
        {
            seed = new SeedGen(s);
            width = 16;
            height = 16;
            init();
        }

        public MazeGenerator(int w, int h, string s)
        {
            seed = new SeedGen(s);
            width = w;
            height = h;
            init();
        }
        
        public Chunk[,] getMap()
        {
            return chunks;
        }

        public void init()
        {
            System.Diagnostics.Debug.WriteLine("MazeGenerator Init");

            chunks = new Chunk[width, height];

            System.Diagnostics.Debug.WriteLine("Debug Block: -1");
            availableTiles.Add(new WallTile(0, 0));
            availableTiles.Add(new FloorTile(0, 0));

            System.Diagnostics.Debug.WriteLine("Debug Block: 0");
            foreach (Tile tile in availableTiles)
            {
                if (tile.traversible)
                    traversible.Add(tile);
                else
                    nonTraversible.Add(tile);
            }
            System.Diagnostics.Debug.WriteLine("Debug Block: 1");
            int chunkWidth = 16, chunkHeight = 16;
            bool[] up = { true, false, true, true }, left = { false, true, true, true }, right = { true, true, false, true }, down = { true, true, true, false },
                tlCorner = { false, false, true, true }, trCorner = { true, false, false, true }, blCorner = { false, true, true, false }, brCorner = { true, true, false, false };
            System.Diagnostics.Debug.WriteLine("Debug Block: 2");
            chunks[0, 0] = new Chunk(chunkWidth, chunkHeight, tlCorner);
            chunks[0, height - 1] = new Chunk(chunkWidth, chunkHeight, blCorner);
            chunks[width - 1, 0] = new Chunk(chunkWidth, chunkHeight, trCorner);
            chunks[width - 1, height - 1] = new Chunk(chunkWidth, chunkHeight, brCorner);
            System.Diagnostics.Debug.WriteLine("Debug Block: 3");
            for (int y = 1; y < height - 1; y++)
            {
                chunks[0, y] = new Chunk(chunkWidth, chunkHeight, left);
                chunks[width - 1, y] = new Chunk(chunkWidth, chunkHeight, right);
            }
            System.Diagnostics.Debug.WriteLine("Debug Block: 4");
            for (int x = 1; x < width - 1; x++)
            {
                chunks[x, 0] = new Chunk(chunkWidth, chunkHeight, up);
                chunks[x, height - 1] = new Chunk(chunkWidth, chunkHeight, down);

                for (int y = 1; y < height - 1; y++)
                {
                    chunks[x, y] = new Chunk(chunkWidth, chunkHeight);
                }
            }
            System.Diagnostics.Debug.WriteLine("Debug Block: 5");
            int[,] walkingWalls = new int[width * height / 4, 2];
            bool[] terminated = new bool[width * height / 4];
            bool allTerminated = false;
            System.Diagnostics.Debug.WriteLine("Debug Block: 6");
            for (int i = 0; i < terminated.Length; i++)
            {
                terminated[i] = false;
                walkingWalls[i, 0] = seed.nextNum(8) % (width - 1);
                walkingWalls[i, 1] = seed.nextNum(8) % (height - 1);
                System.Diagnostics.Debug.WriteLine("Wall start " + i + ": " + walkingWalls[i, 0] + ", " + walkingWalls[i, 1]);
            }

            System.Diagnostics.Debug.WriteLine("Starting Random Walk");

            while (!allTerminated)
            {
                int numTerm = 0;
                allTerminated = true;
                for (int i = 0; i < terminated.Length; i++)
                {
                    System.Diagnostics.Debug.WriteLine("Debug Block: 1");
                    if (terminated[i] || walkingWalls[i, 0] == -1 || walkingWalls[i, 0] == width - 1 || walkingWalls[i, 1] == -1 || walkingWalls[i, 1] == height - 1)
                    {
                        terminated[i] = true;
                        numTerm++;
                        continue;
                    }
                    System.Diagnostics.Debug.WriteLine("Debug Block: 2");
                    allTerminated = false;
                    System.Diagnostics.Debug.WriteLine("Debug Block: 3");
                    int hitting = 0, direction = seed.nextNum(2);
                    if (!chunks[walkingWalls[i, 0], walkingWalls[i, 1]].exitable[3])
                    {
                        hitting++;
                        direction = 0;
                        System.Diagnostics.Debug.WriteLine("Debug Block: 4.1");
                    }
                    if (!chunks[walkingWalls[i, 0], walkingWalls[i, 1]].exitable[2])
                    {
                        hitting++;
                        direction = 1;
                        System.Diagnostics.Debug.WriteLine("Debug Block: 4.2");
                    }
                    if (!chunks[walkingWalls[i, 0] + 1, walkingWalls[i, 1] + 1].exitable[1])
                    {
                        hitting++;
                        direction = 2;
                        System.Diagnostics.Debug.WriteLine("Debug Block: 4.3");
                    }
                    if (!chunks[walkingWalls[i, 0] + 1, walkingWalls[i, 1] + 1].exitable[0])
                    {
                        hitting++;
                        direction = 3;
                        System.Diagnostics.Debug.WriteLine("Debug Block: 4.4");
                    }
                    System.Diagnostics.Debug.WriteLine("Debug Block: 4");
                    if (hitting > 1)
                    {
                        terminated[i] = true;
                        continue;
                    }
                    System.Diagnostics.Debug.WriteLine("Debug Block: 5");
                    int shouldTurn = seed.nextNum(2);
                    if(shouldTurn < 1)
                    {
                        direction = (direction - 1) % 4;
                    }
                    else if(shouldTurn < 2)
                    {
                        direction = (direction + 1) % 4;
                    }
                    System.Diagnostics.Debug.WriteLine("Debug Block: 6");
                    if (direction == 0)
                    {
                        chunks[walkingWalls[i, 0] + 1, walkingWalls[i, 1]].exitable[3] = false;
                        chunks[walkingWalls[i, 0] + 1, walkingWalls[i, 1] + 1].exitable[1] = false;
                        walkingWalls[i, 0]++;
                    }
                    else if(direction == 1)
                    {
                        chunks[walkingWalls[i, 0], walkingWalls[i, 1] + 1].exitable[2] = false;
                        chunks[walkingWalls[i, 0] + 1, walkingWalls[i, 1] + 1].exitable[0] = false;
                        walkingWalls[i, 1]++;
                    }
                    else if(direction == 2)
                    {
                        chunks[walkingWalls[i, 0], walkingWalls[i, 1]].exitable[3] = false;
                        chunks[walkingWalls[i, 0], walkingWalls[i, 1] + 1].exitable[1] = false;
                        walkingWalls[i, 0]--;
                    }
                    else
                    {
                        chunks[walkingWalls[i, 0], walkingWalls[i, 1]].exitable[2] = false;
                        chunks[walkingWalls[i, 0] + 1, walkingWalls[i, 1]].exitable[0] = false;
                        walkingWalls[i, 1]--;
                    }
                    System.Diagnostics.Debug.WriteLine("Debug Block: 7");
                }
                System.Diagnostics.Debug.WriteLine("Terminated: " + numTerm);
                System.Diagnostics.Debug.WriteLine("WalkingWalls0, x: " + walkingWalls[0, 0] + ", y: " + walkingWalls[0, 1]);
            }
            System.Diagnostics.Debug.WriteLine("Finished Random Walk");
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    chunks[x, y].init(x * chunkWidth, y * chunkHeight);
                }
            }

        }


    }
}