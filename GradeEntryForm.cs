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
    public partial class GradeEntryForm : Form
    {
        private readonly string studentID;
        private readonly string subjectID;
        private readonly string subjectName;
        private readonly string schoolYear;
        public GradeEntryForm(string studentID, string subjectID, string subjectName, string schoolYear)
        {
            InitializeComponent();
            this.studentID = studentID;
            this.subjectID = subjectID;
            this.subjectName = subjectName;
            this.schoolYear = schoolYear;

        }

        private void GradeEntryForm_Load(object sender, EventArgs e)
        {
            tbFirst.Focus();
            tbSubject.Text = subjectName;
            tbFirst.TextChanged += (s, ea) => CalculateAverage();
            tbSecond.TextChanged += (s, ea) => CalculateAverage();
            tbThird.TextChanged += (s, ea) => CalculateAverage();
            tbFourth.TextChanged += (s, ea) => CalculateAverage();
            LoadExistingGrades();
        }

        private void CalculateAverage()
        {
            decimal total = 0;
            int count = 0;

            // Check each quarter textbox
            if (decimal.TryParse(tbFirst.Text, out decimal q1))
            {
                total += q1;
                count++;
            }
            if (decimal.TryParse(tbSecond.Text, out decimal q2))
            {
                total += q2;
                count++;
            }
            if (decimal.TryParse(tbThird.Text, out decimal q3))
            {
                total += q3;
                count++;
            }
            if (decimal.TryParse(tbFourth.Text, out decimal q4))
            {
                total += q4;
                count++;
            }

            if (count > 0)
            {
                decimal average = total / count;
                tbAverage.Text = average.ToString("0.##");
            }
            else
            {
                tbAverage.Text = "";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate grades are within acceptable range (0-100)
                if (!ValidateGrades())
                {
                    MessageBox.Show("Please enter valid grades between 0 and 100.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get values from textboxes, default to "0" if empty
                string quarter1 = string.IsNullOrWhiteSpace(tbFirst.Text) ? "0" : tbFirst.Text;
                string quarter2 = string.IsNullOrWhiteSpace(tbSecond.Text) ? "0" : tbSecond.Text;
                string quarter3 = string.IsNullOrWhiteSpace(tbThird.Text) ? "0" : tbThird.Text;
                string quarter4 = string.IsNullOrWhiteSpace(tbFourth.Text) ? "0" : tbFourth.Text;
                string finalGrade = string.IsNullOrWhiteSpace(tbAverage.Text) ? "0" : tbAverage.Text;

                // Calculate remarks based on final grade
                string remarks = "";
                if (decimal.TryParse(finalGrade, out decimal avgGrade))
                {
                    if (avgGrade >= 75)
                        remarks = "Passed";
                    else
                        remarks = "Failed";
                }
                // Determine if semester applies based on grade level
                string semester = DetermineApplicableSemester();

                SaveGrades(quarter1, quarter2, quarter3, quarter4, finalGrade, remarks, semester);

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
            string[] grades = { tbFirst.Text, tbSecond.Text, tbThird.Text, tbFourth.Text };

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
                    string query = @"SELECT quarter1, quarter2, quarter3, quarter4, final_grade 
                           FROM grades 
                           WHERE student_id = @studentID AND subject_id = @subjectID AND school_year = @schoolYear";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@studentID", studentID);
                        cmd.Parameters.AddWithValue("@subjectID", subjectID);
                        cmd.Parameters.AddWithValue("@schoolYear", schoolYear);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tbFirst.Text = reader["quarter1"].ToString();
                                tbSecond.Text = reader["quarter2"].ToString();
                                tbThird.Text = reader["quarter3"].ToString();
                                tbFourth.Text = reader["quarter4"].ToString();
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
        private string DetermineApplicableSemester()
        {
            // You can get the grade level from your student data or pass it as a parameter
            // For now, let's check if it's Senior High School (grades 11-12) which typically use semesters

            // This is a simple approach - you might want to get the actual grade level
            // and determine semester applicability based on your school's system

            // For nursery to grade 10: no semester system (return null or empty)
            // For grades 11-12 (SHS): use semester system

            return ""; 
        }
        private void SaveGrades(string q1, string q2, string q3, string q4, string finalGrade, string remarks, string semester)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    string checkQuery = @"SELECT grade_id FROM grades 
                                 WHERE student_id = @studentID 
                                 AND subject_id = @subjectID 
                                 AND school_year = @schoolYear";

                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@studentID", studentID);
                        checkCmd.Parameters.AddWithValue("@subjectID", subjectID);
                        checkCmd.Parameters.AddWithValue("@schoolYear", schoolYear);

                        object result = checkCmd.ExecuteScalar();

                        if (result != null)
                        {
                            // Update existing record
                            string updateQuery = @"UPDATE grades SET 
                                          quarter1 = @q1, 
                                          quarter2 = @q2, 
                                          quarter3 = @q3, 
                                          quarter4 = @q4,
                                          final_grade = @finalGrade, 
                                          remarks = @remarks, 
                                          semester = @semester,
                                          updated_at = CURRENT_TIMESTAMP
                                          WHERE grade_id = @gradeID";

                            using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@q1", q1);
                                updateCmd.Parameters.AddWithValue("@q2", q2);
                                updateCmd.Parameters.AddWithValue("@q3", q3);
                                updateCmd.Parameters.AddWithValue("@q4", q4);
                                updateCmd.Parameters.AddWithValue("@finalGrade", finalGrade);
                                updateCmd.Parameters.AddWithValue("@remarks", remarks);
                                updateCmd.Parameters.AddWithValue("@semester", semester ?? (object)DBNull.Value);
                                updateCmd.Parameters.AddWithValue("@gradeID", result);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Insert new record
                            string insertQuery = @"INSERT INTO grades 
                                          (student_id, subject_id, school_year, quarter1, quarter2, quarter3, quarter4, final_grade, remarks, semester)
                                          VALUES (@studentID, @subjectID, @schoolYear, @q1, @q2, @q3, @q4, @finalGrade, @remarks, @semester)";

                            using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@studentID", studentID);
                                insertCmd.Parameters.AddWithValue("@subjectID", subjectID);
                                insertCmd.Parameters.AddWithValue("@schoolYear", schoolYear);
                                insertCmd.Parameters.AddWithValue("@q1", q1);
                                insertCmd.Parameters.AddWithValue("@q2", q2);
                                insertCmd.Parameters.AddWithValue("@q3", q3);
                                insertCmd.Parameters.AddWithValue("@q4", q4);
                                insertCmd.Parameters.AddWithValue("@finalGrade", finalGrade);
                                insertCmd.Parameters.AddWithValue("@remarks", remarks);
                                insertCmd.Parameters.AddWithValue("@semester", semester ?? (object)DBNull.Value);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lblAdmin_Click(object sender, EventArgs e) 
        {

        }
    }
}
