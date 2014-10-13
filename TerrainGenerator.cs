using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
//Author Denis Thamrin
// For the used version http://www.gameprogrammer.com/fractal.html#diamond
// Based on For Recursive version http://www.smokycogs.com/blog/plasma-fractals/ but not entirely based on it 
// and also http://en.wikipedia.org/wiki/Diamond-square_algorithm
// Uses generate fracatl terrain using diamond square algorithm

namespace Lab
{
    using SharpDX.Toolkit.Graphics;
    class TerrainGenerator
    {

        /*private Double roughness;
        private int width;
        private int height;
        private int iteration;
        
        private Double maxDepth;
        private Double maxHeight;
        private Random random;*/

        private int N;
        private int size;
        Random r = new Random();
        private int range;
        private int ran;
        private int init;
        
        List<VertexPositionNormalColor> result = new List<VertexPositionNormalColor>();
      
        // Array to store the height value
        public float[,] arr;
        // Array to store the 2D vector3  
        public Vector3[,] vectArr;
        private bool turn;

        //Constructor
        //dimension should be a power of two, plus one (e.g. 33x33, 65x65, 129x129, etc.).
        public TerrainGenerator(int N)
        {
            this.N = N;
            this.size = (int)(Math.Pow(2, N)) + 1;
            this.range = N * N;
            this.ran = r.Next(-range / 2, range / 2);
            this.init = r.Next(-range / 2, N);
            this.arr = new float[size, size];
            this.vectArr = new Vector3[size, size];
            this.turn = false;
        }


        /**
         * @return Color according to the height which is the input
         * 
         */
        private Color makeColor(float y)
        {
            if (y < N)
            {
                return Color.SandyBrown;
            }

            if (y < N + 5)
            {
                return Color.SaddleBrown;
            }

            if (y < N + 10)
            {
                return Color.ForestGreen;
            }

            if (y > N + 10 && y < N + 20)
            {
                return Color.DarkGreen;
            }

            if (y > N + 20 && y < N + 30)
            {
                return Color.DarkGray;
            }
            if ((y > N + 40 && y < N + 50))
            {
                return Color.Gray;
            }
            if ((y > N + 55 && y < N + 60))
            {
                return Color.Gray;
            }
            return Color.Snow;
        }

        /**
         * @return a normal vector of the center point 
         * 
         */
        private Vector3 findNormal(Vector3 center, Vector3 left, Vector3 right)
        {
            Vector3 centerRight = right - center;
            Vector3 centerLeft = left - center;

            // Using Cross Product of Vector3 to get the normal vector 
            return Vector3.Cross(centerLeft, centerRight);
        }

        /**
         * @return void. Change the value of the arr array so that it contains
         * the normal vector of each point in the landscape
         */
        private void generateNormal(float[,] arr)
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {

                    // At the origin
                    if (x == 0 & y == 0)
                    {
                        vectArr[x, y] = (findNormal(new Vector3(x, arr[x, y], y), new Vector3(x, arr[x, y + 1], y + 1), new Vector3(x + 1, arr[x + 1, y + 1], y + 1)) +
                                        findNormal(new Vector3(x, arr[x, y], y), new Vector3(x + 1, arr[x + 1, y + 1], y + 1), new Vector3(x + 1, arr[x + 1, y], y))) / 2;

                    }

                    // Corner
                    else if (x == size - 1 & y == 0)
                    {
                        vectArr[x, y] = findNormal(new Vector3(x, arr[x, y], y), new Vector3(x - 1, arr[x - 1, y], y), new Vector3(x, arr[x, y + 1], y + 1));

                    }

                    // Corner
                    else if (x == size - 1 & x == size - 1)
                    {
                        vectArr[x, y] = (findNormal(new Vector3(x, arr[x, y], y), new Vector3(x, arr[x, y - 1], y - 1), new Vector3(x - 1, arr[x - 1, y - 1], y - 1)) +
                                        findNormal(new Vector3(x, arr[x, y], y), new Vector3(x - 1, arr[x - 1, y - 1], y - 1), new Vector3(x - 1, arr[x - 1, y], y))) / 2;

                    }

                    // Corner    
                    else if (x == 0 & y == size - 1)
                    {
                        vectArr[x, y] = findNormal(new Vector3(x, arr[x, y], y), new Vector3(x + 1, arr[x + 1, y], y), new Vector3(x, arr[x, y - 1], y - 1));
                    }


                    // Side
                    else if (x == 0)
                    {
                        vectArr[x, y] = (findNormal(new Vector3(x, arr[x, y], y), new Vector3(x, arr[x, y + 1], y + 1), new Vector3(x + 1, arr[x + 1, y + 1], y + 1)) +
                                        findNormal(new Vector3(x, arr[x, y], y), new Vector3(x + 1, arr[x + 1, y + 1], y + 1), new Vector3(x + 1, arr[x + 1, y], y)) +
                                        findNormal(new Vector3(x, arr[x, y], y), new Vector3(x + 1, arr[x + 1, y], y), new Vector3(x, arr[x, y - 1], y - 1))) / 3;
                    }

                    // Side
                    else if (y == 0)
                    {
                        vectArr[x, y] = (findNormal(new Vector3(x, arr[x, y], y), new Vector3(x - 1, arr[x - 1, y], y), new Vector3(x, arr[x, y + 1], y + 1)) +
                                         findNormal(new Vector3(x, arr[x, y], y), new Vector3(x, arr[x, y + 1], y + 1), new Vector3(x + 1, arr[x + 1, y + 1], y + 1)) +
                                         findNormal(new Vector3(x, arr[x, y], y), new Vector3(x + 1, arr[x + 1, y + 1], y + 1), new Vector3(x + 1, arr[x + 1, y], y))) / 3;

                    }

                     // Side
                    else if (x == size - 1)
                    {
                        vectArr[x, y] = (findNormal(new Vector3(x, arr[x, y], y), new Vector3(x, arr[x, y - 1], y - 1), new Vector3(x - 1, arr[x - 1, y - 1], y - 1)) +
                                        findNormal(new Vector3(x, arr[x, y], y), new Vector3(x - 1, arr[x - 1, y - 1], y - 1), new Vector3(x - 1, arr[x - 1, y], y)) +
                                        findNormal(new Vector3(x, arr[x, y], y), new Vector3(x - 1, arr[x - 1, y], y), new Vector3(x, arr[x, y + 1], y + 1))) / 3;

                    }

                    // Side
                    else if (y == size - 1)
                    {
                        vectArr[x, y] = (findNormal(new Vector3(x, arr[x, y], y), new Vector3(x + 1, arr[x + 1, y], y), new Vector3(x, arr[x, y - 1], y - 1)) +
                                        findNormal(new Vector3(x, arr[x, y], y), new Vector3(x, arr[x, y - 1], y - 1), new Vector3(x - 1, arr[x - 1, y - 1], y - 1)) +
                                        findNormal(new Vector3(x, arr[x, y], y), new Vector3(x - 1, arr[x - 1, y - 1], y - 1), new Vector3(x - 1, arr[x - 1, y], y))) / 3;

                    }
                    // Point inside and middle
                    else
                    {
                        vectArr[x, y] = (findNormal(new Vector3(x, arr[x, y], y), new Vector3(x, arr[x, y + 1], y + 1), new Vector3(x + 1, arr[x + 1, y + 1], y + 1)) +
                                        findNormal(new Vector3(x, arr[x, y], y), new Vector3(x + 1, arr[x + 1, y + 1], y + 1), new Vector3(x + 1, arr[x + 1, y], y)) +
                                        findNormal(new Vector3(x, arr[x, y], y), new Vector3(x + 1, arr[x + 1, y], y), new Vector3(x, arr[x, y - 1], y - 1)) +
                                        findNormal(new Vector3(x, arr[x, y], y), new Vector3(x, arr[x, y - 1], y - 1), new Vector3(x - 1, arr[x - 1, y - 1], y - 1)) +
                                        findNormal(new Vector3(x, arr[x, y], y), new Vector3(x - 1, arr[x - 1, y - 1], y - 1), new Vector3(x, arr[x, y + 1], y + 1))) / 6;
                    }

                }
            }
        }

        /*
       * @return void. Using diamond-squre algorithm to generate the height value
       * of each point for the landcape
       */
        public float[,] generateTerrain()
        {
            
            // Set the array to all zero
            Array.Clear(this.arr, 0, arr.Length);

            // Initial value of the 4 corner sides
            arr[0, 0] = init;
            arr[0, size - 1] = init;
            arr[size - 1, 0] = init;
            arr[size - 1, size - 1] = init;

            int sideLength = size - 1;
            int halfLength = sideLength / 2;
            float avg = 0;
            float adding = 0;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    arr[x, y] = 10;
                }
            }

                /*while (sideLength > 1)
                {
                    // Square Algoritm
                    for (x = 0; x < size - 1; x += sideLength)
                    {

                        for (y = 0; y < size - 1; y += sideLength)
                        {
                            adding = (float)r.NextDouble(-ran / 2, ran);
                            avg = (arr[x, y] + arr[x, y + sideLength]
                                + arr[x + sideLength, y] + arr[x + sideLength, y + sideLength]) / 4;
                            arr[x + halfLength, y + halfLength] = avg + adding;
                        }
                    }
                    ran++;

                    // Diamond Algorithm
                    for (x = 0; x < size; x += halfLength)
                    {

                        for (y = (x + halfLength) % sideLength; y < size; y += sideLength)
                        {
                            adding = (float)r.NextDouble(-ran / 2, ran);
                            avg = (arr[(x - halfLength + size) % size, y] +
                                  arr[(x + halfLength) % size, y] +
                                  arr[x, (y + halfLength) % size] +
                                  arr[x, (y - halfLength + size) % size]) / 4;
                            arr[x, y] = avg + adding;
                        }
                    }

                    sideLength /= 2;
                    halfLength = sideLength / 2;
                    ran /= 2;
                }*/
                return this.arr;
        }


        /*
         * @return an array of VertexNormalPositionNormalColor that already sets the 
         * position of each vector to form a landscape.
         */
        public VertexPositionNormalColor[] generateVertex(float[,] arr)
        {

            // Generate the normal vector 
            generateNormal(arr);

            // Iteration goes bottom up
            for (int x = size - 1; x >= 1; x--)
            {
                // Iteration goes forward
                if (x % 2 == 0)
                {
                    turn = true;

                    for (int y = 0; y < size - 1; y++)
                    {

                        // First Triangle
                        result.Add(new VertexPositionNormalColor(new Vector3(x, arr[x, y], y), vectArr[x, y], makeColor(arr[x, y])));
                        result.Add(new VertexPositionNormalColor(new Vector3(x - 1, arr[x - 1, y], y), vectArr[x - 1, y], makeColor(arr[x - 1, y])));
                        result.Add(new VertexPositionNormalColor(new Vector3(x - 1, arr[x - 1, y + 1], y + 1), vectArr[x - 1, y + 1], makeColor(arr[x - 1, y + 1])));

                        // Second Triangle
                        result.Add(new VertexPositionNormalColor(new Vector3(x, arr[x, y], y), vectArr[x, y], makeColor(arr[x, y])));
                        result.Add(new VertexPositionNormalColor(new Vector3(x - 1, arr[x - 1, y + 1], y + 1), vectArr[x - 1, y + 1], makeColor(arr[x - 1, y + 1])));
                        result.Add(new VertexPositionNormalColor(new Vector3(x, arr[x, y + 1], y + 1), vectArr[x, y + 1], makeColor(arr[x, y + 1])));
                    }
                }
                else
                {
                    turn = false;
                    for (int y = size - 1; y >= 1; y--)
                    {
                        // First Triangle
                        result.Add(new VertexPositionNormalColor(new Vector3(x, arr[x, y], y), vectArr[x, y], makeColor(arr[x, y])));
                        result.Add(new VertexPositionNormalColor(new Vector3(x, arr[x, y - 1], y - 1), vectArr[x, y - 1], makeColor(arr[x, y - 1])));
                        result.Add(new VertexPositionNormalColor(new Vector3(x - 1, arr[x - 1, y], y), vectArr[x - 1, y], makeColor(arr[x - 1, y])));

                        // Second Triangle
                        result.Add(new VertexPositionNormalColor(new Vector3(x, arr[x, y - 1], y - 1), vectArr[x, y - 1], makeColor(arr[x, y - 1])));
                        result.Add(new VertexPositionNormalColor(new Vector3(x - 1, arr[x - 1, y - 1], y - 1), vectArr[x - 1, y - 1], makeColor(arr[x - 1, y - 1])));
                        result.Add(new VertexPositionNormalColor(new Vector3(x - 1, arr[x - 1, y], y), vectArr[x - 1, y], makeColor(arr[x - 1, y])));
                    }
                }
            }
            return result.ToArray();
        }

    }
     
}
