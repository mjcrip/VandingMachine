using System;
using System.Collections.Generic;


/* This is a console application Following are the test cases 
 * Test Case 1 => Product: Candy, Input Coin: 0.25, 0.25, 0.25, 0.1, 0.1, 0.05, Dispensed Coins: 0.25, 0.1
 * Test Case 2 => Product: Chips, Input Coin: 0.25, 0.1, 0.1, 0.1, 0.25, Dispensed Coins: 0.1, 0.1, 0.1
 * 
 */



namespace VandingMachine
{
    public class VandingMaching
    {
        public static void Main()
        {
            var machine = new Machine();
        }

    }
    public class Machine
    {
        public Machine()
        {
            ReInstantiateMachine();
        }
        private List<Coin> InsertedCoins { get; set; }
        public Product SelectedProduct { get; set; }

        /// <summary>
        /// Display the message on screen
        /// </summary>
        public void Display()
        {
            if (SelectedProduct == null)
            {
                Console.WriteLine("Select Product \n");
                SelectProduct();
                Dispence();
            }
            else if (InsertedCoins.Count == 0)
            {
                Console.Write("Insert Coin \n");
                InsertCoin();
            }
            else if (InsertedCoins.Count > 0)
            {
                Console.WriteLine("Available Amount:" + InsertedCoins.Where(t => t.GetCoinType() != CoinType.Other).Sum(t => t.Value));
            }
        }

        /// <summary>
        /// Dispense the product and coins
        /// </summary>
        public void Dispence()
        {
            var remainingCoins = new List<Coin>();
            if (CanDispenseProduct(ref remainingCoins))
            {

                Console.WriteLine("\n\n\n\nThank You \n\n" +
                    (remainingCoins.Count > 0 ? "Coin dispensed with count " + string.Join(", ", remainingCoins.GroupBy(t => t.GetCoinType()).ToDictionary(y => y.Key, y => y.ToList().Count).Select(r => r.Key + ": " + r.Value).ToList()) : "No coin were dispense"));
                Console.WriteLine("Press key to check another test case");
                Console.ReadKey();
                ReInstantiateMachine();
            }
            else
            {
                Display();
                InsertCoin();
            }
        }

        /// <summary>
        /// Check the state to dispense coin and product
        /// </summary>
        /// <param name="coins"></param>
        /// <returns></returns>
        public bool CanDispenseProduct(ref List<Coin> coins)
        {
            coins = new List<Coin>();
            if (SelectedProduct != null)
            {
                var productValue = SelectedProduct.GetProductValue();
                InsertedCoins = InsertedCoins.OrderByDescending(t => t.Value).ToList();

                float calculatedValue = 0f;
                foreach (var item in InsertedCoins)
                {
                    if (Math.Round(calculatedValue, 2) + Math.Round(item.Value, 2) > Math.Round(productValue, 2))
                    {
                        coins.Add(item);
                    }
                    else
                    {
                        calculatedValue += item.Value;
                    }
                }
                if (Math.Round(calculatedValue, 2) == Math.Round(productValue, 2))
                    return true;
                else
                    return false;
            }
            else
            {
                ReInstantiateMachine();
            }
            return false;
        }

        /// <summary>
        /// Handle Insert coin
        /// </summary>
        /// <param name="coin"></param>
        public void InsertCoin(Coin coin)
        {
            InsertedCoins.Add(coin);
            Dispence();
        }

        /// <summary>
        /// Reinstantiate Machine
        /// </summary>
        private void ReInstantiateMachine()
        {
            Console.Clear();
            Console.WriteLine("A. In case of select product press \n1. Cola ($1) \n2. Chips ($0.5)\n3. Candy ($0.65)\n\n\n In case of Insert Coin press \n1. $0.05 \n2. $0.1\n3. $0.25 \n\n\n");
            InsertedCoins = new List<Coin>();
            SelectedProduct = null;
            Display();
        }

        #region Console Functions
        private void InsertCoin()
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            if (key.Key == ConsoleKey.D1)
                InsertCoin(new Coin(0.05f));
            if (key.Key == ConsoleKey.D2)
                InsertCoin(new Coin(0.1f));
            if (key.Key == ConsoleKey.D3)
                InsertCoin(new Coin(0.25f));
        }

        private void SelectProduct()
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.D1)
                SelectedProduct = new Product(ProductType.Cola);
            else if (key.Key == ConsoleKey.D2)
                SelectedProduct = new Product(ProductType.Chips);
            if (key.Key == ConsoleKey.D3)
                SelectedProduct = new Product(ProductType.Candy);
            Console.WriteLine();
        }
        #endregion
    }


    /// <summary>
    /// Coin Class
    /// </summary>
    public class Coin
    {
        public Coin(float value)
        {
            Value = value;
        }
        public float Value { get; private set; }

        public CoinType GetCoinType()
        {
            switch (Value)
            {
                case 0.05f:
                    return CoinType.Nickel;
                case 0.1f:
                    return CoinType.Dime;
                case 0.25f:
                    return CoinType.Quarter;
                default:
                    return CoinType.Other;
            }
        }
    }

    /// <summary>
    /// Product Class
    /// </summary>
    public class Product
    {
        public Product(ProductType productType)
        {
            ProductType = productType;
        }

        public ProductType ProductType { get; private set; }
        public float GetProductValue()
        {
            switch (ProductType)
            {
                case ProductType.Cola:
                    return 1f;
                case ProductType.Chips:
                    return 0.5f;
                case ProductType.Candy:
                    return 0.65f;
                default:
                    throw new Exception("Invalid Product!");
            }
        }
    }






    public enum ProductType
    {
        Cola,
        Chips,
        Candy
    }

    public enum CoinType
    {
        Nickel,
        Dime,
        Quarter,
        Other
    }
}


