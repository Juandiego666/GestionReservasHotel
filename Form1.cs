using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionReservasHotel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numHabitacion_ValueChanged(object sender, EventArgs e)
        {

        }

        List<Reserva> listaReservas = new List<Reserva>();

        private void Form1_Load(object sender, EventArgs e)
        {
            // Esto llena las opciones del cuadrito apenas abre el programa
            cmbTipoHabitacion.Items.Clear();
            cmbTipoHabitacion.Items.Add("Estándar");
            cmbTipoHabitacion.Items.Add("VIP");

            // Selecciona la primera opción por defecto para que no aparezca vacío
            cmbTipoHabitacion.SelectedIndex = 0;
        }

        private void LimpiarCampos()
        {
            // Vaciamos los cuadros de texto
            txtNombreCliente.Clear();
            txtDocumento.Clear();
            txtTarifa.Clear();

            // Reiniciamos los selectores numéricos a sus valores iniciales
            numHabitacion.Value = 0;
            numNoches.Value = 0;

            // Reiniciamos la fecha a la fecha actual
            dtpFechaReserva.Value = DateTime.Now;

            // Quitamos la selección del combo de tipo de habitación
            cmbTipoHabitacion.SelectedIndex = -1;
        } 
        private void ActualizarPantalla()
        {
            // 1. Limpiamos el origen de datos actual para que se refresque la tabla
            dgvReservas.DataSource = null;

            // Esto cumple con la Regla 9 de mantener la interfaz actualizada 
            dgvReservas.DataSource = listaReservas.Select(r => new {
                Cliente = r.NombreCliente,
                Documento = r.DocumentoCliente,
                Habitacion = r.NumeroHabitacion,
                // Determinamos el tipo de habitación para mostrarlo en el Grid 

                Tipo = r is HabitacionVIP ? "VIP" : "Estándar",
                Fecha = r.FechaReserva.ToShortDateString(),
                Estadia = r.DuracionEstadia,
                // Llamamos al método que calcula el costo con o sin descuento 

                Total = r.CalcularCostoTotal()
            }).ToList();
        }

        private void btnResgistrar_Click(object sender, EventArgs e)
        {

            try
            {

                string nombre = txtNombreCliente.Text;
                string documento = txtDocumento.Text;
                int habitacion = (int)numHabitacion.Value;
                string tipo = cmbTipoHabitacion.Text; // "Estándar" o "VIP" 
                DateTime fecha = dtpFechaReserva.Value;
                int noches = (int)numNoches.Value;
                double tarifa = double.Parse(txtTarifa.Text);

                // REGLA 5: VALIDAR DISPONIBILIDAD 
                foreach (var r in listaReservas)
                {
                    if (r.NumeroHabitacion == habitacion && r.FechaReserva.Date == fecha.Date)
                    {
                        throw new Exception("La habitación " + habitacion + " ya está ocupada en esa fecha.");
                    }
                }


                Reserva nuevaReserva;

                if (tipo == "VIP")
                {
                    nuevaReserva = new HabitacionVIP(nombre, documento, habitacion, fecha, noches, tarifa);
                }
                else if (tipo == "Estándar")
                {
                    nuevaReserva = new HabitacionEstandar(nombre, documento, habitacion, fecha, noches, tarifa);
                }
                else
                {
                    throw new Exception("Debe seleccionar un tipo de habitación válido."); // Regla 7 
                }

                //VALIDAR REGLAS BÁSICAS (REGLA 1, 2 Y 3) 
                nuevaReserva.ValidarDatos();

                // 5. GUARDAR Y REFRESCAR INTERFAZ 
                listaReservas.Add(nuevaReserva);
                ActualizarPantalla();

                MessageBox.Show("Reserva registrada con éxito.");
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                // MANEJO DE EXCEPCIONES (TRY-CATCH) 
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dgvReservas_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            // Verificamos que se haya hecho clic en una fila real y no en el encabezado
            if (e.RowIndex >= 0)
            {
                // Obtenemos la reserva de nuestra lista usando el índice de la tabla
                var reserva = listaReservas[e.RowIndex];


                txtNombreCliente.Text = reserva.NombreCliente;
                txtDocumento.Text = reserva.DocumentoCliente;
                numHabitacion.Value = reserva.NumeroHabitacion;
                numNoches.Value = reserva.DuracionEstadia;
                txtTarifa.Text = reserva.TarifaPorNoche.ToString();
                dtpFechaReserva.Value = reserva.FechaReserva;


                cmbTipoHabitacion.Text = reserva is HabitacionVIP ? "VIP" : "Estándar";
            }
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificamos si hay una fila seleccionada en el DataGridView
                if (dgvReservas.CurrentRow == null)
                {
                    throw new Exception("Por favor, seleccione una reserva de la tabla para eliminar.");
                }

                // Preguntar al usuario para evitar eliminaciones accidentales 
                DialogResult respuesta = MessageBox.Show("¿Está seguro de eliminar esta reserva?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    // Obtenemos el índice de la fila seleccionada
                    int indice = dgvReservas.CurrentRow.Index;

                    // Lo eliminamos de la lista en memoria 
                    listaReservas.RemoveAt(indice);

                    // Actualizamos la pantalla para reflejar el cambio 
                    ActualizarPantalla();
                    MessageBox.Show("Reserva eliminada correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

       

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validar que seleccionó algo
                if (dgvReservas.CurrentRow == null)
                    throw new Exception("Primero selecciona una reserva en la tabla.");

                // 2. Obtener la posición (índice)
                int indice = dgvReservas.CurrentRow.Index;

                // 3. Actualizar el objeto que ya está en la lista
                listaReservas[indice].NombreCliente = txtNombreCliente.Text;
                listaReservas[indice].DocumentoCliente = txtDocumento.Text;
                listaReservas[indice].DuracionEstadia = (int)numNoches.Value;
                listaReservas[indice].TarifaPorNoche = double.Parse(txtTarifa.Text);
                listaReservas[indice].FechaReserva = dtpFechaReserva.Value;

                // 4. Validar que los nuevos datos sean correctos (Regla 1 y 2)
                listaReservas[indice].ValidarDatos();

                // 5. Refrescar la tabla y limpiar
                ActualizarPantalla();
                LimpiarCampos();

                MessageBox.Show("¡Reserva actualizada con éxito!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar: " + ex.Message);
            }
        }

        private void txtBuscarCliente_TextChanged(object sender, EventArgs e)
        {
            string busqueda = txtBuscarCliente.Text.ToLower();

            // Filtramos la lista sin borrar la original
            var listaFiltrada = listaReservas.Where(r => r.NombreCliente.ToLower().Contains(busqueda)).ToList();

            // Mostramos solo los resultados que coinciden
            dgvReservas.DataSource = null;
            dgvReservas.DataSource = listaFiltrada.Select(r => new {
                r.NombreCliente,
                r.NumeroHabitacion,
                Tipo = r is HabitacionVIP ? "VIP" : "Estándar",
                Total = r.CalcularCostoTotal()
            }).ToList();
        }

        

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            // Simplemente llamamos a la función que ya sabe vaciar todo
            LimpiarCampos();

            // Opcional: Quitar la selección de la tabla para que no quede nada marcado
            dgvReservas.ClearSelection();
        }
    }
}
