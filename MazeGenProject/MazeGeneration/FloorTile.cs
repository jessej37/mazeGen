using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenProject.MazeGeneration
{
    class FloorTile : Tile
    {
        private bool traversible_ = true;
        private int x_, y_;

        public FloorTile(int xIn, int yIn)
        {
            x = xIn;
            y = yIn;
        }

        public bool traversible
        {
            get
            {
                return traversible_;
            }
            set
            {
                Console.WriteLine("Error: Cannot change traversibility of this tile");
            }
        }

        public int x
        {
            get
            {
                return x_;
            }
            set
            {
                x_ = value;
            }
        }

        public int y
        {
            get
            {
                return y_;
            }
            set
            {
                y_ = value;
            }
        }


    }
}
