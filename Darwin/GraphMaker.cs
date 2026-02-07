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
        }
}