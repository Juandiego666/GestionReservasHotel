using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionReservasHotel
{
    public class HabitacionVIP : Reserva
    {
        public HabitacionVIP(string nombre, string documento, int habitacion, DateTime fecha, int noches, double tarifa)
            : base(nombre, documento, habitacion, fecha, noches, tarifa) { }

        public override double CalcularCostoTotal()
        {
            double total = base.CalcularCostoTotal();
            if (DuracionEstadia > 5)
            {
                total *= 0.80; // Aplicamos el 20% de descuento
            }
            return total;
        }
    }
}
