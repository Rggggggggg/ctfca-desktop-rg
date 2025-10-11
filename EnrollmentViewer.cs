using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CAPSTONE_PROJ
{
    public partial class EnrollmentViewer : Form
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlDataAdapter adapter;
        DataTable dt;

        public EnrollmentViewer()
        {
            InitializeComponent();
            conn = new MySqlConnection("server=sql12.freesqldatabase.com;user=sql12788974;password=RSVXSTAU5I;database=sql12788974;");
        }

        private void EnrollmentViewer_Load(object sender, EventArgs e)
        {
            cbDepartmentFilter.Items.AddRange(new string[] { "All", "Basic Education", "Junior High School", "Senior High School" });
            cbDepartmentFilter.SelectedIndex = 0;
            LoadEnrollees("All");
        }

        private void cbDepartmentFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEnrollees(cbDepartmentFilter.SelectedItem.ToString());
        }

        private void LoadEnrollees(string department)
        {
            string query = "";
            switch (department)
            {
                case "Basic Education":
                    query = "SELECT id, student_number, CONCAT(first_name, ' ', middle_name, ' ', surname) AS full_name, level_applied AS year_level, enrollment_status FROM basic_ed_enrollment ORDER BY id DESC";
                    break;
                case "Junior High School":
                    query = "SELECT id, student_number, CONCAT(first_name, ' ', middle_name, ' ', surname) AS full_name, level_applied AS year_level, enrollment_status FROM jhs_enrollments ORDER BY id DESC";
                    break;
                case "Senior High School":
                    query = "SELECT id, student_number, CONCAT(first_name, ' ', middle_name, ' ', surname) AS full_name, strand AS year_level, enrollment_status FROM shs_enrollments ORDER BY id DESC";
                    break;
                default: // All
                    query = @"
                        SELECT id, student_number, CONCAT(first_name, ' ', middle_name, ' ', surname) AS full_name, level_applied AS year_level, 'Basic Education' AS department, enrollment_status FROM basic_ed_enrollment
                        UNION ALL
                        SELECT id, student_number, CONCAT(first_name, ' ', middle_name, ' ', surname) AS full_name, level_applied AS year_level, 'Junior High School' AS department, enrollment_status FROM jhs_enrollments
                        UNION ALL
                        SELECT id, student_number, CONCAT(first_name, ' ', middle_name, ' ', surname) AS full_name, strand AS year_level, 'Senior High School' AS department, enrollment_status FROM shs_enrollments
                        ORDER BY student_number DESC";
                    break;
            }

            adapter = new MySqlDataAdapter(query, conn);
            dt = new DataTable();
            adapter.Fill(dt);
            dtgEnrollees.DataSource = dt;

            // Add View Info button if not already added
            if (!dtgEnrollees.Columns.Contains("ViewInfo"))
            {
                DataGridViewButtonColumn viewBtn = new DataGridViewButtonColumn();
                viewBtn.Name = "ViewInfo";
                viewBtn.HeaderText = "Action";
                viewBtn.Text = "View Info";
                viewBtn.UseColumnTextForButtonValue = true;
                dtgEnrollees.Columns.Add(viewBtn);
            }
        }

        private void dtgEnrollees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dtgEnrollees.Columns["ViewInfo"].Index)
            {
                string department = cbDepartmentFilter.SelectedItem.ToString();
                if (department == "All")
                {
                    // Try to detect department based on student_number prefix or use popup
                    MessageBox.Show("Please select a specific department to view details.");
                    return;
                }

                string studentNumber = dtgEnrollees.Rows[e.RowIndex].Cells["student_number"].Value.ToString();
                LoadStudentDetails(department, studentNumber);
            }
        }

        private void LoadStudentDetails(string department, string studentNumber)
        {
            string query = "";
            switch (department)
            {
                case "Basic Education":
                    query = $"SELECT * FROM basic_ed_enrollment WHERE student_number = '{studentNumber}'";
                    break;
                case "Junior High School":
                    query = $"SELECT * FROM jhs_enrollments WHERE student_number = '{studentNumber}'";
                    break;
                case "Senior High School":
                    query = $"SELECT * FROM shs_enrollments WHERE student_number = '{studentNumber}'";
                    break;
            }

            adapter = new MySqlDataAdapter(query, conn);
            DataTable dtDetails = new DataTable();
            adapter.Fill(dtDetails);

            if (dtDetails.Rows.Count > 0)
            {
                dtgDetails.DataSource = dtDetails;
                lblSelectedStudent.Text = $"Details for {studentNumber} ({department})";
            }
            else
            {
                MessageBox.Show("No details found for this student.");
            }
        }
    }
}
