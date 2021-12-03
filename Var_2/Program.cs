using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Var_2
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Используя методы LINQ, напишите программу
            А) Для объединения внутреннего соединения между двумя наборами данных с группировкой по определенному признаку
            (признак группировки выбирает пользователь через меню).
            Б) Удаления заданной группы со всеми элементами из нового списка(создать отдельный метод).
                     Даны списки данных(тип данных – коллекция) в файле:
            1 - ый список содержит – идентификатор, название товара;
            2 - ой список содержит – инвентарный номер, идентификатор, название группы, цена товара*/

            List<Product> list1 = new List<Product>
            {
                new Product(1, "Масло слив.", 1, 1,"Масло", 800),
                new Product(2, "Масло олив.", 2, 2, "Масло", 900),
                new Product(3, "Масла", 3, 1, "Масло", 900),
                new Product(4, "Brade", 4, 4, "Масло", 700),
                new Product(5, "Brade", 5, 4, "Масло", 800),
                new Product(6, "Печенье", 6, 6, "Кулинария", 200),
                new Product(7, "Шоколад", 7, 7, "Кулинария", 300),
            };

            string FileName = "t.txt";
            StreamWriter sr = new StreamWriter(FileName);
            int Counter = 0;
            foreach(var item in list1)//пишем в файл
            {
                sr.WriteLine(item.Indef1+"_"+item.ProductName+"_"+item.InventNum+"_"+item.Indef2+"_"+item.GroupName+"_"+item.coast);
            }
            sr.Close();

            if (File.Exists(FileName))
            {
                StreamReader sr2 = new StreamReader(FileName);
                List<Product> list2 = new List<Product>();//1 - ый список содержит – идентификатор, название товара;
                List<Product> list3 = new List<Product>();//2-ой список содержит – инвентарный номер, идентификатор, название группы, цена товара
                try
                {
                    while (!sr2.EndOfStream)
                    {
                        string[] STR = sr2.ReadLine().Split('_');
                        list2.Add(new Product(Int32.Parse(STR[0]), STR[1]));
                    }
                }
                catch { Console.WriteLine("Были введены не корректные данные!"); }
                sr2.Close();

                StreamReader sr3 = new StreamReader(FileName);
                try
                {
                    while (!sr3.EndOfStream)
                    {
                        string[] STR = sr3.ReadLine().Split('_');
                        list3.Add(new Product(Int32.Parse(STR[2]), Int32.Parse(STR[3]), STR[4], Double.Parse(STR[5])));
                    }
                }
                catch { Console.WriteLine("Были введены не корректные данные!"); }
                sr3.Close();

                Console.WriteLine("--------------------------------------------------------");

                List<Product> list4 = new List<Product>();//список для объединения

                for(int i = 0; i<list2.Count(); i++)
                {
                    list4.Add(new Product(list2[i].Indef1, list2[i].ProductName, list3[i].InventNum, list3[i].Indef2, list3[i].GroupName, list3[i].coast));
                }

                foreach (var item in list4)
                {
                        Console.WriteLine(item.Indef1 + " " + item.ProductName + " " + item.GroupName + " " + item.coast);//выводим элементы
                }

                Console.WriteLine("Выберите элемент, по которому будем группировать. Название продукты, название группы или по цене.\nВвести 1, 2 или 3.");
                int ChoiceChecker;
                if (Int32.TryParse(Console.ReadLine(), out ChoiceChecker))
                {
                    if (ChoiceChecker == 1)//по названию продукта
                    {
                        var GroupList = list4.GroupBy(x => x.ProductName).Select(s => s.Key);//считываем все имена групп

                        foreach (var item in GroupList)
                        {
                            foreach (var item2 in list4)
                            {
                                if (item2.ProductName == item)
                                {
                                    Counter++;
                                }
                            }

                            Console.WriteLine(item + " " + Counter + " ед.");
                            Counter = 0;

                            foreach (var item2 in list4)
                            {
                                if (item2.ProductName == item)
                                {
                                    Console.WriteLine(item2.Indef1 + " " + item2.ProductName + " " + item2.coast);//выводим элементы
                                }
                            }
                        }

                        string GroupName;
                        do
                        {
                            Console.WriteLine("Какую группу хотите удалить?");
                            GroupName = Console.ReadLine();

                            if (!GroupList.Contains(GroupName))
                            {
                                Console.WriteLine("Вы ввели несуществующую группу! Возможно, неправильно написали название \n(Например: 'Масло' -> 'масло' неправильный ввод)");
                            }

                        } while (!GroupList.Contains(GroupName));


                        var list5 = RemoveElementPrName(list4, GroupName);//удаляем элемент

                        Console.WriteLine("--------------------------------------------------------\nНовый список выглядит таким образом:");

                        GroupList = list5.GroupBy(x => x.ProductName).Select(s => s.Key);//считываем все имена групп

                        if (File.Exists(FileName))
                        {
                            StreamWriter sr4 = new StreamWriter(FileName);
                            foreach (var item in GroupList)
                            {
                                foreach (var item2 in list5)
                                {
                                    if (item2.ProductName == item)
                                    {
                                        Counter++;
                                    }
                                }

                                Console.WriteLine(item + " " + Counter + " ед.");
                                sr4.WriteLine(item + " " + Counter + " ед.");

                                Counter = 0;

                                foreach (var item2 in list5)
                                {
                                    if (item2.ProductName == item)
                                    {
                                        Console.WriteLine(item2.Indef1 + " " + item2.ProductName + " " + item2.coast);//выводим элементы
                                        sr4.WriteLine(item2.Indef1 + " " + item2.ProductName + " " + item2.coast);
                                    }
                                }
                            }
                            Console.WriteLine("Результат был записан в исходный файл - " + "'" + FileName + "'.");
                            sr4.Close();
                        }
                    }
                    else if (ChoiceChecker == 2)//по названию группы
                    {
                        var GroupList = list4.GroupBy(x => x.GroupName).Select(s => s.Key);//считываем все имена групп

                        foreach (var item in GroupList)
                        {
                            foreach (var item2 in list4)
                            {
                                if (item2.GroupName == item)
                                {
                                    Counter++;
                                }
                            }

                            Console.WriteLine(item + " " + Counter + " ед.");
                            Counter = 0;

                            foreach (var item2 in list4)
                            {
                                if (item2.GroupName == item)
                                {
                                    Console.WriteLine(item2.Indef1 + " " + item2.ProductName + " " + item2.coast);//выводим элементы
                                }
                            }
                        }

                        string GroupName;
                        do
                        {
                            Console.WriteLine("Какую группу хотите удалить?");
                            GroupName = Console.ReadLine();

                            if (!GroupList.Contains(GroupName))
                            {
                                Console.WriteLine("Вы ввели несуществующую группу! Возможно, неправильно написали название \n(Например: 'Масло' -> 'масло' неправильный ввод)");
                            }

                        } while (!GroupList.Contains(GroupName));


                        var list5 = RemoveElementPrName(list4, GroupName);//удаляем элемент

                        Console.WriteLine("--------------------------------------------------------\nНовый список выглядит таким образом:");

                        GroupList = list5.GroupBy(x => x.GroupName).Select(s => s.Key);//считываем все имена групп

                        if (File.Exists(FileName))
                        {
                            StreamWriter sr4 = new StreamWriter(FileName);
                            foreach (var item in GroupList)
                            {
                                foreach (var item2 in list5)
                                {
                                    if (item2.ProductName == item)
                                    {
                                        Counter++;
                                    }
                                }

                                Console.WriteLine(item + " " + Counter + " ед.");
                                sr4.WriteLine(item + " " + Counter + " ед.");

                                Counter = 0;

                                foreach (var item2 in list5)
                                {
                                    if (item2.GroupName == item)
                                    {
                                        Console.WriteLine(item2.Indef1 + " " + item2.ProductName + " " + item2.coast);//выводим элементы
                                        sr4.WriteLine(item2.Indef1 + " " + item2.ProductName + " " + item2.coast);
                                    }
                                }
                            }
                            Console.WriteLine("Результат был записан в исходный файл - " + "'" + FileName + "'.");
                            sr4.Close();
                        }
                    }
                    else if (ChoiceChecker == 3)//по цене
                    {
                        var GroupList = list4.GroupBy(x => x.coast).Select(s => s.Key);//считываем все имена групп
                        foreach (var item in GroupList)
                        {
                            foreach (var item2 in list4)
                            {
                                if (item2.coast == item)
                                {
                                    Counter++;
                                }
                            }

                            Console.WriteLine(item + " " + Counter + " ед.");
                            Counter = 0;

                            foreach (var item2 in list4)
                            {
                                if (item2.coast == item)
                                {
                                    Console.WriteLine(item2.Indef1 + " " + item2.ProductName + " " + item2.coast);//выводим элементы
                                }
                            }
                        }

                        double GroupName;
                        do
                        {
                            Console.WriteLine("Какую группу хотите удалить?");
                            if (Double.TryParse(Console.ReadLine(), out GroupName))
                            {
                                if (!GroupList.Contains(GroupName))
                                {
                                    Console.WriteLine("Вы ввели несуществующую группу! Возможно, неправильно написали название \n(Например: 'Масло' -> 'масло' неправильный ввод)");
                                }
                            }
                            else Console.WriteLine("Вы ввели цену в неверном формате!");

                        } while (!GroupList.Contains(GroupName));


                        var list5 = RemoveElement(list4, GroupName);//удаляем элемент

                        Console.WriteLine("--------------------------------------------------------\nНовый список выглядит таким образом:");

                        GroupList = list5.GroupBy(x => x.coast).Select(s => s.Key);//считываем все имена групп

                        if (File.Exists(FileName))
                        {
                            StreamWriter sr4 = new StreamWriter(FileName);
                            foreach (var item in GroupList)
                            {
                                foreach (var item2 in list5)
                                {
                                    if (item2.coast == item)
                                    {
                                        Counter++;
                                    }
                                }

                                Console.WriteLine(item + " " + Counter + " ед.");
                                sr4.WriteLine(item + " " + Counter + " ед.");

                                Counter = 0;

                                foreach (var item2 in list5)
                                {
                                    if (item2.coast == item)
                                    {
                                        Console.WriteLine(item2.Indef1 + " " + item2.ProductName + " " + item2.coast);//выводим элементы
                                        sr4.WriteLine(item2.Indef1 + " " + item2.ProductName + " " + item2.coast);
                                    }
                                }
                            }
                            Console.WriteLine("Результат был записан в исходный файл - " + "'" + FileName + "'.");
                            sr4.Close();
                        }
                    }
                    else Console.WriteLine("Неверно! Вы должны были ввести 1, 2 или 3!");
                }
                else Console.WriteLine("Неверно! Вы должны были ввести 1, 2 или 3!");
            }
            else Console.WriteLine("Файл не найден!");
            Console.ReadKey();
        }

        public static List<Product> RemoveElement(List<Product> list1,string GroupName)//функция удаления по назв. группы
        {
            list1.RemoveAll(x => x.GroupName == GroupName);
            return list1;
        }

        public static List<Product> RemoveElement(List<Product> list1, double coast)//функция удаления по имени продукта
        {
            list1.RemoveAll(x => x.coast == coast);
            return list1;
        }

        public static List<Product> RemoveElementPrName(List<Product> list1, string ProductName)//функция удаления по цене
        {
            list1.RemoveAll(x => x.ProductName == ProductName);
            return list1;
        }
    }
}
