using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassField;

namespace _2017._09._14__BattleSea2
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Play play = new Play();

            play.FillFieldAndGenerateForComputer();

            while (play.ComputerShipDead != 10 || play.UserShipDead != 10)
            {
                while (play.ShotUser())
                {
                    play.Paint();
                }                
                while (play.ShotComputer())
                {
                    play.Paint();
                }
                play.Paint();
                Console.WriteLine("Press any key to continue, or Esc to exit");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    return;
                }
                else
                {
                    continue;
                }

            }           
            
        }
    }
}
