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
    public partial class ClientLoginForm : Form
    {
        public int ClientId { get; private set; } // Свойство для хранения идентификатора клиента

        public ClientLoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CarWashDB"].ConnectionString))
            {
                conn.Open();
                string query = "SELECT ClientID FROM Clients WHERE Email = @Email AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        ClientId = Convert.ToInt32(result);
                        MessageBox.Show("Вход успешно выполнен!");
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Неверный email или пароль.");
                    }
                }
            }
        }
    }
}