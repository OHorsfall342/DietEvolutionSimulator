using System.Drawing;

namespace Darwin
{   
    static public class GraphMaker
    {
        static public void MakeLineGraph(string name, int[] days, List<int> veg, List<int> om, List<int>carn)
            {
                ScottPlot.Plot Graph = new();

                var omCountPlot = Graph.Add.Scatter(days, om.ToArray());
                omCountPlot.LineWidth = 10;
                omCountPlot.MarkerSize = 0;
                omCountPlot.LegendText = "Omnivore";

                var carnCountPlot = Graph.Add.Scatter(days, carn.ToArray());
                carnCountPlot.LineWidth = 10;
                carnCountPlot.MarkerSize = 0;
                carnCountPlot.LegendText = "Carnivore";

                var vegCountPlot = Graph.Add.Scatter(days, veg.ToArray());
                vegCountPlot.LineWidth = 10;
                vegCountPlot.MarkerSize = 0;
                vegCountPlot.LegendText = "Herbivore";

                Graph.SavePng(name + ".png", 10000, 2000);//save file with name and dimensions
            }

        static public void MakeLineGraph(string name, int[] days, List<float> veg, List<float> om, List<float>carn)
            {
                ScottPlot.Plot Graph = new();

                var omCountPlot = Graph.Add.Scatter(days, om.ToArray());
                omCountPlot.LineWidth = 10;
                omCountPlot.MarkerSize = 0;
                omCountPlot.LegendText = "Omnivore";

                var carnCountPlot = Graph.Add.Scatter(days, carn.ToArray());
                carnCountPlot.LineWidth = 10;
                carnCountPlot.MarkerSize = 0;
                carnCountPlot.LegendText = "Carnivore";

                var vegCountPlot = Graph.Add.Scatter(days, veg.ToArray());
                vegCountPlot.LineWidth = 10;
                vegCountPlot.MarkerSize = 0;
                vegCountPlot.LegendText = "Herbivore";

                Graph.SavePng(name + ".png", 10000, 2000);//save file with name and dimensions
            }
        

        static public void MakeScatterGraph(string name, List<Animal> animalList)
            {
                ScottPlot.Plot Graph = new();

                List<double> vegX = new(), vegY = new();//three different lists so they can eb different colours
                List<double> omX = new(), omY = new();
                List<double> carnX = new(), carnY = new();

                foreach (Animal animal in animalList)
                {
                    if (animal.dietName == "Herbivore")
                    { 
                        vegX.Add(animal.size);
                        vegY.Add(animal.speed + (Globals.random.NextDouble() - 0.5) * 0.5);
                    }
                    else if (animal.dietName == "Omnivore")
                    {
                        omX.Add(animal.size);
                        omY.Add(animal.speed + (Globals.random.NextDouble() - 0.5) * 0.5);
                    }
                    else
                    {
                        carnX.Add(animal.size);
                        carnY.Add(animal.speed + (Globals.random.NextDouble() - 0.5) * 0.5);
                    }
                    
                }

                var vegPlot = Graph.Add.Scatter(vegX, vegY);//size x axis speed y axis
                vegPlot.LineWidth = 0;
                vegPlot.MarkerSize = 20;
                vegPlot.Color = ScottPlot.Colors.Green;
                
                var omPlot = Graph.Add.Scatter(omX, omY);//size x axis speed y axis
                omPlot.LineWidth = 0;
                omPlot.MarkerSize = 20;
                omPlot.Color = ScottPlot.Colors.Blue;

                var carnPlot = Graph.Add.Scatter(carnX, carnY);//size x axis speed y axis
                carnPlot.LineWidth = 0;
                carnPlot.MarkerSize = 20;
                carnPlot.Color = ScottPlot.Colors.Red;
                Graph.SavePng(name + ".png", 4000, 4000);
            }

    }
}