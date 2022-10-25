using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Assignment_Part_1
{
	class Program
	{
		static void Main(string[] args)
		{
            // PART 1: Making Chocolates
            string allChocosPath = @".\All Chocolates.txt";
            if (File.Exists(allChocosPath))
            {
                // Empties existing file
                using (StreamWriter sw = File.CreateText(allChocosPath))
                {
                    sw.WriteLine("");
                }
            }
            // All ingredients and processes put into arrays
            string[] ingredient1 = { "Strawberry", "Mint", "Nougat", "Truffle", "Hazelnut", "Orange", "Toffee" };
            string[] ingredient2 = { "Rosemary", "Thyme", "Sage", "Chilli", "Pepper", "Lemongrass", "Sea salt" };
            string[] process = { "Surprise", "Whip", "Delight", "Truffle", "Hazelnut", "Orange", "Whirl" };

            int counter = 0; // Used for numbering each chocolate
            // Interates through every ingredient and process in 3 for loops
            for (int i = 0; i < ingredient1.Length; i++)
            {
                for (int j = 0; j < ingredient2.Length; j++)
                {
                    for (int k = 0; k < process.Length; k++)
                    {
                        counter++;
                        using (StreamWriter sw = File.AppendText(allChocosPath))
                        {
                            // Writes to file and to console
                            sw.WriteLine("{0}. {1} and {2} {3}", counter, ingredient1[i], ingredient2[j], process[k]);
                            Console.WriteLine("{0}. {1} and {2} {3}", counter, ingredient1[i], ingredient2[j], process[k]);
                        }
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
    }
}
