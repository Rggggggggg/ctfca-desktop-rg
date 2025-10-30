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
        private string originalGradeLevel = null;
        private string originalSubjectName = null;
        private bool isEditMode = false;

        // Constructor for ADD mode
        public add_Subject()
        {
            InitializeComponent();
            isEditMode = false;
        }

        // Constructor for EDIT mode
        public add_Subject(string gradeLevel, string subjectName)
        {
            InitializeComponent();
            isEditMode = true;
            originalGradeLevel = gradeLevel;
            originalSubjectName = subjectName;
            LoadSubjectData(gradeLevel, subjectName);
        }

        private void LoadSubjectData(string gradeLevel, string subjectName)
        {
            tbSubjects.Text = subjectName;
            cbGradeLevel.SelectedItem = gradeLevel;
        }

        private void add_Subject_Load(object sender, EventArgs e)
        {
            // Update UI based on mode
            if (isEditMode)
            {
                lblTitle.Text = "Edit Subject";
                btnSave.Text = "Save Changes";
            }
            else
            {
                this.Text = "Add New Subject";
                btnSave.Text = "Save";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate inputs before attempting to save
            if (!ValidateInputs())
            {
                return;
            }

            if (isEditMode)
            {
                UpdateSubject();
            }
            else
            {
                AddSubject();
            }
        }

        private void AddSubject()
        {
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

                    string query = @"INSERT INTO subjects (grade_level, subject_name) 
                                    VALUES (@grade_level, @subject_name)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@subject_name", tbSubjects.Text.Trim());
                        cmd.Parameters.AddWithValue("@grade_level", cbGradeLevel.SelectedItem.ToString());

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Subject added successfully!", "Success",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
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

        private void UpdateSubject()
        {
            using (var conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Check if the new subject name already exists (only if name/grade changed)
                    string newGrade = cbGradeLevel.SelectedItem.ToString();
                    string newSubject = tbSubjects.Text.Trim();

                    // Only check for duplicates if the values changed
                    if (newGrade != originalGradeLevel || newSubject != originalSubjectName)
                    {
                        if (SubjectExists(conn, newSubject, newGrade))
                        {
                            MessageBox.Show("This subject already exists for the selected grade level.",
                                          "Duplicate Subject", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string query = @"UPDATE subjects 
                                    SET grade_level = @new_grade, subject_name = @new_subject 
                                    WHERE grade_level = @old_grade AND subject_name = @old_subject";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@old_grade", originalGradeLevel);
                        cmd.Parameters.AddWithValue("@old_subject", originalSubjectName);
                        cmd.Parameters.AddWithValue("@new_grade", newGrade);
                        cmd.Parameters.AddWithValue("@new_subject", newSubject);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Subject updated successfully!", "Success",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update subject. The subject may have been deleted.",
                                          "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (tbSubjects.Text.Trim().Length > 100)
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
            string checkQuery = @"SELECT COUNT(*) FROM subjects 
                                 WHERE LOWER(subject_name) = LOWER(@subject_name) 
                                 AND grade_level = @grade_level";
            using (var cmd = new MySqlCommand(checkQuery, conn))
            {
                cmd.Parameters.AddWithValue("@subject_name", subjectName);
                cmd.Parameters.AddWithValue("@grade_level", gradeLevel);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
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