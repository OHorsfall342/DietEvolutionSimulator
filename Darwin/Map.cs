namespace Darwin{


    public class Map
    {
        // Static member to hold the main instance of the Map class.
        public static Map main;//initialise static Map
        public Tile[,] gridMap = new Tile[Globals.mapsize, Globals.mapsize];//initialise gridMap as what is defined in globals

        public Map()
        {
            
            for (int i = 0; i < Globals.mapsize; i++)
            {
                for (int j = 0; j < Globals.mapsize; j++)
                {
                    gridMap[i,j] = new Tile(i, j);
                    //fill the gridMap array with tiles
                }
            }
                
        }
        public void UpdateTiles()//update each Tile before the animals use their turn
        {
            for (int i = 0; i < Globals.mapsize; i++)
            {
                for (int j = 0; j < Globals.mapsize; j++)
                {
                    gridMap[i, j].UpdateTile();
                    //fill the gridMap array with tiles
                }
            }
        }
    }

}