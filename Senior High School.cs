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
                    if (rowsAffected > 0)
                    {
                        UpdateStatus(conn, studentNumber, "Confirmed");
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
                    HttpResponseMessage response = await client.PostAsync("http://localhost/CAPSTONE_PROJ/api/cs/notify-student-shs.php", content);
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

                    DataRow r = dt.Rows[0];

                    // Create enhanced form with standard Windows Forms controls
                    ShowEnhancedStudentInfo(r);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading details: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowEnhancedStudentInfo(DataRow student)
        {
            Form infoForm = new Form();
            infoForm.Text = $"Student Information - {student["student_number"]}";
            infoForm.Size = new Size(1100, 750);
            infoForm.StartPosition = FormStartPosition.CenterScreen;
            infoForm.BackColor = Color.White;
            infoForm.Font = new Font("Segoe UI", 9F);
            infoForm.Icon = SystemIcons.Information;

            // Main Panel
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(20);
            mainPanel.AutoScroll = true;

            // Header with Photo
            Panel headerPanel = new Panel();
            headerPanel.Location = new Point(20, 20);
            headerPanel.Size = new Size(1040, 80);
            headerPanel.BackColor = Color.Transparent;

            // Student Photo in header
            PictureBox headerPhoto = new PictureBox();
            headerPhoto.Size = new Size(70, 70);
            headerPhoto.Location = new Point(0, 5);
            headerPhoto.SizeMode = PictureBoxSizeMode.Zoom;
            headerPhoto.BorderStyle = BorderStyle.FixedSingle;

            // Load student photo for header
            if (student["id_photo_filename"] != DBNull.Value)
            {
                try
                {
                    byte[] photoBytes = (byte[])student["id_photo_filename"];
                    if (photoBytes.Length > 0)
                    {
                        using (var ms = new System.IO.MemoryStream(photoBytes))
                        {
                            headerPhoto.Image = Image.FromStream(ms);
                        }
                    }
                }
                catch
                {
                    headerPhoto.Image = Properties.Resources.no_image;
                }
            }
            else
            {
                headerPhoto.Image = Properties.Resources.no_image;
            }

            // Make header photo clickable to show full size
            headerPhoto.Cursor = Cursors.Hand;
            if (student["id_photo_filename"] != DBNull.Value)
            {
                byte[] photoBytes = (byte[])student["id_photo_filename"];
                headerPhoto.Click += (s, e) => ShowFullSizePhotoFromBytes(photoBytes,
                    $"{student["surname"]}, {student["first_name"]} {student["middle_name"]}",
                    student["student_number"].ToString());
            }

            // Header labels
            Label headerLabel = new Label();
            headerLabel.Text = $"{student["surname"]}, {student["first_name"]} {student["middle_name"]}";
            headerLabel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            headerLabel.ForeColor = Color.FromArgb(33, 42, 57);
            headerLabel.Location = new Point(80, 10);
            headerLabel.AutoSize = true;

            // Student Number and Status
            Label subHeaderLabel = new Label();
            string status = student["enrollment_status"].ToString();
            Color statusColor = status == "Confirmed" ? Color.Green :
                               status == "Rejected" ? Color.Red :
                               Color.Orange;

            subHeaderLabel.Text = $"Student #: {student["student_number"]} | Application #: {student["application_no"]} | Status: {status}";
            subHeaderLabel.Font = new Font("Segoe UI", 10);
            subHeaderLabel.ForeColor = Color.Gray;
            subHeaderLabel.Location = new Point(80, 45);
            subHeaderLabel.AutoSize = true;

            headerPanel.Controls.Add(headerPhoto);
            headerPanel.Controls.Add(headerLabel);
            headerPanel.Controls.Add(subHeaderLabel);

            // Create Tab Control
            TabControl tabControl = new TabControl();
            tabControl.Location = new Point(20, 100);
            tabControl.Size = new Size(1040, 520);
            tabControl.Font = new Font("Segoe UI", 9F);

            // Personal Information Tab
            TabPage tabPersonal = new TabPage();
            tabPersonal.Text = "Personal Information";
            tabPersonal.AutoScroll = true;
            tabPersonal.Padding = new Padding(20);
            tabPersonal.BackColor = Color.White;

            // Family Information Tab
            TabPage tabFamily = new TabPage();
            tabFamily.Text = "Family Information";
            tabFamily.AutoScroll = true;
            tabFamily.Padding = new Padding(20);
            tabFamily.BackColor = Color.White;

            // Educational Background Tab
            TabPage tabEducation = new TabPage();
            tabEducation.Text = "Educational Background";
            tabEducation.AutoScroll = true;
            tabEducation.Padding = new Padding(20);
            tabEducation.BackColor = Color.White;

            // Health & Testing Tab
            TabPage tabHealth = new TabPage();
            tabHealth.Text = "Health & Testing";
            tabHealth.AutoScroll = true;
            tabHealth.Padding = new Padding(20);
            tabHealth.BackColor = Color.White;

            // Add tabs to control
            tabControl.Controls.Add(tabPersonal);
            tabControl.Controls.Add(tabFamily);
            tabControl.Controls.Add(tabEducation);
            tabControl.Controls.Add(tabHealth);

            // ===== PERSONAL INFORMATION TAB =====
            int yPos = 20;

            // Row 1: Basic Info
            AddInfoRow(tabPersonal, "Grade Level:", student["grade_level"].ToString(), 20, ref yPos);
            AddInfoRow(tabPersonal, "Strand:", student["strand"].ToString(), 250, ref yPos);
            AddInfoRow(tabPersonal, "Gender:", student["gender"].ToString(), 500, ref yPos);

            yPos += 10;
            AddInfoRow(tabPersonal, "Nickname:", student["nickname"].ToString(), 20, ref yPos);
            AddInfoRow(tabPersonal, "Age:", student["age"].ToString(), 250, ref yPos);
            AddInfoRow(tabPersonal, "Birth Date:", Convert.ToDateTime(student["birth_date"]).ToString("MM/dd/yyyy"), 500, ref yPos);

            yPos += 10;
            AddInfoRow(tabPersonal, "Marital Status:", student["marital_status"].ToString(), 20, ref yPos);
            AddInfoRow(tabPersonal, "Handed:", student["handed"].ToString(), 250, ref yPos);
            AddInfoRow(tabPersonal, "Religion:", student["religion"].ToString(), 500, ref yPos);

            yPos += 10;
            AddInfoRow(tabPersonal, "Church Name:", student["church_name"].ToString(), 20, ref yPos);
            AddInfoRow(tabPersonal, "Pastor Name:", student["pastor_name"].ToString(), 250, ref yPos);

            yPos += 30;
            AddInfoRow(tabPersonal, "Email:", student["shsEmail"].ToString(), 20, ref yPos);
            AddInfoRow(tabPersonal, "Cellphone:", student["shsCellphone"].ToString(), 250, ref yPos);
            AddInfoRow(tabPersonal, "Telephone:", student["shsTelephone"].ToString(), 500, ref yPos);

            yPos += 30;
            AddInfoRow(tabPersonal, "Home Address:", student["home_address"].ToString(), 20, ref yPos, 700);

            // ===== FAMILY INFORMATION TAB =====
            yPos = 20;

            // Father Information
            AddSectionHeader(tabFamily, "FATHER'S INFORMATION", ref yPos);
            AddInfoRow(tabFamily, "Name:", student["father_name"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Occupation:", student["father_occupation"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Company:", student["father_company"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Office:", student["father_office"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Income:", student["father_income"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Religion:", student["father_religion"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Telephone:", student["father_tel"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Mobile:", student["father_mobile"].ToString(), 20, ref yPos, 400);

            yPos += 20;

            // Mother Information
            AddSectionHeader(tabFamily, "MOTHER'S INFORMATION", ref yPos);
            AddInfoRow(tabFamily, "Name:", student["mother_name"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Occupation:", student["mother_occupation"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Company:", student["mother_company"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Office:", student["mother_office"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Income:", student["mother_income"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Religion:", student["mother_religion"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Telephone:", student["mother_tel"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Mobile:", student["mother_mobile"].ToString(), 20, ref yPos, 400);

            yPos += 20;

            // Step Parent Information (if exists)
            if (!string.IsNullOrEmpty(student["step_parent_name"].ToString()))
            {
                AddSectionHeader(tabFamily, "STEP PARENT'S INFORMATION", ref yPos);
                AddInfoRow(tabFamily, "Name:", student["step_parent_name"].ToString(), 20, ref yPos, 400);
                AddInfoRow(tabFamily, "Occupation:", student["step_parent_occupation"].ToString(), 20, ref yPos, 400);
                AddInfoRow(tabFamily, "Company:", student["step_parent_company"].ToString(), 20, ref yPos, 400);
                AddInfoRow(tabFamily, "Office:", student["step_parent_office"].ToString(), 20, ref yPos, 400);
                AddInfoRow(tabFamily, "Income:", student["step_parent_income"].ToString(), 20, ref yPos, 400);
                AddInfoRow(tabFamily, "Religion:", student["step_parent_religion"].ToString(), 20, ref yPos, 400);
                AddInfoRow(tabFamily, "Telephone:", student["step_parent_tel"].ToString(), 20, ref yPos, 400);
                AddInfoRow(tabFamily, "Mobile:", student["step_parent_mobile"].ToString(), 20, ref yPos, 400);
                yPos += 20;
            }

            // Guardian Information (if different from parents)
            if (!string.IsNullOrEmpty(student["guardian_name"].ToString()) &&
                student["guardian_name"].ToString() != student["father_name"].ToString() &&
                student["guardian_name"].ToString() != student["mother_name"].ToString())
            {
                AddSectionHeader(tabFamily, "GUARDIAN'S INFORMATION", ref yPos);
                AddInfoRow(tabFamily, "Name:", student["guardian_name"].ToString(), 20, ref yPos, 400);
                AddInfoRow(tabFamily, "Occupation:", student["guardian_occupation"].ToString(), 20, ref yPos, 400);
                AddInfoRow(tabFamily, "Company:", student["guardian_company"].ToString(), 20, ref yPos, 400);
                AddInfoRow(tabFamily, "Office:", student["guardian_office"].ToString(), 20, ref yPos, 400);
                AddInfoRow(tabFamily, "Income:", student["guardian_income"].ToString(), 20, ref yPos, 400);
                AddInfoRow(tabFamily, "Religion:", student["guardian_religion"].ToString(), 20, ref yPos, 400);
                AddInfoRow(tabFamily, "Telephone:", student["guardian_tel"].ToString(), 20, ref yPos, 400);
                AddInfoRow(tabFamily, "Mobile:", student["guardian_mobile"].ToString(), 20, ref yPos, 400);
            }

            // ===== EDUCATIONAL BACKGROUND TAB =====
            yPos = 20;

            AddInfoRow(tabEducation, "Last School Attended:", student["last_school"].ToString(), 20, ref yPos, 600);
            AddInfoRow(tabEducation, "School Address:", student["last_school_address"].ToString(), 20, ref yPos, 600);

            yPos += 20;
            AddSectionHeader(tabEducation, "TESTING INFORMATION", ref yPos);
            AddInfoRow(tabEducation, "NCAE Result:", student["ncae_result"].ToString(), 20, ref yPos, 600);
            AddInfoRow(tabEducation, "Test Schedule:", student["test_schedule"].ToString(), 20, ref yPos, 600);
            AddInfoRow(tabEducation, "Test Result:", student["test_result"].ToString(), 20, ref yPos, 600);
            AddInfoRow(tabEducation, "Remarks:", student["remarks"].ToString(), 20, ref yPos, 600);

            // ===== HEALTH & TESTING TAB =====
            yPos = 20;

            AddSectionHeader(tabHealth, "HEALTH INFORMATION", ref yPos);
            AddInfoRow(tabHealth, "General Health:", student["health"].ToString(), 20, ref yPos, 600);
            AddInfoRow(tabHealth, "Eye Condition:", student["health_eye"].ToString(), 20, ref yPos, 600);
            AddInfoRow(tabHealth, "Ear Condition:", student["health_ear"].ToString(), 20, ref yPos, 600);
            AddInfoRow(tabHealth, "Allergies:", student["health_allergies"].ToString(), 20, ref yPos, 600);
            AddInfoRow(tabHealth, "Other Health Conditions:", student["health_others"].ToString(), 20, ref yPos, 600);

            yPos += 30;
            AddSectionHeader(tabHealth, "ENROLLMENT DETAILS", ref yPos);
            AddInfoRow(tabHealth, "Application Date:", Convert.ToDateTime(student["application_date"]).ToString("MM/dd/yyyy"), 20, ref yPos, 400);
            AddInfoRow(tabHealth, "School Year:", student["school_year"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabHealth, "Date Created:", Convert.ToDateTime(student["created_at"]).ToString("MM/dd/yyyy hh:mm tt"), 20, ref yPos, 400);

            // Close Button
            Button closeButton = new Button();
            closeButton.Text = "Close";
            closeButton.Size = new Size(100, 35);
            closeButton.Location = new Point(960, 640);
            closeButton.BackColor = Color.FromArgb(220, 53, 69);
            closeButton.ForeColor = Color.White;
            closeButton.Font = new Font("Segoe UI", 9F);
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.Click += (s, e) => infoForm.Close();

            // Add controls to main panel
            mainPanel.Controls.Add(headerPanel);
            mainPanel.Controls.Add(tabControl);
            mainPanel.Controls.Add(closeButton);

            infoForm.Controls.Add(mainPanel);
            infoForm.ShowDialog();
        }

        private void ShowFullSizePhotoFromBytes(byte[] photoBytes, string studentName, string studentNumber)
        {
            if (photoBytes != null && photoBytes.Length > 0)
            {
                try
                {
                    using (var ms = new System.IO.MemoryStream(photoBytes))
                    {
                        Image fullSizeImage = Image.FromStream(ms);

                        // Create a form to display the full-size image
                        Form imageForm = new Form();
                        imageForm.Text = $"Student Photo - {studentNumber}";
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
                        infoLabel.Text = $"Student: {studentName}\nStudent #: {studentNumber}";
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

        // Helper method to add information rows
        private void AddInfoRow(Control parent, string label, string value, int x, ref int y, int width = 200)
        {
            Label lblLabel = new Label();
            lblLabel.Text = label;
            lblLabel.Location = new Point(x, y);
            lblLabel.AutoSize = true;
            lblLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblLabel.ForeColor = Color.FromArgb(64, 64, 64);

            Label lblValue = new Label();
            lblValue.Text = string.IsNullOrEmpty(value) ? "N/A" : value;
            lblValue.Location = new Point(x + 150, y);
            lblValue.Size = new Size(width, 20);
            lblValue.AutoSize = false;
            lblValue.Font = new Font("Segoe UI", 9F);
            lblValue.ForeColor = Color.Black;

            parent.Controls.Add(lblLabel);
            parent.Controls.Add(lblValue);

            y += 25;
        }

        // Helper method to add section headers
        private void AddSectionHeader(Control parent, string text, ref int y)
        {
            Label header = new Label();
            header.Text = text;
            header.Location = new Point(20, y);
            header.AutoSize = true;
            header.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            header.ForeColor = Color.FromArgb(40, 96, 144);

            parent.Controls.Add(header);
            y += 30;

            // Add separator line
            Panel separator = new Panel();
            separator.Location = new Point(20, y - 5);
            separator.Size = new Size(600, 1);
            separator.BackColor = Color.LightGray;
            parent.Controls.Add(separator);
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
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

        private void cbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbSearch.Clear();
            string selectedStatus = cbStatusFilter.SelectedItem?.ToString() ?? "Pending";
            LoadStudentData(selectedStatus);
        }
    }
}