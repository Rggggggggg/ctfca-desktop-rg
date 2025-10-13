using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class StudentDetailsForm : Form
    {
        private DataRow student;
        private DataTable siblings;
        private int yPos = 20; // keep track of vertical layout position

        public StudentDetailsForm(DataRow studentRow, DataTable siblingTable)
        {
            InitializeComponent();
            student = studentRow;
            siblings = siblingTable;
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = $"Student Information - {student["student_number"]}";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9F);
            this.Icon = SystemIcons.Information;

            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };

            // Header
            Panel headerPanel = BuildHeaderPanel();

            // Tabs
            TabControl tabs = BuildTabs();

            // Close button
            Button closeButton = new Button
            {
                Text = "Close",
                Size = new Size(100, 35),
                Location = new Point(860, 640),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat
            };
            closeButton.Click += (s, e) => this.Close();

            mainPanel.Controls.Add(headerPanel);
            mainPanel.Controls.Add(tabs);
            mainPanel.Controls.Add(closeButton);

            this.Controls.Add(mainPanel);
        }

        private Panel BuildHeaderPanel()
        {
            Panel headerPanel = new Panel { Size = new Size(940, 80) };

            PictureBox photo = new PictureBox
            {
                Size = new Size(70, 70),
                Location = new Point(0, 5),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Load student photo
            if (student["student_photo"] != DBNull.Value)
            {
                try
                {
                    byte[] photoBytes = (byte[])student["student_photo"];
                    using (var ms = new System.IO.MemoryStream(photoBytes))
                        photo.Image = Image.FromStream(ms);
                }
                catch { photo.Image = Properties.Resources.no_image; }
            }
            else
            {
                photo.Image = Properties.Resources.no_image;
            }

            photo.Cursor = Cursors.Hand;
            photo.Click += (s, e) =>
            {
                if (student["student_photo"] != DBNull.Value)
                    ShowFullSizePhotoFromBytes((byte[])student["student_photo"]);
            };

            Label nameLabel = new Label
            {
                Text = $"{student["surname"]}, {student["first_name"]} {student["middle_name"]}",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 42, 57),
                Location = new Point(80, 10),
                AutoSize = true
            };

            Label subLabel = new Label
            {
                Text = $"Student #: {student["student_number"]} | LRN: {student["lrn"]} | Status: {student["enrollment_status"]}",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Location = new Point(80, 45),
                AutoSize = true
            };

            headerPanel.Controls.Add(photo);
            headerPanel.Controls.Add(nameLabel);
            headerPanel.Controls.Add(subLabel);

            return headerPanel;
        }

        private TabControl BuildTabs()
        {
            TabControl tabControl = new TabControl
            {
                Location = new Point(20, 120),
                Size = new Size(940, 500),
                Font = new Font("Segoe UI", 9F)
            };

            TabPage tabPersonal = new TabPage("Personal Info") { Padding = new Padding(20), AutoScroll = true };
            TabPage tabFamily = new TabPage("Family Info") { Padding = new Padding(20), AutoScroll = true };
            TabPage tabEducation = new TabPage("Education") { Padding = new Padding(20), AutoScroll = true };
            TabPage tabHealth = new TabPage("Health & Emergency") { Padding = new Padding(20), AutoScroll = true };

            FillPersonalTab(tabPersonal);
            FillFamilyTab(tabFamily);
            FillEducationTab(tabEducation);
            FillHealthTab(tabHealth);

            tabControl.Controls.Add(tabPersonal);
            tabControl.Controls.Add(tabFamily);
            tabControl.Controls.Add(tabEducation);
            tabControl.Controls.Add(tabHealth);

            return tabControl;
        }

        private void FillPersonalTab(TabPage tab)
        {
            yPos = 20;
            AddInfoRow(tab, "Level Applied:", student["level_applied"].ToString(), 20, ref yPos);
            AddInfoRow(tab, "Registrar Level:", student["level_for_registrar"].ToString(), 250, ref yPos);
            AddInfoRow(tab, "Gender:", student["gender"].ToString(), 500, ref yPos);

            yPos += 10;
            AddInfoRow(tab, "Age:", student["age"].ToString(), 20, ref yPos);
            AddInfoRow(tab, "Date of Birth:", Convert.ToDateTime(student["dob"]).ToString("MM/dd/yyyy"), 250, ref yPos);
            AddInfoRow(tab, "Place of Birth:", student["pob"].ToString(), 500, ref yPos);

            yPos += 10;
            AddInfoRow(tab, "Citizenship:", student["citizenship"].ToString(), 20, ref yPos);
            AddInfoRow(tab, "Religion:", student["religion"].ToString(), 250, ref yPos);

            yPos += 30;
            AddInfoRow(tab, "Contact Number:", student["contact"].ToString(), 20, ref yPos);
            AddInfoRow(tab, "Email:", student["email"].ToString(), 250, ref yPos);

            yPos += 30;
            AddInfoRow(tab, "Address:", student["address"].ToString(), 20, ref yPos, 600);
        }

        private void FillFamilyTab(TabPage tab)
        {
            yPos = 20;

            AddSectionHeader(tab, "FATHER'S INFORMATION", ref yPos);
            AddInfoRow(tab, "Name:", student["father_name"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tab, "Occupation:", student["father_occupation"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tab, "Office:", student["father_office"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tab, "Contact:", student["father_contact"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tab, "Email:", student["father_email"].ToString(), 20, ref yPos, 400);

            yPos += 20;
            AddSectionHeader(tab, "MOTHER'S INFORMATION", ref yPos);
            AddInfoRow(tab, "Name:", student["mother_name"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tab, "Occupation:", student["mother_occupation"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tab, "Office:", student["mother_office"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tab, "Contact:", student["mother_contact"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tab, "Email:", student["mother_email"].ToString(), 20, ref yPos, 400);

            yPos += 20;
            AddSectionHeader(tab, "GUARDIAN'S INFORMATION", ref yPos);
            AddInfoRow(tab, "Name:", student["guardian_name"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tab, "Relation:", student["guardian_relation"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tab, "Occupation:", student["guardian_occupation"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tab, "Contact:", student["guardian_contact"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tab, "Email:", student["guardian_email"].ToString(), 20, ref yPos, 400);

            yPos += 20;
            AddSectionHeader(tab, "SIBLINGS", ref yPos);

            if (siblings.Rows.Count > 0)
            {
                foreach (DataRow s in siblings.Rows)
                    AddInfoRow(tab, "•", $"{s["name"]} ({s["grade_occupation"]})", 30, ref yPos, 500);
            }
            else
            {
                AddInfoRow(tab, "", "No siblings recorded", 30, ref yPos, 500);
            }
        }

        private void FillEducationTab(TabPage tab)
        {
            yPos = 20;
            AddInfoRow(tab, "Previous School:", student["prev_school"].ToString(), 20, ref yPos, 600);
            AddInfoRow(tab, "Previous Grade:", student["prev_grade"].ToString(), 20, ref yPos, 600);
            AddInfoRow(tab, "School Year:", student["prev_sy"].ToString(), 20, ref yPos, 600);
            AddInfoRow(tab, "School Address:", student["prev_school_addr"].ToString(), 20, ref yPos, 600);
            yPos += 10;
            AddInfoRow(tab, "Previous CTFC-AI Student:", student["prev_ctfcai"].ToString(), 20, ref yPos, 600);
        }

        private void FillHealthTab(TabPage tab)
        {
            yPos = 20;
            AddInfoRow(tab, "Health Conditions:", student["health_conditions"].ToString(), 20, ref yPos, 600);

            yPos += 30;
            AddSectionHeader(tab, "EMERGENCY CONTACT", ref yPos);
            AddInfoRow(tab, "Name:", student["emergency_name"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tab, "Contact:", student["emergency_contact"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tab, "Address:", student["emergency_address"].ToString(), 20, ref yPos, 400);

            yPos += 30;
            AddSectionHeader(tab, "ENROLLMENT DETAILS", ref yPos);
            AddInfoRow(tab, "School Year:", student["school_year"].ToString(), 20, ref yPos, 400);
            AddInfoRow(tab, "Submitted:", Convert.ToDateTime(student["submitted_at"]).ToString("MM/dd/yyyy hh:mm tt"), 20, ref yPos, 400);
        }

        private void AddInfoRow(Control parent, string label, string value, int x, ref int y, int width = 200)
        {
            Label lbl = new Label
            {
                Text = label,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            Label val = new Label
            {
                Text = string.IsNullOrEmpty(value) ? "N/A" : value,
                Location = new Point(x + 150, y),
                Size = new Size(width, 20),
                AutoSize = false
            };

            parent.Controls.Add(lbl);
            parent.Controls.Add(val);
            y += 25;
        }

        private void AddSectionHeader(Control parent, string text, ref int y)
        {
            Label header = new Label
            {
                Text = text,
                Location = new Point(20, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 96, 144)
            };

            parent.Controls.Add(header);
            y += 30;
        }

        private void ShowFullSizePhotoFromBytes(byte[] photoBytes)
        {
            if (photoBytes == null || photoBytes.Length == 0)
            {
                MessageBox.Show("No photo available.", "Photo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var ms = new System.IO.MemoryStream(photoBytes))
            {
                Image img = Image.FromStream(ms);
                Form f = new Form
                {
                    Text = "Student Photo",
                    Size = new Size(600, 700),
                    StartPosition = FormStartPosition.CenterScreen
                };

                PictureBox pb = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    Image = img,
                    SizeMode = PictureBoxSizeMode.Zoom
                };

                f.Controls.Add(pb);
                f.ShowDialog();
            }
        }
    }
}
    