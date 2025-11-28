using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.IO;
using System.Text;
using ScottPlot;
using System.Runtime.ExceptionServices;


//TODO:
//Make find animals return a list

//Make food expire after a few days




//Make children inherit information from parents
//Update animal constructor method
//Add more globals to allow for greater customisation

//Make searchmate only work for those with similar diet
//Add a mutation chance and movement penalty to globals
//Add more detail for the 1-3 diets as currently identical

//make system that chooses mate from list
//improve the find mate system to only find valid mates
//may want to make different values for fresh and old meat
//update graph system
//make carnivore food search system search for animals


//Suppose each day had 10 action points, s with a speed of 10 could move once a day, while somethign with a speed of 5 could move twice a day?

public static class Globals
{
    public const int totaldays = 6000;
    public const int actionsperday = 10; // amount of actions in a day, standard speed of 10 would compelte 1, speed of 5 would complete 2 etc
    public const int mapsize = 10;
    public const int plantval = 60;
    public const int meatval = 200;
    public const int mutationchance = 5;
    public const int foodchance = 7;//change these values to see different effects
}


//IF CHANGING MAP SIZE, NEED TO CHANGE ANIMAL EDGE CASES
//USE SPEED ATTRIBUTE TO DECIDE WHICH ANIMAL GOES FIRST IN TURN
public class Manager
{
    //public static animal[] animallist = new animal[100];//list to store animals in
    public static LinkedList animallist = new LinkedList();

    
    
    public static void Main()
    {
        List<int> animalCounts = new List<int>();//list to store the amount of animals at the end of each day
        List<int> vegCounts = new List<int>();
        List<int> omCounts = new List<int>();
        List<int> carnCounts = new List<int>();//count the number of each diet in a day


        int daycount = 0;//count the days, end sim after certain number of days
        bool alldead = false;
        map.main = new map();//initialise new map

        //animallist[0] = new animal(map.main.gridmap[2,2]);//initialise first animal and assign a tile

        animallist.AddNode(new animal(map.main.gridmap[2, 2], 0, 10));//initialise a new animal and add it to the linked lsit
        animallist.AddNode(new animal(map.main.gridmap[1, 2], 0, 10));
        animallist.AddNode(new animal(map.main.gridmap[2, 1], 0, 10));

        //Console.WriteLine(map.main.gridmap[4, 4].posx);//access the food item of a value

        while (daycount < Globals.totaldays)
        {
            if (daycount % 20 == 0)
            {
                Console.WriteLine(daycount);
            }
            map.main.updatetiles();//update all tiles
            for (int i = 0; i < Globals.actionsperday; i++)
            {
                animallist.ActionList();//make animals take action
            }

                
            daycount++;//increment the days
            //Console.WriteLine(animallist.CountList(0));
            animalCounts.Add(animallist.CountList(0));//add to list for graph
            vegCounts.Add(animallist.CountList(1));
            omCounts.Add(animallist.CountList(2));
            carnCounts.Add(animallist.CountList(3));
        }

        ScottPlot.Plot popgraph = new();

        // Create X and Y data arrays for plotting
        int[] days = new int[animalCounts.Count];
        int[] counts = animalCounts.ToArray();//Create two arrays to be plotted
        int[] vegcountsarray = vegCounts.ToArray();
        int[] omcountsarray = omCounts.ToArray();
        int[] carncountsarray = carnCounts.ToArray();

        for (int i = 0; i < days.Length; i++)
        {
            days[i] = i + 1;
            counts[i] = 0;//make graphs without total pop
        }

        popgraph.Add.Scatter(days, counts);//plot the graph
        popgraph.Add.Scatter(days, omcountsarray);
        popgraph.Add.Scatter(days, vegcountsarray);
        
        popgraph.Add.Scatter(days, carncountsarray);

        popgraph.SavePng("Poplinegraph.png", 25000, 5000);//save file with name and dimensions
    }

}

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

public class tile
{
    public int plants = 0;
    public int meat = 0;
    public int posx;
    public int posy;

    public tile(int _x, int _y)//constructor method 
    {
        posx = _x; 
        posy = _y;
    }

    public void updatethetile()//update the tile, randomly check if food is generated
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
    public int diet = 0;//0 = vege, 4 = meat eater

    public animal(tile _tile, int _diet, int _speed)//constructor method
    {
        speed = _speed;
        diet = _diet;
        currentTile = _tile;
        currentx = currentTile.posx;
        currenty = currentTile.posy;//set current positions to the tile
    }

    public bool takeaction()//call for the animal to take their turn, return false if dead
    {
        if (speed > lastaction)
            lastaction++;
            return;

        lastaction == 0; //set last action to 0 since its ongoing
        //If hunger > 90, check for mate on same tile, then check for food in tile, then check for mate in nearby tile, then check food nearby
        //Make separate behaviours for each diet

        //Console.WriteLine(currentx);
        //Console.WriteLine(currenty);
        //Console.WriteLine(hunger);
        //Console.WriteLine();
        if (diet == 0)
        {
            if (hunger > 120)
            {
                tileAnimals = Manager.animallist.SearchList(currentx, currenty, this);//find if theres a mate on this tile
                if (tileAnimals.Count != 0)//check to see if the list is empty
                {
                    mate = tileAnimals[0];//for nor make mate just first animal
                    makechild(mate);
                    return true;
                }

                else if (currentTile.plants > 0)
                {
                    //current tile has food
                    hunger = hunger + Globals.plantval;
                    currentTile.plants--;//set food to false
                    return true;
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

                    if (hunger <= 0)//check if animal is dead
                    {
                        currentTile.meat++;//increase the meat since animal is dead
                        return false;//returns false if dead
                    }
                    return true;
                }
            }

            //for if hunger is lower than 90
            if (currentTile.plants > 0)
            {
                //current tile has food
                hunger = hunger + Globals.plantval;
                currentTile.plants--;//set food to false
                return true;
            }
            else
            {
                if (findfood() == true)
                {
                    hunger = hunger - 10;//make hungrier due to moving
                }
                hunger = hunger - 10;//loses 10 if doesnt move, 20 if does
            }
            if (hunger <= 0)//check if animal is dead
            {
                //Console.WriteLine("BRO is ded");
                return false;//returns false if dead
            }
            return true;


            return true;
        }
        if (diet == 4)
        {
            if (hunger > 120)
            {
                tileAnimals = Manager.animallist.SearchList(currentx, currenty, this);//find if theres a mate on this tile
                if (tileAnimals.Count != 0)//check to see if the list is empty
                {
                    mate = tileAnimals[0];//for nor make mate just first animal
                    makechild(mate);
                    return true;
                }

                else if (currentTile.meat > 0)
                {
                    //current tile has food
                    hunger = hunger + Globals.meatval;
                    currentTile.meat--;//set food to false
                    return true;
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

                    if (hunger <= 0)//check if animal is dead
                    {
                        currentTile.meat++;//increase the meat since animal is dead
                        return false;//returns false if dead
                    }
                    return true;
                }
            }

            //for if hunger is lower than 90
            if (currentTile.meat > 0)
            {
                //current tile has food
                hunger = hunger + Globals.meatval;
                currentTile.meat--;//set food to false
                return true;
            }
            else
            {
                if (huntforfood() == true)//true means ate food on this tile
                {
                    hunger = hunger + (int)Math.Round(Globals.meatval * 1.5);//make hungrier due to moving
                }
                else if (findfood() == true)
                {
                    hunger = hunger - 10;//make hungrier due to moving
                }

                hunger = hunger - 10;//loses 10 if doesnt move, 20 if does
            }
            if (hunger <= 0)//check if animal is dead
            {
                //Console.WriteLine("BRO is ded");
                return false;//returns false if dead
            }
            return true;


        }

        else//for diets of 1, 2 and 3
        {
            if (hunger > 120)
            {
                tileAnimals = Manager.animallist.SearchList(currentx, currenty, this);//find if theres a mate on this tile
                if (tileAnimals.Count != 0)//check to see if the list is empty
                {
                    mate = tileAnimals[0];//for nor make mate just first animal
                    makechild(mate);
                    return true;
                }

                else if (currentTile.meat > 0)
                {
                    //current tile has food
                    hunger = hunger + Globals.meatval;
                    currentTile.meat--;//set food to false
                    return true;
                }
                else if (currentTile.plants > 0)
                {
                    //current tile has food
                    hunger = hunger + Globals.plantval;
                    currentTile.plants--;//set food to false
                    return true;
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

                    if (hunger <= 0)//check if animal is dead
                    {
                        currentTile.meat++;//increase the meat since animal is dead
                        return false;//returns false if dead
                    }
                    return true;
                }
            }

            //for if hunger is lower than 90
            if (currentTile.meat > 0)
            {
                //current tile has food
                hunger = hunger + Globals.meatval;
                currentTile.meat--;//set food to false
                return true;
            }
            else if (currentTile.plants > 0)
            {
                //current tile has food
                hunger = hunger + Globals.plantval;
                currentTile.plants--;//set food to false
                return true;
            }
            else
            {
                if (findfood() == true)
                {
                    hunger = hunger - 10;//make hungrier due to moving
                }
                hunger = hunger - 10;//loses 10 if doesnt move, 20 if does
            }
            if (hunger <= 0)//check if animal is dead
            {
                //Console.WriteLine("BRO is ded");
                return false;//returns false if dead
            }
            return true;

        }
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
        if (currentx < Globals.mapsize)//check if on edge
        {
            if (Manager.animallist.SearchList(currentx + 1, currenty, this).Count != 0)//find mate, move to that tile
            {
                currentTile = map.main.gridmap[currentx + 1, currenty];
                currentx = currentx + 1;//set new currentx
                return true;
            }
        }
        if (currenty < Globals.mapsize)//check if on edge
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
        if (diet == 0)
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

        if (diet == 4)
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
                tilefoods[2] = map.main.gridmap[currentx + 1, currenty].plants + tilefoods[2] + map.main.gridmap[currentx + 1, currenty].meat;

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
                    victim = tileAnimals[i];
                }
            }
            if (victim == this)//if no victim found return false
            {
                return false;
            }
        }
        //Console.WriteLine("NOM");
        Manager.animallist.RemoveNode(victim);//eat the victim
        return true;
    }
    void makechild(animal partner)//pass in a partner to make the child with
    {
        int newdiet = (int)Math.Round((partner.diet + this.diet) / 2.0);//generate a diet for the new item
        Random random = new Random();
        int randomNumber = random.Next(0, Globals.mutationchance); // Generates a random number between 0 to 20
        if (randomNumber == 1)
        {
            if (newdiet != 0)//if mutation occurs, check it wont go negative
            {
                newdiet--;
            }
        }
        if (randomNumber == 2)
        {
            if (newdiet != 4)//if mutation occurs, check it wont go over 4
            {
                newdiet++;
            }
        }
        int newspeed = (int)Math.Round((partner.speed + this.speed) / 2.0);//generate a diet for the new item
        int randomNumber = random.Next(0, Globals.mutationchance); // Generates a random number between 0 to 20
        if (randomNumber == 1)
        {
            if (newspeed > 1)//if mutation occurs, check it wont go negative
            {
                newspeed--;
            }
        }
        if (randomNumber == 2)
        {
            newspeed++;
        }

        Manager.animallist.AddNode(new animal(map.main.gridmap[currentx, currenty], newdiet, newspeed));//initialise new animal
        hunger = hunger - 40;
        partner.hunger = partner.hunger - 40;//subtract hunger from each animal as a penalty
        //initialise new animal and add it to the position
        //change this for evo later
        //Console.WriteLine("BABY");

    }
}


public class LinkedList
{
    private Node head;

    public LinkedList()//initialise the linked list
    {
        this.head = null;
    }

    // Add a node to the end of the list
    public void AddNode(animal data)//add a node to the list
    {
        Node newNode = new Node(data);
        if (head == null)
        {
            head = newNode;
        }
        else
        {
            Node current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = newNode;
        }
    }

    // Remove a node with the specified data
    public void RemoveNode(animal data)//remove a node
    {
        if (head == null)
        {
            Console.WriteLine("The list is empty.");
            return;
        }

        if (head.Data == data)
        {
            head = head.Next;
            return;
        }

        Node current = head;
        Node previous = null;

        while (current != null && current.Data != data)
        {
            previous = current;
            current = current.Next;
        }

        if (current == null)
        {
            Console.WriteLine("Node with data {0} not found.", data);
            return;
        }

        previous.Next = current.Next;//once data is removed, change pointer to next in list
    }

    // Display the list
    public void ActionList()//updates every item on the list
    {
        Node current = head;
        while (current != null)
        {
            if (current.Data.takeaction() == false)
            {

                RemoveNode(current.Data);//if return false, hunger = 0 so kill 
                //Console.WriteLine("bro is ded");
            }
            current = current.Next;
        }
        //Console.WriteLine("fin");
    }

    public List<animal> SearchList(int searchx, int searchy, animal thisanimal)//Need to make it so it can find multiple different animals
    {
        Node current = head;
        List<animal> tileanimals = new List<animal>();
        while (current != null)
        {
            if (current.Data.currentx == searchx)
            {
                if (current.Data.currenty == searchy)
                {
                    if (current.Data != thisanimal)//prevent the animals from finding themselves
                    {
                        tileanimals.Add(current.Data);//return the animl found in the square
                    }
                    
                }
            }
            current = current.Next;
        }
        return tileanimals;//if no animals found in square return null
        
    }
    public int CountList(int filter)//counts total number of animals alive
    {
        //filter 0 means all, 1 means veg, 2 means om, 3 means carn
        int counter = 0;
        Node current = head;
        while (current != null)
        {
            if (filter == 0)
            {
                counter++;
            }
            else if (filter == 1 && current.Data.diet == 0)//outputs number of vegetarians
            {
                counter++;
            }
            else if (filter == 2)//outputs number of omnivores
            {
                if (current.Data.diet > 0 && current.Data.diet < 4)
                {
                    counter++;
                }
            }
            else
            {
                if (current.Data.diet == 4)//outputs number of carnivores
                {
                    counter++;
                }
            }
            current = current.Next;
        }
        return counter;//return the total number of animals

    }
}

public class Node//code for ndoes in the linked lsit
{
    public animal Data { get; set; }
    public Node Next { get; set; }

    public Node(animal data)
    {
        this.Data = data;
        this.Next = null;
    }
}

