using System;
using System.Collections.Generic;
using System.Text;

namespace Var_2
{
    class Product
    {
        public int Indef1;
        public int Indef2;
        public string ProductName;
        public int InventNum;
        public string GroupName;
        public double coast;

        public Product(int indef1, string ProductName, int InventNum, int indef2, string GroupName, double coast)
        {
            this.Indef1 = indef1;
            this.ProductName = ProductName;
            this.InventNum = InventNum;
            this.Indef2 = indef2;
            this.GroupName = GroupName;
            this.coast = coast;
        }

        public Product(int indef1, string ProductName)
        {
            this.Indef1 = indef1;
            this.ProductName = ProductName;
        }

        public Product(int InventNum, int indef2, string GroupName, double coast)
        {
            this.InventNum = InventNum;
            this.Indef2 = indef2;
            this.GroupName = GroupName;
            this.coast = coast;
        }
       
    }
}
