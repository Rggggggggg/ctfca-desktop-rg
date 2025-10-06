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
    public partial class instructors : UserControl
    {
        public instructors()
        {
            InitializeComponent();
        }

        public void LoadInstructors()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT id, firstname, middlename, lastname, gender, age, date_of_birth, 
                                    contact_no, email,image, employment, instructors_id, 
                                    subject_assigned, grades_level_handled
                             FROM instructor_accounts";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtgInstructors.Rows.Clear(); // Clear previous data

                        while (reader.Read())
                        {
                            byte[] imgBytes = (byte[])reader["image"];
                            using (MemoryStream ms = new MemoryStream(imgBytes))
                            {
                                Image img = ResizeImage(Image.FromStream(ms), 80, 80); // 100x100 or your preferred size
                                string fullName = $"{reader["lastname"]}, {reader["firstname"]} {reader["middlename"]}";
                                dtgInstructors.Rows.Add(
                                    reader["instructors_id"].ToString(),
                                    img,
                                    fullName,
                                    reader["gender"].ToString(),
                                    reader["age"].ToString(),
                                    reader["contact_no"].ToString(),
                                    reader["employment"].ToString(),
                                    reader["subject_assigned"].ToString(),
                                    reader["grades_level_handled"].ToString()
                                );
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading instructor data: " + ex.Message);
                }
            }
        }


        private void instructors_Load(object sender, EventArgs e)
        {
            dtgInstructors.AlternatingRowsDefaultCellStyle = dtgInstructors.RowsDefaultCellStyle;
            LoadInstructors();
        }

        private void dtgInstructors_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            int editColumnIndex = 9;   
            int deleteColumnIndex = 10; 

            string instructorId = dtgInstructors.Rows[e.RowIndex].Cells[0].Value?.ToString();

            if (e.ColumnIndex == editColumnIndex)
            {
                // Open your edit form or user control here
                Form parentform = this.FindForm();
                OverlayForm overlay = new OverlayForm(parentform);
                overlay.Show();
                add_instructor AddInstructor = new add_instructor(instructorId);
                AddInstructor.ShowDialog();
                overlay.Dispose();
                LoadInstructors(); 
            }
            else if (e.ColumnIndex == deleteColumnIndex)
            {
                var confirm = MessageBox.Show("Are you sure you want to delete this instructor?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    using (MySqlConnection conn = Database.GetConnection())
                    {
                        try
                        {
                            conn.Open();
                            string query = "DELETE FROM instructor_accounts WHERE instructors_id = @id";
                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@id", instructorId);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Instructor deleted successfully!");
                            LoadInstructors();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error deleting instructor: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Form parentform = this.FindForm();
            OverlayForm overlay = new OverlayForm(parentform);
            overlay.Show();
            add_instructor AddInstructor = new add_instructor();
            AddInstructor.ShowDialog(null);
            overlay.Dispose();
            LoadInstructors(); 
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string filterText = tbSearch.Text.ToLower();

            foreach (DataGridViewRow row in dtgInstructors.Rows)
            {
                if (row.IsNewRow) continue;

                string InstructorID = row.Cells[0].Value?.ToString().ToLower();
                string fullName = row.Cells[2].Value?.ToString().ToLower();

                bool visible = InstructorID.Contains(filterText) || fullName.Contains(filterText);
                row.Visible = visible;
            }
        }

        private void dtgInstructors_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgInstructors.Columns[e.ColumnIndex].Name == "Subjects" && e.RowIndex >= 0)
            {
                string fullSubjects = dtgInstructors.Rows[e.RowIndex].Cells["Subjects"].Value.ToString();
                string instructorName = dtgInstructors.Rows[e.RowIndex].Cells[2].Value.ToString(); // Full name
                SubjectsModal modal = new SubjectsModal(fullSubjects, instructorName);
                modal.ShowDialog();

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
    }
}