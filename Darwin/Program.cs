using System;
using System.Reflection;
using ScottPlot;


//FEATURES:
//Grid-based ecosystem simulation with evolving animals
//Three diet types: Herbivore (0-0.3), Omnivore (0.3-0.7), Carnivore (0.7-1.0) - can be varied in globals
//Three evolvable traits: diet, speed, size
//Trait inheritance: children average parent traits with mutation chance
//Mate validation: animals must be similar enough across all traits to breed 
//Hunting system: carnivores and omnivores can hunt other animals on their tile
//Hunt success based on size difference, failures cost hunger
//Omnivores consume food less efficiently
//Size affects hunt success, base hunger cost and baby cost
//Speed affects how frequently an animal moves
//Dead animals drop meat, less valuable
//Plants spawn randomly on tiles each day
//Population, speed, and size tracked and graphed over time per diet type


//TODO:


//Make food expire after a few days





//Update Animal constructor method
//Add more globals to allow for greater customisation

//Make searchmate only work for those with similar diet

//Add more detail for the 1-3 diets as currently identical

//make system that chooses mate from list
//improve the find mate system to only find valid mates
//may want to make different values for fresh and old meat
//update graph system
//make carnivore food search system search for animals





//system to assure mates are similar enough


//USE SPEED ATTRIBUTE TO DECIDE WHICH ANIMAL GOES FIRST IN TURN
namespace Darwin
{
    public class Manager
    {
        //public static Animal[] animalList = new Animal[100];//list to store animals in
        public static LinkedList animalList = new LinkedList();

        
        
        public static void Main()
        {
            List<int> animalCounts = new List<int>();//list to store the amount of animals at the end of each day
            List<int> vegCounts = new List<int>();
            List<int> omCounts = new List<int>();
            List<int> carnCounts = new List<int>();//count the number of each diet in a day

            List<float> vegSpeed = new List<float>();//average the speed of every type every day.
            List<float> omSpeed = new List<float>();
            List<float> carnSpeed = new List<float>();

            List<float> vegSize = new List<float>();//average the speed of every type every day.
            List<float> omSize = new List<float>();
            List<float> carnSize = new List<float>();


            int daycount = 0;//count the days, end sim after certain number of days
            Map.main = new Map();//initialise new Map

            //animalList[0] = new Animal(Map.main.gridMap[2,2]);//initialise first Animal and assign a Tile

            animalList.AddNode(new Animal(Map.main.gridMap[2, 2], 0.0f, 10, 1.0f));//initialise a new Animal and add it to the linked lsit
            animalList.AddNode(new Animal(Map.main.gridMap[1, 2], 0.0f, 10, 1.0f));
            animalList.AddNode(new Animal(Map.main.gridMap[2, 1], 0.0f, 10, 1.0f));

            //Console.WriteLine(Map.main.gridMap[4, 4].posx);//access the food item of a value

            while (daycount < Globals.TotalDays)
            {
                int total = animalList.CountList("All");
                if (daycount % 20 == 0)
                {
                    Console.WriteLine("Day: " + daycount + " Alive: " + total);
                }
                Map.main.UpdateTiles();//update all tiles
                for (int i = 0; i < Globals.ActionsPerDay; i++)
                {
                    animalList.ActionList();//make animals take action
                }
                animalList.ResetHunts();

                    
                daycount++;//increment the days
                //Console.WriteLine(animalList.CountList(0));
                animalCounts.Add(total);//add to list for graph
                vegCounts.Add(animalList.CountList("Herbivore"));
                omCounts.Add(animalList.CountList("Omnivore"));
                carnCounts.Add(animalList.CountList("Carnivore"));

                vegSpeed.Add(animalList.AvgSpeed("Herbivore"));
                omSpeed.Add(animalList.AvgSpeed("Omnivore"));
                carnSpeed.Add(animalList.AvgSpeed("Carnivore"));

                vegSize.Add(animalList.AvgSize("Herbivore"));
                omSize.Add(animalList.AvgSize("Omnivore"));
                carnSize.Add(animalList.AvgSize("Carnivore"));
            }

            

            // Create X and Y data arrays for plotting
            int[] days = new int[animalCounts.Count];

            for (int i = 0; i < days.Length; i++)
            {
                days[i] = i + 1;
            }

            GraphMaker.MakeLineGraph("Poplinegraph", days, vegCounts, omCounts, carnCounts);
            GraphMaker.MakeLineGraph("Speedlinegraph", days, vegSpeed, omSpeed, carnSpeed);
            GraphMaker.MakeLineGraph("Sizelinegraph", days, vegSize, omSize, carnSize);
            GraphMaker.MakeScatterGraph("FinalPop", animalList.ReturnAllAnimals());
            


        }

    }

}