using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class EnrollmentControl : UserControl
    {
        public EnrollmentControl()
        {
            InitializeComponent();
        }

        private void EnrollmentControl_Load(object sender, EventArgs e)
        {
            LoadEnrollmentStatus();
        }

        private void LoadEnrollmentStatus()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT enrollment_open, enrollment_message, 
                                   last_updated FROM enrollment_settings WHERE id = 1";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        bool isOpen = Convert.ToBoolean(reader["enrollment_open"]);

                        // Update status label
                        if (isOpen)
                        {
                            lblStatusValue.Text = "OPEN";
                            lblStatusValue.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            lblStatusValue.Text = "CLOSED";
                            lblStatusValue.ForeColor = System.Drawing.Color.Red;
                        }

                        // Update message
                        txtEnrollmentMessage.Text = reader["enrollment_message"].ToString();

                        // Update last updated time
                        if (!reader.IsDBNull(reader.GetOrdinal("last_updated")))
                        {
                            DateTime lastUpdated = Convert.ToDateTime(reader["last_updated"]);
                            lblLastUpdatedValue.Text = lastUpdated.ToString("MMM dd, yyyy hh:mm tt");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading enrollment status: " + ex.Message,
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnOpenEnrollment_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to OPEN enrollment? Students will be able to enroll.",
                "Confirm Open Enrollment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                UpdateEnrollmentStatus(true);
            }
        }

        private void btnCloseEnrollment_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to CLOSE enrollment? Students will NOT be able to enroll.",
                "Confirm Close Enrollment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                UpdateEnrollmentStatus(false);
            }
        }

        private void UpdateEnrollmentStatus(bool isOpen)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE enrollment_settings 
                                   SET enrollment_open = @status,
                                       updated_by = @user,
                                       last_updated = NOW()
                                   WHERE id = 1";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@status", isOpen);
                    cmd.Parameters.AddWithValue("@user", "Admin"); // Replace with actual admin username

                    cmd.ExecuteNonQuery();

                    string statusText = isOpen ? "OPENED" : "CLOSED";
                    MessageBox.Show(
                        $"Enrollment has been {statusText} successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    LoadEnrollmentStatus(); // Refresh display
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating enrollment status: " + ex.Message,
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdateMessage_Click(object sender, EventArgs e)
        {
            string message = txtEnrollmentMessage.Text.Trim();

            if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("Please enter a message.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE enrollment_settings 
                                   SET enrollment_message = @message,
                                       last_updated = NOW()
                                   WHERE id = 1";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@message", message);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Enrollment message updated successfully!",
                                  "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating message: " + ex.Message,
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}