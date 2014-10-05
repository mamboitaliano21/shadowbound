using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Author Denis Thamrin
// For the used version http://www.gameprogrammer.com/fractal.html#diamond
// Based on For Recursive version http://www.smokycogs.com/blog/plasma-fractals/ but not entirely based on it 
// and also http://en.wikipedia.org/wiki/Diamond-square_algorithm
// Uses generate fracatl terrain using diamond square algorithm

namespace Lab
{
    using System.Drawing;
    class TerrainGenerator
    {

        private Double roughness;
        private int width;
        private int height;
        private int iteration;
        // terrain max depth and height for random generator
        private Double maxDepth;
        private Double maxHeight;
        private Random random;
        //Constructor
        //dimension should be a power of two, plus one (e.g. 33x33, 65x65, 129x129, etc.).
        public TerrainGenerator(Double roughness, int width, int height, int iteration, int maxDepth, int maxHeight)
        {
            this.roughness = roughness;
            this.width = width;
            this.height = height;
            this.iteration = iteration;
            this.maxDepth = maxDepth;
            this.maxHeight = maxHeight;
            this.random = new Random();
        }

        public float[,] generateTerrain()
        {
            float[,] terrain = new float[width , height];
            float height1 = (float)random.Next(-15, 20);
            float height2 = (float)random.Next(-15, 20);
            float height3 = (float)random.Next(-15, 20);
            float height4 = (float)random.Next(-15, 20);

            Point p1 = new Point(0, 0);
            Point p2 = new Point(width-1, 0);
            Point p3 = new Point(width-1, height-1);
            Point p4 = new Point(0, height-1);

            //add value to the four corner
            terrain[p1.X, p1.Y] = height1;
            terrain[p2.X, p2.Y] = height2;
            terrain[p3.X, p3.Y] = height3;
            terrain[p4.X, p4.Y] = height4;

            int length = width - 1;
            while(length > 1)
            {

                //Mid point displacement
                for (int x = 0; x < width - 1; x += length)
                {
                    for (int y = 0; y < height - 1; y += length)
                    {
                        float average = (terrain[x, y] + terrain[x + length,y]
                            + terrain[x, y+length] + terrain[x + length, y + length]) /4 ;
                        //displace midpoint
                        terrain[(x + length / 2), (y + length / 2)] = average + 
                            (float)(random.NextDouble() * (maxHeight - maxDepth) + maxDepth);
                    }
                }
                //square
                for (int x = 0; x < width - 1; x += length / 2)
                {
                    for (int y = ((x+length/2)) % length;  y < height - 1; y += length)
                    {
                        float average = (terrain[(x - (length / 2) + (width - 1)) % (width - 1), y] +
                            terrain[(x + (length / 2)) % (width - 1), y] +
                            terrain[x, (y - (length / 2) + (height - 1)) % (height - 1)] +
                            terrain[x, (y + (length / 2)) % (height - 1)]) / 4;
                        terrain[x, y] = average + (float)(random.NextDouble() * (maxHeight - maxDepth) + maxDepth);
                    }
                }
                maxDepth *= roughness;
                maxHeight *= roughness;
                length /= 2;
            }
            return terrain;
        }


        public Double[,] recursiveGenerateTerrain()
        {
            this.width = width - 1;
            this.height = height - 1;
            Double[,] terrain = new Double[width + 1, height + 1];
            //initialize the terrain
            Double height1 = random.NextDouble() * (maxHeight - maxDepth) + maxDepth;
            Double height2 = random.NextDouble() * (maxHeight - maxDepth) + maxDepth;
            Double height3 = random.NextDouble() * (maxHeight - maxDepth) + maxDepth;
            Double height4 = random.NextDouble() * (maxHeight - maxDepth) + maxDepth;
            Point p1 = new Point(0, 0);
            Point p2 = new Point(width, 0);
            Point p3 = new Point(width, height);
            Point p4 = new Point(0, height);

            //add value to the four corner
            terrain[p1.X, p1.Y] = height1;
            terrain[p2.X, p2.Y] = height2;
            terrain[p3.X, p3.Y] = height3;
            terrain[p4.X, p4.Y] = height4;

            //call the recursion
            recursiveDiamondSquare(ref terrain, p1, p2, p3, p4, this.iteration);
            return terrain;
        }

        // side effect of affecting the terrain
        private void recursiveDiamondSquare(ref double[,] terrain, Point p1, Point p2, Point p3, Point p4, int currentIteration)
        {
            // if length between the points is > 1 or iteration ?
            if (currentIteration > 0)
            {
                // insert midpoint
                Point midPoint = new Point((p1.X + p3.X) / 2, (p1.Y + p3.Y) / 2);
                terrain[midPoint.X, midPoint.Y] = random.NextDouble() * (maxHeight - maxDepth) + maxDepth;

                //Clockwise movement starting from 12 oclock
                Point diamond1 = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                Point diamond2 = new Point((p2.X + p3.X) / 2, (p2.Y + p3.Y) / 2);
                Point diamond3 = new Point((p3.X + p4.X) / 2, (p3.Y + p4.Y) / 2);
                Point diamond4 = new Point((p4.X + p1.X) / 2, (p4.Y + p1.Y) / 2);

                // getCornerValue
                Double averageTerrain = (terrain[p1.X, p1.Y] + terrain[p2.X, p2.Y] + terrain[p3.X, p3.Y]
                    + terrain[p4.X, p4.Y]) / 4;

                // insert diamond into terrain
                terrain[diamond1.X, diamond1.Y] = averageTerrain + random.NextDouble()
                    * (maxHeight - maxDepth) + maxDepth;
                terrain[diamond2.X, diamond2.Y] = averageTerrain + random.NextDouble()
                    * (maxHeight - maxDepth) + maxDepth;
                terrain[diamond3.X, diamond3.Y] = averageTerrain + random.NextDouble()
                    * (maxHeight - maxDepth) + maxDepth;
                terrain[diamond4.X, diamond4.Y] = averageTerrain + random.NextDouble()
                    * (maxHeight - maxDepth) + maxDepth;

                //reduce the limit at each iteration
                this.maxHeight = maxHeight * this.roughness;
                this.maxDepth = maxDepth * this.roughness;

                currentIteration--;
                // recurse throught the square
                recursiveDiamondSquare(ref terrain, p1, diamond1, midPoint, diamond4, currentIteration);
                recursiveDiamondSquare(ref terrain, diamond1, p2, diamond2, midPoint, currentIteration);
                recursiveDiamondSquare(ref terrain, midPoint, diamond2, p3, diamond3, currentIteration);
                recursiveDiamondSquare(ref terrain, diamond4, midPoint, diamond3, p4, currentIteration);

            }


        }
        // http://danielbeard.wordpress.com/2010/08/07/terrain-generation-and-smoothing 
        // testing function

    }
}
