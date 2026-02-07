namespace Darwin
{
    public static class Globals
    {
        public const int TotalDays = 15000;
        public const int ActionsPerDay = 10; // amount of actions in a day, standard speed of 10 would compelte 1, speed of 5 would complete 2 etc
        public const int MapSize = 12;
        public const int PlantVal = 60;
        public const int MeatVal = 250;
        public const int MutationChance= 4;
        public const float MutationAmount = 0.1f;
        public const int MaxSpeed = 10;
        public const float MaxSize = 2.0f;
        public const float MinSize = 2.0f;
        public const float HerbivoreThreshold = 0.3f;
        public const float CarnivoreThreshold = 0.7f;
        public const int FoodChance = 7;//change these values to see different effects
    }
}