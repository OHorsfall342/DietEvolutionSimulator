using System;
using ScottPlot;


//TODO:
//Make find animals return a list

//Make food expire after a few days




//Make children inherit information from parents
//Update Animal constructor method
//Add more globals to allow for greater customisation

//Make searchmate only work for those with similar diet
//Add a mutation chance and movement penalty to globals
//Add more detail for the 1-3 diets as currently identical

//make system that chooses mate from list
//improve the find mate system to only find valid mates
//may want to make different values for fresh and old meat
//update graph system
//make carnivore food search system search for animals


//Suppose each day had 10 action points, s with a speed of 10 could move once a day, while somethign with a speed of 5 could move twice a day?


//system to assure mates are similar enough

//IF CHANGING Map SIZE, NEED TO CHANGE Animal EDGE CASES
//USE SPEED ATTRIBUTE TO DECIDE WHICH Animal GOES FIRST IN TURN
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


            int daycount = 0;//count the days, end sim after certain number of days
            bool alldead = false;
            Map.main = new Map();//initialise new Map

            //animalList[0] = new Animal(Map.main.gridMap[2,2]);//initialise first Animal and assign a Tile

            animalList.AddNode(new Animal(Map.main.gridMap[2, 2], 0.0f, 10));//initialise a new Animal and add it to the linked lsit
            animalList.AddNode(new Animal(Map.main.gridMap[1, 2], 0.0f, 10));
            animalList.AddNode(new Animal(Map.main.gridMap[2, 1], 0.0f, 10));

            //Console.WriteLine(Map.main.gridMap[4, 4].posx);//access the food item of a value

            while (daycount < Globals.totaldays)
            {
                if (daycount % 20 == 0)
                {
                    Console.WriteLine(daycount);
                }
                Map.main.UpdateTiles();//update all tiles
                for (int i = 0; i < Globals.actionsperday; i++)
                {
                    animalList.ActionList();//make animals take action
                }

                    
                daycount++;//increment the days
                //Console.WriteLine(animalList.CountList(0));
                animalCounts.Add(animalList.CountList("All"));//add to list for graph
                vegCounts.Add(animalList.CountList("Herbivore"));
                omCounts.Add(animalList.CountList("Omnivore"));
                carnCounts.Add(animalList.CountList("Carnivore"));
            }

            ScottPlot.Plot popgraph = new();

            // Create X and Y data arrays for plotting
            int[] days = new int[animalCounts.Count];
            int[] counts = animalCounts.ToArray();//Create two arrays to be plotted
            int[] vegcountsarray = vegCounts.ToArray();
            int[] omcountsarray = omCounts.ToArray();
            int[] carncountsarray = carnCounts.ToArray();

            for (int i = 0; i < days.Length; i++)
            {
                days[i] = i + 1;
                counts[i] = 0;//make graphs without total pop
            }

            popgraph.Add.Scatter(days, counts);//plot the graph
            popgraph.Add.Scatter(days, omcountsarray);
            popgraph.Add.Scatter(days, vegcountsarray);
            
            popgraph.Add.Scatter(days, carncountsarray);

            popgraph.SavePng("Poplinegraph.png", 25000, 5000);//save file with name and dimensions
        }

    }

}