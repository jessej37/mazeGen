using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackAndSlash.MazeGen
{
    class SeedGen
    {
        public string seed;
        public int seedSize = 128, bitNum = 0, charNum = 0;

        public SeedGen()
        {
            seed = generateSeed();
        }

        public SeedGen(string s)
        {
            if (seed.Length < seedSize)
            {
                Console.WriteLine("SeedGen: Seed too short. Making you a random one instead. Seed size should be " + seedSize + " characters long.");
                s = generateSeed();
            }
            seed = s;
        }

        public bool nextBool()
        {
            if (bitNum % 8 == 0)
                bitNum++;
            if (bitNum % 8 == 1)
                bitNum++;

            int next = (int)seed.ToCharArray()[charNum];
            BitArray thisByte = new BitArray(next);

            bitNum++;

            if (bitNum % 8 == 0)
                charNum++;

            return thisByte[bitNum % 8];
        }

        public string generateSeed()
        {
            string s = "";
            Random r = new Random();

            for (int i = 0; i < seedSize; i++)
            {
                int next = r.Next(0, 64);
                s += seedIntToChar(next);
            }
            Console.WriteLine("SeedGen: Seed generated = " + s);

            return s;
        }

        public char seedIntToChar(int i)
        {
            if (i <= 9)
                i += 48;
            else if (i <= 36)
                i += 55;
            else
                i += 60;
            return (char)i;
        }

        public int seedCharToInt(char c)
        {
            int i = (int)c;
            if (i >= 97)
                i -= 60;
            else if (i >= 65)
                i -= 55;
            else
                i -= 48;
            return i;
        }
    }
}
