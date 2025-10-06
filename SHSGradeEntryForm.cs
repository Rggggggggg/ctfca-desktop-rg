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
    public partial class SHSGradeEntryForm : Form
    {
        private string studentID;
        private string subjectID;
        private string subjectName;
        private string schoolYear;
        private string semester;

        public SHSGradeEntryForm(string studentID, string subjectID, string subjectName, string schoolYear, string semester)
        {
            InitializeComponent();
            this.studentID = studentID;
            this.subjectID = subjectID;
            this.subjectName = subjectName;
            this.schoolYear = schoolYear;
            this.semester = semester;

            // DEBUG: Show what student ID was received
            MessageBox.Show($"SHSGradeEntryForm Constructor:\nStudent ID: {studentID}\nSubject ID: {subjectID}\nSubject: {subjectName}\nSemester: {semester}",
                "Debug - Constructor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SHSGradeEntryForm_Load(object sender, EventArgs e)
        {
            tbFirst.Focus();
            tbSubject.Text = subjectName;
            tbSemester.Text = semester;
            tbFirst.TextChanged += (s, ea) => CalculateAverage();
            tbSecond.TextChanged += (s, ea) => CalculateAverage();
            LoadExistingGrades();
        }

        private void CalculateAverage()
        {
            decimal total = 0;
            int count = 0;

            if (decimal.TryParse(tbFirst.Text, out decimal first))
            {
                total += first;
                count++;
            }
            if (decimal.TryParse(tbSecond.Text, out decimal second))
            {
                total += second;
                count++;
            }

            tbAverage.Text = count > 0 ? (total / count).ToString("0.##") : "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateGrades())
                {
                    MessageBox.Show("Please enter valid grades between 0 and 100.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string first = string.IsNullOrWhiteSpace(tbFirst.Text) ? "0" : tbFirst.Text;
                string second = string.IsNullOrWhiteSpace(tbSecond.Text) ? "0" : tbSecond.Text;
                string average = string.IsNullOrWhiteSpace(tbAverage.Text) ? "0" : tbAverage.Text;

                string remarks = "";
                if (decimal.TryParse(average, out decimal avgGrade))
                {
                    remarks = avgGrade >= 75 ? "Passed" : "Failed";
                }

                // DEBUG: Show what will be saved
                MessageBox.Show($"About to save grades:\nStudent ID: {studentID}\nSubject ID: {subjectID}\nSchool Year: {schoolYear}\nSemester: {semester}\nFirst: {first}\nSecond: {second}\nAverage: {average}",
                    "Debug - Before Save", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SaveGrades(first, second, average, remarks);

                MessageBox.Show("Grades saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving grades: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateGrades()
        {
            string[] grades = { tbFirst.Text, tbSecond.Text };

            foreach (string grade in grades)
            {
                if (!string.IsNullOrWhiteSpace(grade))
                {
                    if (!decimal.TryParse(grade, out decimal gradeValue) || gradeValue < 0 || gradeValue > 100)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void LoadExistingGrades()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT quarter1 AS first, quarter2 AS second, final_grade 
                                 FROM grades 
                                 WHERE student_id = @studentID 
                                 AND subject_id = @subjectID 
                                 AND school_year = @schoolYear 
                                 AND semester = @semester";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@studentID", studentID);
                        cmd.Parameters.AddWithValue("@subjectID", subjectID);
                        cmd.Parameters.AddWithValue("@schoolYear", schoolYear);
                        cmd.Parameters.AddWithValue("@semester", semester);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tbFirst.Text = reader["first"].ToString();
                                tbSecond.Text = reader["second"].ToString();
                                tbAverage.Text = reader["final_grade"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading existing grades: " + ex.Message);
                }
            }
        }

        private void SaveGrades(string first, string second, string average, string remarks)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string checkQuery = @"SELECT grade_id FROM grades 
                                      WHERE student_id = @studentID 
                                      AND subject_id = @subjectID 
                                      AND school_year = @schoolYear 
                                      AND semester = @semester";

                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@studentID", studentID);
                        checkCmd.Parameters.AddWithValue("@subjectID", subjectID);
                        checkCmd.Parameters.AddWithValue("@schoolYear", schoolYear);
                        checkCmd.Parameters.AddWithValue("@semester", semester);

                        object result = checkCmd.ExecuteScalar();

                        if (result != null)
                        {
                            // Update
                            string updateQuery = @"UPDATE grades SET 
                                               quarter1 = @first, 
                                               quarter2 = @second, 
                                               final_grade = @average, 
                                               remarks = @remarks,
                                               updated_at = CURRENT_TIMESTAMP
                                               WHERE grade_id = @gradeID";

                            using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@first", first);
                                updateCmd.Parameters.AddWithValue("@second", second);
                                updateCmd.Parameters.AddWithValue("@average", average);
                                updateCmd.Parameters.AddWithValue("@remarks", remarks);
                                updateCmd.Parameters.AddWithValue("@gradeID", result);
                                updateCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show($"Updated existing grade record (grade_id: {result})", "Debug - Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // Insert
                            string insertQuery = @"INSERT INTO grades 
                                               (student_id, subject_id, school_year, semester, quarter1, quarter2, final_grade, remarks)
                                               VALUES (@studentID, @subjectID, @schoolYear, @semester, @first, @second, @average, @remarks)";

                            using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@studentID", studentID);
                                insertCmd.Parameters.AddWithValue("@subjectID", subjectID);
                                insertCmd.Parameters.AddWithValue("@schoolYear", schoolYear);
                                insertCmd.Parameters.AddWithValue("@semester", semester);
                                insertCmd.Parameters.AddWithValue("@first", first);
                                insertCmd.Parameters.AddWithValue("@second", second);
                                insertCmd.Parameters.AddWithValue("@average", average);
                                insertCmd.Parameters.AddWithValue("@remarks", remarks);
                                int rowsAffected = insertCmd.ExecuteNonQuery();

                                MessageBox.Show($"Inserted new grade record\nStudent ID: {studentID}\nRows affected: {rowsAffected}", "Debug - Insert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message + "\n\nStack Trace: " + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}