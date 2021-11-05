using System;
using System.Collections.Generic;
using System.Linq;


//-----------------------------------------------------------------------------------
//      SOURCE: https://exceptionnotfound.net/command-pattern-in-csharp/
//------------------------------------------------------------------------------------

namespace CommandPattern
{
     public class Program
     {

        ///////////////////////////////////////////////////////////////
        /// Represents an item being ordered from this restaurant. ////
        ///////////////////////////////////////////////////////////////
        public class MenuItem
        {
            public string Name { get; set; }
            public int Amount { get; set; }
            public double Price { get; set; }

            public MenuItem(string name, int amount, double price)
            {
                Name = name;
                Amount = amount;
                Price = price;
            }

            public void Display()
            {
                Console.WriteLine("\nName: " + Name);
                Console.WriteLine("Amount: " + Amount.ToString());
                Console.WriteLine("Price: $" + Price.ToString());
            }
        }



        //////////////////////////////////
        /// The Invoker class ////////////
        //////////////////////////////////
        public class Patron
        {
            private OrderCommand _orderCommand;
            private MenuItem _menuItem;
            private FastFoodOrder _order;

            public Patron()
            {
                _order = new FastFoodOrder();
            }

            public void SetCommand(int commandOption)
            {
                _orderCommand = new CommandFactory().GetCommand(commandOption);
            }

            public void SetMenuItem(MenuItem item)
            {
                _menuItem = item;
            }

            public void ExecuteCommand()
            {
                _order.ExecuteCommand(_orderCommand, _menuItem);
            }

            public void ShowCurrentOrder()
            {
                _order.ShowCurrentItems();
            }
        }

        public class CommandFactory
        {
            //Factory method
            public OrderCommand GetCommand(int commandOption)
            {
                switch (commandOption)
                {
                    case 1:
                        return new AddCommand();
                    case 2:
                        return new ModifyCommand();
                    case 3:
                        return new RemoveCommand();
                    default:
                        return new AddCommand();
                }
            }
        }


        ///////////////////////
        /// The Receiver //////
        ///////////////////////
        public class FastFoodOrder
        {
            public List<MenuItem> currentItems { get; set; }
            public FastFoodOrder()
            {
                currentItems = new List<MenuItem>();
            }

            public void ExecuteCommand(OrderCommand command, MenuItem item)
            {
                command.Execute(this.currentItems, item);
            }

            public void ShowCurrentItems()
            {
                foreach (var item in currentItems)
                {
                    item.Display();
                }
                Console.WriteLine("-----------------------");
            }
        }



        ////////////////////////////////////
        /// The Command abstract class /////
        ////////////////////////////////////
        public abstract class OrderCommand
        {
            public abstract void Execute(List<MenuItem> order, MenuItem newItem);
        }



        ///////////////////////////
        /// A concrete command ////
        ///////////////////////////
        public class AddCommand : OrderCommand
        {
            public override void Execute(List<MenuItem> currentItems, MenuItem newItem)
            {
                currentItems.Add(newItem);
            }
        }

        ///////////////////////////////
        /// A concrete command ////////
        ///////////////////////////////
        public class RemoveCommand : OrderCommand
        {
            public override void Execute(List<MenuItem> currentItems, MenuItem newItem)
            {
                currentItems.Remove(currentItems.Where(x => x.Name == newItem.Name).First());
            }
        }

        //////////////////////////
        /// A concrete command ///
        //////////////////////////
        public class ModifyCommand : OrderCommand
        {
            public override void Execute(List<MenuItem> currentItems, MenuItem newItem)
            {
                var item = currentItems.Where(x => x.Name == newItem.Name).First();
                item.Price = newItem.Price;
                item.Amount = newItem.Amount;
            }
        }


        static void Main(string[] args)
        {
            Patron patron = new Patron();
            patron.SetCommand(1 /*Add*/);
            patron.SetMenuItem(new MenuItem("Milk Tea", 2, 1.99));
            patron.ExecuteCommand();

            patron.SetCommand(1 /*Add*/);
            patron.SetMenuItem(new MenuItem("BlueSky", 2, 2.59));
            patron.ExecuteCommand();

            patron.SetCommand(1 /*Add*/);
            patron.SetMenuItem(new MenuItem("Pepsi", 2, 1.19));
            patron.ExecuteCommand();

            patron.ShowCurrentOrder();

            //Remove the french fries
            patron.SetCommand(3 /*Remove*/);
            patron.SetMenuItem(new MenuItem("Milk Tea", 2, 1.99));
            patron.ExecuteCommand();

            patron.ShowCurrentOrder();

            //Now we want 4 hamburgers rather than 2
            patron.SetCommand(2 /*Edit*/);
            patron.SetMenuItem(new MenuItem("Pepsi", 4, 2.59));
            patron.ExecuteCommand();

            patron.ShowCurrentOrder();

            Console.ReadKey();
        }



    }
}

