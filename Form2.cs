using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CFCA_ADMIN
{
    public partial class Form2 : Form
    {
        private string _displayName;
        private byte[] _imageData;
        private string role;

        public Form2(string displayName, byte[] imageData, string role)
        {
            InitializeComponent();
            _displayName = displayName;
            _imageData = imageData;
            this.role = role;
        }

        private void LoadControl(System.Windows.Forms.UserControl uc)
        {
            panelContainer.Controls.Clear();       // Clear previous controls
            uc.Dock = DockStyle.Fill;              // Make it fill the space
            panelContainer.Controls.Add(uc);       // Load new control
        }

        public void LoadGradingForm(string studentID, string name, string gradeLevel, string strand)
        {
            lblAdmin.Text = "Student Grades"; // Update the header label
            LoadControl(new Gradingform(studentID, name, gradeLevel, strand));
        }

        // New method to load EnrollmentControl
        public void LoadEnrollmentControl()
        {
            lblAdmin.Text = "Enrollment";
            LoadControl(new EnrollmentControl());
        }

        private void SetNewEnrolleesArrowRight()
        {
            btnNewEnrollees.CustomImages = new Guna.UI2.WinForms.Suite.ButtonImages
            {
                Image = Properties.Resources.arrow_right
            };
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            lblAdmin.Text = btnDashboard.Text; // Set the label text to the button text
            LoadControl(new dashboard());
            HideSubMenu();
            SetNewEnrolleesArrowRight();
        }

        private void btnInstructor_Click(object sender, EventArgs e)
        {
            lblAdmin.Text = btnInstructor.Text; // Set the label text to the button text
            LoadControl(new instructors());
            HideSubMenu();
            SetNewEnrolleesArrowRight();
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            lblAdmin.Text = btnSchedule.Text;
            LoadControl(new schedule());
            HideSubMenu();
            SetNewEnrolleesArrowRight();
        }

        private bool isSubMenuVisible = false;

        private void btnNewEnrollees_Click_1(object sender, EventArgs e)
        {
            lblAdmin.Text = btnBasicEd.Text; // Set the label text to the button text

            // Check the fill color of the button
            Color targetColor = Color.FromArgb(42, 130, 254);
            bool isBlueFill = btnNewEnrollees.CheckedState.FillColor == targetColor;

            if (isSubMenuVisible)
            {
                // Collapse
                panelSubEnrollees.Height = 0;
                panelSubEnrollees.Visible = false;

                btnNewEnrollees.CustomImages = new Guna.UI2.WinForms.Suite.ButtonImages
                {
                    Image = isBlueFill ? Properties.Resources.arrow_right_white : Properties.Resources.arrow_right
                };
            }
            else
            {
                panelSubEnrollees.Height = panelSubEnrollees.Controls.Count * 46;
                panelSubEnrollees.Visible = true;
                btnNewEnrollees.CustomImages = new Guna.UI2.WinForms.Suite.ButtonImages
                {
                    Image = Properties.Resources.arrow_down
                };
            }

            isSubMenuVisible = !isSubMenuVisible;
            AdjustSidebarButtons();
            btnBasicEd.PerformClick();
        }

        private void AdjustSidebarButtons()
        {
            if (panelSubEnrollees.Visible)
            {
                btnStudents.Top = panelSubEnrollees.Bottom + 5;
                btnAdmin.Top = btnStudents.Bottom + 5;
                btnSubjects.Top = btnAdmin.Bottom + 5;
                btnEnrollment.Top = btnSubjects.Bottom + 5;
            }
            else
            {
                btnStudents.Top = btnNewEnrollees.Bottom + 5;
                btnAdmin.Top = btnStudents.Bottom + 5;
                btnSubjects.Top = btnAdmin.Bottom + 5;
                btnEnrollment.Top = btnSubjects.Bottom + 5;
            }

            // Position logout button below enrollment
            btnLogout.Top = btnEnrollment.Bottom + 10;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Admin_Load(object sender, EventArgs e)
        {
            lblName.Text = _displayName;
            lblRole.Text = role;
            if (_imageData != null && _imageData.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(_imageData))
                {
                    pic.Image = Image.FromStream(ms);
                }
            }
            panelSubEnrollees.Visible = false;
            panelSubEnrollees.Height = 0;
            AdjustSidebarButtons();
            btnBasicEd.Click += SubMenuButton_Click;
            btnJHS.Click += SubMenuButton_Click;
            btnSHS.Click += SubMenuButton_Click;
            btnDashboard.PerformClick(); // Load dashboard by default
        }

        private void SubMenuButton_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button clickedBtn = sender as Guna.UI2.WinForms.Guna2Button;
            if (clickedBtn != null)
            {
                lblAdmin.Text = clickedBtn.Text;

                // Load a user control based on the button
                if (clickedBtn == btnBasicEd)
                {
                    LoadControl(new Enrollees());
                }
                else if (clickedBtn == btnJHS)
                {
                    LoadControl(new Junior_High_School());
                }
                else if (clickedBtn == btnSHS)
                {
                    LoadControl(new Senior_High_School());
                }
            }
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            lblAdmin.Text = btnAdmin.Text; // Set the label text to the button text
            LoadControl(new Admin());
            HideSubMenu();
            SetNewEnrolleesArrowRight();
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                Form1 login = new Form1();
                Application.Exit();
            }
        }

        private void lblRole_Click(object sender, EventArgs e)
        {

        }

        private void panelContainer_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
            lblAdmin.Text = btnStudents.Text;
            LoadControl(new Students());
            HideSubMenu();
            SetNewEnrolleesArrowRight();
        }

        private void btnSubjects_Click(object sender, EventArgs e)
        {
            lblAdmin.Text = btnSubjects.Text;
            LoadControl(new Subjects());
            HideSubMenu();
            SetNewEnrolleesArrowRight();
        }

        private void btnEnrollment_Click(object sender, EventArgs e)
        {
            lblAdmin.Text = "Enrollment";
            LoadEnrollmentControl();
            HideSubMenu();
            SetNewEnrolleesArrowRight();
        }

        private void HideSubMenu()
        {
            panelSubEnrollees.Visible = false;
            panelSubEnrollees.Height = 0;
            isSubMenuVisible = false;
            AdjustSidebarButtons();
        }
    }
}