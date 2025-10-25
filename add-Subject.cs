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
    public partial class add_Subject : Form
    {
        public add_Subject()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate inputs before attempting to save
            if (!ValidateInputs())
            {
                return;
            }

            using (var conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Check if subject already exists for this grade level
                    if (SubjectExists(conn, tbSubjects.Text.Trim(), cbGradeLevel.SelectedItem.ToString()))
                    {
                        MessageBox.Show("This subject already exists for the selected grade level.",
                                      "Duplicate Subject", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string query = @"INSERT INTO subjects (grade_level, subject_name) VALUES (@grade_level, @subject_name)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@subject_name", tbSubjects.Text.Trim());
                        cmd.Parameters.AddWithValue("@grade_level", cbGradeLevel.SelectedItem.ToString());

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Subject added successfully!", "Success",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close(); // Close the form after saving
                        }
                        else
                        {
                            MessageBox.Show("Failed to add subject. Please try again.", "Error",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (MySqlException mysqlEx)
                {
                    MessageBox.Show($"Database error: {mysqlEx.Message}", "Database Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInputs()
        {
            // Check if subject name is empty or whitespace
            if (string.IsNullOrWhiteSpace(tbSubjects.Text))
            {
                MessageBox.Show("Please enter a subject name.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbSubjects.Focus();
                return false;
            }

            // Check if grade level is selected
            if (cbGradeLevel.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a grade level.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbGradeLevel.Focus();
                return false;
            }

            // Check subject name length (adjust max length as needed)
            if (tbSubjects.Text.Trim().Length > 100) // Assuming 100 char limit
            {
                MessageBox.Show("Subject name is too long. Maximum 100 characters allowed.",
                              "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbSubjects.Focus();
                return false;
            }

            return true;
        }

        private bool SubjectExists(MySqlConnection conn, string subjectName, string gradeLevel)
        {
            string checkQuery = @"SELECT COUNT(*) FROM subjects WHERE LOWER(subject_name) = LOWER(@subject_name) AND grade_level = @grade_level";
            using (var cmd = new MySqlCommand(checkQuery, conn))
            {
                cmd.Parameters.AddWithValue("@subject_name", subjectName);
                cmd.Parameters.AddWithValue("@grade_level", gradeLevel);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        private void add_Subject_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Show confirmation dialog before canceling
            DialogResult result = MessageBox.Show(
                "Are you sure you want to cancel? Any unsaved changes will be lost.",
                "Cancel Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                this.Close();

            }
        }
    }
}
