using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneration
{
    interface Room
    {
        int width
        {
            get;
            set;
        }

        int height
        {
            get;
            set;
        }
        
        bool rotatable
        {
            get;
            set;
        }

        Tile[,] tiles
        {
            get;
            set;
        }

        Tile[,] copy();

        bool[] exitable
        {
            get;
            set;
        }
    }
}
