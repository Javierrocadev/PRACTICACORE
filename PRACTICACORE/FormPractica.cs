using Microsoft.VisualBasic.Logging;
using PRACTICACORE.Models;
using PRACTICACORE.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#region
//create procedure SP_CLIENTES
//as
//	select empresa from CLIENTES
//go

//create procedure SP_CLIENTE_DATOS
//(@empresa nvarchar(50))
//as
//	select * from CLIENTES
//	where Empresa=@empresa
//go

//CREATE PROCEDURE SP_PEDIDOS_CLIENTE_2
//    @empresa NVARCHAR(50)
//AS
//BEGIN
//    DECLARE @codigoCliente NVARCHAR(50);

//--Obtener el código del cliente basado en el nombre de la empresa
//    SELECT @codigoCliente = CodigoCliente
//    FROM Clientes
//    WHERE Empresa = @empresa;

//--Seleccionar los pedidos del cliente
//    SELECT *
//    FROM Pedidos
//    WHERE CodigoCliente = @codigoCliente;
//END

#endregion
namespace PRACTICACORE
{
    public partial class FormPractica : Form
    {
        private readonly DatosRepository repository;

        public FormPractica()
        {
            InitializeComponent();
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=NETCORE;User ID=javier;Password=MCSD2023;";
            this.repository = new DatosRepository(connectionString);
            this.LoadClientes();
        }

        private void LoadClientes()
        {
            List<string> clientes = repository.ObtenerClientes();
            cmbclientes.Items.AddRange(clientes.ToArray());
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string clienteSeleccionado = cmbclientes.SelectedItem.ToString();
            LimpiarDatosPedidos();

            Cliente cliente = repository.ObtenerDatosCliente(clienteSeleccionado);
            if (cliente != null)
            {
                MostrarDatosCliente(cliente);
            }

            List<Pedido> pedidos = repository.ObtenerPedidosCliente(clienteSeleccionado);
            MostrarPedidosCliente(pedidos);
        }

        private void MostrarDatosCliente(Cliente cliente)
        {
            txtempresa.Text = cliente.Empresa;
            txtcontacto.Text = cliente.Contacto;
            txtcargo.Text = cliente.Cargo;
            txtciudad.Text = cliente.Ciudad;
            txttelefono.Text = cliente.Telefono;
        }

        private void MostrarPedidosCliente(List<Pedido> pedidos)
        {
            foreach (Pedido pedido in pedidos)
            {
                lstpedidos.Items.Add(pedido.FechaEntrega.ToString());
            }
        }

        private void LimpiarDatosPedidos()
        {
            lstpedidos.Items.Clear();
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            if (lstpedidos.SelectedIndex != -1)
            {
                string codigoPedidoSeleccionado = txtcodigopedido.Text;
                repository.EliminarPedido(codigoPedidoSeleccionado);
                LimpiarDatosPedidoSeleccionado();
                cmbclientes_SelectedIndexChanged(null, null);
            }
        }

        private void LimpiarDatosPedidoSeleccionado()
        {
            txtcodigopedido.Clear();
            txtfechaentrega.Clear();
            txtformaenvio.Clear();
            txtimporte.Clear();
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstpedidos.SelectedIndex != -1)
            {
                int indicePedidoSeleccionado = lstpedidos.SelectedIndex;

                LimpiarDatosPedidoSeleccionado();

                // Obtener el pedido seleccionado desde el ListBox
                Pedido pedidoSeleccionado = lstpedidos.SelectedItem as Pedido;

                if (pedidoSeleccionado != null)
                {
                    MostrarDatosPedidoSeleccionado(pedidoSeleccionado);
                }
            }
        }

        private void MostrarDatosPedidoSeleccionado(Pedido pedido)
        {
            txtcodigopedido.Text = pedido.CodigoPedido;
            txtfechaentrega.Text = pedido.FechaEntrega.ToString();
            txtformaenvio.Text = pedido.FormaEnvio;
            txtimporte.Text = pedido.Importe.ToString();
        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
           
            string codigoPedido = txtcodigopedido.Text;
            DateTime fechaEntrega = DateTime.Parse(txtfechaentrega.Text);
            string formaEnvio = txtformaenvio.Text;
            decimal importe = decimal.Parse(txtimporte.Text); 

           
            Pedido nuevoPedido = new Pedido
            {
                CodigoPedido = codigoPedido,
                FechaEntrega = fechaEntrega,
                FormaEnvio = formaEnvio,
                Importe = importe
            };

           
            repository.AgregarPedido(nuevoPedido);

            LimpiarCamposPedido();

          
            cmbclientes_SelectedIndexChanged(null, null);
        }

        private void LimpiarCamposPedido()
        {
            txtcodigopedido.Clear();
            txtfechaentrega.Clear();
            txtformaenvio.Clear();
            txtimporte.Clear();
        }
    }
}
