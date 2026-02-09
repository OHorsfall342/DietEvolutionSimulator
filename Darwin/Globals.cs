namespace Darwin
{
    public static class Globals
    {
        public const int TotalDays = 3000;
        public const int ActionsPerDay = 10; // amount of actions in a day, standard speed of 10 would compelte 1, speed of 5 would complete 2 etc
        public const int MapSize = 30;
        public const int PlantVal = 100;
        public const int MeatVal = 180;
        public const int OldMeatVal = 60; //value of meat found on floor
        public const int MutationChance= 3;
        public const float MutationAmount = 0.1f;
        public const int MaxSpeed = 10;
        public const float MaxSize = 2.0f;
        public const float MinSize = 0.5f;
        public const int BabyPenalty = 40; //base hunger subtracted for child
        public const int BabyThreshold = 120;
        public const int BaseCost = 10; //base cost of living
        public const float MateThreshold = 0.5f; //allows mating between further apart species
        public const float HerbivoreThreshold = 0.3f;
        public const float CarnivoreThreshold = 0.7f;
        public const int FoodChance = 4;//change these values to see different effects
        public const int MaxFood = 5; //Max amount of food that can be stored on one tile
    }
}