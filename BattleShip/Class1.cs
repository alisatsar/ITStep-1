using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassField
{
    public enum Letter
    {
        a,
        b,
        c,
        d,
        e,
        f,
        g,
        h,
        i,
        j
    };

    public class Field
    {
        private const int sizeField = 10;
        private StringBuilder fieldPaint = new StringBuilder();
        public ClassShip[] arrayShip = new ClassShip[10];
        private bool[,] fieldShip = new bool[sizeField, sizeField];
        private bool[,] fieldBoundary = new bool[sizeField, sizeField];
        private bool[,] shotEmpty = new bool[sizeField, sizeField];
        private bool[,] shotGot = new bool[sizeField, sizeField];
        private int countX1;
        private int countX2;
        private int countX3;
        private int countX4;
        int index = 0;

        public Field()
        {
            countX1 = 0;
            countX2 = 0;
            countX3 = 0;
            countX4 = 0;
            //сделали поле содержащие информации о есть корабль или нет корабля по заданной координате
            for (int i = 0; i < sizeField; i++)
            {
                for (int j = 0; j < sizeField; j++)
                {
                    fieldShip[i, j] = false;
                }
            }
            //поле, которое содержит в себе информацию о граница корабля, в которые нельзя поставить другие корабли
            for (int i = 0; i < sizeField; i++)
            {
                for (int j = 0; j < sizeField; j++)
                {
                    fieldBoundary[i, j] = false;
                }
            }
            //поле, которое будет содержать пустые удары
            for (int i = 0; i < sizeField; i++)
            {
                for (int j = 0; j < sizeField; j++)
                {
                    shotEmpty[i, j] = false;
                }
            }
            //поле, которое будет содержать попавшие удары
            for (int i = 0; i < sizeField; i++)
            {
                for (int j = 0; j < sizeField; j++)
                {
                    shotGot[i, j] = false;
                }
            }
        }

        public void AddNewShip()
        {
            string xYCoordinates;
            int x = 0;
            Letter y = 0;
            char[] xYAr;
            string sizeShipStr;
            int sizeShip = 0;
            string locationStr;
            int location;

            Console.WriteLine("Your field is 10x10.");
            //спрашиваем координаты для начала прорисовки корабля
            do
            {
                do
                {
                    Console.WriteLine("Enter the coordinates (for example: 2a(x = 1; y = A) or 5b) which you want to start drawing the ship:");
                    xYCoordinates = Console.ReadLine();
                } while (xYCoordinates.Length != 2);

                xYAr = xYCoordinates.ToCharArray();
                x = int.Parse(xYAr[0].ToString());
                y = (Letter)Enum.Parse(typeof(Letter), xYAr[1].ToString());
            } while (fieldShip[x, (int)y] || fieldBoundary[x, (int)y]);

            if (AskExit() == ConsoleKey.Escape)
            {
                return;
            }

            do
            {
                Console.WriteLine("Enter the size of ship (1 - 4):");
                sizeShipStr = Console.ReadLine();
                sizeShip = int.Parse(sizeShipStr);

            } while (!(sizeShip > 0 && sizeShip < 5) || !(CheckCoordinateAndSize(x, (int)y, sizeShip)));

            if (AskExit() == ConsoleKey.Escape)
            {
                return;
            }

            do
            {
                do
                {
                    Console.WriteLine("How must be located the ship: horizontally(1) or vertically(0)?");
                    locationStr = Console.ReadLine();
                    location = int.Parse(locationStr);
                } while (!(location > -1 && location < 2));

            } while (!CheckCoordinate(x, (int)y, sizeShip, location));

            arrayShip[index] = new ClassShip(x, (int)y, sizeShip, location == 0 ? false : true);
            AddShipToFieldShip(arrayShip[index]);
            AddToFieldBoundary(arrayShip[index++], fieldBoundary);
        }

        public void Paint()
        {
            ClearStringBuider();
            Letter letter = 0;
            for (int y = 0; y < sizeField + 3; y++)
            {
                for (int x = 0; x < sizeField + 3; x++)
                {
                    if (x == 2 && y == 0)
                    {
                        for (int i = 0; i < sizeField; i++)
                        {
                            fieldPaint.Append(i);
                        }
                    }
                    else if (x == 0 && y > 1 && y < sizeField + 2)
                    {
                        fieldPaint.Append(letter++.ToString());
                    }
                    else if (x == 1 && y == 1)
                    {
                        fieldPaint.Append("╔");
                    }
                    else if (y == 1 && x == sizeField + 2)
                    {
                        fieldPaint.Append("╗");
                    }
                    else if ((x == 1 || x == sizeField + 2) && (y > 1 && y < sizeField + 2))
                    {
                        fieldPaint.Append("║");
                    }
                    else if ((y == 1 || y == sizeField + 2) && (x > 1 && x < sizeField + 2))
                    {
                        fieldPaint.Append("═");
                    }
                    else if (x == 1 && y == sizeField + 2)
                    {
                        fieldPaint.Append("╚");
                    }
                    else if (x == sizeField + 2 && y == sizeField + 2)
                    {
                        fieldPaint.Append("╝");
                    }
                    else if (x > 1 && y > 1 && shotEmpty[x - 2, y - 2])
                    {
                        fieldPaint.Append(".");
                    }
                    else if (x > 1 && y > 1 && shotGot[x - 2, y - 2])
                    {
                        fieldPaint.Append("X");
                    }
                    else if (x > 1 && y > 1 && fieldShip[x - 2, y - 2])
                    {
                        fieldPaint.Append("O");
                    }
                    else
                    {
                        fieldPaint.Append(" ");
                    }
                }
                fieldPaint.Append("\n");
            }
            Console.WriteLine(fieldPaint);
        }

        public void AddToFieldBoundary(ClassShip ship, bool[,] array)
        {
            int Xship = ship.GetX();
            int Yship = ship.GetY();
            bool location = ship.GetFlagHor();

            int buf;

            if (!ship.GetFlagHor())
            {
                buf = Xship;
                Xship = Yship;
                Yship = buf;
            }

            if (CheckSize(Xship, ship.GetSize()))
            {
                if (Yship == 0 || Yship < sizeField - 1)
                {
                    for (int i = Xship; i < Xship + ship.GetSize(); i++)
                    {
                        if (location)
                        {
                            array[i, Yship + 1] = true;
                        }
                        else
                        {
                            array[Yship + 1, i] = true;
                        }
                    }
                }
                if (Yship == sizeField - 1 || Yship > 0)
                {
                    for (int i = Xship; i < Xship + ship.GetSize(); i++)
                    {
                        if (location)
                        {
                            array[i, Yship - 1] = true;
                        }
                        else
                        {
                            array[Yship - 1, i] = true;
                        }
                    }
                }

                if (Xship >= 0 && Xship + ship.GetSize() != sizeField)
                {
                    if (location)
                    {
                        array[Xship + ship.GetSize(), Yship] = true;
                        if (Yship > 0)
                        {
                            array[Xship + ship.GetSize(), Yship - 1] = true;
                        }
                        if (Yship < sizeField - 1)
                        {
                            array[Xship + ship.GetSize(), Yship + 1] = true;
                        }
                    }
                    else
                    {
                        array[Yship, Xship + ship.GetSize()] = true;
                        if (Yship > 0)
                        {
                            array[Yship - 1, Xship + ship.GetSize()] = true;
                        }
                        if (Yship < sizeField - 1)
                        {
                            array[Yship + 1, Xship + ship.GetSize()] = true;
                        }
                    }
                }
                if (Xship <= sizeField - 1 && Xship != 0)
                {
                    if (location)
                    {
                        array[Xship - 1, Yship] = true;
                        if (Yship > 0)
                        {
                            array[Xship - 1, Yship - 1] = true;
                        }
                        if (Yship < sizeField - 1)
                        {
                            array[Xship - 1, Yship + 1] = true;
                        }
                    }
                    else
                    {
                        array[Yship, Xship - 1] = true;
                        if (Yship > 0)
                        {
                            array[Yship - 1, Xship - 1] = true;
                        }
                        if (Yship < sizeField - 1)
                        {
                            array[Yship + 1, Xship - 1] = true;
                        }
                    }
                }
            }
        }

        private bool CheckSize(int point, int size)
        {
            return point + size <= sizeField;
        }

        private ConsoleKey AskExit()
        {
            Console.WriteLine("Press any key to continue, or Esc to exit");
            var key = Console.ReadKey();

            return key.Key;
        }

        public bool CheckSizeShip(int sizeShip)
        {
            //проверяем на наличие кораблей заданного размера
            if (countX1 < 4 && sizeShip == 1)
            {
                ++countX1;
            }
            else if (countX2 < 3 && sizeShip == 2)
            {
                ++countX2;
            }
            else if (countX3 < 2 && sizeShip == 3)
            {
                ++countX3;
            }
            else if (countX4 < 1 && sizeShip == 4)
            {
                ++countX4;
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool CheckCoordinate(int x, int y, int size, int location)
        {

            if (location == 1 && x + size <= sizeField)
            {
                return (!fieldShip[x + size - 1, y]) && (!fieldBoundary[x + size - 1, y]);
            }
            else if (location == 0 && y + size <= sizeField)
            {
                return (!fieldShip[x, y + size - 1]) && (!fieldBoundary[x, y + size - 1]);
            }
            return false;
        }

        private void AddShipToFieldShip(ClassShip ship)
        {
            if (ship.GetFlagHor())
            {
                for (int i = ship.GetX(); i < ship.GetX() + ship.GetSize(); i++)
                {
                    fieldShip[i, ship.GetY()] = true;
                }
            }
            else
            {
                for (int i = ship.GetY(); i < ship.GetY() + ship.GetSize(); i++)
                {
                    fieldShip[ship.GetX(), i] = true;
                }
            }
        }

        public int GetIndex()
        {
            return index;
        }

        public void ClearStringBuider()
        {
            fieldPaint.Clear();
        }

        public bool CheckCoordinateAndSize(int x, int y, int size)
        {
            if (CheckSizeShip(size))
            {
                return true;
            }
            if (CheckSize(x, size))
            {
                if (!fieldShip[x + size - 1, y] && !fieldBoundary[x + size - 1, y])
                {
                    return true;
                }
            }
            else if (CheckSize(y, size))
            {
                if (!fieldShip[x, y + size - 1] && !fieldBoundary[x, y + size - 1])
                {
                    return true;
                }
            }
            return false;
        }

        public void GenerateRandomField()
        {
            Random rand = new Random();
            int x;
            int y;
            int sizeShip;
            int location;
            bool flag = true;

            if (index == 0)
            {
                sizeShip = 4;
            }
            else if (index == 1 || index == 2)
            {
                sizeShip = 3;
            }
            else if (index > 2 && index < 6)
            {
                sizeShip = 2;
            }
            else
            {
                sizeShip = 1;
            }

            do
            {
                do
                {
                    x = rand.Next(0, 9);
                    y = rand.Next(0, 9);
                } while ((fieldShip[x, (int)y] || fieldBoundary[x, (int)y]) || !(CheckCoordinateAndSize(x, y, sizeShip)));
                if (flag)
                {
                    location = 1;
                }
                else
                {
                    location = 0;
                }
                flag = false;
            } while (!CheckCoordinate(x, y, sizeShip, location));

            arrayShip[index] = new ClassShip(x, (int)y, sizeShip, location == 0 ? false : true);
            AddShipToFieldShip(arrayShip[index]);
            AddToFieldBoundary(arrayShip[index++], fieldBoundary);
        }

        public void PaintShot()
        {
            ClearStringBuider();
            Letter letter = 0;
            for (int y = 0; y < sizeField + 3; y++)
            {
                for (int x = 0; x < sizeField + 3; x++)
                {
                    if (x == 2 && y == 0)
                    {
                        for (int i = 0; i < sizeField; i++)
                        {
                            fieldPaint.Append(i);
                        }
                    }
                    else if (x == 0 && y > 1 && y < sizeField + 2)
                    {
                        fieldPaint.Append(letter++.ToString());
                    }
                    else if (x == 1 && y == 1)
                    {
                        fieldPaint.Append("╔");
                    }
                    else if (y == 1 && x == sizeField + 2)
                    {
                        fieldPaint.Append("╗");
                    }
                    else if ((x == 1 || x == sizeField + 2) && (y > 1 && y < sizeField + 2))
                    {
                        fieldPaint.Append("║");
                    }
                    else if ((y == 1 || y == sizeField + 2) && (x > 1 && x < sizeField + 2))
                    {
                        fieldPaint.Append("═");
                    }
                    else if (x == 1 && y == sizeField + 2)
                    {
                        fieldPaint.Append("╚");
                    }
                    else if (x == sizeField + 2 && y == sizeField + 2)
                    {
                        fieldPaint.Append("╝");
                    }
                    else if (x > 1 && y > 1 && shotEmpty[x - 2, y - 2])
                    {
                        fieldPaint.Append(".");
                    }
                    else if ((x > 1 && y > 1 && shotGot[x - 2, y - 2]))
                    {
                        fieldPaint.Append("X");
                    }
                    else
                    {
                        fieldPaint.Append(" ");
                    }
                }
                fieldPaint.Append("\n");
            }
            Console.WriteLine(fieldPaint);
        }

        public bool CheckShotEmpty(int x, int y)
        {
            return shotEmpty[x, y];
        }

        public bool CheckShotGot(int x, int y)
        {
            return shotGot[x, y];
        }

        public bool CheckFieldShip(int x, int y)
        {
            return fieldShip[x, y];
        }

        public bool CheckFieldBoundary(int x, int y)
        {
            return fieldBoundary[x, y];
        }

        public void AddShotEmpty(int x, int y)
        {
            shotEmpty[x, y] = true;
        }

        public void AddShotGot(int x, int y)
        {
            shotGot[x, y] = true;
        }

        public int FindShipAfterShot(int x, int y)
        {
            int index = 0;
            foreach(var ship in arrayShip)
            {
                if(ship.GetFlagHor())
                {
                    for(int i = 0; i < ship.GetSize(); i++)
                    {
                        if (ship.GetX() + i == x)
                        {
                            index = Array.IndexOf(arrayShip, ship);
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ship.GetSize(); i++)
                    {
                        if (ship.GetY() + i == x)
                        {
                            index = Array.IndexOf(arrayShip, ship); ;
                            break;
                        }
                    }
                }
            }
            return (int)index;
        }
        
        public void AddBoundaryAfterShot(ClassShip ship)
        {
            AddToFieldBoundary(ship, shotEmpty);
        }
    }
}
