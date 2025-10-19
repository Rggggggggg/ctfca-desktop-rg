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
    public partial class Junior_High_School : UserControl
    {
        public Junior_High_School()
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
                             date_created, email, enrollment_status, student_photo
                             FROM jhs_enrollments";

                    if (statusFilter != "All")
                    {
                        query += $" WHERE enrollment_status = '{statusFilter}'";
                    }

                    query += " ORDER BY date_created DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dtgEnrollees.Rows.Clear();

                        // ✅ Ensure Photo column exists (only once)
                        if (!dtgEnrollees.Columns.Contains("colPhoto"))
                        {
                            DataGridViewImageColumn colPhoto = new DataGridViewImageColumn();
                            colPhoto.Name = "colPhoto";
                            colPhoto.HeaderText = "Photo";
                            colPhoto.ImageLayout = DataGridViewImageCellLayout.Zoom;
                            colPhoto.Width = 80;
                            dtgEnrollees.Columns.Insert(0, colPhoto);
                        }

                        dtgEnrollees.RowTemplate.Height = 60; // make room for the photo

                        while (reader.Read())
                        {
                            // ✅ Convert BLOB to image
                            Image studentImage = null;
                            byte[] photoBytes = null;
                            if (reader["student_photo"] != DBNull.Value)
                            {
                                try
                                {
                                    photoBytes = (byte[])reader["student_photo"];
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
                            int rowIndex = dtgEnrollees.Rows.Add(
                                studentImage, // new column for photo
                                reader["student_number"].ToString(),
                                fullName,
                                reader["level_applied"].ToString(),
                                reader["gender"].ToString(),
                                reader["age"].ToString(),
                                reader["contact"].ToString(),
                                Convert.ToDateTime(reader["date_created"]).ToString("MM/dd/yyyy")
                            );

                            // 🔹 Store email in the row (hidden, not displayed in grid)
                            dtgEnrollees.Rows[rowIndex].Tag = reader["email"].ToString();

                            // Store the full-size photo bytes in the row for later use
                            if (photoBytes != null && photoBytes.Length > 0)
                            {
                                // Store both email and photo bytes in the Tag
                                dtgEnrollees.Rows[rowIndex].Tag = new Tuple<string, byte[]>(reader["email"].ToString(), photoBytes);
                            }
                            else
                            {
                                dtgEnrollees.Rows[rowIndex].Tag = new Tuple<string, byte[]>(reader["email"].ToString(), null);
                            }

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

        private void JHEnrollees_Load(object sender, EventArgs e)
        {
            cbStatusFilter.SelectedIndex = 0;
            LoadStudentData("Pending");

            // ✅ Add "View Info" button column (if not already added)
            if (!dtgEnrollees.Columns.Contains("btnViewInfo"))
            {
                DataGridViewButtonColumn btnViewInfo = new DataGridViewButtonColumn();
                btnViewInfo.HeaderText = "Action";
                btnViewInfo.Name = "btnViewInfo";
                btnViewInfo.Text = "View Info";
                btnViewInfo.UseColumnTextForButtonValue = true;

                // Insert after "Date Created" (index 7 because of photo)
                dtgEnrollees.Columns.Insert(8, btnViewInfo);
            }
        }

        private void DtgEnrollees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow row = dtgEnrollees.Rows[e.RowIndex];
            if (row.Cells.Count <= 0) return;

            string studentNumber = row.Cells[1].Value?.ToString();
            string name = row.Cells[2].Value?.ToString();
            string yearLevel = row.Cells[3].Value?.ToString();
            string columnName = dtgEnrollees.Columns[e.ColumnIndex].Name;

            if (string.IsNullOrEmpty(studentNumber)) return;

            // Get email from Tag
            string email = "";
            if (row.Tag is Tuple<string, byte[]> tagData)
            {
                email = tagData.Item1;
            }

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
                Form2 parentForm = this.ParentForm as Form2;
                parentForm?.LoadJHSDetails(studentNumber);
            }
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
                            student_number, lrn, level_applied, surname, first_name, middle_name, age, gender, birth_date,
                            birth_place, citizenship, religion, address, contact, email, father_name, father_occupation, father_office, father_contact, 
                            father_email, mother_name, mother_occupation, mother_office, mother_contact, mother_email, marital_status, guardian_name, guardian_relation, 
                            guardian_occupation, guardian_contact, guardian_email, has_siblings, previously_enrolled, prev_grade, prev_sy, prev_school, prev_school_sy, prev_school_addr, 
                            health_conditions, emergency_name, emergency_address, emergency_contact, student_photo, signature_filename, signature_date,
                            enrollment_status
                        )
                        SELECT 
                            student_number, lrn, level_applied, surname, first_name, middle_name, age, gender, birth_date,
                            birth_place, citizenship, religion, address, contact, email, father_name, father_occupation, father_office, father_contact, 
                            father_email, mother_name, mother_occupation, mother_office, mother_contact, mother_email, marital_status, guardian_name, guardian_relation, 
                            guardian_occupation, guardian_contact, guardian_email, has_siblings, previously_enrolled, prev_grade, prev_sy, prev_school, prev_school_sy, prev_school_addr, 
                            health_conditions, emergency_name, emergency_address, emergency_contact, student_photo, signature_filename, signature_date,
                            enrollment_status
                        FROM jhs_enrollments 
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
                    string updateQuery = "UPDATE jhs_enrollments SET enrollment_status = 'Rejected' WHERE student_number = @student_number";
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
            string updateQuery = "UPDATE jhs_enrollments SET enrollment_status = @status WHERE student_number = @student_number";
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
                    enrollment_type = "Junior High School"
                };

                string json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync("http://localhost/CAPSTONE_PROJ/api/cs/notify-student-jhs.php", content);
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

                    string query = "SELECT * FROM jhs_enrollments WHERE student_number = @student_number";
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
            infoForm.Size = new Size(1000, 700);
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
            headerPanel.Size = new Size(940, 80);
            headerPanel.BackColor = Color.Transparent;

            // Student Photo in header
            PictureBox headerPhoto = new PictureBox();
            headerPhoto.Size = new Size(70, 70);
            headerPhoto.Location = new Point(0, 5);
            headerPhoto.SizeMode = PictureBoxSizeMode.Zoom;
            headerPhoto.BorderStyle = BorderStyle.FixedSingle;

            // Load student photo for header
            if (student["student_photo"] != DBNull.Value)
            {
                try
                {
                    byte[] photoBytes = (byte[])student["student_photo"];
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
            if (student["student_photo"] != DBNull.Value)
            {
                byte[] photoBytes = (byte[])student["student_photo"];
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

            subHeaderLabel.Text = $"Student #: {student["student_number"]} | LRN: {student["lrn"]} | Status: {status}";
            subHeaderLabel.Font = new Font("Segoe UI", 10);
            subHeaderLabel.ForeColor = Color.Gray;
            subHeaderLabel.Location = new Point(80, 45);
            subHeaderLabel.AutoSize = true;

            headerPanel.Controls.Add(headerPhoto);
            headerPanel.Controls.Add(headerLabel);
            headerPanel.Controls.Add(subHeaderLabel);

            // Create Tab Control
            TabControl tabControl = new TabControl();
            tabControl.Location = new Point(20, 120);
            tabControl.Size = new Size(940, 500);
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

            // Health & Emergency Tab
            TabPage tabHealth = new TabPage();
            tabHealth.Text = "Health & Emergency";
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
            AddInfoRow(tabPersonal, "Level Applied:", student["level_applied"].ToString(), 20, ref yPos);
            AddInfoRow(tabPersonal, "Gender:", student["gender"].ToString(), 250, ref yPos);
            AddInfoRow(tabPersonal, "Age:", student["age"].ToString(), 500, ref yPos);

            yPos += 10;
            AddInfoRow(tabPersonal, "Date of Birth:", Convert.ToDateTime(student["birth_date"]).ToString("MM/dd/yyyy"), 20, ref yPos);
            AddInfoRow(tabPersonal, "Place of Birth:", student["birth_place"].ToString(), 250, ref yPos);
            AddInfoRow(tabPersonal, "Marital Status:", student["marital_status"].ToString(), 500, ref yPos);

            yPos += 10;
            AddInfoRow(tabPersonal, "Citizenship:", student["citizenship"].ToString(), 20, ref yPos);
            AddInfoRow(tabPersonal, "Religion:", student["religion"].ToString(), 250, ref yPos);

            yPos += 30;
            AddInfoRow(tabPersonal, "Contact Number:", student["contact"].ToString(), 20, ref yPos);
            AddInfoRow(tabPersonal, "Email:", student["email"].ToString(), 250, ref yPos);

            yPos += 30;
            AddInfoRow(tabPersonal, "Address:", student["address"].ToString(), 20, ref yPos, 600);

            // ===== FAMILY INFORMATION TAB =====
            yPos = 20;

            // Father Information
            AddSectionHeader(tabFamily, "FATHER'S INFORMATION", ref yPos);
            AddInfoRow(tabFamily, "Name:", student["father_name"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Occupation:", student["father_occupation"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Office:", student["father_office"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Contact:", student["father_contact"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Email:", student["father_email"].ToString(), 20, ref yPos, 400);

            yPos += 20;

            // Mother Information
            AddSectionHeader(tabFamily, "MOTHER'S INFORMATION", ref yPos);
            AddInfoRow(tabFamily, "Name:", student["mother_name"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Occupation:", student["mother_occupation"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Office:", student["mother_office"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Contact:", student["mother_contact"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Email:", student["mother_email"].ToString(), 20, ref yPos, 400);

            yPos += 20;

            // Guardian Information
            AddSectionHeader(tabFamily, "GUARDIAN'S INFORMATION", ref yPos);
            AddInfoRow(tabFamily, "Name:", student["guardian_name"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Relation:", student["guardian_relation"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Occupation:", student["guardian_occupation"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Contact:", student["guardian_contact"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabFamily, "Email:", student["guardian_email"].ToString(), 20, ref yPos, 400);

            yPos += 20;

            // Siblings Information
            AddSectionHeader(tabFamily, "SIBLINGS INFORMATION", ref yPos);
            AddInfoRow(tabFamily, "Has Siblings:", student["has_siblings"].ToString(), 20, ref yPos, 400);

            // ===== EDUCATIONAL BACKGROUND TAB =====
            yPos = 20;

            AddInfoRow(tabEducation, "Previously Enrolled:", student["previously_enrolled"].ToString(), 20, ref yPos, 600);

            if (student["previously_enrolled"].ToString().ToLower() == "yes")
            {
                AddInfoRow(tabEducation, "Previous School:", student["prev_school"].ToString(), 20, ref yPos, 600);
                AddInfoRow(tabEducation, "Previous Grade:", student["prev_grade"].ToString(), 20, ref yPos, 600);
                AddInfoRow(tabEducation, "School Year:", student["prev_sy"].ToString(), 20, ref yPos, 600);
                AddInfoRow(tabEducation, "Previous School SY:", student["prev_school_sy"].ToString(), 20, ref yPos, 600);
                AddInfoRow(tabEducation, "School Address:", student["prev_school_addr"].ToString(), 20, ref yPos, 600);
            }

            // ===== HEALTH & EMERGENCY TAB =====
            yPos = 20;

            AddInfoRow(tabHealth, "Health Conditions:", student["health_conditions"].ToString(), 20, ref yPos, 600);

            yPos += 30;
            AddSectionHeader(tabHealth, "EMERGENCY CONTACT", ref yPos);
            AddInfoRow(tabHealth, "Name:", student["emergency_name"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabHealth, "Contact:", student["emergency_contact"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabHealth, "Address:", student["emergency_address"].ToString(), 20, ref yPos, 400);

            yPos += 30;
            AddSectionHeader(tabHealth, "ENROLLMENT DETAILS", ref yPos);
            AddInfoRow(tabHealth, "School Year:", student["school_year"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tabHealth, "Date Created:", Convert.ToDateTime(student["date_created"]).ToString("MM/dd/yyyy hh:mm tt"), 20, ref yPos, 400);

            // Close Button
            Button closeButton = new Button();
            closeButton.Text = "Close";
            closeButton.Size = new Size(100, 35);
            closeButton.Location = new Point(860, 640);
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

        private void CbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbSearch.Clear();
            string selectedStatus = cbStatusFilter.SelectedItem?.ToString() ?? "Pending";
            LoadStudentData(selectedStatus);
        }

        private void tbSearch_TextChanged_1(object sender, EventArgs e)
        {
            string filterText = tbSearch.Text.ToLower();

            foreach (DataGridViewRow row in dtgEnrollees.Rows)
            {
                if (row.Cells.Count < 3) continue;

                string studentNumber = row.Cells[1].Value?.ToString().ToLower() ?? "";
                string levelApplied = row.Cells[3].Value?.ToString().ToLower() ?? "";
                string fullName = row.Cells[2].Value?.ToString().ToLower() ?? "";

                bool visible = studentNumber.Contains(filterText) || fullName.Contains(filterText) || levelApplied.Contains(filterText);
                row.Visible = visible;
            }
        }
    }
}