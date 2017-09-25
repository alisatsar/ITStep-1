using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassField
{
    public class Play
    {
        private Field user;
        private Field computer;
        public int ComputerShipDead { set; get; }
        public int UserShipDead { set; get; }
        private int computerXShot = -1;
        private int computerYShot = -1;
        private int flagRotation = 12;
        

        public Play()
        {
            user = new Field();
            computer = new Field();
            ComputerShipDead = 0;
            UserShipDead = 0;
        }

        public void FillFieldAndGenerateForComputer()
        {
            string choose;

            do
            {
                Console.WriteLine("How you want to fill the field? 1 - manually or 0 - automatically:");
                choose = Console.ReadLine();
            } while (!(int.Parse(choose) > -1 && int.Parse(choose) < 2));

            if (int.Parse(choose) == 1)
            {
                while (user.GetIndex() < 10)
                {
                    user.Paint();
                    user.AddNewShip();                    
                }
            }
            else
            {
                while (user.GetIndex() < 10)
                {
                    user.GenerateRandomField();
                }
            }

            Console.Clear();
            user.ClearStringBuider();
            
            while (computer.GetIndex() < 10)
            {
                computer.GenerateRandomField();
            }
            //computer.ClearStringBuider();
            Console.WriteLine("Computer");
            computer.PaintShot();
            Console.WriteLine("User");
            user.Paint();
        }

        public bool ShotUser()
        {
            string xYCoordinates;
            int x = 0;
            Letter y = 0;
            char[] xYAr;            

            do
            {
                do
                {
                    Console.WriteLine("Please, enter coordinate shot: ");
                    xYCoordinates = Console.ReadLine();
                } while (xYCoordinates.Length != 2);

                xYAr = xYCoordinates.ToCharArray();
                x = int.Parse(xYAr[0].ToString());
                y = (Letter)Enum.Parse(typeof(Letter), xYAr[1].ToString());
            } while (computer.CheckShotEmpty(x, (int)y) || computer.CheckShotGot(x, (int)y));
            
            return CheckShot(x, (int)y, ComputerShipDead, computer);
        }

        public bool ShotComputer()
        {
            Random rand = new Random();
            
            if (computerXShot == -1 && computerYShot == -1)
            {
                do
                {
                    computerXShot = rand.Next(0, 9);
                    computerYShot = rand.Next(0, 9);
                } while (computer.CheckShotEmpty(computerXShot, computerYShot) || computer.CheckShotGot(computerXShot, computerYShot));
            }
            else if(flagRotation == 12 && computerXShot != -1 && computerYShot != -1)
            {
                if (computerYShot > 0 && computer.CheckFieldShip(computerXShot, computerYShot - 1))
                {
                    computerYShot -= 1;
                    flagRotation = 12;
                }
                else
                {
                    flagRotation = 3;
                }                
            }
            else if(flagRotation == 3 && computerXShot != -1 && computerYShot != -1)
            {
                if (computerXShot < 9 && computer.CheckFieldShip(computerXShot + 1, computerYShot))
                {
                    computerXShot += 1;
                    flagRotation = 3;
                }
                else
                {
                    flagRotation = 6;
                }
            }
            else if (flagRotation == 6 && computerXShot != -1 && computerYShot != -1)
            {
                if (computerYShot < 9 && computer.CheckFieldShip(computerXShot, computerYShot + 1))
                {
                    computerYShot += 1; 
                    flagRotation = 6;
                }
                else
                {
                    flagRotation = 9;
                }
            }
            else if (flagRotation == 9 && computerXShot != -1 && computerYShot != -1)
            {
                if (computerXShot > 0 && computer.CheckFieldShip(computerXShot - 1, computerYShot))
                {
                    computerXShot -= 1;
                    flagRotation = 9;
                }
                else
                {
                    flagRotation = 12;
                }
            }

            return CheckShot(computerXShot, computerYShot, UserShipDead, user);
        }

        public bool CheckShot(int x, int y, int shipDead, Field typeField)
        {
            bool result = false;            
            if (typeField.CheckFieldShip(x, (int)y))
            {
                int index = typeField.FindShipAfterShot(x, (int)y);
                typeField.AddShotGot(x, (int)y);

                if (typeField.arrayShip[index].HowMuchFire < typeField.arrayShip[index].GetSize())
                {
                    ++typeField.arrayShip[index].HowMuchFire;
                    result = true;
                }
                if (typeField.arrayShip[index].HowMuchFire == typeField.arrayShip[index].GetSize())
                {
                    typeField.AddBoundaryAfterShot(typeField.arrayShip[index]);                    
                    ++shipDead;                    
                    result = false;
                    if (typeField == computer)
                    {
                        computerXShot = -1;
                        computerYShot = -1;
                    }
                }                              
            }
            else
            {
                typeField.AddShotEmpty(x, (int)y);                
            }

            return result;
        }

        public void Paint()
        {
            Console.Clear();
            Console.WriteLine("Computer");            
            computer.PaintShot();
            Console.WriteLine("User");
            user.Paint();
        }
        
        
    }
}
