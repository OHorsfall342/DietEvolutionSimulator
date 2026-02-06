namespace Darwin
{
    public class animal
    {
        tile currentTile;
        animal mate;
        List<animal> tileAnimals;
        int hunger = 100; //higher speed should decrease hunger by more
        //should speed also effect chance of being caught by predators? e.g. same speed gives 75% chance of caught
        //while a higher speed might give only 50%? i mean a hippo doesnt catch a emu veryoften
        public int speed = 10;

        int lastaction = 0; //how many turns their last action was
        public int currentx;
        public int currenty;
        public string dietName;
        public float diet = 0.0f;//0 = vege, 1.0 = meat eater, check globals for threshholds, inbetween is omnivore

        public animal(tile _tile, float _diet, int _speed)//constructor method
        {
            speed = _speed;
            diet = _diet;
            currentTile = _tile;
            currentx = currentTile.posx;
            currenty = currentTile.posy;//set current positions to the tile

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

        public bool takeaction()//call for the animal to take their turn, return false if dead
        {
            if (speed > lastaction)
            {
                lastaction++;
                return true;
            }

            lastaction = 0; //set last action to 0 since its ongoing

            
            if (hunger > 120)
            {
                SeekMateOrFood();
            }
            else{
                SearchOrStarve();
            }
            //for if hunger is lower than decided value
            
            if (hunger <= 0)//check if animal is dead
            {
                return false;//returns false if dead
            }
            return true;
        }
        
        
        bool findmate()//move to anearby tile with a mate
        {
            if (currentx > 0)//check if on edge
            {
                if (Manager.animallist.SearchList(currentx - 1, currenty, this).Count != 0)//find mate, move to that tile
                {
                    currentTile = map.main.gridmap[currentx - 1, currenty];
                    currentx = currentx - 1;//set new currentx
                    return true;
                }
            }
            if (currenty > 0)//check if on edge
            {
                if (Manager.animallist.SearchList(currentx, currenty - 1, this).Count != 0)//find mate, move to that tile
                {
                    currentTile = map.main.gridmap[currentx, currenty - 1];
                    currenty = currenty - 1; //set new currenty
                    return true;
                }
            }
            if (currentx < Globals.mapsize - 1)//check if on edge
            {
                if (Manager.animallist.SearchList(currentx + 1, currenty, this).Count != 0)//find mate, move to that tile
                {
                    currentTile = map.main.gridmap[currentx + 1, currenty];
                    currentx = currentx + 1;//set new currentx
                    return true;
                }
            }
            if (currenty < Globals.mapsize - 1)//check if on edge
            {
                if (Manager.animallist.SearchList(currentx, currenty + 1, this).Count != 0)//find mate, move to that tile
                {
                    currentTile = map.main.gridmap[currentx, currenty + 1];
                    currenty = currenty + 1;
                    return true;
                }
            }
            return false;
        }
        bool findfood()//move to a nearby tile with food, return true if food found and moved
        {
            int besttile = 0;
            int currentbest = 0;
            int[] tilefoods = new int[4];//stores the amount of food in each tile, so the best tile can be selected
            // 0 = left, 1 = up, 2 = right, 3 = down
            tilefoods[0] = -1; tilefoods[1] = -1; tilefoods[2] = -1; tilefoods[3] = -1;
            //initialise all to -1 so an invalid tile doesnt get chosen, as the min a valid tile can be is 0
            if (dietName == "Herbivore")
            {
                if (currentx > 0)//check if on edge
                {
                    tilefoods[0] = map.main.gridmap[currentx - 1, currenty].plants;
                }
                if (currenty < Globals.mapsize - 1)//check if on edge
                {
                    tilefoods[1] = map.main.gridmap[currentx, currenty + 1].plants;
                }
                if (currentx < Globals.mapsize - 1)//check if on edge
                {
                    tilefoods[2] = map.main.gridmap[currentx + 1, currenty].plants;

                }
                if (currenty > 0)//check if on edge
                {
                    tilefoods[3] = map.main.gridmap[currentx, currenty - 1].plants;//store the amount of food in each tile to be compared
                }

                for (int i = 0; i < tilefoods.Length; i++)//find best tile
                {
                    if (tilefoods[i] > currentbest)
                    {
                        currentbest = tilefoods[i];//will find the best tile and the value
                        besttile = i;
                    }
                }
                if (currentbest == 0)
                {
                    return false;//return false as no tiles have any food
                }
                if (besttile == 0)
                {
                    currentTile = map.main.gridmap[currentx - 1, currenty];//update co-ords
                    currentx = currentx - 1;//set new currentx
                    return true;
                }
                if (besttile == 1)
                {
                    currentTile = map.main.gridmap[currentx, currenty + 1];
                    currenty = currenty + 1; //set new currenty
                    return true;
                }
                if (besttile == 2)
                {
                    currentTile = map.main.gridmap[currentx + 1, currenty];
                    currentx = currentx + 1;//set new currentx
                    return true;
                }
                else
                {
                    currentTile = map.main.gridmap[currentx, currenty - 1];
                    currenty = currenty - 1; //set new currenty
                    return true;
                }
            }

            if (dietName == "Carnivore")
            {
                if (currentx > 0)//check if on edge
                {
                    tilefoods[0] = map.main.gridmap[currentx - 1, currenty].meat;
                }
                if (currenty < Globals.mapsize - 1)//check if on edge
                {
                    tilefoods[1] = map.main.gridmap[currentx, currenty + 1].meat;
                }
                if (currentx < Globals.mapsize - 1)//check if on edge
                {
                    tilefoods[2] = map.main.gridmap[currentx + 1, currenty].meat;

                }
                if (currenty > 0)//check if on edge
                {
                    tilefoods[3] = map.main.gridmap[currentx, currenty - 1].meat;//store the amount of food in each tile to be compared
                }

                for (int i = 0; i < tilefoods.Length; i++)//find best tile
                {
                    if (tilefoods[i] > currentbest)
                    {
                        currentbest = tilefoods[i];//will find the best tile and the value
                        besttile = i;
                    }
                }
                if (currentbest == 0)
                {
                    return false;//return false as no tiles have any food
                }
                if (besttile == 0)
                {
                    currentTile = map.main.gridmap[currentx - 1, currenty];//update co-ords
                    currentx = currentx - 1;//set new currentx
                    return true;
                }
                if (besttile == 1)
                {
                    currentTile = map.main.gridmap[currentx, currenty + 1];
                    currenty = currenty + 1; //set new currenty
                    return true;
                }
                if (besttile == 2)
                {
                    currentTile = map.main.gridmap[currentx + 1, currenty];
                    currentx = currentx + 1;//set new currentx
                    return true;
                }
                else
                {
                    currentTile = map.main.gridmap[currentx, currenty - 1];
                    currenty = currenty - 1; //set new currenty
                    return true;
                }
            }

            else//for if they are omnivorous
            {
                if (currentx > 0)//check if on edge
                {
                    tilefoods[0] = map.main.gridmap[currentx - 1, currenty].plants + map.main.gridmap[currentx - 1, currenty].meat;
                }
                if (currenty < Globals.mapsize - 1)//check if on edge
                {
                    tilefoods[1] = map.main.gridmap[currentx, currenty + 1].plants + map.main.gridmap[currentx, currenty + 1].meat;
                }
                if (currentx < Globals.mapsize - 1)//check if on edge
                {
                    tilefoods[2] = map.main.gridmap[currentx + 1, currenty].plants + map.main.gridmap[currentx + 1, currenty].meat;

                }
                if (currenty > 0)//check if on edge
                {
                    tilefoods[3] = map.main.gridmap[currentx, currenty - 1].plants + map.main.gridmap[currentx, currenty - 1].meat;//store the amount of food in each tile to be compared
                }

                for (int i = 0; i < tilefoods.Length; i++)//find best tile
                {
                    if (tilefoods[i] > currentbest)
                    {
                        currentbest = tilefoods[i];//will find the best tile and the value
                        besttile = i;
                    }
                }
                if (currentbest == 0)
                {
                    return false;//return false as no tiles have any food
                }
                if (besttile == 0)
                {
                    currentTile = map.main.gridmap[currentx - 1, currenty];//update co-ords
                    currentx = currentx - 1;//set new currentx
                    return true;
                }
                if (besttile == 1)
                {
                    currentTile = map.main.gridmap[currentx, currenty + 1];
                    currenty = currenty + 1; //set new currenty
                    return true;
                }
                if (besttile == 2)
                {
                    currentTile = map.main.gridmap[currentx + 1, currenty];
                    currentx = currentx + 1;//set new currentx
                    return true;
                }
                else
                {
                    currentTile = map.main.gridmap[currentx, currenty - 1];
                    currenty = currenty - 1; //set new currenty
                    return true;
                }
            }//for omnivores
        }

        bool huntforfood()
        {
            tileAnimals = Manager.animallist.SearchList(currentx, currenty, this);//if a valid mea exists on tile, eat it
            animal victim = this;
            if (tileAnimals.Count != 0)
            {
                for(int i = 0; i < tileAnimals.Count; i++)
                {
                    if (tileAnimals[i].diet < victim.diet)//try to find a victim that isnt a carnivore, will prioritise veges
                    {
                        victim = tileAnimals[i];//Change to be size based later
                    }
                }
                if (victim == this)//if no victim found as the victim has remained as the animal that called it
                {
                    return false;
                }
            }
            else //return false if no victims found
            {
                return false;
            }
            //Console.WriteLine("NOM");
            Manager.animallist.RemoveNode(victim);//eat the victim
            return true;
        }

        void makechild(animal partner)//pass in a partner to make the child with
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
            
            Manager.animallist.AddNode(new animal(map.main.gridmap[currentx, currenty], newDiet, newSpeed));//initialise new animal
            hunger = hunger - 40;
            partner.hunger = partner.hunger - 40;//subtract hunger from each animal as a penalty
            //initialise new animal and add it to the position
            //change this for evo later
            //Console.WriteLine("BABY");

        }

        void SeekMateOrFood()
        {
            tileAnimals = Manager.animallist.SearchList(currentx, currenty, this);//find if theres a mate on this tile
            if (tileAnimals.Count != 0)//check to see if the list is empty
            {
                
                mate = tileAnimals[0];//for nor make mate just first animal
                makechild(mate);
            }

            else if (EatFromTile())
            {
            }

                    
            else
            {
                if (findmate() == true)
                {
                    hunger = hunger - 10;//make hungrier due to moving
                }
                else if (findfood() == true)
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
                    if (findfood() == true)
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
