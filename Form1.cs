using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AcceptButton = btnLogin;
            this.Shown += (s, e) => tbUsername.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = tbUsername.Text.Trim();
            string password = tbPassword.Text.Trim();

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT firstname, middlename, lastname, role, password, photo FROM admin_accounts WHERE username=@username";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string hashedPassword = reader.GetString("password");

                                if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                                {
                                    string firstName = reader.GetString("firstname");
                                    string middleName = reader.GetString("middlename");
                                    string lastName = reader.GetString("lastname");
                                    string role = reader.GetString("role");
                                    string displayName = $"{firstName} {middleName} {lastName}";

                                    byte[] imgData = (byte[])reader["photo"];

                                    // ✅ Close the reader BEFORE showing Form2
                                    reader.Close();
                                    conn.Close();

                                    Form2 dashboard = new Form2(displayName, imgData, role);
                                    dashboard.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    MessageBox.Show("Invalid username or password.", "Login Failed");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password.", "Login Failed");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        bool passwordShown = false;

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            if (passwordShown)
            {
                tbPassword.UseSystemPasswordChar = true;
                guna2PictureBox1.Image = Properties.Resources.hide; // Assuming you have an eye_closed image
                passwordShown = false;
            }
            else
            {
                tbPassword.UseSystemPasswordChar = false;
                guna2PictureBox1.Image = Properties.Resources.view; // Assuming you have an eye_open image
                passwordShown = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbUsername.Focus();
        }

        private void ForgotPass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            ForgotPassword forgotPasswordForm = new ForgotPassword();
            forgotPasswordForm.ShowDialog(); // Show as modal dialog
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
