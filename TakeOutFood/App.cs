namespace TakeOutFood
{
    using System;
    using System.Collections.Generic;

    public class App
    {
        private IItemRepository itemRepository;
        private ISalesPromotionRepository salesPromotionRepository;

        public App(IItemRepository itemRepository, ISalesPromotionRepository salesPromotionRepository)
        {
            this.itemRepository = itemRepository;
            this.salesPromotionRepository = salesPromotionRepository;
        }

        public string BestCharge(List<string> inputs)
        {
            //TODO: write code here
            List<Item> items = itemRepository.FindAll();
            List<SalesPromotion> salesItemsList = salesPromotionRepository.FindAll();
            List<string> salesItems = salesItemsList[0].RelatedItems;
            string firstPartResult = "============= Order details =============\n";
            string secondPartResult = "-----------------------------------\n" + "Promotion used:\n"
                + "Half price for certain dishes (";
            double totalPrice = 0;
            double totalSaving = 0;
            int saleCount = 0;

            foreach(string s in inputs)
            {
                string[] stringArray = s.Split(" x ");
                string barcode = stringArray[0];
                int number = Convert.ToInt32(stringArray[1]);
                bool isSale = false;
                
                for(int i = 0; i < salesItems.Count; i++)
                {
                    if (barcode == salesItems[i])
                    {
                        isSale = true;
                        saleCount += 1;
                    }
                }
                double price = 0;
                string name = "";
                for (int j = 0; j < items.Count; j++)
                {
                    if (barcode == items[j].Id)
                    {
                        price = items[j].Price;
                        name = items[j].Name;
                    }
                }
                if (isSale)
                {
                    totalPrice += (number * price) / 2;
                    totalSaving += (number * price) / 2;
                    if (saleCount == 1)
                    {
                        secondPartResult += name;
                    }
                    else
                    {
                        secondPartResult += ", " + name;
                    }
                }

                else
                {
                    totalPrice += number * price;
                }
                firstPartResult += name + " x " + number + " = " + number * price + " yuan\n";
            }

            secondPartResult += "), saving " + totalSaving + " yuan\n" ;
            string thirdPartResult =  "-----------------------------------\n" + "Total：" + totalPrice + " yuan\n" + "===================================";

            if (saleCount > 0)
            {
                return firstPartResult + secondPartResult + thirdPartResult;
            }
            else
            {
                return firstPartResult + thirdPartResult;
            }
        }
    }
}
