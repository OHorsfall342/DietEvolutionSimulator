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

        int lastAction = 0; //how many turns their last action was
        public int currentX;
        public int currentY;
        public string dietName;
        public float diet = 0.0f;//0 = vege, 1.0 = meat eater, check globals for threshholds, inbetween is omnivore

        public Animal(Tile _tile, float _diet, int _speed)//constructor method
        {
            speed = _speed;
            diet = _diet;
            currentTile = _tile;
            currentX = currentTile.posx;
            currentY = currentTile.posy;//set current positions to the Tile

            if (diet < Globals.herbivoreThreshold)
            {
                dietName = "Herbivore";
            }
            else if (diet > Globals.carnivoreThreshold)
            {
                dietName = "Carnivore";
            }
            else
            {
                dietName = "Omnivore";
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

            
            if (hunger > 120)
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
                if (Manager.animalList.SearchList(currentX - 1, currentY, this).Count != 0)//find mate, move to that Tile
                {
                    currentTile = Map.main.gridMap[currentX - 1, currentY];
                    currentX = currentX - 1;//set new currentX
                    return true;
                }
            }
            if (currentY > 0)//check if on edge
            {
                if (Manager.animalList.SearchList(currentX, currentY - 1, this).Count != 0)//find mate, move to that Tile
                {
                    currentTile = Map.main.gridMap[currentX, currentY - 1];
                    currentY = currentY - 1; //set new currentY
                    return true;
                }
            }
            if (currentX < Globals.mapsize - 1)//check if on edge
            {
                if (Manager.animalList.SearchList(currentX + 1, currentY, this).Count != 0)//find mate, move to that Tile
                {
                    currentTile = Map.main.gridMap[currentX + 1, currentY];
                    currentX = currentX + 1;//set new currentX
                    return true;
                }
            }
            if (currentY < Globals.mapsize - 1)//check if on edge
            {
                if (Manager.animalList.SearchList(currentX, currentY + 1, this).Count != 0)//find mate, move to that Tile
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
                if (currentY < Globals.mapsize - 1)//check if on edge
                {
                    tilefoods[1] = Map.main.gridMap[currentX, currentY + 1].plants;
                }
                if (currentX < Globals.mapsize - 1)//check if on edge
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
                if (currentY < Globals.mapsize - 1)//check if on edge
                {
                    tilefoods[1] = Map.main.gridMap[currentX, currentY + 1].meat;
                }
                if (currentX < Globals.mapsize - 1)//check if on edge
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
                if (currentY < Globals.mapsize - 1)//check if on edge
                {
                    tilefoods[1] = Map.main.gridMap[currentX, currentY + 1].plants + Map.main.gridMap[currentX, currentY + 1].meat;
                }
                if (currentX < Globals.mapsize - 1)//check if on edge
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
            //Console.WriteLine("NOM");
            Manager.animalList.RemoveNode(victim);//eat the victim
            return true;
        }

        void MakeChild(Animal partner)//pass in a partner to make the child with
        {
            
            Random random = new Random();
            int randomNumber = random.Next(0, Globals.mutationchance); 
            float newDiet = (partner.diet + this.diet) / 2.0f;//generate a diet for the new item
            int newSpeed = (int)Math.Round((partner.speed + this.speed) / 2.0);//generate a diet for the new item
            if (randomNumber == 0)
            {
                //mutate one variable up or down
                int trait = random.Next(0, 2); // 0 = diet, 1 = speed
                int direction = random.Next(0, 2) == 0 ? -1 : 1; // positive or negative

                if (trait == 0)
                {
                    
                    newDiet = Math.Clamp(newDiet + direction * Globals.mutationAmount, 0.0f, 1.0f);
                }
                else if (trait == 1)
                {
                    
                    newSpeed = Math.Clamp(newSpeed + direction, 1, Globals.maxSpeed); //find new speed between 1 and max speed
                }

            }   
            
            Manager.animalList.AddNode(new Animal(Map.main.gridMap[currentX, currentY], newDiet, newSpeed));//initialise new Animal
            hunger = hunger - 40;
            partner.hunger = partner.hunger - 40;//subtract hunger from each Animal as a penalty
            //initialise new Animal and add it to the position
            //change this for evo later
            //Console.WriteLine("BABY");

        }

        void SeekMateOrFood()
        {
            tileAnimals = Manager.animalList.SearchList(currentX, currentY, this);//find if theres a mate on this Tile
            if (tileAnimals.Count != 0)//check to see if the list is empty
            {
                
                mate = tileAnimals[0];//for nor make mate just first Animal
                MakeChild(mate);
            }

            else if (EatFromTile())
            {
            }

                    
            else
            {
                if (FindMate() == true)
                {
                    hunger = hunger - 10;//make hungrier due to moving
                }
                else if (FindFood() == true)
                {
                    hunger = hunger - 10;//make hungrier due to moving
                }
                hunger = hunger - 10;//loses 10 if doesnt move, 20 if does
            }
        }

        void SearchOrStarve()
        {
            if (EatFromTile())
                {
                }
                else
                {
                    if (FindFood() == true)
                    {
                        hunger = hunger - 10;//make hungrier due to moving
                    }
                    hunger = hunger - 10;//loses 10 if doesnt move, 20 if does
                }
        }

        bool EatFromTile()
        {
            if (dietName == "Herbivore" && currentTile.plants > 0)
            {
                hunger += Globals.plantval;
                currentTile.plants--;
                return true;
            }
            if (dietName == "Carnivore" && currentTile.meat > 0)
            {
                hunger += Globals.meatval;
                currentTile.meat--;
                return true;
            }
            if (dietName == "Omnivore")
            {
                if (currentTile.meat > 0)
                {
                    hunger += Globals.meatval;
                    currentTile.meat--;
                    return true;
                }
                if (currentTile.plants > 0)
                {
                    hunger += Globals.plantval;
                    currentTile.plants--;
                    return true;
                }
            }
            return false;
        }
    }
}
