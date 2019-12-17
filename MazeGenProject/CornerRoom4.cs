using MazeGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenProject
{
    class CornerRoom4 : Room
    {
        int width_, height_;
        bool[] exitable_;
        Tile[,] tiles_;
        bool rotatable_;

        public CornerRoom4()
        {
            width_ = 16;
            height_ = 16;
            exitable_ = new bool[4];
            exitable_[0] = false;
            exitable_[1] = true;
            exitable_[2] = true;
            exitable_[3] = false;
            tiles_ = new Tile[width_, height_];
            for (int x = 0; x < width_; x++)
            {
                for(int y = 0; y < height_; y++)
                {
                    if(x - y < 0)
                    {
                        tiles_[x, y] = new WallTile(x, y);
                    }
                    else
                    {
                        tiles_[x, y] = new FloorTile(x, y);
                    }
                }
            }
        }

        public Tile[,] copy()
        {
            Tile[,] copy = new Tile[width, height];

            for (int x = 0; x < width_; x++)
            {
                for (int y = 0; y < height_; y++)
                {
                    if (x - y < 0)
                    {
                        copy[x, y] = new WallTile(x, y);
                    }
                    else
                    {
                        copy[x, y] = new FloorTile(x, y);
                    }
                }
            }

            return copy;
        }

        public int width {
            get
            {
                return width_;
            }
            set
            {
                width_ = value;
            }
        }

        public int height
        {
            get
            {
                return height_;
            }
            set
            {
                height_ = value;
            }
        }

        public bool rotatable {
            get
            {
                return rotatable_;
            }
            set
            {
                rotatable_ = value;
            }
        }

        public Tile[,] tiles {
            get
            {
                return tiles_;
            }
            set
            {
                tiles_ = value;
            }
        }

        public bool[] exitable {
            get
            {
                return exitable_;
            }
            set
            {
                exitable_ = value;
            }
        }
    }
}
