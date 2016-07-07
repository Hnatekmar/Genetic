using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    class Program
    {

        static int Index(int x, int y, int w)
        {
            return y * w + x;
        }

        static double Evaluator(BitArray input)
        {
            byte[] arr = new byte[9];
            input.CopyTo(arr, 0);
            double err = 0.0;
            int sum = 0;
            /**
             * -> ###
             *    ###
             *    ###
             * 
             *    ###
             * -> ###
             *    ###
             **/
            for(int y = 0; y < 3; y++)
            {
                sum = 0;
                for(int x = 0; x < 3; x++)
                {
                    sum += arr[Index(x, y, 3)] % 9 + 1;
                }
                err += Math.Abs(15 - sum);
            }

            /**
             *    |
             *    V
             *    ###
             *    ###
             *    ###
             * 
             *     |
             *     V
             *    ###
             *    ###
             *    ###
             **/
            for(int x = 0; x < 3; x++)
            {
                sum = 0;
                for(int y = 0; y < 3; y++)
                {
                    sum += arr[Index(x, y, 3)] % 9 + 1;
                }
                err += Math.Abs(15 - sum);
            }
            /**
             *       /
             *   ###
             *   ###
             *   ###
             *  /
             **/
            int px = 0;
            int py = 0;
            do
            {
                sum += arr[Index(px, py, 3)] % 9 + 1;
                px += 1;
                py += 1;
            } while (px != 3 && py != 3);
            err += Math.Abs(15 - sum);
            /**
             *  \    
             *   ###
             *   ###
             *   ###
             *      \
             **/
            px = 2;
            py = 0;
            sum = 0;
            do
            {
                sum += arr[Index(px, py, 3)] % 9 + 1;
                px -= 1;
                py += 1;
            } while (px != -1 && py != 3);
            err += Math.Abs(15 - sum);
            return err / 120.0;
        }

        static void PrintMatrix(byte[] matrix)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Console.Write(matrix[Index(x, y, 3)] % 9 + 1 + " ");
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            GeneticAlgorithm algorithm = new GeneticAlgorithm(10, 8 * 3 * 3, 25, Evaluator);
            foreach(List<Genom> genoms in algorithm)
            {
                Genom best = genoms.First();
                byte[] byteArr = new byte[9];
                best.Representation.CopyTo(byteArr, 0);
                double evaluation = best.Evaluate(Evaluator);
                Console.WriteLine("Populace " + algorithm.Population);
                Console.WriteLine("Nejlepší genom s evaluací: " + evaluation);
                PrintMatrix(byteArr);
                if(evaluation == 0.0)
                {
                    break;
                }
            }
            Console.WriteLine("Genetický algoritmus skončil s generaci číslo: " + algorithm.Population);
            Console.ReadKey();
        }
    }
}
