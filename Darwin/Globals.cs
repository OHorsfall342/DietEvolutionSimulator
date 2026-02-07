namespace Darwin
{
    public static class Globals
    {
        public const int TotalDays = 15000;
        public const int ActionsPerDay = 10; // amount of actions in a day, standard speed of 10 would compelte 1, speed of 5 would complete 2 etc
        public const int MapSize = 12;
        public const int PlantVal = 30;
        public const int MeatVal = 120;
        public const int MutationChance= 4;
        public const float MutationAmount = 0.1f;
        public const int MaxSpeed = 10;
        public const float MaxSize = 2.0f;
        public const float MinSize = 0.5f;
        public const int BabyPenalty = 40; //base hunger subtracted for child
        public const int BaseCost = 10; //base cost of living
        public const float MateThreshold = 0.5f; //allows mating between further apart species
        public const float HerbivoreThreshold = 0.3f;
        public const float CarnivoreThreshold = 0.7f;
        public const int FoodChance = 7;//change these values to see different effects
    }
}