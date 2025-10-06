using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class Admin : UserControl
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Form parentform = this.FindForm();
            OverlayForm overlay = new OverlayForm(parentform); 
            overlay.Show();
            add_Admin addAdmin = new add_Admin(null);
            addAdmin.ShowDialog();
            overlay.Dispose();
            LoadAdmins(); 
        }

        public void LoadAdmins()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT id, photo, firstname, middlename, lastname, role, contact_no FROM admin_accounts";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    dtgAdmins.Rows.Clear(); // Clear previous data

                    while (reader.Read())
                    {
                        // Get image and resize 
                        byte[] imgBytes = (byte[])reader["photo"];
                        MemoryStream ms = new MemoryStream(imgBytes);
                        Image img = ResizeImage(Image.FromStream(ms), 80, 80);

                        // Format full name
                        string fullName = $"{reader["lastname"]}, {reader["firstname"]} {reader["middlename"]}";

                        // Add row to DataGridView
                        dtgAdmins.Rows.Add(
                            reader["id"].ToString(),
                            img,
                            fullName,
                            reader["role"].ToString(),
                            reader["contact_no"].ToString()
                        );
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading admin data: " + ex.Message);
                }
            }
        }


        private Image ResizeImage(Image img, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, 0, 0, width, height);
            }
            return bmp;
        }
        private void Admin_Load(object sender, EventArgs e)
        {
            LoadAdmins();
        }

        private void dtgAdmins_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            int editColumnIndex = 5; // Example: Edit button column index
            int deleteColumnIndex = 6; // Example: Delete button column index

            // Assuming column 0 or a hidden column has the admin ID
            string adminId = dtgAdmins.Rows[e.RowIndex].Cells[0].Value?.ToString();

            if (e.ColumnIndex == editColumnIndex)
            {
                Form parentform = this.FindForm();
                OverlayForm overlay = new OverlayForm(parentform);
                overlay.Show();
                add_Admin addAdmin = new add_Admin(adminId);
                addAdmin.ShowDialog();
                overlay.Dispose();
                LoadAdmins();
            }
            else if (e.ColumnIndex == deleteColumnIndex)
            {
                var confirm = MessageBox.Show("Are you sure you want to delete this admin?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    using (MySqlConnection conn = Database.GetConnection())
                    {
                        try
                        {
                            conn.Open();
                            string query = "DELETE FROM admin_accounts WHERE id = @id";
                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@id", adminId);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Admin deleted successfully!");
                            LoadAdmins(); // Refresh table
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error deleting admin: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string filtertext = tbSearch.Text.Trim().ToLower();
            {
                foreach (DataGridViewRow row in dtgAdmins.Rows)
                {
                    string fullName = row.Cells[2].Value?.ToString().ToLower();
                    string role = row.Cells[3].Value?.ToString().ToLower();
                    row.Visible = fullName.Contains(filtertext) || role.Contains(filtertext);
                }
            }
        }
    }
}
