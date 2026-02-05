namespace Darwin{


    public class map
    {
        // Static member to hold the main instance of the map class.
        public static map main;//initialise static map
        public tile[,] gridmap = new tile[Globals.mapsize, Globals.mapsize];//initialise gridmap as what is defined in globals

        public map()
        {
            
            for (int i = 0; i < Globals.mapsize; i++)
            {
                for (int j = 0; j < Globals.mapsize; j++)
                {
                    gridmap[i,j] = new tile(i, j);
                    //fill the gridmap array with tiles
                }
            }
                
        }
        public void updatetiles()//update each tile before the animals use their turn
        {
            for (int i = 0; i < Globals.mapsize; i++)
            {
                for (int j = 0; j < Globals.mapsize; j++)
                {
                    gridmap[i, j].updatethetile();
                    //fill the gridmap array with tiles
                }
            }
        }
    }

}