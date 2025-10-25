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
        public Senior_High_School()
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
                    string query = @"SELECT student_number, grade_level,
                                surname, first_name, middle_name, gender, age,
                                shsCellphone, shsEmail, application_date, enrollment_status, id_photo_filename
                                FROM shs_enrollments";

                    if (statusFilter != "All")
                        query += $" WHERE enrollment_status = '{statusFilter}'";

                    query += " ORDER BY application_date DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtgSHS.Rows.Clear();

                        // ✅ Ensure Photo column exists (only once)
                        if (!dtgSHS.Columns.Contains("colPhoto"))
                        {
                            DataGridViewImageColumn colPhoto = new DataGridViewImageColumn();
                            colPhoto.Name = "colPhoto";
                            colPhoto.HeaderText = "Photo";
                            colPhoto.ImageLayout = DataGridViewImageCellLayout.Zoom;
                            colPhoto.Width = 80;
                            dtgSHS.Columns.Insert(0, colPhoto);
                        }

                        dtgSHS.RowTemplate.Height = 60; // make room for the photo

                        while (reader.Read())
                        {
                            // ✅ Convert BLOB to image
                            Image studentImage = null;
                            byte[] photoBytes = null;
                            if (reader["id_photo_filename"] != DBNull.Value)
                            {
                                try
                                {
                                    photoBytes = (byte[])reader["id_photo_filename"];
                                    if (photoBytes.Length > 0)
                                    {
                                        using (var ms = new System.IO.MemoryStream(photoBytes))
                                        {
                                            studentImage = Image.FromStream(ms);
                                        }
                                    }
                                }
                                catch
                                {
                                    studentImage = Properties.Resources.no_image; // fallback image
                                }
                            }

                            // Use placeholder if no photo exists
                            if (studentImage == null)
                            {
                                studentImage = Properties.Resources.no_image;
                            }

                            string fullName = $"{reader["surname"]}, {reader["first_name"]} {reader["middle_name"]}";
                            string status = reader["enrollment_status"].ToString();

                            // ✅ Add new photo cell as first column
                            int rowIndex = dtgSHS.Rows.Add(
                                studentImage, // new column for photo
                                reader["student_number"].ToString(),
                                fullName,
                                reader["grade_level"].ToString(),
                                reader["gender"].ToString(),
                                reader["age"].ToString(),
                                reader["shsCellphone"].ToString(),
                                Convert.ToDateTime(reader["application_date"]).ToString("MM/dd/yyyy"),
                                Properties.Resources.edit_interface_sign,
                                Properties.Resources.delete_square
                            );

                            // Store the full-size photo bytes in the row for later use
                            if (photoBytes != null && photoBytes.Length > 0)
                            {
                                // Store both email and photo bytes in the Tag
                                dtgSHS.Rows[rowIndex].Tag = new Tuple<string, byte[]>(reader["shsEmail"].ToString(), photoBytes);
                            }
                            else
                            {
                                dtgSHS.Rows[rowIndex].Tag = new Tuple<string, byte[]>(reader["shsEmail"].ToString(), null);
                            }

                            // Color code rows
                            if (status == "Confirmed")
                                dtgSHS.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(230, 245, 230);
                            else if (status == "Rejected")
                                dtgSHS.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Senior_High_School_Load(object sender, EventArgs e)
        {
            cbStatusFilter.SelectedIndex = 0;
            LoadStudentData("Pending");

            // ✅ Add "View Info" button column (if not already added)
            if (!dtgSHS.Columns.Contains("btnViewInfo"))
            {
                DataGridViewButtonColumn btnViewInfo = new DataGridViewButtonColumn();
                btnViewInfo.HeaderText = "Action";
                btnViewInfo.Name = "btnViewInfo";
                btnViewInfo.Text = "View Info";
                btnViewInfo.UseColumnTextForButtonValue = true;

                // Insert after "Application Date" (index 7 because of photo column at index 0)
                dtgSHS.Columns.Insert(8, btnViewInfo);
            }
        }

        private void dtgSHS_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow row = dtgSHS.Rows[e.RowIndex];
            if (row.Cells.Count <= 0) return;

            string studentNumber = row.Cells[1].Value?.ToString();
            string name = row.Cells[2].Value?.ToString();
            string yearLevel = row.Cells[3].Value?.ToString();

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
            else if (columnName == "colPhoto")
            {
                // Show full-size photo when photo cell is clicked
                ShowFullSizePhoto(row);
            }
        }

        private void ShowFullSizePhoto(DataGridViewRow row)
        {
            byte[] photoBytes = null;
            if (row.Tag is Tuple<string, byte[]> tagData)
            {
                photoBytes = tagData.Item2;
            }

            if (photoBytes != null && photoBytes.Length > 0)
            {
                try
                {
                    using (var ms = new System.IO.MemoryStream(photoBytes))
                    {
                        Image fullSizeImage = Image.FromStream(ms);

                        // Create a form to display the full-size image
                        Form imageForm = new Form();
                        imageForm.Text = $"Student Photo - {row.Cells[1].Value}";
                        imageForm.Size = new Size(600, 700);
                        imageForm.StartPosition = FormStartPosition.CenterScreen;
                        imageForm.BackColor = Color.White;
                        imageForm.Icon = SystemIcons.Information;

                        // PictureBox to display the image
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Dock = DockStyle.Fill;
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox.Image = fullSizeImage;

                        // Add student info below the photo
                        Panel infoPanel = new Panel();
                        infoPanel.Dock = DockStyle.Bottom;
                        infoPanel.Height = 80;
                        infoPanel.BackColor = Color.LightGray;
                        infoPanel.Padding = new Padding(10);

                        Label infoLabel = new Label();
                        infoLabel.Text = $"Student: {row.Cells[2].Value}\nStudent #: {row.Cells[1].Value}";
                        infoLabel.Dock = DockStyle.Fill;
                        infoLabel.TextAlign = ContentAlignment.MiddleCenter;
                        infoLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);

                        // Close button
                        Button closeButton = new Button();
                        closeButton.Text = "Close";
                        closeButton.Size = new Size(80, 30);
                        closeButton.Location = new Point(260, 45);
                        closeButton.BackColor = Color.FromArgb(220, 53, 69);
                        closeButton.ForeColor = Color.White;
                        closeButton.FlatStyle = FlatStyle.Flat;
                        closeButton.Click += (s, e) => imageForm.Close();

                        // Add controls
                        infoPanel.Controls.Add(infoLabel);
                        infoPanel.Controls.Add(closeButton);
                        imageForm.Controls.Add(pictureBox);
                        imageForm.Controls.Add(infoPanel);

                        imageForm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error displaying photo: " + ex.Message, "Photo Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No photo available for this student.", "No Photo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                string studentNumber = row.Cells[1].Value?.ToString().ToLower() ?? "";
                string fullName = row.Cells[2].Value?.ToString().ToLower() ?? "";
                string yearLevel = row.Cells[3].Value?.ToString().ToLower() ?? "";

                row.Visible = studentNumber.Contains(filtertext) || fullName.Contains(filtertext) || yearLevel.Contains(filtertext);
            }
        }

        private void cbStatusFilter_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            tbSearch.Clear();
            string selectedStatus = cbStatusFilter.SelectedItem?.ToString() ?? "Pending";
            LoadStudentData(selectedStatus);
        }
    }
}