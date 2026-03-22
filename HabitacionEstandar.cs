using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionReservasHotel
{
    public class HabitacionEstandar : Reserva
    {
        public HabitacionEstandar(string n, string d, int h, DateTime f, int nch, double t)
            : base(n, d, h, f, nch, t) { }
    }
}
