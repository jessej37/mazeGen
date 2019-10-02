using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAndSlash.MazeGen
{
    class MazeGen
    {
        SeedGen seed;

        public MazeGen()
        {
            seed = new SeedGen();
        }

        public MazeGen(string s)
        {
            seed = new SeedGen(s);
        }
        

    }
}
