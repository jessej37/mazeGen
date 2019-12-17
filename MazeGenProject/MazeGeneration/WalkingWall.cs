using MazeGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneration
{
    class WalkingWall
    {
        Point curPoint;
        List<Point> pastPoints = new List<Point>();
        public bool terminated = false;
        Chunk[,] chunks;
        SeedGen seed;
        int lastWall, life;

        public WalkingWall(Chunk[,] c, SeedGen s, int l)
        {
            life = l;
            chunks = c;
            seed = s;

            curPoint = new Point(seed.nextNum(8) % (chunks.GetLength(0) - 1), seed.nextNum(8) % (chunks.GetLength(1) - 1));

            for(int i = 0; i < life; i++)
            {
                if (chunks[curPoint.X, curPoint.Y].exitable[3] && chunks[curPoint.X, curPoint.Y].exitable[2] && chunks[curPoint.X + 1, curPoint.Y + 1].exitable[1] && chunks[curPoint.X + 1, curPoint.Y + 1].exitable[0])
                {
                    life -= i;
                    break;
                }
                else
                {
                    curPoint.X = seed.nextNum(8) % (chunks.GetLength(0) - 1);
                    curPoint.Y = seed.nextNum(8) % (chunks.GetLength(1) - 1);
                }
            }

            if (life == 0)
                terminated = true;

        }

        public void step()
        {
            if(!terminated)
            {
                if(curPoint.X == -1 || curPoint.X == chunks.GetLength(0) - 1 || curPoint.Y == chunks.GetLength(1) - 1 || curPoint.Y == -1)
                {
                    terminated = true;
                }
                else
                {
                    int hitting = 0, direction = seed.nextNum(2);

                    if (!chunks[curPoint.X, curPoint.Y].exitable[3])
                    {
                        hitting++;
                        direction = 0;
                    }
                    if (!chunks[curPoint.X, curPoint.Y].exitable[2])
                    {
                        hitting++;
                        direction = 1;
                    }
                    if (!chunks[curPoint.X + 1, curPoint.Y + 1].exitable[1])
                    {
                        hitting++;
                        direction = 2;
                    }
                    if (!chunks[curPoint.X + 1, curPoint.Y + 1].exitable[0])
                    {
                        hitting++;
                        direction = 3;
                    }

                    if (hitting > 1)
                    {
                        for(int i = 0; i < pastPoints.Count; i++)
                        {
                            if(pastPoints[i].X == curPoint.X && pastPoints[i].Y == curPoint.Y)
                            {
                                if(lastWall == 0)
                                {
                                    chunks[curPoint.X, curPoint.Y].exitable[3] = true;
                                    chunks[curPoint.X, curPoint.Y + 1].exitable[1] = true;
                                }
                                else if(lastWall == 1)
                                {
                                    chunks[curPoint.X, curPoint.Y].exitable[2] = true;
                                    chunks[curPoint.X + 1, curPoint.Y].exitable[0] = true;
                                }
                                else if(lastWall == 2)
                                {
                                    chunks[curPoint.X + 1, curPoint.Y].exitable[3] = true;
                                    chunks[curPoint.X + 1, curPoint.Y + 1].exitable[1] = true;
                                }
                                else
                                {
                                    chunks[curPoint.X, curPoint.Y + 1].exitable[2] = true;
                                    chunks[curPoint.X + 1, curPoint.Y + 1].exitable[0] = true;
                                }
                                break;
                            }
                        }

                        terminated = true;
                    }
                    else
                    {
                        int shouldTurn = seed.nextNum(2);
                        if (shouldTurn < 1)
                        {
                            direction = (direction - 1) % 4;
                        }
                        else if (shouldTurn < 2)
                        {
                            direction = (direction + 1) % 4;
                        }

                        pastPoints.Add(curPoint);
                        Point newPoint;
                        if (direction == 0)
                        {
                            chunks[curPoint.X + 1, curPoint.Y].exitable[3] = false;
                            chunks[curPoint.X + 1, curPoint.Y + 1].exitable[1] = false;

                            lastWall = 0;
                            newPoint = new Point(curPoint.X + 1, curPoint.Y);
                        }
                        else if (direction == 1)
                        {
                            chunks[curPoint.X, curPoint.Y + 1].exitable[2] = false;
                            chunks[curPoint.X + 1, curPoint.Y + 1].exitable[0] = false;

                            lastWall = 1;
                            newPoint = new Point(curPoint.X, curPoint.Y + 1);
                        }
                        else if (direction == 2)
                        {
                            chunks[curPoint.X, curPoint.Y].exitable[3] = false;
                            chunks[curPoint.X, curPoint.Y + 1].exitable[1] = false;

                            lastWall = 2;
                            newPoint = new Point(curPoint.X - 1, curPoint.Y);
                        }
                        else
                        {
                            chunks[curPoint.X, curPoint.Y].exitable[2] = false;
                            chunks[curPoint.X + 1, curPoint.Y].exitable[0] = false;

                            lastWall = 3;
                            newPoint = new Point(curPoint.X, curPoint.Y - 1);
                        }
                        curPoint = newPoint;
                        life -= 1;
                        if (life == 0)
                            terminated = true;
                    }
                }
            }

        }


    }
}
