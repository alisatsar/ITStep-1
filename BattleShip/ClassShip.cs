using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassField
{
    public class ClassShip
    {
        private int size;
        private int coordinateX;
        private int coordinateY;
        private bool flagHorizontally;
        public int HowMuchFire { get; set; }

        public ClassShip()
        {
            size = 0;
            coordinateX = 0;
            coordinateY = 0;
            HowMuchFire = 0;
        }

        public ClassShip(int x, int y, int size, bool flag)
        {
            this.size = size;
            coordinateX = x;
            coordinateY = y;
            flagHorizontally = flag;
            HowMuchFire = 0;
        }

        public int GetSize()
        {
            return size;
        }

        public int GetX()
        {
            return coordinateX;
        }

        public int GetY()
        {
            return coordinateY;
        }        

        public bool GetFlagHor()
        {
            return flagHorizontally;
        }

        
    }
}
