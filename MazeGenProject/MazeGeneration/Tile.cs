using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenProject.MazeGeneration
{
    interface Tile
    {
        bool traversible
        {
            get;
            set;
        }

        int x
        {
            get;
            set;
        }

        int y
        {
            get;
            set;
        }
    }
}
