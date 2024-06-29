using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistrationAndLogin
{
    public partial class OrdersForm : Form
    {
        private int clientId;

        public OrdersForm(int clientId)
        {
            InitializeComponent();
            this.clientId = clientId;
            LoadClientOrders();
        }

        private void LoadClientOrders()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarWashDB"].ConnectionString))
            {
                conn.Open();
                string query = "SELECT Orders.OrderID, Services.ServiceName, Orders.OrderDate, Employees.FirstName + ' ' + Employees.LastName AS EmployeeName, Orders.Status FROM Orders INNER JOIN Services ON Orders.ServiceID = Services.ServiceID INNER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID WHERE Orders.ClientID = @ClientID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClientID", clientId);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable ordersTable = new DataTable();
                        adapter.Fill(ordersTable);
                        dataGridViewOrders.DataSource = ordersTable;
                    }
                }
            }
        }

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0)
            {
                int orderId = Convert.ToInt32(dataGridViewOrders.SelectedRows[0].Cells["OrderID"].Value);
                PrintOrder(orderId);
            }
            else
            {
                MessageBox.Show("Выберите заказ для печати талона.");
            }
        }

        private void PrintOrder(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarWashDB"].ConnectionString))
            {
                conn.Open();
                string query = "SELECT Orders.OrderID, Clients.FirstName + ' ' + Clients.LastName AS ClientName, Services.ServiceName, Employees.FirstName + ' ' + Employees.LastName AS EmployeeName, Orders.OrderDate, Orders.Status FROM Orders INNER JOIN Clients ON Orders.ClientID = Clients.ClientID INNER JOIN Services ON Orders.ServiceID = Services.ServiceID INNER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID WHERE Orders.OrderID = @OrderID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string receipt = $"Заказ ID: {reader["OrderID"]}\n" +
                                             $"Клиент: {reader["ClientName"]}\n" +
                                             $"Услуга: {reader["ServiceName"]}\n" +
                                             $"Сотрудник: {reader["EmployeeName"]}\n" +
                                             $"Дата: {reader["OrderDate"]}\n" +
                                             $"Статус: {reader["Status"]}";

                            string fileName = $"Order_{orderId}.txt";
                            File.WriteAllText(fileName, receipt);

                            MessageBox.Show($"Талон успешно распечатан!\nСохранен в файл: {fileName}");
                        }
                    }
                }
            }
        }
    }
}