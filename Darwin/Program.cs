using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.IO;
using System.Text;
using ScottPlot;
using System.Runtime.ExceptionServices;


//TODO:
//Make find animals return a list

//Make food expire after a few days




//Make children inherit information from parents
//Update animal constructor method
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




//IF CHANGING MAP SIZE, NEED TO CHANGE ANIMAL EDGE CASES
//USE SPEED ATTRIBUTE TO DECIDE WHICH ANIMAL GOES FIRST IN TURN
public class Manager
{
    //public static animal[] animallist = new animal[100];//list to store animals in
    public static LinkedList animallist = new LinkedList();

    
    
    public static void Main()
    {
        List<int> animalCounts = new List<int>();//list to store the amount of animals at the end of each day
        List<int> vegCounts = new List<int>();
        List<int> omCounts = new List<int>();
        List<int> carnCounts = new List<int>();//count the number of each diet in a day


        int daycount = 0;//count the days, end sim after certain number of days
        bool alldead = false;
        map.main = new map();//initialise new map

        //animallist[0] = new animal(map.main.gridmap[2,2]);//initialise first animal and assign a tile

        animallist.AddNode(new animal(map.main.gridmap[2, 2], 0, 10));//initialise a new animal and add it to the linked lsit
        animallist.AddNode(new animal(map.main.gridmap[1, 2], 0, 10));
        animallist.AddNode(new animal(map.main.gridmap[2, 1], 0, 10));

        //Console.WriteLine(map.main.gridmap[4, 4].posx);//access the food item of a value

        while (daycount < Globals.totaldays)
        {
            if (daycount % 20 == 0)
            {
                Console.WriteLine(daycount);
            }
            map.main.updatetiles();//update all tiles
            for (int i = 0; i < Globals.actionsperday; i++)
            {
                animallist.ActionList();//make animals take action
            }

                
            daycount++;//increment the days
            //Console.WriteLine(animallist.CountList(0));
            animalCounts.Add(animallist.CountList(0));//add to list for graph
            vegCounts.Add(animallist.CountList(1));
            omCounts.Add(animallist.CountList(2));
            carnCounts.Add(animallist.CountList(3));
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

