using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GestionReservasHotel
{
    public abstract class Reserva
    {
        // Atributos básicos
        public string NombreCliente { get; set; }
        public string DocumentoCliente { get; set; }
        public int NumeroHabitacion { get; set; }
        public DateTime FechaReserva { get; set; }
        public int DuracionEstadia { get; set; }
        public double TarifaPorNoche { get; set; }

        // inicializar los datos
        public Reserva(string nombre, string documento, int habitacion, DateTime fecha, int noches, double tarifa)
        {
            NombreCliente = nombre;
            DocumentoCliente = documento;
            NumeroHabitacion = habitacion;
            FechaReserva = fecha;
            DuracionEstadia = noches;
            TarifaPorNoche = tarifa;
        }

        
        public virtual double CalcularCostoTotal()
        {
            return DuracionEstadia * TarifaPorNoche;
        }

       


        public virtual void ValidarDatos()
        {
            // Regla 1: Si el nombre está vacío, lanza un error.
            if (string.IsNullOrEmpty(NombreCliente))
            {
                throw new Exception("El nombre es obligatorio.");
            }

            // Regla 2: Si se queda 1 noche o menos, lanza un error.
            if (DuracionEstadia <= 1)
            {
                throw new Exception("La estadía debe ser mayor a 1 noche.");
            }

            // Regla 3: La tarifa no puede ser gratis o negativa.
            if (TarifaPorNoche <= 0)
            {
                throw new Exception("La tarifa debe ser mayor a cero.");
            }

            //Regla 4: El documento no puede contener una letra.
            // Usamos LINQ para revisar que cada caracter sea un número.
            if (!DocumentoCliente.All(char.IsDigit))
            {
                throw new Exception("El documento solo puede contener números, no se permiten letras ni caracteres.");
            }
        }
    }
}
