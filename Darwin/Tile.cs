namespace Darwin
{
    
    public class Tile
    {
        public int plants = 0;
        public int meat = 0;
        public int posx;
        public int posy;

        public Tile(int _x, int _y)//constructor method 
        {
            posx = _x; 
            posy = _y;
        }

        public void UpdateTile()//update the Tile, randomly check if food is generated
        {
            Random random = new Random();
            int randomNumber = random.Next(0, Globals.foodchance); // Generates a random number between 0 to 14
            if (randomNumber == 0)
            {
                updatefood();
            }
        }
        public void updatefood()//if food is true, set to false. If food is false, set to true
        {
            plants++;
            return;
        }

    }
}