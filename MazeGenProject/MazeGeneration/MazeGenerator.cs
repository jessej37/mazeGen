using MazeGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneration
{
    class MazeGenerator
    {
        List<Room> rooms = new List<Room>();
        SeedGen seed;
        public List<Tile> availableTiles = new List<Tile>();
        List<Tile> traversible = new List<Tile>(), nonTraversible = new List<Tile>();
        int width, height, wallLife = 20;
        public int chunkWidth = 16, chunkHeight = 16;
        Chunk[,] chunks;

        public MazeGenerator()
        {
            seed = new SeedGen();
            width = 16;
            height = 16;
        }

        public MazeGenerator(int w, int h)
        {
            seed = new SeedGen();
            width = w;
            height = h;
        }

        public MazeGenerator(string s)
        {
            seed = new SeedGen(s);
            width = 16;
            height = 16;
        }

        public MazeGenerator(int w, int h, string s)
        {
            seed = new SeedGen(s);
            width = w;
            height = h;
        }
        
        public Chunk[,] getMap()
        {
            return chunks;
        }

        public void addRoom(Room room)
        {
            if (room.width == chunkWidth && room.height == chunkHeight)
                rooms.Add(room);
            else
                Console.WriteLine("Error: Room dimensions do not equal chunk dimensions");
        }

        public void roomFill()
        {
            for(int x = 0; x < width - 1; x++)
            {
                for(int y = 0; y < height - 1; y++)
                {
                    bool[] e = chunks[x, y].exitable;
                    List<Room> myRooms = new List<Room>();
                    for(int i = 0; i < rooms.Count; i++)
                    {
                        if (rooms[i].exitable[0] == e[0] && rooms[i].exitable[1] == e[1] && rooms[i].exitable[2] == e[2] && rooms[i].exitable[3] == e[3])
                            myRooms.Add(rooms[i]);
                    }

                    if(myRooms.Count > 0)
                    {
                        Room toFill = myRooms[seed.nextNum(8) % myRooms.Count];
                        Tile[,] copy = toFill.copy();
                        for (int i = 0; i < chunkWidth - 1; i++)
                        {
                            for(int o = 0; o < chunkHeight - 1; o++)
                            {
                                copy[i, o].x = chunks[x, y].tiles[i, o].x;
                                copy[i, o].y = chunks[x, y].tiles[i, o].y;
                                chunks[x, y].tiles[i, o] = copy[i, o];
                            }
                        }
                    }
                }
            }
        }

        public void removeEnclosures()
        {
            bool done = false;

            while (!done)
            {
                done = true;
                Queue<Chunk> flood = new Queue<Chunk>();
                List<Chunk> inside = new List<Chunk>();
                flood.Enqueue(chunks[0, 0]);
                while (flood.Count > 0)
                {
                    Chunk cur = flood.Dequeue();
                    inside.Add(cur);
                    if (cur.exitable[0])
                    {
                        if (!inside.Contains(chunks[cur.x - 1, cur.y]) && !flood.Contains(chunks[cur.x - 1, cur.y]))
                        {
                            flood.Enqueue(chunks[cur.x - 1, cur.y]);
                        }
                    }
                    if (cur.exitable[1])
                    {
                        if (!inside.Contains(chunks[cur.x, cur.y - 1]) && !flood.Contains(chunks[cur.x, cur.y - 1]))
                        {
                            flood.Enqueue(chunks[cur.x, cur.y - 1]);
                        }
                    }
                    if (cur.exitable[2])
                    {
                        if (!inside.Contains(chunks[cur.x + 1, cur.y]) && !flood.Contains(chunks[cur.x + 1, cur.y]))
                        {
                            flood.Enqueue(chunks[cur.x + 1, cur.y]);
                        }
                    }
                    if (cur.exitable[3])
                    {
                        if (!inside.Contains(chunks[cur.x, cur.y + 1]) && !flood.Contains(chunks[cur.x, cur.y + 1]))
                        {
                            flood.Enqueue(chunks[cur.x, cur.y + 1]);
                        }
                    }
                }

                int badX = 0, badY = 0;

                for(int i = 0; i < width; i++)
                {
                    for(int o = 0; o < height; o++)
                    {
                        if (!inside.Contains(chunks[i, o]) && ((!chunks[i, o].exitable[0] && i != 0 && inside.Contains(chunks[i - 1, o])) || (!chunks[i, o].exitable[1] && o != 0 && inside.Contains(chunks[i, o - 1])) || (!chunks[i, o].exitable[2] && i != width - 1 && inside.Contains(chunks[i + 1, o])) || (!chunks[i, o].exitable[3] && o != height - 1 && inside.Contains(chunks[i, o + 1]))))
                        {
                            done = false;
                            badX = i;
                            badY = o;
                        }
                    }
                }

                if(!done)
                {
                    if (badX != 0 && inside.Contains(chunks[badX - 1, badY]))
                        chunks[badX, badY].exitable[0] = true;
                    else if (badY != 0 && inside.Contains(chunks[badX, badY - 1]))
                        chunks[badX, badY].exitable[1] = true;
                    else if (badX != width - 1 && inside.Contains(chunks[badX + 1, badY]))
                        chunks[badX, badY].exitable[2] = true;
                    else if (badY != height - 1 && inside.Contains(chunks[badX, badY + 1]))
                        chunks[badX, badY].exitable[3] = true;
                }
            }
        }

        public void init()
        {
            Console.WriteLine("MazeGenerator Init");

            chunks = new Chunk[width, height];
            
            availableTiles.Add(new WallTile(0, 0));
            availableTiles.Add(new FloorTile(0, 0));
            
            foreach (Tile tile in availableTiles)
            {
                if (tile.traversible)
                    traversible.Add(tile);
                else
                    nonTraversible.Add(tile);
            }
            
            bool[] up = { true, false, true, true }, left = { false, true, true, true }, right = { true, true, false, true }, down = { true, true, true, false },
                tlCorner = { false, false, true, true }, trCorner = { true, false, false, true }, blCorner = { false, true, true, false }, brCorner = { true, true, false, false };
            
            chunks[0, 0] = new Chunk(chunkWidth, chunkHeight, 0, 0, tlCorner);
            chunks[0, height - 1] = new Chunk(chunkWidth, chunkHeight, 0, height - 1, blCorner);
            chunks[width - 1, 0] = new Chunk(chunkWidth, chunkHeight, width - 1, 0, trCorner);
            chunks[width - 1, height - 1] = new Chunk(chunkWidth, chunkHeight, width - 1, height - 1, brCorner);
            
            for (int y = 1; y < height - 1; y++)
            {
                chunks[0, y] = new Chunk(chunkWidth, chunkHeight, 0, y, left);
                chunks[width - 1, y] = new Chunk(chunkWidth, chunkHeight, width - 1, y, right);
            }
            
            for (int x = 1; x < width - 1; x++)
            {
                chunks[x, 0] = new Chunk(chunkWidth, chunkHeight, x, 0, up);
                chunks[x, height - 1] = new Chunk(chunkWidth, chunkHeight, x, height - 1, down);

                for (int y = 1; y < height - 1; y++)
                {
                    chunks[x, y] = new Chunk(chunkWidth, chunkHeight, x, y);
                }
            }
            
            WalkingWall[] walls = new WalkingWall[width * height / 16];
            
            for (int i = 0; i < walls.Length; i++)
            {
                walls[i] = new WalkingWall(chunks, seed, wallLife);
                while(!walls[i].terminated)
                {
                    walls[i].step();
                }
            }

            //removeEnclosures();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    chunks[x, y].init(x * chunkWidth, y * chunkHeight);
                }
            }

            roomFill();

            Console.WriteLine("Init done");
        }


    }
}