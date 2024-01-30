using PRACTICACORE.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRACTICACORE.Repository
{
    public class DatosRepository
    {
        private readonly string connectionString;

        public DatosRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<string> ObtenerClientes()
        {
            List<string> clientes = new List<string>();
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand com = new SqlCommand("SP_CLIENTES", cn))
            {
                com.CommandType = CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string empresa = reader["EMPRESA"].ToString();
                        clientes.Add(empresa);
                    }
                }
            }
            return clientes;
        }

        public Cliente ObtenerDatosCliente(string empresa)
        {
            Cliente cliente = null;
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand com = new SqlCommand("SP_CLIENTE_DATOS", cn))
            {
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@empresa", empresa);
                cn.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cliente = new Cliente
                        {
                            Empresa = reader["EMPRESA"].ToString(),
                            Contacto = reader["CONTACTO"].ToString(),
                            Cargo = reader["CARGO"].ToString(),
                            Ciudad = reader["CIUDAD"].ToString(),
                            Telefono = reader["TELEFONO"].ToString()
                        };
                    }
                }
            }
            return cliente;
        }

        public List<Pedido> ObtenerPedidosCliente(string empresa)
        {
            List<Pedido> pedidos = new List<Pedido>();
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand com = new SqlCommand("SP_PEDIDOS_CLIENTE_2", cn))
            {
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@empresa", empresa);
                cn.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pedido pedido = new Pedido
                        {
                            CodigoPedido = reader["CodigoPedido"].ToString(),
                            FechaEntrega = Convert.ToDateTime(reader["FechaEntrega"]),
                            FormaEnvio = reader["FormaEnvio"].ToString(),
                            Importe = Convert.ToDecimal(reader["Importe"])
                        };
                        pedidos.Add(pedido);
                    }
                }
            }
            return pedidos;
        }

        public void EliminarPedido(string codigoPedido)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand com = new SqlCommand("DELETE FROM PEDIDOS WHERE CodigoPedido = @codigoPedido", cn))
            {
                com.Parameters.AddWithValue("@codigoPedido", codigoPedido);
                cn.Open();
                com.ExecuteNonQuery();
            }
        }
        public void AgregarPedido(Pedido pedido)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Pedidos (CodigoPedido, CodigoCliente, FechaEntrega, FormaEnvio, Importe)
                                VALUES (@CodigoPedido, @CodigoCliente, @FechaEntrega, @FormaEnvio, @Importe)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CodigoPedido", pedido.CodigoPedido);
                //command.Parameters.AddWithValue("@CodigoCliente", pedido.CodigoCliente); 
                command.Parameters.AddWithValue("@FechaEntrega", pedido.FechaEntrega);
                command.Parameters.AddWithValue("@FormaEnvio", pedido.FormaEnvio);
                command.Parameters.AddWithValue("@Importe", pedido.Importe);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
