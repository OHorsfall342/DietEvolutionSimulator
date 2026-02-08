namespace Darwin
{
    public class LinkedList
    {
        private Node head;

        public LinkedList()//initialise the linked list
        {
            this.head = null;
        }

        // Add a node to the end of the list
        public void AddNode(Animal data)//add a node to the list
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
        public void RemoveNode(Animal data)//remove a node
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
                if (current.Data.TakeAction() == false)
                {
                    Map.main.gridMap[current.Data.currentX, current.Data.currentY].meat += 1;//add meat on death, could add size scaling later
                    RemoveNode(current.Data);//if return false, hunger = 0 so kill 
                    //Console.WriteLine("bro is ded");
                }
                current = current.Next;
            }
            //Console.WriteLine("fin");
        }

        //if no other animals can be found, return the origina an imal
        public List<Animal> SearchList(int searchx, int searchy, Animal thisanimal)//Need to make it so it can find multiple different animals
        {
            Node current = head;
            List<Animal> tileanimals = new List<Animal>();
            while (current != null)
            {
                if (current.Data.currentX == searchx)
                {
                    if (current.Data.currentY == searchy)
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
        public int CountList(string filterDiet)//counts total number of animals alive
        {
            //filters based on diet, e.g. carnivore or All
            int counter = 0;
            Node current = head;
            while (current != null)
            {
                if (filterDiet == "All")
                {
                    counter++;
                }
                else if (filterDiet == current.Data.dietName)//outputs number of vegetarians, omnivores or carnivores
                {
                    counter++;
                }
                
                current = current.Next;
            }
            return counter;//return the total number of animals

        }

        public float AvgSpeed(string filterDiet)//counts average speed of diet.
        {
            //filters based on diet, e.g. carnivore or All
            int counter = 0;
            float sum = 0;
            Node current = head;
            while (current != null)
            {
                if (filterDiet == "All")
                {
                    counter++;
                    sum += current.Data.speed;
                }
                else if (filterDiet == current.Data.dietName)
                {
                    counter++;
                    sum += current.Data.speed;
                }
                
                current = current.Next;
            }

            if (counter == 0)//prevent div by 0 error
            {
                return 0;
            }
            return sum / counter;//return the average sopeed

        }
    

        public float AvgSize(string filterDiet)//counts average speed of diet.
        {
            //filters based on diet, e.g. carnivore or All
            int counter = 0;
            float sum = 0;
            Node current = head;
            while (current != null)
            {
                if (filterDiet == "All")
                {
                    counter++;
                    sum += current.Data.size;
                }
                else if (filterDiet == current.Data.dietName)
                {
                    counter++;
                    sum += current.Data.size;
                }
                
                current = current.Next;
            }

            if (counter == 0)//prevent div by 0 error
            {
                return 0;
            }
            return sum / counter;//return the average sopeed

        }
    }
    

    public class Node//code for ndoes in the linked lsit
    {
        public Animal Data { get; set; }
        public Node Next { get; set; }

        public Node(Animal data)
        {
            this.Data = data;
            this.Next = null;
        }
    }

}