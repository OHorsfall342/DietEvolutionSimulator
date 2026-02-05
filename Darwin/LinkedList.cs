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

}