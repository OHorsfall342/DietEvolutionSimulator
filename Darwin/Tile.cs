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
            UpdateFood(Globals.random.Next(0, Globals.FoodChance));
            if (plants > Globals.MaxFood)
            {
                plants = Globals.MaxFood;
            }
            if (meat > Globals.MaxFood)
            {
                meat = Globals.MaxFood;
            }
        }
        public void UpdateFood(int amount)
        {
            plants += amount;
            return;
        }

    }
}