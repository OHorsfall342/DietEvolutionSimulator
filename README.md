# This is an ecosystem simulation.

A simple simulation made in C#, animals move around a grid (or ecosystem) and simulate mating, feeding, hunting etc for several thousand iterations (days)

At the end of the simulation, population trends are exhibited in a graph 


# How To Run

-Download the ZIP in releases

-Run darwin.exe

-Results should be published in PopLineGraph.png


# Example output

-This is an example of the population line graph

-![Population Graph](PopulationGraphExample.png)


# Features

-Animal traits (Carnivore, Herbivore, Omnivore)

-Simple reproduction, mutation and inheritance system

-Food creation and growth mechanics

-Graphing of population using ScottPlot

-Values such as Days, Meatvalues, MutationChance, and FoodScarcity can be edited to provide different results


# Project Structure

-Manager iterates through the days, manages the graph, initialises first animals

-Map is a grid of Tiles

  -Each Tile is updated at the start of each dat, if theres food, animals, etc.
  
  -Tiles contain info such as meat, plant food and position.
    
-Animals are initiated with Diet and Position. They store other information such as hunger and mate.

  -TakeAction is called for every Animal every turn, decides what they should do and if they are dead.
  
  -FindMate and FindFood both use pathfinding to find the closest relevant Animal/food.
  
  -HuntForFood allows eating of other animals, MakeChild breeds an offspring
  
-A handmade linked list is used to store every Animal and iterated through to action all of them.


# Next Steps

-Add more Animal traits, like being able to have multiple children, get hungry slower, be faster etc.

-Add environments in different parts of the Map.

-More detailed mutation mechanics

