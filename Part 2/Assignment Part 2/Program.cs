using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Assignment_Part_2
{
    class Chocolate
    {
        public string CChocoName;
        public double CChocoProductionCost;
        public double CChocoRetailValue;

        public void AddDataFromFile(string[] values)
        {
            CChocoName = values[0];
            CChocoProductionCost = Convert.ToDouble(values[1]);
            CChocoRetailValue = Convert.ToDouble(values[2]);
        }

        public override string ToString()
        {
            return "Name: " + CChocoName + ", Production Cost: " + CChocoProductionCost + ", Value: " + CChocoRetailValue;
        }
    }

    class Program
	{
        const int DECIMAL_PLACES = 2;
        const int MAX_CHOCOLATES = 20;
        const int MIN_CHOCOLATES = 14;
        const float MAX_PROD_COST = 1.96f;


		static void Main(string[] args)
		{
            // PART 2: Making Gift Boxes
            List<Chocolate> billyChoco = new List<Chocolate>();
            string billyChocosPath = @".\chocolates.txt";
            if (!File.Exists(billyChocosPath))
            {
                // Create a file to write to.
                Console.WriteLine("ERROR: chocolates.txt not found");
            }

            // Readies the file to be read from
            using (StreamReader sr = new StreamReader(billyChocosPath))
            {
                // Loops through the file and adds data to the class object array
                InputDataFromFile(sr, ref billyChoco);

                //close the file
                sr.Close();
            }

            int numItems = billyChoco.Count; // Counts the number of useable chocolates (should be 20)
            int numSolutions = (int)Math.Pow(2, numItems) - 1; // Works out number of solutions
            int bestIndex = 0; // Stores the index of the bit array used

            double bestSolutionValue = 0; // Stores/Identifies the best solution by retail value
            // Ignores all solutions that only consider less than 14 chocolates by starting from 2^14
            int startingValue = (int)Math.Pow(2, 14);   // All values below this can not satisfy the at least 14 chocolates conditoin so we ignore them
            for (int i = startingValue; i < numSolutions; i++)
            {
                double currentSolutionProdCost = 0;
                double currentSolutionValue = 0;    // Resets current values for each iteration
                int numChocos = 0;
                BitArray chocoBitArray = new BitArray(new int[] { i });    // Creates a new bit array for this iteration
                for (int j = 0; j < billyChoco.Count; j++)  // Ignores the last 12 values, since we only have 20 values to check and bitarrays always have 32
                {
                    if (chocoBitArray[j]) // If the current bit is "True" (or "1")
                    {
                        numChocos++;
                        currentSolutionProdCost += billyChoco[j].CChocoProductionCost;  // Adds all relevant values
                        currentSolutionValue += billyChoco[j].CChocoRetailValue;
                    }
                }
                // Checks against every condition, and makes sure all of them are met, as well as the solution value being better than the current best value
                if (numChocos >= MIN_CHOCOLATES && currentSolutionProdCost <= MAX_PROD_COST && currentSolutionValue > bestSolutionValue)
                {
                    bestIndex = i; // Current bitarray index used to find this solution
                    // This gets called later to build a List<T> of the best solution
                    bestSolutionValue = currentSolutionValue;   // Updates the best value

                }
            }
            List<string> bestSolutionList = new List<string>(); // New list to store the best solution
            BitArray bestBitArray = new BitArray(new int[] { bestIndex }); // New bitarray built from the index of the best solution
            double bestSolutionWeight = 0;  // Resetting or defining the weight and value to 0 for the best list to be updated
            bestSolutionValue = 0;
            for (int i = 0; i < bestBitArray.Length; i++)   // Runs through the new bitarray
            {
                if (bestBitArray[i])    // If value is "True" or "1"
                {
                    bestSolutionList.Add(billyChoco[i].CChocoName); // Adds the name of the chocolate in the list
                    bestSolutionWeight += billyChoco[i].CChocoProductionCost;   // Adds the weights and values to a total amount 
                    bestSolutionValue += billyChoco[i].CChocoRetailValue;
                }
            }
            bestSolutionWeight = Math.Round(bestSolutionWeight, DECIMAL_PLACES);
            bestSolutionValue = Math.Round(bestSolutionValue, DECIMAL_PLACES);   // Rounds these totals to 2 decimal places
            double profit = bestSolutionValue - bestSolutionWeight; // Calculates the profit from this solution
            profit = Math.Round(profit, DECIMAL_PLACES); // Rounds it
            bestSolutionList.Add(bestSolutionWeight.ToString());
            bestSolutionList.Add(bestSolutionValue.ToString()); // Converts the total amounts to a string and adds them to the list
            Console.WriteLine();

            string bestSolutionPath = @".\Best Solution.txt";
            if (File.Exists(bestSolutionPath))
            {
                // Empties existing file
                using (StreamWriter sw = File.CreateText(bestSolutionPath))
                {
                    sw.WriteLine("");
                }
            }
            for (int i = 0; i < bestSolutionList.Count; i++)    // Iterates through the list
            {
                using (StreamWriter sw = File.AppendText(bestSolutionPath))
                {
                    // Writes to file and to console
                    if (i <= bestSolutionList.Count - 3)    // For every value that is a chocolate name
                    {
                        sw.WriteLine("{0}: {1}", i + 1, bestSolutionList[i]);
                        Console.WriteLine("{0}: {1}", i + 1, bestSolutionList[i]);
                    }
                    else if (i == bestSolutionList.Count - 2)   // For the cost value
                    {
                        sw.WriteLine("Cost: £{0}", bestSolutionList[i]);
                        Console.WriteLine("Cost: £{0}", bestSolutionList[i]);
                    }
                    else if (i == bestSolutionList.Count - 1)   // For the retail value, with profit added on as it's the last iteration
                    {
                        sw.WriteLine("Retail Price: £{0}", bestSolutionList[i]);
                        Console.WriteLine("Retail Price: £{0}", bestSolutionList[i]);
                        sw.WriteLine("Profit: £{0}", profit);
                        Console.WriteLine("Profit: £{0}", profit);
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("End of PART 2. Files are made in the same folder where the .exe is stored.");
            Console.ReadLine();
        }

        static void InputDataFromFile(StreamReader file, ref List<Chocolate> Choco)
        {
            string line;
            string[] lineSeparated;

            for (int i = 0; i < MAX_CHOCOLATES; i++)
            {
                Chocolate currentChoc = new Chocolate();
                // Read the next line
                line = file.ReadLine();

                // Splits the line into the three values
                lineSeparated = line.Split(',');

                // Adds the data from the file into the class
                currentChoc.AddDataFromFile(lineSeparated);
                Choco.Add(currentChoc);

                // Write the line to console window
                Console.WriteLine(currentChoc);
            }
        }
    }
}
