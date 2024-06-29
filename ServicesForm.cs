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
    public partial class ServicesForm : Form
    {
        private int clientId;

        public ServicesForm(int clientId)
        {
            InitializeComponent();
            this.clientId = clientId;
            LoadServices();
        }

        private void LoadServices()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarWashDB"].ConnectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Services";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable servicesTable = new DataTable();
                    adapter.Fill(servicesTable);
                    dataGridViewServices.DataSource = servicesTable;
                }
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            if (dataGridViewServices.SelectedRows.Count > 0)
            {
                int serviceId = Convert.ToInt32(dataGridViewServices.SelectedRows[0].Cells["ServiceID"].Value);

                // Открываем форму для оформления заказа
                BookingForm bookingForm = new BookingForm(clientId, serviceId);
                bookingForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Выберите услугу для оформления заказа.");
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}

