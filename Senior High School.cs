using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class Senior_High_School : UserControl
    {
        private Form parentForm;
        private int currentPage = 1;
        private int recordsPerPage = 10;
        private int totalRecords = 0;
        private int totalPages = 0;

        public Senior_High_School(Form parentForm = null)
        {
            InitializeComponent();
            this.parentForm = parentForm ?? this.FindForm();
        }

        private void LoadStudentData(string statusFilter = "Pending")
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    // ✅ Count total records first
                    string countQuery = "SELECT COUNT(*) FROM shs_enrollments";
                    if (statusFilter != "All")
                        countQuery += " WHERE enrollment_status = @status";

                    MySqlCommand countCmd = new MySqlCommand(countQuery, conn);
                    if (statusFilter != "All")
                        countCmd.Parameters.AddWithValue("@status", statusFilter);

                    totalRecords = Convert.ToInt32(countCmd.ExecuteScalar());
                    totalPages = (int)Math.Ceiling((double)totalRecords / recordsPerPage);

                    // ✅ Fetch only limited records for current page
                    int offset = (currentPage - 1) * recordsPerPage;
                    string query = @"SELECT student_number, grade_level, surname, first_name, middle_name, 
                            gender, age, shsCellphone, shsEmail, application_date, 
                            enrollment_status, id_photo_filename
                            FROM shs_enrollments";

                    if (statusFilter != "All")
                        query += " WHERE enrollment_status = @status";

                    query += " ORDER BY application_date DESC LIMIT @limit OFFSET @offset";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (statusFilter != "All")
                        cmd.Parameters.AddWithValue("@status", statusFilter);
                    cmd.Parameters.AddWithValue("@limit", recordsPerPage);
                    cmd.Parameters.AddWithValue("@offset", offset);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtgSHS.Rows.Clear();

                        while (reader.Read())
                        {
                            string fullName = $"{reader["surname"]}, {reader["first_name"]} {reader["middle_name"]}";
                            string status = reader["enrollment_status"].ToString();

                            int rowIndex = dtgSHS.Rows.Add(
                                reader["student_number"].ToString(),
                                fullName,
                                reader["grade_level"].ToString(),
                                reader["gender"].ToString(),
                                reader["age"].ToString(),
                                reader["shsCellphone"].ToString(),
                                Convert.ToDateTime(reader["application_date"]).ToString("MM/dd/yyyy")
                            );

                            dtgSHS.Rows[rowIndex].Tag = reader["shsEmail"].ToString();

                            if (status == "Confirmed")
                                dtgSHS.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(230, 245, 230);
                            else if (status == "Rejected")
                                dtgSHS.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                        }
                    }

                    // ✅ Update pagination label
                    lblPageInfo.Text = $"Page {currentPage} of {totalPages}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool viewInfoColumnAdded = false;
        private void Senior_High_School_Load(object sender, EventArgs e)
        {
            cbStatusFilter.SelectedIndex = 0;
            LoadStudentData("Pending");

            if (!viewInfoColumnAdded)
            {
                DataGridViewButtonColumn btnViewInfo = new DataGridViewButtonColumn();
                btnViewInfo.HeaderText = "Action";
                btnViewInfo.Name = "btnViewInfo";
                btnViewInfo.Text = "View Info";
                btnViewInfo.UseColumnTextForButtonValue = true;

                dtgSHS.Columns.Insert(7, btnViewInfo);
                viewInfoColumnAdded = true;
            }
        }

        private void dtgSHS_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow row = dtgSHS.Rows[e.RowIndex];
            if (row.Cells.Count <= 0) return;

            string studentNumber = row.Cells[0].Value?.ToString();
            string name = row.Cells[1].Value?.ToString();
            string yearLevel = row.Cells[2].Value?.ToString();

            // Get email from Tag
            string email = "";
            if (row.Tag is Tuple<string, byte[]> tagData)
            {
                email = tagData.Item1;
            }

            string columnName = dtgSHS.Columns[e.ColumnIndex].Name;
            if (string.IsNullOrEmpty(studentNumber)) return;

            if (columnName == "btnConfirm")
                ConfirmEnrollment(studentNumber, name, email, yearLevel);
            else if (columnName == "btnDelete")
                RejectEnrollment(studentNumber, name, email, yearLevel);
            else if (columnName == "btnViewInfo")
                ViewEnrollmentDetails(studentNumber);
            // Create the details user control
            StudentDetailsFormSHS detailsUC = new StudentDetailsFormSHS();
            detailsUC.StudentNumber = studentNumber;

            // Subscribe to Back button event
            detailsUC.BackButtonClicked += (s, args) =>
            {
                // Reload Enrollees list when Back is clicked
                Senior_High_School enrolleesUC = new Senior_High_School(parentForm);
                Form2 mainForm = parentForm as Form2;
                mainForm?.LoadControl(enrolleesUC);
            };

            // Load the details UserControl into Form2 panel
            if (parentForm != null)
            {
                if (parentForm is Form2 mainForm)
                {
                    mainForm.LoadControl(detailsUC);
                }
            }
        }

        private string GetStudentEmail(string studentNumber)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT shsEmail FROM shs_enrollments WHERE student_number = @student_number";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@student_number", studentNumber);
                object result = cmd.ExecuteScalar();
                return result?.ToString() ?? "";
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
                        INSERT INTO students 
                            (student_number, application_no, grade_level, strand, surname, first_name, middle_name, age, gender, email, contact, telephone, cellphone, id_photo_filename)
                        SELECT 
                            student_number,
                            application_no,
                            grade_level, 
                            strand, 
                            surname, 
                            first_name, 
                            middle_name, 
                            age, 
                            gender, 
                            shsEmail,
                            COALESCE(shsCellphone, shsTelephone, '') AS contact,
                            shsTelephone,
                            shsCellphone,
                            id_photo_filename
                        FROM shs_enrollments 
                        WHERE student_number = @student_number";

                    MySqlCommand transferCmd = new MySqlCommand(transferQuery, conn);
                    transferCmd.Parameters.AddWithValue("@student_number", studentNumber);

                    int rowsAffected = transferCmd.ExecuteNonQuery();
                    conn.Close(); // ✅ Close connection early before sending email

                    if (rowsAffected > 0)
                    {
                        using (MySqlConnection updateConn = Database.GetConnection())
                        {
                            updateConn.Open();
                            UpdateStatus(updateConn, studentNumber, "Confirmed");
                        }

                        // ✅ Now safely send email notification
                        bool notificationSent = await NotifyStudent(studentNumber, name, email, "Approved", yearLevel);

                        MessageBox.Show($"Student {name} enrolled successfully" +
                            (notificationSent ? "" : "\nNote: Email notification failed"),
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
                    string updateQuery = "UPDATE shs_enrollments SET enrollment_status = 'Rejected' WHERE student_number = @student_number";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@student_number", studentNumber);
                    updateCmd.ExecuteNonQuery();

                    bool notificationSent = await NotifyStudent(studentNumber, name, email, "Rejected", yearLevel);

                    MessageBox.Show($"Student {name} rejected successfully" +
                        (notificationSent ? "" : "\nNote: Email notification failed"),
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
            string updateQuery = "UPDATE shs_enrollments SET enrollment_status = @status WHERE student_number = @student_number";
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
                    enrollment_type = "Senior High School"
                };

                string json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    //HttpResponseMessage response = await client.PostAsync("http://localhost/CAPSTONE_PROJ/api/cs/notify-student-shs.php", content);
                    HttpResponseMessage response = await client.PostAsync("https://ctfca-enrollment.onrender.com/api/cs/notify-student-shs.php", content);
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

                    string query = "SELECT * FROM shs_enrollments WHERE student_number = @student_number";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@student_number", studentNumber);

                    DataTable dt = new DataTable();
                    new MySqlDataAdapter(cmd).Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Student record not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DataRow studentData = dt.Rows[0];

                    ShowStudentDetails(studentData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading details: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowStudentDetails(DataRow studentData)
        {
            Control parentContainer = this.Parent;
            if (parentContainer == null) return;

            StudentDetailsFormSHS studentDetails = new StudentDetailsFormSHS();

            // ✅ Set the StudentNumber property
            studentDetails.StudentNumber = studentData["student_number"].ToString();

            studentDetails.BackButtonClicked += (s, e) => ShowSeniorHighSchoolControl();

            this.Hide();
            studentDetails.Dock = DockStyle.Fill;
            parentContainer.Controls.Add(studentDetails);

            // ✅ Manually call ShowStudentInfo after setting the property
            studentDetails.Show(); // Ensure it's visible
        }

        // Method to show the Senior High School control again
        private void ShowSeniorHighSchoolControl()
        {
            Control parentContainer = this.Parent;
            if (parentContainer == null) return;

            // Remove all controls from parent container
            parentContainer.Controls.Clear();

            // Add this control back
            this.Dock = DockStyle.Fill;
            parentContainer.Controls.Add(this);
            this.Show();

            // Refresh the data
            LoadStudentData(cbStatusFilter.Text);
        }

        private void tbSearch_TextChanged_1(object sender, EventArgs e)
        {
            string filtertext = tbSearch.Text.ToLower();

            foreach (DataGridViewRow row in dtgSHS.Rows)
            {
                if (row.Cells.Count < 3) continue;

                string studentNumber = row.Cells[0].Value?.ToString().ToLower() ?? "";
                string fullName = row.Cells[1].Value?.ToString().ToLower() ?? "";
                string yearLevel = row.Cells[2].Value?.ToString().ToLower() ?? "";

                row.Visible = studentNumber.Contains(filtertext) || fullName.Contains(filtertext) || yearLevel.Contains(filtertext);
            }
        }

        private void cbStatusFilter_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            tbSearch.Clear();
            currentPage = 1; // reset to first page
            string selectedStatus = cbStatusFilter.SelectedItem?.ToString() ?? "Pending";
            LoadStudentData(selectedStatus);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadStudentData(cbStatusFilter.Text);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadStudentData(cbStatusFilter.Text);
            }
        }

        private void lblPageInfo_Click(object sender, EventArgs e)
        {
            if (totalPages == 0) return;

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Enter page number (1 - {totalPages}):",
                "Go to Page",
                currentPage.ToString()
            );

            if (int.TryParse(input, out int requestedPage))
            {
                if (requestedPage >= 1 && requestedPage <= totalPages)
                {
                    currentPage = requestedPage;
                    LoadStudentData(cbStatusFilter.SelectedItem?.ToString() ?? "Pending");
                }
                else
                {
                    MessageBox.Show($"Please enter a number between 1 and {totalPages}.",
                        "Invalid Page", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}