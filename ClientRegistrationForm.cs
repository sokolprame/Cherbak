using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistrationAndLogin
{
    public partial class ClientRegistrationForm : Form
    {
        public ClientRegistrationForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Проверка, что все необходимые поля заполнены
            if (string.IsNullOrEmpty(txtFirstName.Text) ||
                string.IsNullOrEmpty(txtLastName.Text) ||
                string.IsNullOrEmpty(txtPhone.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) ||
                string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // Убедитесь, что строка подключения корректна
            string connectionString = @"Data Source=DESKTOP-RTM981H\SQLEXPRESS;Initial Catalog=CarWashDB;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Clients (FirstName, LastName, Phone, Email, Password) VALUES (@FirstName, @LastName, @Phone, @Email, @Password)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("FirstName", txtFirstName.Text);
                        command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                        command.Parameters.AddWithValue("Phone", txtPhone.Text);
                        command.Parameters.AddWithValue("Password", txtPassword.Text);
                        command.Parameters.AddWithValue("@Email", txtEmail.Text);


                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Регистрация прошла успешно.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при регистрации: " + ex.Message);
                }
            }
        }
    }
}

