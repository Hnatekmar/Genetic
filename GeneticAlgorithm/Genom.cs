using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GeneticAlgorithm
{
    class Genom
    {
        private BitArray representation;

        public BitArray Representation
        {
            get
            {
                return representation;
            }
        }

        /// <summary>
        /// Default constructor of Genom takes size as parameter
        /// </summary>
        /// <param name="size">Number of bits in genom</param>
        public Genom(int size, bool randomBits = false)
        {
            representation = new BitArray(size);
            if (randomBits)
            {
                representation = Mutate(100).representation;
            }
        }

        /// <summary>
        /// Evaluates genom
        /// </summary>
        /// <param name="evaluateFn">Function to evaluate genom</param>
        /// <returns>Evaluation</returns>
        public double Evaluate(Func<BitArray, double> evaluateFn)
        {
            return evaluateFn(representation);
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="genom">Genom to copy from</param>
        public Genom(Genom genom)
        {
            representation = new BitArray(genom.representation.Length);
            representation.Or(genom.representation);
        }

        /// <summary>
        /// Mutates random bits in genom
        /// </summary>
        /// <param name="mutationRate">Chance of mutation</param>
        public Genom Mutate(int mutationRate)
        {
            Debug.Assert(mutationRate >= 0 && mutationRate <= 100, "Mutation rate must be between 0 - 100");
            Genom result = new Genom(this);
            Random randomGenerator = new Random();
            for (int i = 0; i < representation.Count; i++)
            {
                if (mutationRate <= randomGenerator.Next(101))
                {
                    result.representation.Set(i, !representation.Get(i)); // Mutate bit
                }
            }
            return result;
        }

        /// <summary>
        /// Merges two genes together with random border
        /// </summary>
        /// <param name="other">Gene to merge with</param>
        public Genom Crossover(Genom other)
        {
            Debug.Assert(other != null, "Other cannot be null!");
            Random randomGenerator = new Random();
            int border = randomGenerator.Next(representation.Length);
            if(border == 0)
            {
                border = 1;
            }
            Genom result = new Genom(representation.Length);
            for(int i = 0; i < border; i++)
            {
                result.representation.Set(i, representation.Get(i));
            }
            for(int i = border; i < representation.Length; i++)
            {
                result.representation.Set(i, other.representation.Get(i));
            }
            return result;
        }
    }
}
