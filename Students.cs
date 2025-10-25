using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class Students : UserControl
    {
        // Fixed dimensions for student photos
        private const int PHOTO_WIDTH = 60;
        private const int PHOTO_HEIGHT = 60;
        private int currentPage = 1;
        private int pageSize = 10; // number of students per page
        private int totalRecords = 0;
        private int totalPages = 0;

        public Students()
        {
            InitializeComponent();
        }

        private void Students_Load(object sender, EventArgs e)
        {
            cbGradeLevel.SelectedIndex = 0;
            LoadStudentData();

            // Set cursor to hand pointer for photo column
            dtgStudents.CellMouseEnter += DtgStudents_CellMouseEnter;
            dtgStudents.CellMouseLeave += DtgStudents_CellMouseLeave;
        }

        private void DtgStudents_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0) // Photo column
            {
                dtgStudents.Cursor = Cursors.Hand;
            }
        }

        private void DtgStudents_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dtgStudents.Cursor = Cursors.Default;
        }

        private void LoadStudentData(string gradeFilter = "All", string searchKeyword = "")
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Count total records first
                    string countQuery = @"SELECT COUNT(*) FROM students WHERE 1=1";
                    if (gradeFilter != "All") countQuery += " AND COALESCE(level_applied, grade_level) = @gradeFilter";
                    if (!string.IsNullOrWhiteSpace(searchKeyword)) countQuery += " AND (surname LIKE @search OR first_name LIKE @search OR middle_name LIKE @search)";

                    using (MySqlCommand countCmd = new MySqlCommand(countQuery, conn))
                    {
                        if (gradeFilter != "All") countCmd.Parameters.AddWithValue("@gradeFilter", gradeFilter);
                        if (!string.IsNullOrWhiteSpace(searchKeyword)) countCmd.Parameters.AddWithValue("@search", "%" + searchKeyword + "%");
                        totalRecords = Convert.ToInt32(countCmd.ExecuteScalar());
                    }

                    totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                    int offset = (currentPage - 1) * pageSize;

                    // Actual data query
                    string query = @"SELECT 
                COALESCE(student_number, lrn, application_no) AS id,
                COALESCE(level_applied, grade_level) AS level_applied,
                COALESCE(contact, cellphone, telephone) AS contact,
                CONCAT(surname, ' ', first_name, ' ', middle_name) AS name,
                gender, age, strand,
                COALESCE(payment_status, 'Not Paid') AS payment_status,
                student_photo,
                id_photo_filename
            FROM students
            WHERE 1=1";

                    if (gradeFilter != "All") query += " AND COALESCE(level_applied, grade_level) = @gradeFilter";
                    if (!string.IsNullOrWhiteSpace(searchKeyword)) query += " AND (surname LIKE @search OR first_name LIKE @search OR middle_name LIKE @search)";
                    query += " LIMIT @pageSize OFFSET @offset";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (gradeFilter != "All") cmd.Parameters.AddWithValue("@gradeFilter", gradeFilter);
                        if (!string.IsNullOrWhiteSpace(searchKeyword)) cmd.Parameters.AddWithValue("@search", "%" + searchKeyword + "%");
                        cmd.Parameters.AddWithValue("@pageSize", pageSize);
                        cmd.Parameters.AddWithValue("@offset", offset);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dtgStudents.Rows.Clear();
                            while (reader.Read())
                            {
                                Image studentPhoto = LoadStudentPhoto(reader);

                                dtgStudents.Rows.Add(
                                    studentPhoto,
                                    reader["id"].ToString(),
                                    reader["name"].ToString(),
                                    reader["level_applied"].ToString(),
                                    reader["gender"].ToString(),
                                    reader["age"].ToString(),
                                    reader["contact"].ToString(),
                                    reader["strand"] != DBNull.Value ? reader["strand"].ToString() : "N/A",
                                    reader["payment_status"].ToString()
                                );
                            }
                        }
                    }

                    UpdatePaginationLabel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Load student photo from database and resize to fixed dimensions
        /// </summary>
        private Image LoadStudentPhoto(MySqlDataReader reader)
        {
            try
            {
                Image originalImage = null;

                // First try to get student_photo
                if (reader["student_photo"] != DBNull.Value)
                {
                    byte[] imageBytes = (byte[])reader["student_photo"];
                    originalImage = ByteArrayToImage(imageBytes);
                }
                // If student_photo is not available, try id_photo_filename
                else if (reader["id_photo_filename"] != DBNull.Value)
                {
                    byte[] imageBytes = (byte[])reader["id_photo_filename"];
                    originalImage = ByteArrayToImage(imageBytes);
                }

                if (originalImage != null)
                {
                    // Resize image to fixed dimensions
                    Image resizedImage = ResizeImage(originalImage, PHOTO_WIDTH, PHOTO_HEIGHT);
                    originalImage.Dispose(); // Dispose the original to free memory
                    return resizedImage;
                }

                // If no photo in database, return default image
                return GetDefaultImage();
            }
            catch (Exception ex)
            {
                // Log error and return default image
                Console.WriteLine("Error loading student photo: " + ex.Message);
                return GetDefaultImage();
            }
        }

        /// <summary>
        /// Resize image to fixed dimensions while maintaining aspect ratio
        /// </summary>
        private Image ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
                {
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        /// <summary>
        /// Convert byte array from database BLOB to Image
        /// </summary>
        private Image ByteArrayToImage(byte[] byteArray)
        {
            try
            {
                if (byteArray == null || byteArray.Length == 0)
                    return null;

                using (MemoryStream ms = new MemoryStream(byteArray))
                {
                    return Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error converting byte array to image: " + ex.Message);
                return GetDefaultImage();
            }
        }

        /// <summary>
        /// Get default no-image photo with fixed size
        /// </summary>
        private Image GetDefaultImage()
        {
            try
            {
                string defaultImagePath = Path.Combine(Application.StartupPath, "Resources", "no-image.png");
                if (File.Exists(defaultImagePath))
                {
                    Image defaultImage = Image.FromFile(defaultImagePath);
                    return ResizeImage(defaultImage, PHOTO_WIDTH, PHOTO_HEIGHT);
                }

                // If file doesn't exist, create a simple placeholder image with fixed size
                Bitmap placeholder = new Bitmap(PHOTO_WIDTH, PHOTO_HEIGHT);
                using (Graphics g = Graphics.FromImage(placeholder))
                {
                    g.Clear(Color.LightGray);
                    using (Font font = new Font("Arial", 8))
                    using (StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        g.DrawString("No Image", font, Brushes.Black, new RectangleF(0, 0, PHOTO_WIDTH, PHOTO_HEIGHT), sf);
                    }
                }
                return placeholder;
            }
            catch
            {
                // Final fallback - create a basic bitmap with fixed size
                return new Bitmap(PHOTO_WIDTH, PHOTO_HEIGHT);
            }
        }

        /// <summary>
        /// Get full-size original photo from database
        /// </summary>
        private Image GetFullSizePhoto(string studentID)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT student_photo, id_photo_filename 
                                   FROM students 
                                   WHERE COALESCE(student_number, lrn, application_no) = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", studentID);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // First try to get student_photo
                                if (reader["student_photo"] != DBNull.Value)
                                {
                                    byte[] imageBytes = (byte[])reader["student_photo"];
                                    return ByteArrayToImage(imageBytes);
                                }
                                // If student_photo is not available, try id_photo_filename
                                else if (reader["id_photo_filename"] != DBNull.Value)
                                {
                                    byte[] imageBytes = (byte[])reader["id_photo_filename"];
                                    return ByteArrayToImage(imageBytes);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading full-size photo: " + ex.Message);
                }
            }
            return null;
        }

        /// <summary>
        /// Show full-size photo in a modal dialog
        /// </summary>
        private void ShowPhotoViewer(string studentID, string studentName)
        {
            Image fullPhoto = GetFullSizePhoto(studentID);

            if (fullPhoto == null)
            {
                MessageBox.Show("No photo available for this student.", "No Photo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Create a form to display the photo
            Form photoViewer = new Form
            {
                Text = $"Student Photo - {studentName}",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White,
                Size = new Size(Math.Min(fullPhoto.Width + 40, 800), Math.Min(fullPhoto.Height + 80, 600))
            };

            PictureBox pictureBox = new PictureBox
            {
                Image = fullPhoto,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill
            };

            photoViewer.Controls.Add(pictureBox);

            // Dispose the image when form closes
            photoViewer.FormClosed += (s, e) => {
                pictureBox.Image = null;
                fullPhoto?.Dispose();
            };

            photoViewer.ShowDialog();
        }

        // Declare an event
        public event Action<string, string> GradesRequested;

        private void dtgStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string studentID = dtgStudents.Rows[e.RowIndex].Cells["id"].Value?.ToString();
            string name = dtgStudents.Rows[e.RowIndex].Cells["name"].Value?.ToString();
            string gradeLevel = dtgStudents.Rows[e.RowIndex].Cells["Grades"].Value?.ToString();
            string columnName = dtgStudents.Columns[e.ColumnIndex].Name;
            string strand = dtgStudents.Rows[e.RowIndex].Cells["strand"].Value?.ToString();

            // Check if photo column was clicked (first column, index 0)
            if (e.ColumnIndex == 0)
            {
                ShowPhotoViewer(studentID, name);
                return;
            }

            if (columnName == "btnConfirm")
            {
                EditStudent(studentID);
            }
            else if (columnName == "btnDelete")
            {
                DeleteStudent(studentID, name);
            }
            else if (columnName == "btnGrades")
            {
                Form2 parentForm = this.ParentForm as Form2;
                parentForm?.LoadGradingForm(studentID, name, gradeLevel, strand);
            }
            else if (columnName == "btnPayment")
            {
                UpdatePaymentStatus(studentID, name);
            }
            else if (columnName == "view_info") // ✅ NEW: Handle View Info button
            {
                ViewStudentDetails(studentID, gradeLevel);
            }
        }

        // ✅ NEW METHOD: Determine student type and show appropriate detail form
        private void ViewStudentDetails(string studentID, string gradeLevel)
        {
            if (string.IsNullOrEmpty(studentID))
            {
                MessageBox.Show("Invalid student record.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Determine student type based on grade level
            string studentType = GetStudentType(studentID, gradeLevel);

            if (string.IsNullOrEmpty(studentType))
            {
                MessageBox.Show("Could not determine student type.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Control parentContainer = this.Parent;
            if (parentContainer == null) return;

            UserControl detailsForm = null;

            // Create the appropriate detail form based on student type
            switch (studentType)
            {
                case "Basic Ed":
                    detailsForm = new StudentDetailsFormBasic(studentID);
                    if (detailsForm is StudentDetailsFormBasic basicForm)
                    {
                        basicForm.BackButtonClicked += (s, ev) => ShowStudentsForm();
                    }
                    break;

                case "Junior High":
                    detailsForm = new StudentDetailsFormJHS(studentID);
                    break;

                case "Senior High":
                    detailsForm = new StudentDetailsFormSHS();
                    ((StudentDetailsFormSHS)detailsForm).StudentNumber = studentID;
                    ((StudentDetailsFormSHS)detailsForm).BackButtonClicked += (s, ev) => ShowStudentsForm();
                    break;

                default:
                    MessageBox.Show("Unknown student type.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            // Hide current Students form
            this.Hide();

            // Show the detail form
            detailsForm.Dock = DockStyle.Fill;
            parentContainer.Controls.Add(detailsForm);
            detailsForm.BringToFront();
        }

        // ✅ NEW METHOD: Determine which table the student belongs to
        private string GetStudentType(string studentID, string gradeLevel)
        {
            // First, try to determine by grade level
            if (!string.IsNullOrEmpty(gradeLevel))
            {
                if (gradeLevel.Contains("Nursery") || gradeLevel.Contains("Kinder 1") ||
                    gradeLevel.Contains("Kinder 2") || gradeLevel.Contains("Grade 1") ||
                    gradeLevel.Contains("Grade 2") || gradeLevel.Contains("Grade 3") ||
                    gradeLevel.Contains("Grade 4") || gradeLevel.Contains("Grade 5") ||
                    gradeLevel.Contains("Grade 6"))
                {
                    return "Basic Ed";
                }
                else if (gradeLevel.Contains("Grade 7") || gradeLevel.Contains("Grade 8") ||
                         gradeLevel.Contains("Grade 9") || gradeLevel.Contains("Grade 10"))
                {
                    return "Junior High";
                }
                else if (gradeLevel.Contains("Grade 11") || gradeLevel.Contains("Grade 12"))
                {
                    return "Senior High";
                }
            }

            // If grade level is unclear, check which source table has this student
            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Check Basic Ed
                    string queryBasic = "SELECT COUNT(*) FROM basic_ed_enrollment WHERE student_number = @id OR lrn = @id";
                    using (MySqlCommand cmd = new MySqlCommand(queryBasic, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", studentID);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count > 0) return "Basic Ed";
                    }

                    // Check JHS
                    string queryJHS = "SELECT COUNT(*) FROM jhs_enrollments WHERE student_number = @id OR lrn = @id";
                    using (MySqlCommand cmd = new MySqlCommand(queryJHS, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", studentID);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count > 0) return "Junior High";
                    }

                    // Check SHS
                    string querySHS = "SELECT COUNT(*) FROM shs_enrollments WHERE student_number = @id OR application_no = @id";
                    using (MySqlCommand cmd = new MySqlCommand(querySHS, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", studentID);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count > 0) return "Senior High";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error determining student type: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return null;
        }

        // ✅ NEW METHOD: Return to Students form
        private void ShowStudentsForm()
        {
            Control parentContainer = this.Parent;
            if (parentContainer == null) return;

            // Remove all controls from parent
            parentContainer.Controls.Clear();

            // Add this Students form back
            this.Dock = DockStyle.Fill;
            parentContainer.Controls.Add(this);
            this.Show();

            // Refresh the data
            LoadStudentData();
        }

        private void EditStudent(string studentID)
        {
            Form parentForm = this.FindForm();
            OverlayForm overlay = new OverlayForm(parentForm);
            overlay.Show();
            EditStudent editForm = new EditStudent(studentID);
            editForm.ShowDialog();
            LoadStudentData();
            overlay.Dispose();
        }

        private void DeleteStudent(string studentID, string name)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this student: " + name, "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM students WHERE COALESCE(student_number, lrn, application_no) = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", studentID);
                        cmd.ExecuteNonQuery();
                    }
                    LoadStudentData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void UpdatePaymentStatus(string studentID, string name)
        {
            string newStatus = "";

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    // Step 1: Get current payment status
                    string currentStatus = "Not Paid";
                    string getStatusQuery = "SELECT COALESCE(payment_status, 'Not Paid') AS current_status FROM students WHERE COALESCE(student_number, lrn, application_no) = @id";

                    using (MySqlCommand getCmd = new MySqlCommand(getStatusQuery, conn))
                    {
                        getCmd.Parameters.AddWithValue("@id", studentID);
                        using (MySqlDataReader reader = getCmd.ExecuteReader())
                        {
                            if (reader.Read())
                                currentStatus = reader["current_status"].ToString();
                        }
                    }

                    // Step 2: Toggle payment status
                    newStatus = currentStatus == "Paid" ? "Not Paid" : "Paid";

                    // Step 3: Update payment status
                    string updateQuery = "UPDATE students SET payment_status = @status WHERE COALESCE(student_number, lrn, application_no) = @id";
                    using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@status", newStatus);
                        updateCmd.Parameters.AddWithValue("@id", studentID);
                        updateCmd.ExecuteNonQuery();
                    }

                    // Step 4: Forcefully close connection to kill MySQL process immediately
                    conn.Close();
                }

                // Step 5: Reload data AFTER connection is fully closed
                MessageBox.Show($"Payment status for {name} has been updated to: {newStatus}",
                    "Payment Status Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reload only after connection is disposed
                GC.Collect();
                GC.WaitForPendingFinalizers();

                LoadStudentData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating payment status: " + ex.Message);
            }
        }


        private void cbGradeLevel_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            tbSearch.Clear();
            string selectedGrade = cbGradeLevel.SelectedItem.ToString();
            string searchKeyword = tbSearch.Text.Trim();
            LoadStudentData(selectedGrade, searchKeyword);
        }

        private void tbSearch_TextChanged_1(object sender, EventArgs e)
        {
            string selectedGrade = cbGradeLevel.SelectedItem?.ToString() ?? "All";
            string searchKeyword = tbSearch.Text.Trim();

            if (selectedGrade != "All")
            {
                // Use SQL filter for specific grade
                LoadStudentData(selectedGrade, searchKeyword);
            }
            else
            {
                // Just filter existing rows in DataGridView
                string search = searchKeyword.ToLower();

                foreach (DataGridViewRow row in dtgStudents.Rows)
                {
                    bool visible =
                        row.Cells["Grades"].Value.ToString().ToLower().Contains(search) ||
                        row.Cells["strand"].Value.ToString().ToLower().Contains(search) ||
                        row.Cells["name"].Value.ToString().ToLower().Contains(search) ||
                        row.Cells["payment_status"].Value.ToString().ToLower().Contains(search);

                    row.Visible = visible;
                }
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadStudentData(cbGradeLevel.SelectedItem?.ToString() ?? "All", tbSearch.Text.Trim());
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadStudentData(cbGradeLevel.SelectedItem?.ToString() ?? "All", tbSearch.Text.Trim());
            }
        }

        private void UpdatePaginationLabel()
        {
            lblPageInfo.Text = $"Page {currentPage} of {totalPages} ({totalRecords} records)";
            btnPrev.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;
        }
    }
}