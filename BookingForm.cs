using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistrationAndLogin
{
    public partial class BookingForm : Form
    {
        private int clientId;
        private int serviceId;

        public BookingForm(int clientId, int serviceId)
        {
            InitializeComponent();
            this.clientId = clientId;
            this.serviceId = serviceId;
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarWashDB"].ConnectionString))
            {
                conn.Open();
                string query = "SELECT Employees.EmployeeID, Employees.FirstName + ' ' + Employees.LastName AS EmployeeName FROM Employees INNER JOIN ServiceAssignments ON Employees.EmployeeID = ServiceAssignments.EmployeeID WHERE ServiceAssignments.ServiceID = @ServiceID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ServiceID", serviceId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable employeesTable = new DataTable();
                        employeesTable.Load(reader);
                        comboBoxEmployees.DataSource = employeesTable;
                        comboBoxEmployees.DisplayMember = "EmployeeName";
                        comboBoxEmployees.ValueMember = "EmployeeID";
                    }
                }
            }
        }

        private void btnConfirmOrder_Click(object sender, EventArgs e)
        {
            int employeeId = Convert.ToInt32(comboBoxEmployees.SelectedValue);
            DateTime orderDate = dateTimePickerOrderDate.Value;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarWashDB"].ConnectionString))
            {
                conn.Open();
                string query = "INSERT INTO Orders (ClientID, ServiceID, EmployeeID, OrderDate, Status) VALUES (@ClientID, @ServiceID, @EmployeeID, @OrderDate, @Status)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClientID", clientId);
                    cmd.Parameters.AddWithValue("@ServiceID", serviceId);
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                    cmd.Parameters.AddWithValue("@OrderDate", orderDate);
                    cmd.Parameters.AddWithValue("@Status", "Pending");
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Заказ успешно создан!");
        }
    }
}
