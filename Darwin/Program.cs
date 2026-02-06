using System;
using ScottPlot;


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


            int daycount = 0;//count the days, end sim after certain number of days
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

                vegSpeed.Add(animalList.AvgSpeed("Herbivore"));
                omSpeed.Add(animalList.AvgSpeed("Omnivore"));
                carnSpeed.Add(animalList.AvgSpeed("Carnivore"));
            }

            ScottPlot.Plot popGraph = new();
            ScottPlot.Plot speedGraph = new();

            // Create X and Y data arrays for plotting
            int[] days = new int[animalCounts.Count];

            for (int i = 0; i < days.Length; i++)
            {
                days[i] = i + 1;
            }

            var omCountPlot = popGraph.Add.Scatter(days, omCounts.ToArray());
            omCountPlot.LineWidth = 50;
            omCountPlot.MarkerSize = 0;
            omCountPlot.LegendText = "Omnivore";

            var vegCountPlot = popGraph.Add.Scatter(days, vegCounts.ToArray());
            vegCountPlot.LineWidth = 10;
            vegCountPlot.MarkerSize = 0;
            vegCountPlot.LegendText = "Herbivore";

            var carnCountPlot = popGraph.Add.Scatter(days, carnCounts.ToArray());
            carnCountPlot.LineWidth = 10;
            carnCountPlot.MarkerSize = 0;
            carnCountPlot.LegendText = "Carnivore";

            popGraph.SavePng("Poplinegraph.png", 25000, 5000);//save file with name and dimensions

            var omSpeedPlot = speedGraph.Add.Scatter(days, omSpeed.ToArray());
            omSpeedPlot.LineWidth = 30;
            omSpeedPlot.MarkerSize = 0;
            omSpeedPlot.LegendText = "Omnivore";

            var vegSpeedPlot = speedGraph.Add.Scatter(days, vegSpeed.ToArray());
            vegSpeedPlot.LineWidth = 10;
            vegSpeedPlot.MarkerSize = 0;
            vegSpeedPlot.LegendText = "Herbivore";

            var carnSpeedPlot = speedGraph.Add.Scatter(days, carnSpeed.ToArray());
            carnSpeedPlot.LineWidth = 10;
            carnSpeedPlot.MarkerSize = 0;
            carnSpeedPlot.LegendText = "Carnivore";

            speedGraph.SavePng("Speedlinegraph.png", 25000, 5000);
        }

    }

}