using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    class GeneticAlgorithm : IEnumerable<List<Genom>>
    {
        private List<Genom> genoms = new List<Genom>();
        private ulong population = 0;
        private int mutationRate;
        private Func<BitArray, double> evaluationFn;

        public ulong Population
        {
            get
            {
                return population;
            }
        }

        public GeneticAlgorithm(int populationSize, int genomSize, int mutationRate, Func<BitArray, double> evaluationFn)
        {
            this.mutationRate = mutationRate;
            this.evaluationFn = evaluationFn;
            for(int i = 0; i < populationSize; i++)
            {
                genoms.Add(new Genom(genomSize, true));
            }
        }

        void Breed()
        {
            List<Genom> newPopulation = new List<Genom>();
            Genom parentA = genoms.ElementAt(0);
            Genom parentB = genoms.ElementAt(1);
            for(int i = 0; i < genoms.Count; i++)
            {
                newPopulation.Add(parentB.Crossover(parentA.Mutate(this.mutationRate)));
            }
            genoms = newPopulation;
        }

        /// <summary>
        /// Starts genetic algorithm
        /// </summary>
        /// <param name="evaluation"></param>
        /// <param name="mutationRate"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        void Evolve()
        {
            population++;
            genoms.Sort((Genom gA, Genom gB) =>
            {
                double gAEval = gA.Evaluate(evaluationFn);
                double gBEval = gB.Evaluate(evaluationFn);
                return gAEval == gBEval ? 0 : (gAEval > gBEval ? 1 : -1);
            });
            Breed();
        }

        public IEnumerator<List<Genom>> GetEnumerator()
        {
            while (true)
            {
                Evolve();
                yield return genoms;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
    }
}
