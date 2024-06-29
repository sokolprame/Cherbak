using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistrationAndLogin
{
    public partial class MainForm : Form
    {
        private int clientId = -1; // Идентификатор текущего клиента

        public MainForm()
        {
            InitializeComponent();

        }

        private void регистрацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClientRegistrationForm registrationForm = new ClientRegistrationForm();
            registrationForm.ShowDialog();
        }

        private void входToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClientLoginForm loginForm = new ClientLoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                clientId = loginForm.ClientId; // Получаем идентификатор клиента после успешного входа
            }
        }

        private void услугиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clientId > 0)
            {
                ServicesForm servicesForm = new ServicesForm(clientId);
                servicesForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Пожалуйста, войдите в систему, чтобы просматривать услуги.");
            }
        }

        private void моиЗаказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clientId > 0)
            {
                OrdersForm ordersForm = new OrdersForm(clientId);
                ordersForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Пожалуйста, войдите в систему, чтобы просматривать свои заказы.");
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}

