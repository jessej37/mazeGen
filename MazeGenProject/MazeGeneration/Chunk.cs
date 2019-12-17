using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneration
{
    class Chunk
    {
        int width, height;
        public int x, y;
        public Tile[,] tiles;
        public bool initialized = false;
        //exitable order: left, up, right, down
        public bool[] exitable = {true, true, true, true};

        public Chunk(int w, int h, int x, int y)
        {
            width = w;
            height = h;
            this.x = x;
            this.y = y;
            tiles = new Tile[width, height];
        }

        public Chunk(int w, int h, int x, int y, bool[] e)
        {
            width = w;
            height = h;
            this.x = x;
            this.y = y;
            tiles = new Tile[width, height];
            e.CopyTo(exitable, 0);
        }

        public void init(int xOffset, int yOffset)
        {
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    tiles[x, y] = new FloorTile(x + xOffset, y + yOffset);
                }
            }

            if(!exitable[0])
            {
                for (int i = 0; i < height; i++)
                    tiles[0, i] = new WallTile(xOffset, i + yOffset);
            }

            if(!exitable[1])
            {
                for (int i = 0; i < width; i++)
                    tiles[i, 0] = new WallTile(i + xOffset, yOffset);
            }

            if (!exitable[2])
            {
                for (int i = 0; i < height; i++)
                    tiles[width - 1, i] = new WallTile(width - 1 + xOffset, i + yOffset);
            }

            if (!exitable[3])
            {
                for (int i = 0; i < width; i++)
                    tiles[i, height - 1] = new WallTile(i + xOffset, height - 1 + yOffset);
            }

            initialized = true;
        }


    }
}
