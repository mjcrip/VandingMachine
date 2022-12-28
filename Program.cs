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
            //Test Cases:
            TestCase(ProductType.Cola, new float[] { 1, 0.05f, 0.1f, 0.25f, 0.25f, 0.25f, 0.1f, 0.1f, 0.1f });
            TestCase(ProductType.Candy, new float[] { 1, 0.05f, 0.1f, 0.25f, 0.25f, 0.25f, 0.1f, 0.1f, 0.1f, 0.25f, 0.25f, 0.25f });
            TestCase(ProductType.Chips, new float[] { 1, 0.05f, 0.1f, 0.25f, 0.25f, 0.25f, 0.1f, 0.1f, 0.1f, 0.25f, 0.25f, 0.25f });

            TestCase(ProductType.Chips, new float[] { 1, 0.05f, 0.1f, 0.25f, 0.25f, 0.25f, 0.1f, 0.1f, 0.1f });
            TestCase(ProductType.Cola, new float[] { 1, 0.05f, 0.1f, 0.25f, 0.25f, 0.25f, 0.1f, 0.1f, 0.1f, 0.25f, 0.25f, 0.25f });
            TestCase(ProductType.Candy, new float[] { 1, 0.05f, 0.1f, 0.25f, 0.25f, 0.25f, 0.1f, 0.1f, 0.1f, 0.25f, 0.25f, 0.25f });
            TestCase(ProductType.Candy, new float[] { 1 });
        }

        private static void TestCase(ProductType cola, float[] floats)
        {
            var machine = new Machine(cola);
            foreach (var item in floats)
            {
                machine.InsertCoin(new Coin(item));
                if (machine.IsSuccessfull)
                    return;
            }
            Console.WriteLine("Price: " + machine.SelectedProduct.Value);
            Console.Write("Insert Coin \n");
        }
    }
    public class Machine
    {
        public Machine(ProductType productType)
        {
            ReInstantiateMachine();
            SelectedProduct = new Product(productType);
        }
        public bool IsSuccessfull { get; private set; }
        private List<Coin> InsertedCoins { get; set; }
        public Product SelectedProduct { get; set; }

        /// <summary>
        /// Display the message on screen
        /// </summary>
        public void Display()
        {
            if (InsertedCoins.Count == 0)
            {
                Console.Write("Insert Coin \n");
            }
            else if (InsertedCoins.Count > 0)
            {
                double sum = 0;
                foreach (var item in InsertedCoins.Where(t => t.CoinType != CoinType.Other))
                {
                    sum += Math.Round(item.Value, 2);
                }
                Console.WriteLine(Math.Round(sum, 2));// + "    //(Available Amount)");
            }
        }

        /// <summary>
        /// Dispense the product and coins
        /// </summary>
        public void Dispense()
        {
            var remainingCoins = new List<Coin>();
            if (CanDispenseProduct(ref remainingCoins))
            {
                var coinDispensed = "Coin Dispensed: Other(" + remainingCoins.Where(t=>t.CoinType == CoinType.Other).Count() + "), "+
                    "Dime(" + remainingCoins.Where(t => t.CoinType == CoinType.Dime).Count() + "), "+
                    "Nickel(" + remainingCoins.Where(t => t.CoinType == CoinType.Nickel).Count() + "), "+
                    "Quarter(" + remainingCoins.Where(t => t.CoinType == CoinType.Quarter).Count() + "), ";

                Console.WriteLine("\n\nThank You   //"+ coinDispensed);
                IsSuccessfull = true;
            }
            else
            {
                IsSuccessfull = false;
                Display();
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
            var productValue = SelectedProduct.Value;
            InsertedCoins = InsertedCoins.OrderByDescending(t => t.Value).ToList();
            float calculatedValue = 0f;
            foreach (var item in InsertedCoins)
            {
                if (Math.Round(calculatedValue, 2) + Math.Round(item.Value, 2) > Math.Round(productValue, 2) || 
                    item.CoinType == CoinType.Other)
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
            return false;
        }

        /// <summary>
        /// Handle Insert coin
        /// </summary>
        /// <param name="coin"></param>
        public void InsertCoin(Coin coin)
        {
            InsertedCoins.Add(coin);
            Dispense();
        }

        /// <summary>
        /// Reinstantiate Machine
        /// </summary>
        private void ReInstantiateMachine()
        {
            InsertedCoins = new List<Coin>();
            SelectedProduct = null;
            Display();
        }
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
        public CoinType CoinType { get => GetCoinType(); }
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
        public float Value { get => GetProductValue(); }
        private float GetProductValue()
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


