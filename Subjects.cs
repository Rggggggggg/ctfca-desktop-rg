using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class Subjects : UserControl
    {
        public Subjects()
        {
            InitializeComponent();
        }

        private void Subjects_Load(object sender, EventArgs e)
        {
            subjects();
            cbGradeLevel.SelectedIndex = 0; 
            EditSubject();
            DeleteSubject();
        }

        private void subjects()
        {
            using (var conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM subjects";
                    using (var cmd = new MySqlCommand(query,conn))
                    {
                      using (var reader = cmd.ExecuteReader())
                        {
                            dtgSubjects.Rows.Clear();
                            while (reader.Read())
                            {
                                dtgSubjects.Rows.Add(
                                    reader["grade_level"].ToString(),
                                    reader["subject_name"].ToString()
                                    );
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private bool _suppressSearchEvent = false;

        private void cbGradeLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedGrade = cbGradeLevel.SelectedItem?.ToString();

            // Temporarily disable search box event
            _suppressSearchEvent = true;
            tbSearch.Clear();
            _suppressSearchEvent = false;

            foreach (DataGridViewRow row in dtgSubjects.Rows)
            {
                if (selectedGrade == "All")
                {
                    row.Visible = true;
                }
                else
                {
                    string gradelevel = row.Cells["grade_level"].Value?.ToString();
                    row.Visible = gradelevel == selectedGrade;
                }
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            if (_suppressSearchEvent) return; // Skip if triggered by grade level change

            cbGradeLevel.SelectedIndex = 0; // Force "All" when searching
            string search = tbSearch.Text.ToLower();

            foreach (DataGridViewRow row in dtgSubjects.Rows)
            {
                bool visible = row.Cells["subject_name"].Value.ToString().ToLower().Contains(search) ||
                               row.Cells["grade_level"].Value.ToString().ToLower().Contains(search);

                row.Visible = visible;
            }
        }

        private void btnAddSubject_Click(object sender, EventArgs e)
        {
            Form parentform = this.FindForm();
            OverlayForm overlay = new OverlayForm(parentform);
            overlay.Show();
            add_Subject addSubject = new add_Subject();
            addSubject.ShowDialog();
            overlay.Dispose();
            subjects();
        }

        private void dtgSubjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void EditSubject()
        {

        }

        private void DeleteSubject()
        {

        }
    }
}
