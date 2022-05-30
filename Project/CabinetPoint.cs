using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class CabinetPoint
    {
        private readonly int cabNum;
        protected Point relpoint;
        public CabinetPoint() { cabNum = 0; }
        public CabinetPoint(int cabNum1, Point p) { cabNum = cabNum1; relpoint = p; }
        public int getCab() { return cabNum; }
        public Point getRelPoint() { return relpoint; }
    }
}