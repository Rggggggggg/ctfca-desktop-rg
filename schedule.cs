using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class schedule : UserControl
    {
        public schedule()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Form parentform = this.FindForm();
            OverlayForm overlay = new OverlayForm(parentform);
            overlay.Show();
            add_Schedule AddInstructor = new add_Schedule(null);
            AddInstructor.ShowDialog();
            overlay.Dispose();
            LoadSchedules();
        }

        private void schedule_Load(object sender, EventArgs e)
        {
            LoadSchedules(); // Load schedules when the control is loaded
        }

        public void LoadSchedules()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT 
                                id,
                                instructor,
                                grade_level,
                                section,
                                school_year,
                                semester,
                                subject,
                                days,
                                start_time,
                                end_time,
                                room
                            FROM instructor_schedule";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    dtgSchedule.Rows.Clear(); // Clear previous data

                    while (reader.Read())
                    {
                        // Format time range
                        string timeRange = $"{reader["start_time"]} - {reader["end_time"]}";

                        // Add row to DataGridView
                        dtgSchedule.Rows.Add(
                            reader["id"].ToString(),
                            reader["instructor"].ToString(),
                            reader["grade_level"].ToString(),
                            reader["section"].ToString(),
                            reader["school_year"].ToString(),
                            reader["semester"].ToString(),
                            reader["subject"].ToString(),
                            reader["days"].ToString(),
                            timeRange,
                            reader["room"].ToString()
                        );
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading schedules: " + ex.Message, "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dtgSchedule_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            int editColumnIndex = 10; // Example: Edit button column index
            int deleteColumnIndex = 11; // Example: Delete button column index

            
            string scheduleID = dtgSchedule.Rows[e.RowIndex].Cells[0].Value?.ToString();

            if (e.ColumnIndex == editColumnIndex)
            {
                Form parentform = this.FindForm();
                OverlayForm overlay = new OverlayForm(parentform);
                overlay.Show();
                add_Schedule AddInstructor = new add_Schedule(scheduleID);
                AddInstructor.ShowDialog();
                overlay.Dispose();
                LoadSchedules(); 
            }
            else if (e.ColumnIndex == deleteColumnIndex)
            {
                var confirm = MessageBox.Show("Are you sure you want to delete this Schedule?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    using (MySqlConnection conn = Database.GetConnection())
                    {
                        try
                        {
                            conn.Open();
                            string query = "DELETE FROM instructor_schedule WHERE id = @id";
                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@id", scheduleID);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Instructor Schedule deleted successfully!");
                            LoadSchedules(); // Refresh table
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error deleting instructor schedule: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string filtertext = tbSearch.Text.Trim().ToLower();
            {
                foreach(DataGridViewRow row in dtgSchedule.Rows)
                {
                    string name = row.Cells[1].Value?.ToString().ToLower();
                    string gradelevel = row.Cells[2].Value?.ToString().ToLower();
                    string subject = row.Cells[6].Value?.ToString().ToLower();
                    string days = row.Cells[7].Value?.ToString().ToLower();
                    string time = row.Cells[8].Value?.ToString().ToLower();
                    string room = row.Cells[9].Value?.ToString().ToLower();
                    row.Visible = name.Contains(filtertext) || gradelevel.Contains(filtertext) || subject.Contains(filtertext)
                        || days.Contains(filtertext) || time.Contains(filtertext) || room.Contains(filtertext);
                }    
            }
        }
    }
}
