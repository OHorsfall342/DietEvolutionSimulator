namespace Darwin
{
    public class Animal
    {
        Tile currentTile;
        Animal mate;
        List<Animal> tileAnimals;
        int hunger = 100; //higher speed should decrease hunger by more
        //should speed also effect chance of being caught by predators? e.g. same speed gives 75% chance of caught
        //while a higher speed might give only 50%? i mean a hippo doesnt catch a emu veryoften
        public int speed = 10;
        public float size = 1.0f; // new trait, 0.5 to 2 range
        int lastAction = 0; //how many turns their last action was
        public int currentX;
        public int currentY;
        public string dietName;
        public float diet = 0.0f;//0 = vege, 1.0 = meat eater, check globals for threshholds, inbetween is omnivore
        float plantEfficiency = 1.0f;
        float meatEfficiency = 1.0f;
        public bool huntedThisTurn = false;

        public Animal(Tile _tile, float _diet, int _speed, float _size)//constructor method
        {
            speed = _speed;
            size = _size;
            diet = _diet;
            currentTile = _tile;
            currentX = currentTile.posx;
            currentY = currentTile.posy;//set current positions to the Tile

            if (diet < Globals.HerbivoreThreshold)
            {
                dietName = "Herbivore";
            }
            else if (diet > Globals.CarnivoreThreshold)
            {
                dietName = "Carnivore";
            }
            else
            {
                dietName = "Omnivore";
                float omPosition = (diet - Globals.HerbivoreThreshold) / (Globals.CarnivoreThreshold - Globals.HerbivoreThreshold);
                // omPosition: 0.0 = near herbivore, 1.0 = near carnivore

                plantEfficiency = 0.8f - (omPosition * 0.4f);  // 0.4-0.8  
                meatEfficiency = 0.4f + (omPosition * 0.4f);   // 0.4-0.8

                //average omnivore will get a 0.8 penalty on all foods
                //clsoer to either side gets clsoer to 1 and 0.6
            }
        }

        public bool TakeAction()//call for the Animal to take their turn, return false if dead
        {
            if (speed > lastAction)
            {
                lastAction++;
                return true;
            }

            lastAction = 0; //set last action to 0 since its ongoing

            
            if (hunger > Globals.BabyThreshold)
            {
                SeekMateOrFood();
            }
            else{
                SearchOrStarve();
            }
            //for if hunger is lower than decided value
            
            if (hunger <= 0)//check if Animal is dead
            {
                return false;//returns false if dead
            }
            return true;
        }
        
        
        bool FindMate()//move to anearby Tile with a mate
        {
            if (currentX > 0)//check if on edge
            {
                if (ValidateMates(Manager.animalList.SearchList(currentX - 1, currentY, this).ToArray()) != null)//find mate, move to that Tile
                {
                    currentTile = Map.main.gridMap[currentX - 1, currentY];
                    currentX = currentX - 1;//set new currentX
                    return true;
                }
            }
            if (currentY > 0)//check if on edge
            {
                if (ValidateMates(Manager.animalList.SearchList(currentX, currentY - 1, this).ToArray()) != null)//find mate, move to that Tile
                {
                    currentTile = Map.main.gridMap[currentX, currentY - 1];
                    currentY = currentY - 1; //set new currentY
                    return true;
                }
            }
            if (currentX < Globals.MapSize - 1)//check if on edge
            {
                if (ValidateMates(Manager.animalList.SearchList(currentX + 1, currentY, this).ToArray()) != null)//find mate, move to that Tile
                {
                    currentTile = Map.main.gridMap[currentX + 1, currentY];
                    currentX = currentX + 1;//set new currentX
                    return true;
                }
            }
            if (currentY < Globals.MapSize - 1)//check if on edge
            {
                if (ValidateMates(Manager.animalList.SearchList(currentX, currentY + 1, this).ToArray()) != null)//find mate, move to that Tile
                {
                    currentTile = Map.main.gridMap[currentX, currentY + 1];
                    currentY = currentY + 1;
                    return true;
                }
            }
            return false;
        }
        bool FindFood()//move to a nearby Tile with food, return true if food found and moved
        {
            int besttile = 0;
            int currentbest = 0;
            int[] tilefoods = new int[4];//stores the amount of food in each Tile, so the best Tile can be selected
            // 0 = left, 1 = up, 2 = right, 3 = down
            tilefoods[0] = -1; tilefoods[1] = -1; tilefoods[2] = -1; tilefoods[3] = -1;
            //initialise all to -1 so an invalid Tile doesnt get chosen, as the min a valid Tile can be is 0
            if (dietName == "Herbivore")
            {
                if (currentX > 0)//check if on edge
                {
                    tilefoods[0] = Map.main.gridMap[currentX - 1, currentY].plants;
                }
                if (currentY < Globals.MapSize - 1)//check if on edge
                {
                    tilefoods[1] = Map.main.gridMap[currentX, currentY + 1].plants;
                }
                if (currentX < Globals.MapSize - 1)//check if on edge
                {
                    tilefoods[2] = Map.main.gridMap[currentX + 1, currentY].plants;

                }
                if (currentY > 0)//check if on edge
                {
                    tilefoods[3] = Map.main.gridMap[currentX, currentY - 1].plants;//store the amount of food in each Tile to be compared
                }

                for (int i = 0; i < tilefoods.Length; i++)//find best Tile
                {
                    if (tilefoods[i] > currentbest)
                    {
                        currentbest = tilefoods[i];//will find the best Tile and the value
                        besttile = i;
                    }
                }
                if (currentbest == 0)
                {
                    return false;//return false as no tiles have any food
                }
                if (besttile == 0)
                {
                    currentTile = Map.main.gridMap[currentX - 1, currentY];//update co-ords
                    currentX = currentX - 1;//set new currentX
                    return true;
                }
                if (besttile == 1)
                {
                    currentTile = Map.main.gridMap[currentX, currentY + 1];
                    currentY = currentY + 1; //set new currentY
                    return true;
                }
                if (besttile == 2)
                {
                    currentTile = Map.main.gridMap[currentX + 1, currentY];
                    currentX = currentX + 1;//set new currentX
                    return true;
                }
                else
                {
                    currentTile = Map.main.gridMap[currentX, currentY - 1];
                    currentY = currentY - 1; //set new currentY
                    return true;
                }
            }

            if (dietName == "Carnivore")
            {
                if (currentX > 0)//check if on edge
                {
                    tilefoods[0] = Map.main.gridMap[currentX - 1, currentY].meat;
                }
                if (currentY < Globals.MapSize - 1)//check if on edge
                {
                    tilefoods[1] = Map.main.gridMap[currentX, currentY + 1].meat;
                }
                if (currentX < Globals.MapSize - 1)//check if on edge
                {
                    tilefoods[2] = Map.main.gridMap[currentX + 1, currentY].meat;

                }
                if (currentY > 0)//check if on edge
                {
                    tilefoods[3] = Map.main.gridMap[currentX, currentY - 1].meat;//store the amount of food in each Tile to be compared
                }

                for (int i = 0; i < tilefoods.Length; i++)//find best Tile
                {
                    if (tilefoods[i] > currentbest)
                    {
                        currentbest = tilefoods[i];//will find the best Tile and the value
                        besttile = i;
                    }
                }
                if (currentbest == 0)
                {
                    return false;//return false as no tiles have any food
                }
                if (besttile == 0)
                {
                    currentTile = Map.main.gridMap[currentX - 1, currentY];//update co-ords
                    currentX = currentX - 1;//set new currentX
                    return true;
                }
                if (besttile == 1)
                {
                    currentTile = Map.main.gridMap[currentX, currentY + 1];
                    currentY = currentY + 1; //set new currentY
                    return true;
                }
                if (besttile == 2)
                {
                    currentTile = Map.main.gridMap[currentX + 1, currentY];
                    currentX = currentX + 1;//set new currentX
                    return true;
                }
                else
                {
                    currentTile = Map.main.gridMap[currentX, currentY - 1];
                    currentY = currentY - 1; //set new currentY
                    return true;
                }
            }

            else//for if they are omnivorous
            {
                if (currentX > 0)//check if on edge
                {
                    tilefoods[0] = Map.main.gridMap[currentX - 1, currentY].plants + Map.main.gridMap[currentX - 1, currentY].meat;
                }
                if (currentY < Globals.MapSize - 1)//check if on edge
                {
                    tilefoods[1] = Map.main.gridMap[currentX, currentY + 1].plants + Map.main.gridMap[currentX, currentY + 1].meat;
                }
                if (currentX < Globals.MapSize - 1)//check if on edge
                {
                    tilefoods[2] = Map.main.gridMap[currentX + 1, currentY].plants + Map.main.gridMap[currentX + 1, currentY].meat;

                }
                if (currentY > 0)//check if on edge
                {
                    tilefoods[3] = Map.main.gridMap[currentX, currentY - 1].plants + Map.main.gridMap[currentX, currentY - 1].meat;//store the amount of food in each Tile to be compared
                }

                for (int i = 0; i < tilefoods.Length; i++)//find best Tile
                {
                    if (tilefoods[i] > currentbest)
                    {
                        currentbest = tilefoods[i];//will find the best Tile and the value
                        besttile = i;
                    }
                }
                if (currentbest == 0)
                {
                    return false;//return false as no tiles have any food
                }
                if (besttile == 0)
                {
                    currentTile = Map.main.gridMap[currentX - 1, currentY];//update co-ords
                    currentX = currentX - 1;//set new currentX
                    return true;
                }
                if (besttile == 1)
                {
                    currentTile = Map.main.gridMap[currentX, currentY + 1];
                    currentY = currentY + 1; //set new currentY
                    return true;
                }
                if (besttile == 2)
                {
                    currentTile = Map.main.gridMap[currentX + 1, currentY];
                    currentX = currentX + 1;//set new currentX
                    return true;
                }
                else
                {
                    currentTile = Map.main.gridMap[currentX, currentY - 1];
                    currentY = currentY - 1; //set new currentY
                    return true;
                }
            }//for omnivores
        }

        bool HuntForFood()
        {
            if (huntedThisTurn)
            {
                return false;
            }
            tileAnimals = Manager.animalList.SearchList(currentX, currentY, this);//if a valid mea exists on Tile, eat it
            Animal victim = this;
            if (tileAnimals.Count != 0)
            {
                for(int i = 0; i < tileAnimals.Count; i++)
                {
                    if (tileAnimals[i].diet < victim.diet)//try to find a victim that isnt a carnivore, will prioritise veges
                    {
                        victim = tileAnimals[i];//Change to be size based later
                    }
                }
                if (victim == this)//if no victim found as the victim has remained as the Animal that called it
                {
                    return false;
                }
            }
            else //return false if no victims found
            {
                return false;
            }
            
            float sizeDiff = this.size - victim.size;
            float successChance = 0.15f + (sizeDiff * 0.5f);
            successChance = Math.Clamp(successChance, 0f, 0.3f);//max and min chances

            if (Globals.random.NextDouble() > successChance)
            {
                hunger -= Globals.BaseCost;
                return true; // hunt failed but action still used
            }

            if (dietName == "Omnivore")
            {
                hunger += (int) Math.Round(Globals.MeatVal * victim.size * meatEfficiency);//add the omnivore penalty
            }
            else
            {
                hunger += (int) Math.Round(Globals.MeatVal * victim.size);
            }
            
            Manager.animalList.RemoveNode(victim);//eat the victim
            huntedThisTurn = true;
            return true;
        }

        void MakeChild(Animal partner)//pass in a partner to make the child with
        {
            
            int randomNumber = Globals.random.Next(0, Globals.MutationChance); 
            float newDiet = (partner.diet + this.diet) / 2.0f;//generate a diet for the new item
            float newSize = (partner.size + this.size) / 2.0f;//generate a size for the new item
            int newSpeed = (int)Math.Round((partner.speed + this.speed) / 2.0);//generate a diet for the new item
            if (randomNumber == 0)
            {
                //mutate one variable up or down
                int trait = Globals.random.Next(0, 3); // 0 = diet, 1 = speed, 2 = size
                int direction = Globals.random.Next(0, 2) == 0 ? -1 : 1; // positive or negative

                if (trait == 0)
                {
                    
                    newDiet = Math.Clamp(newDiet + direction * Globals.MutationAmount, 0.0f, 1.0f);
                }
                else if (trait == 1)
                {
                    
                    newSpeed = Math.Clamp(newSpeed + direction, 1, Globals.MaxSpeed); //find new speed between 1 and max speed
                }
                else if (trait == 2)
                {
                    
                    newSize = Math.Clamp(newSize + direction * Globals.MutationAmount, Globals.MinSize, Globals.MaxSize);
                }

            }   
            
            Manager.animalList.AddNode(new Animal(Map.main.gridMap[currentX, currentY], newDiet, newSpeed, newSize));//initialise new Animal
            hunger = hunger - (int)Math.Round(Globals.BabyPenalty * newSize);
            partner.hunger = partner.hunger - (int)Math.Round(Globals.BabyPenalty * newSize);//subtract hunger from each Animal as a penalty
            //more punishing to birth a large child
            //initialise new Animal and add it to the position
            //change this for evo later
            //Console.WriteLine("BABY");

        }

        void SeekMateOrFood() //findmate -> eat food -> hunt food, -> findmate, -> findfood
        {
            hunger -= (int)Math.Round(Globals.BaseCost * this.size); // base cost

            tileAnimals = Manager.animalList.SearchList(currentX, currentY, this);
            Animal validMate;
            validMate = ValidateMates(tileAnimals.ToArray());
            if (validMate != null)
            {
                int babyCounter = 1;
                float extraBabyChance = 0.3f / size;
                while (Globals.random.NextDouble() < extraBabyChance && babyCounter < 5) //allow for multiple babies, more likely with smaller animals, capepd at 5
                    MakeChild(validMate);
                MakeChild(validMate);
                return;
            }

            if (EatFromTile()) return;

            if (dietName != "Herbivore" && HuntForFood())
            {
                return;
            }

            if (FindMate())
            {
                hunger -= (int)Math.Round(Globals.BaseCost * this.size);
                return;
            }

            if (FindFood())
            {
                hunger -= (int)Math.Round(Globals.BaseCost * this.size);
            }
        }

        void SearchOrStarve()
        {
            hunger -= (int) Math.Round(Globals.BaseCost * this.size); // base cost of existing

            if (EatFromTile()) return;

            if (dietName != "Herbivore" && HuntForFood())
            {
                //successfully eaten an animal
                return;
            }

            if (FindFood())
            {
                hunger -= (int) Math.Round(Globals.BaseCost * this.size); // extra cost for moving
            }
        }

        bool EatFromTile()
        {
            if (dietName == "Herbivore" && currentTile.plants > 0)
            {
                hunger += Globals.PlantVal;
                currentTile.plants--;
                return true;
            }
            if (dietName == "Carnivore" && currentTile.meat > 0)
            {
                hunger += Globals.OldMeatVal;
                currentTile.meat--;
                return true;
            }
            if (dietName == "Omnivore")
            {
                if (currentTile.meat > 0)
                {
                    hunger += (int) Math.Round(Globals.OldMeatVal * meatEfficiency);
                    currentTile.meat--;
                    return true;
                }
                if (currentTile.plants > 0)
                {
                    hunger += (int) Math.Round(Globals.PlantVal * plantEfficiency);
                    currentTile.plants--;
                    return true;
                }
            }
            return false;
        }

        Animal ValidateMates(Animal[] possibleMates)//a valid mate msut be within 1 abs of the traits
        {
            float currentMin = Globals.MateThreshold;
            Animal bestMate = null;
            if (possibleMates.Count() == 0)
            {
                return bestMate;
            }

            foreach (Animal possibleMate in possibleMates)
            {
                float speedDiff = Math.Abs(this.speed - possibleMate.speed) / (float)Globals.MaxSpeed;
                float dietDiff = Math.Abs(this.diet - possibleMate.diet);
                float sizeDiff = Math.Abs(this.size - possibleMate.size) / Globals.MaxSize;
                float totalDiff = (speedDiff + dietDiff + sizeDiff) / 3.0f;
                if (totalDiff <= currentMin)
                {
                    currentMin = totalDiff;
                    bestMate = possibleMate;
                }
            }

            return bestMate;

            }
    }
}
