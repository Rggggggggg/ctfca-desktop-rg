using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CFCA_ADMIN
{
    public partial class Enrollees : UserControl
    {
        public Enrollees()
        {
            InitializeComponent();
        }

        private void LoadStudentData(string statusFilter = "Pending")
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT student_number, level_applied, surname, 
                             first_name, middle_name, gender, age, contact, 
                             submitted_at, email, enrollment_status
                             FROM basic_ed_enrollment";

                    if (statusFilter != "All")
                    {
                        query += $" WHERE enrollment_status = '{statusFilter}'";
                    }

                    query += " ORDER BY submitted_at DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtgEnrollees.Rows.Clear();

                        while (reader.Read())
                        {
                            string fullName = $"{reader["surname"]}, {reader["first_name"]} {reader["middle_name"]}";
                            string status = reader["enrollment_status"].ToString();

                            int rowIndex = dtgEnrollees.Rows.Add(
                                reader["student_number"].ToString(),
                                fullName,
                                reader["level_applied"].ToString(),
                                reader["gender"].ToString(),
                                reader["age"].ToString(),
                                reader["contact"].ToString(),
                                Convert.ToDateTime(reader["submitted_at"]).ToString("MM/dd/yyyy"),
                                reader["email"].ToString()
                            );

                            // Color code rows based on status
                            if (status == "Confirmed")
                                dtgEnrollees.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(230, 245, 230);
                            else if (status == "Rejected")
                                dtgEnrollees.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Enrollees_Load(object sender, EventArgs e)
        {
            cbStatusFilter.SelectedIndex = 0;
            LoadStudentData("Pending");

            if (dtgEnrollees.Columns.Contains("colEmail"))
                dtgEnrollees.Columns["colEmail"].Visible = false;

            // ✅ Add "View Info" button column (if not already added)
            if (!dtgEnrollees.Columns.Contains("btnViewInfo"))
            {
                DataGridViewButtonColumn btnViewInfo = new DataGridViewButtonColumn();
                btnViewInfo.HeaderText = "Action";
                btnViewInfo.Name = "btnViewInfo";
                btnViewInfo.Text = "View Info";
                btnViewInfo.UseColumnTextForButtonValue = true;

                // Insert after "Submitted At" (index 6)
                dtgEnrollees.Columns.Insert(7, btnViewInfo);
            }
        }

        private void DtgEnrollees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow row = dtgEnrollees.Rows[e.RowIndex];
            if (row.Cells.Count <= 0) return;

            string studentNumber = row.Cells[0].Value?.ToString();
            string name = row.Cells[1].Value?.ToString();
            string email = row.Cells[7].Value?.ToString();
            string yearLevel = row.Cells[2].Value?.ToString();

            string columnName = dtgEnrollees.Columns[e.ColumnIndex].Name;
            if (string.IsNullOrEmpty(studentNumber)) return;

            if (columnName == "btnConfirm")
            {
                ConfirmEnrollment(studentNumber, name, email, yearLevel);
            }
            else if (columnName == "btnDelete")
            {
                RejectEnrollment(studentNumber, name, email, yearLevel);
            }
            else if (columnName == "btnViewInfo")
            {
                ViewEnrollmentDetails(studentNumber);
            }
        }

        private async void ConfirmEnrollment(string studentNumber, string name, string email, string yearLevel)
        {
            if (string.IsNullOrEmpty(studentNumber))
            {
                MessageBox.Show("Invalid student record selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to approve this student: {name}?",
                "Confirm Approval",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes) return;

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    string transferQuery = @"
                        INSERT INTO students (
                            student_number, level_for_registrar, lrn, level_applied, surname, first_name, middle_name, age, gender, dob,
                            pob, citizenship, religion, address, contact, email, father_name, father_occupation, father_office, father_contact, 
                            father_email, mother_name, mother_occupation, mother_office, mother_contact, mother_email, status, guardian_name, guardian_relation, 
                            guardian_occupation, guardian_contact, guardian_email, has_siblings, prev_ctfcai, prev_grade, prev_sy, prev_school, prev_school_sy, prev_school_addr, 
                            health_conditions, emergency_name, emergency_address, emergency_contact, student_photo, signature_filename, signature_date
                        )
                        SELECT 
                            student_number, level_for_registrar, lrn, level_applied, surname, first_name, middle_name, age, gender, dob,
                            pob, citizenship, religion, address, contact, email, father_name, father_occupation, father_office, father_contact, 
                            father_email, mother_name, mother_occupation, mother_office, mother_contact, mother_email, status, guardian_name, guardian_relation, 
                            guardian_occupation, guardian_contact, guardian_email, has_siblings, prev_ctfcai, prev_grade, prev_sy, prev_school, prev_school_sy, prev_school_addr, 
                            health_conditions, emergency_name, emergency_address, emergency_contact, student_photo, signature_filename, signature_date
                        FROM basic_ed_enrollment 
                        WHERE student_number = @student_number";

                    MySqlCommand transferCmd = new MySqlCommand(transferQuery, conn);
                    transferCmd.Parameters.AddWithValue("@student_number", studentNumber);

                    int rowsAffected = transferCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        UpdateStatus(conn, studentNumber, "Confirmed");
                        bool notificationSent = await NotifyStudent(studentNumber, name, email, "Approved", yearLevel);

                        MessageBox.Show($"Student {name} enrolled successfully" +
                            (notificationSent ? "" : "\nNote: Email notification failed to send"),
                            "Enrollment Confirmed", MessageBoxButtons.OK,
                            notificationSent ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                        LoadStudentData(cbStatusFilter.Text);
                    }
                    else
                    {
                        MessageBox.Show("Student record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void RejectEnrollment(string studentNumber, string name, string email, string yearLevel)
        {
            if (string.IsNullOrEmpty(studentNumber))
            {
                MessageBox.Show("Invalid student record selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show($"Are you sure you want to reject this student: {name}?",
                                                 "Confirm Reject", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string updateQuery = "UPDATE basic_ed_enrollment SET enrollment_status = 'Rejected' WHERE student_number = @student_number";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@student_number", studentNumber);
                    updateCmd.ExecuteNonQuery();

                    bool notificationSent = await NotifyStudent(studentNumber, name, email, "Rejected", yearLevel);

                    MessageBox.Show($"Student {name} rejected successfully" +
                        (notificationSent ? "" : "\nNote: Email notification failed to send"),
                        "Enrollment Rejected", MessageBoxButtons.OK,
                        notificationSent ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                    LoadStudentData(cbStatusFilter.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateStatus(MySqlConnection conn, string studentNumber, string status)
        {
            string updateQuery = "UPDATE basic_ed_enrollment SET enrollment_status = @status WHERE student_number = @student_number";
            MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
            updateCmd.Parameters.AddWithValue("@student_number", studentNumber);
            updateCmd.Parameters.AddWithValue("@status", status);
            updateCmd.ExecuteNonQuery();
        }

        private async Task<bool> NotifyStudent(string studentNumber, string name, string email, string status, string yearLevel)
        {
            using (HttpClient client = new HttpClient())
            {
                var payload = new
                {
                    student_number = studentNumber,
                    name,
                    email,
                    status,
                    year_level = yearLevel,
                    enrollment_type = "Basic Education"
                };

                string json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync("http://localhost/CAPSTONE_PROJ/api/cs/notify-student.php", content);
                    string result = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Email notification failed: " + result);
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending email: " + ex.Message);
                    return false;
                }
            }
        }

        private void ViewEnrollmentDetails(string studentNumber)
        {
            if (string.IsNullOrEmpty(studentNumber))
            {
                MessageBox.Show("Invalid student record selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM basic_ed_enrollment WHERE student_number = @student_number";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@student_number", studentNumber);

                    DataTable dt = new DataTable();
                    new MySqlDataAdapter(cmd).Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Student record not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DataRow r = dt.Rows[0];

                    // --- Fetch Siblings ---
                    string siblingQuery = @"
                SELECT name, grade_occupation 
                FROM basic_ed_siblings 
                WHERE enrollment_id = (SELECT id FROM basic_ed_enrollment WHERE student_number = @student_number)";
                    MySqlCommand siblingCmd = new MySqlCommand(siblingQuery, conn);
                    siblingCmd.Parameters.AddWithValue("@student_number", studentNumber);
                    DataTable sibs = new DataTable();
                    new MySqlDataAdapter(siblingCmd).Fill(sibs);

                    // Build HTML-like formatted text (for landscape look)
                    StringBuilder info = new StringBuilder();

                    info.AppendLine("==============================================");
                    info.AppendLine("               STUDENT INFORMATION             ");
                    info.AppendLine("==============================================");
                    info.AppendLine($"Student Number: {r["student_number"]}      LRN: {r["lrn"]}");
                    info.AppendLine($"Name: {r["surname"]}, {r["first_name"]} {r["middle_name"]}");
                    info.AppendLine($"Level Applied: {r["level_applied"]}      Registrar Level: {r["level_for_registrar"]}");
                    info.AppendLine($"Gender: {r["gender"]}      Age: {r["age"]}      DOB: {r["dob"]:d}");
                    info.AppendLine($"Citizenship: {r["citizenship"]}      Religion: {r["religion"]}");
                    info.AppendLine($"Contact: {r["contact"]}      Email: {r["email"]}");
                    info.AppendLine($"Address: {r["address"]}");
                    info.AppendLine();

                    info.AppendLine("==============================================");
                    info.AppendLine("             FAMILY INFORMATION               ");
                    info.AppendLine("==============================================");
                    info.AppendLine($"Father: {r["father_name"]} ({r["father_occupation"]})");
                    info.AppendLine($"Mother: {r["mother_name"]} ({r["mother_occupation"]})");
                    info.AppendLine($"Guardian: {r["guardian_name"]} ({r["guardian_relation"]})");
                    info.AppendLine($"Guardian Contact: {r["guardian_contact"]}");
                    info.AppendLine();

                    // ✅ Add siblings inline
                    if (sibs.Rows.Count > 0)
                    {
                        info.Append("Siblings: ");
                        for (int i = 0; i < sibs.Rows.Count; i++)
                        {
                            info.Append($"{sibs.Rows[i]["name"]} ({sibs.Rows[i]["grade_occupation"]})");
                            if (i < sibs.Rows.Count - 1) info.Append(" | ");
                        }
                        info.AppendLine();
                    }
                    else
                    {
                        info.AppendLine("Siblings: None recorded.");
                    }
                    info.AppendLine();

                    info.AppendLine("==============================================");
                    info.AppendLine("           EDUCATIONAL BACKGROUND             ");
                    info.AppendLine("==============================================");
                    info.AppendLine($"Prev School: {r["prev_school"]}");
                    info.AppendLine($"Prev Grade: {r["prev_grade"]}      SY: {r["prev_sy"]}");
                    info.AppendLine($"Prev Address: {r["prev_school_addr"]}");
                    info.AppendLine($"Prev CTFC-AI Student: {r["prev_ctfcai"]}");
                    info.AppendLine();

                    info.AppendLine("==============================================");
                    info.AppendLine("        HEALTH & EMERGENCY INFORMATION        ");
                    info.AppendLine("==============================================");
                    info.AppendLine($"Health Conditions: {r["health_conditions"]}");
                    info.AppendLine($"Emergency Contact: {r["emergency_name"]} ({r["emergency_contact"]})");
                    info.AppendLine($"Address: {r["emergency_address"]}");
                    info.AppendLine();

                    info.AppendLine("==============================================");
                    info.AppendLine("            ENROLLMENT DETAILS                ");
                    info.AppendLine("==============================================");
                    info.AppendLine($"Status: {r["enrollment_status"]}      SY: {r["school_year"]}");
                    info.AppendLine($"Submitted: {Convert.ToDateTime(r["submitted_at"]).ToString("MM/dd/yyyy hh:mm tt")}");
                    info.AppendLine();

                    // === Display in scrollable form ===
                    Form infoForm = new Form();
                    infoForm.Text = $"Student Info - {r["student_number"]}";
                    infoForm.Size = new Size(900, 600);
                    infoForm.StartPosition = FormStartPosition.CenterScreen;

                    TextBox txt = new TextBox();
                    txt.Multiline = true;
                    txt.ScrollBars = ScrollBars.Vertical;
                    txt.ReadOnly = true;
                    txt.Font = new Font("Consolas", 10);
                    txt.Dock = DockStyle.Fill;
                    txt.Text = info.ToString();

                    infoForm.Controls.Add(txt);
                    infoForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading details: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void TbSearch_TextChanged(object sender, EventArgs e)
        {
            string filterText = tbSearch.Text.ToLower();

            foreach (DataGridViewRow row in dtgEnrollees.Rows)
            {
                if (row.Cells.Count < 3) continue;

                string studentNumber = row.Cells[0].Value?.ToString().ToLower() ?? "";
                string levelApplied = row.Cells[2].Value?.ToString().ToLower() ?? "";
                string fullName = row.Cells[1].Value?.ToString().ToLower() ?? "";

                bool visible = studentNumber.Contains(filterText) || fullName.Contains(filterText) || levelApplied.Contains(filterText);
                row.Visible = visible;
            }
        }

        private void CbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbSearch.Clear();
            string selectedStatus = cbStatusFilter.SelectedItem?.ToString() ?? "Pending";
            LoadStudentData(selectedStatus);
        }
    }
}
