using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class Gradingform : UserControl
    {
        private string studentID;
        private string gradeLevel;
        private string strand;
        private string currentSchoolYear = "2025-2026";

        public Gradingform(string studentID, string name, string gradeLevel, string strand)
        {
            InitializeComponent();
            this.studentID = studentID;
            lblname.Text = name;
            this.gradeLevel = gradeLevel;
            this.strand = strand;
            this.KeyDown += GradeEntryForm_KeyDown;
            this.Focus();
        }

        private void GradeEntryForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
        }

        private void Gradingform_Load(object sender, EventArgs e)
        {
            cbSemester.SelectedIndexChanged += cbSemester_SelectedIndexChanged;

            // Convert studentID to actual database ID first
            string actualStudentId = GetActualStudentId(studentID);
            if (string.IsNullOrEmpty(actualStudentId))
            {
                MessageBox.Show($"Student not found in database.\nSearched for: {studentID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // IMPORTANT: Update the instance variable
            this.studentID = actualStudentId;

            // Debug message to verify correct ID
            

            if (IsSHS())
            {
                dtgGrades.Visible = false;
                dtgSHSGrades.Visible = true;
                cbSemester.Visible = true;

                string fullGradeLevel = BuildGradeLevel();

                // Check if student passed all 1st semester
                if (PassedAllFirstSemester(actualStudentId, fullGradeLevel))
                {
                    cbSemester.SelectedIndex = 1;
                    LoadSHSSubjectsWithGrades("2nd Semester");
                }
                else
                {
                    cbSemester.SelectedIndex = 0;
                    LoadSHSSubjectsWithGrades("1st Semester");
                }
            }
            else
            {
                dtgGrades.Visible = true;
                dtgSHSGrades.Visible = false;
                cbSemester.Visible = false;
                LoadSubjectsWithGrades();
            }
        }

        private string GetActualStudentId(string inputStudentId)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    // This query only READS from students table, it doesn't modify anything
                    string getStudentIdQuery = @"
                        SELECT id FROM students 
                        WHERE id = CAST(@studentID AS UNSIGNED)
                           OR student_number = @studentID
                           OR lrn = @studentID
                           OR application_no = @studentID
                        ORDER BY id DESC
                        LIMIT 1";

                    using (MySqlCommand studentCmd = new MySqlCommand(getStudentIdQuery, conn))
                    {
                        studentCmd.Parameters.AddWithValue("@studentID", inputStudentId);
                        object result = studentCmd.ExecuteScalar();
                        if (result != null)
                            return result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving student ID: " + ex.Message);
                }
            }
            return inputStudentId;
        }

        private string BuildGradeLevel()
        {
            // For SHS students, we need to include the strand
            // Database format: "Grade 11 STEM", "Grade 12 ABM", etc.
            if (IsSHS())
            {
                // Clean up the grade level - remove any existing "Grade" prefix first
                string gradePart = gradeLevel.Trim();
                gradePart = gradePart.Replace("Grade", "").Replace("grade", "").Trim();

                // Remove any "G" prefix that might exist (e.g., "G11" -> "11")
                if (gradePart.StartsWith("G", StringComparison.OrdinalIgnoreCase))
                {
                    gradePart = gradePart.Substring(1).Trim();
                }

                // Now add "Grade" prefix properly
                gradePart = "Grade " + gradePart;

                // Add strand if provided and not already in the string
                if (!string.IsNullOrEmpty(strand) && !gradePart.Contains(strand))
                {
                    return $"{gradePart} {strand}";
                }
                return gradePart;
            }
            else
            {
                // For non-SHS, just return the grade level as is
                return gradeLevel;
            }
        }

        private bool IsSHS()
        {
            string gradeLower = gradeLevel.ToLower();
            return gradeLower.Contains("11") || gradeLower.Contains("12");
        }

        private void LoadSubjectsWithGrades()
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = @"SELECT 
                                s.id as subject_id,
                                s.subject_name,
                                COALESCE(g.quarter1, '0') as quarter1,
                                COALESCE(g.quarter2, '0') as quarter2,
                                COALESCE(g.quarter3, '0') as quarter3,
                                COALESCE(g.quarter4, '0') as quarter4,
                                COALESCE(g.final_grade, '0') as final_grade,
                                COALESCE(g.remarks, '') as remarks
                            FROM subjects s
                            LEFT JOIN grades g ON s.id = g.subject_id 
                                AND g.student_id = @studentId
                                AND g.school_year = @schoolYear
                            WHERE s.grade_level = @gradeLevel 
                            ORDER BY s.subject_name";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@studentId", studentID);
                        cmd.Parameters.AddWithValue("@gradeLevel", gradeLevel);
                        cmd.Parameters.AddWithValue("@schoolYear", currentSchoolYear);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dtgGrades.Rows.Clear();

                            while (reader.Read())
                            {
                                double finalGrade = Convert.ToDouble(reader["final_grade"]);
                                string remarks = reader["remarks"].ToString();
                                if (string.IsNullOrEmpty(remarks))
                                {
                                    if (finalGrade >= 75)
                                        remarks = "Passed";
                                    else if (finalGrade > 0)
                                        remarks = "Failed";
                                }

                                dtgGrades.Rows.Add(
                                    reader["subject_id"].ToString(),
                                    reader["subject_name"].ToString(),
                                    reader["quarter1"].ToString(),
                                    reader["quarter2"].ToString(),
                                    reader["quarter3"].ToString(),
                                    reader["quarter4"].ToString(),
                                    reader["final_grade"].ToString(),
                                    remarks,
                                    "Add grades"
                                );
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading subjects and grades: " + ex.Message);
                }
            }
        }

        private void LoadSHSSubjectsWithGrades(string semester)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string fullGradeLevel = BuildGradeLevel();

                    // Debug: Check what subjects exist
                    string debugQuery = @"SELECT grade_level, semester, COUNT(*) as cnt 
                                         FROM subjects 
                                         WHERE grade_level LIKE @searchPattern
                                         GROUP BY grade_level, semester";

                    using (MySqlCommand debugCmd = new MySqlCommand(debugQuery, conn))
                    {
                        debugCmd.Parameters.AddWithValue("@searchPattern", $"%{fullGradeLevel}%");
                        using (MySqlDataReader debugReader = debugCmd.ExecuteReader())
                        {
                            string debugInfo = "Found subjects:\n";
                            while (debugReader.Read())
                            {
                                debugInfo += $"Grade: {debugReader["grade_level"]}, Semester: {debugReader["semester"]}, Count: {debugReader["cnt"]}\n";
                            }
                            // Uncomment for debugging
                            // MessageBox.Show(debugInfo);
                        }
                    }

                    string query = @"SELECT 
                                s.id as subject_id, 
                                s.subject_name,
                                s.semester,
                                s.grade_level,
                                COALESCE(g.quarter1, '0') as first,
                                COALESCE(g.quarter2, '0') as second,
                                COALESCE(g.final_grade, '0') as average,
                                COALESCE(g.remarks, '') as remarks
                             FROM subjects s
                             LEFT JOIN grades g ON s.id = g.subject_id 
                                                AND g.student_id = @studentId  
                                                AND g.school_year = @schoolYear
                                                AND g.semester = @semester
                             WHERE s.grade_level = @fullGradeLevel
                               AND s.semester = @semester
                             ORDER BY s.subject_name";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@studentId", studentID);
                        cmd.Parameters.AddWithValue("@fullGradeLevel", fullGradeLevel);
                        cmd.Parameters.AddWithValue("@schoolYear", currentSchoolYear);
                        cmd.Parameters.AddWithValue("@semester", semester);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dtgSHSGrades.Rows.Clear();
                            int rowCount = 0;

                            while (reader.Read())
                            {
                                rowCount++;
                                double avg = 0;
                                if (double.TryParse(reader["average"].ToString(), out avg) == false)
                                {
                                    avg = 0;
                                }

                                string remarks = reader["remarks"].ToString();
                                if (string.IsNullOrEmpty(remarks))
                                {
                                    if (avg >= 75) remarks = "Passed";
                                    else if (avg > 0) remarks = "Failed";
                                }

                                dtgSHSGrades.Rows.Add(
                                    reader["subject_id"].ToString(),
                                    reader["subject_name"].ToString(),
                                    reader["semester"].ToString(),
                                    reader["first"].ToString(),
                                    reader["second"].ToString(),
                                    avg.ToString("0.##"),
                                    remarks,
                                    "Add grades"
                                );
                            }

                            if (rowCount == 0)
                            {
                                MessageBox.Show($"No subjects found for:\nGrade Level: '{fullGradeLevel}'\nSemester: {semester}\n\nPlease verify:\n1. Subjects exist in database with exact grade level: '{fullGradeLevel}'\n2. Semester matches: '{semester}'\n\nCurrent parameters - Grade: '{gradeLevel}', Strand: '{strand}'",
                                    "No Subjects Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading SHS subjects and grades: " + ex.Message + "\n\nStack Trace: " + ex.StackTrace,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OpenGradeEntryForm(string subjectID, string subjectName, string studentID, string currentSchoolYear)
        {
            Form parentForm = this.FindForm();
            OverlayForm overlay = new OverlayForm(parentForm);
            overlay.Show();
            GradeEntryForm editForm = new GradeEntryForm(studentID, subjectID, subjectName, currentSchoolYear);
            editForm.ShowDialog();
            LoadSubjectsWithGrades();
            overlay.Dispose();
        }

        private void OpenSHSGradeEntryForm(string subjectID, string subjectName, string studentID, string currentSchoolYear, string semester)
        {
            // DEBUG: Verify parameters before creating form
            MessageBox.Show($"OpenSHSGradeEntryForm called with:\nSubject ID: {subjectID}\nSubject Name: {subjectName}\nStudent ID: {studentID}\nSchool Year: {currentSchoolYear}\nSemester: {semester}",
                "Debug - OpenSHSGradeEntryForm", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            Form parentForm = this.FindForm();
            OverlayForm overlay = new OverlayForm(parentForm);
            overlay.Show();

            // Pass parameters in correct order: studentID, subjectID, subjectName, schoolYear, semester
            SHSGradeEntryForm editForm = new SHSGradeEntryForm(studentID, subjectID, subjectName, currentSchoolYear, semester);
            editForm.ShowDialog();

            // Reload the current semester's grades
            string selectedSemester = cbSemester.SelectedItem?.ToString() ?? "1st Semester";
            LoadSHSSubjectsWithGrades(selectedSemester);

            overlay.Dispose();
        }

        private void dtgGrades_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string columnName = dtgGrades.Columns[e.ColumnIndex].Name;

            if (columnName == "Action")
            {
                string subjectID = dtgGrades.Rows[e.RowIndex].Cells["ID"].Value?.ToString();
                string subjectName = dtgGrades.Rows[e.RowIndex].Cells["Subject"].Value?.ToString();

                // Use this.studentID (the instance variable)
                OpenGradeEntryForm(subjectID, subjectName, this.studentID, currentSchoolYear);
            }
        }

        private void dtgSHSGrades_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string columnName = dtgSHSGrades.Columns[e.ColumnIndex].Name;

            if (columnName == "ActionSHS")
            {
                string subjectID = dtgSHSGrades.Rows[e.RowIndex].Cells["IDSHS"].Value?.ToString();
                string subjectName = dtgSHSGrades.Rows[e.RowIndex].Cells["SubjectSHS"].Value?.ToString();
                string semester = dtgSHSGrades.Rows[e.RowIndex].Cells["Semester"].Value?.ToString();

                // Debug: Verify we're using the correct student ID
                MessageBox.Show($"Opening grade entry for:\nStudent ID: {this.studentID}\nSubject: {subjectName}\nSemester: {semester}",
                    "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Use this.studentID (the instance variable, not the parameter)
                OpenSHSGradeEntryForm(subjectID, subjectName, this.studentID, currentSchoolYear, semester);
            }
        }

        private bool PassedAllFirstSemester(string actualStudentId, string fullGradeLevel)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT COUNT(*) 
                             FROM subjects s
                             LEFT JOIN grades g ON s.id = g.subject_id
                               AND g.student_id = @studentID 
                               AND g.school_year = @schoolYear
                               AND g.semester = '1st Semester'
                             WHERE s.grade_level = @fullGradeLevel
                               AND s.semester = '1st Semester'
                               AND (CAST(g.final_grade AS DECIMAL(5,2)) < 75 OR g.final_grade IS NULL)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@studentID", actualStudentId);
                        cmd.Parameters.AddWithValue("@fullGradeLevel", fullGradeLevel);
                        cmd.Parameters.AddWithValue("@schoolYear", currentSchoolYear);

                        int failedCount = Convert.ToInt32(cmd.ExecuteScalar());
                        return failedCount == 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking first semester status: " + ex.Message);
                    return false;
                }
            }
        }

        private void cbSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsSHS())
            {
                string selectedSemester = cbSemester.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(selectedSemester))
                {
                    LoadSHSSubjectsWithGrades(selectedSemester);
                }
            }
        }
    }
}