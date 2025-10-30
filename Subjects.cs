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
        }

        public void subjects()
        {
            using (var conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM subjects";
                    using (var cmd = new MySqlCommand(query, conn))
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

                    // ✅ Reapply filter after loading data
                    ApplyFilters();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private bool _suppressSearchEvent = false;

        private void dtgSubjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Ignore header clicks

            string columnName = dtgSubjects.Columns[e.ColumnIndex].Name;
            DataGridViewRow row = dtgSubjects.Rows[e.RowIndex];

            // Get subject data from visible columns
            string gradeLevel = row.Cells["grade_level"].Value?.ToString();
            string subjectName = row.Cells["subject_name"].Value?.ToString();

            if (string.IsNullOrEmpty(gradeLevel) || string.IsNullOrEmpty(subjectName))
                return;

            if (columnName == "btnEdit")
            {
                EditSubject(gradeLevel, subjectName);
            }
            else if (columnName == "btnDelete")
            {
                DeleteSubject(gradeLevel, subjectName);
            }
        }

        private void EditSubject(string gradeLevel, string subjectName)
        {
            // ✅ Save current filter state
            string currentFilter = cbGradeLevel.SelectedItem?.ToString();

            // Open edit form with existing data
            Form parentform = this.FindForm();
            OverlayForm overlay = new OverlayForm(parentform);
            overlay.Show();

            add_Subject editSubject = new add_Subject(gradeLevel, subjectName);
            editSubject.ShowDialog();

            overlay.Dispose();
            subjects(); // Refresh the list

            // ✅ Restore filter state
            if (!string.IsNullOrEmpty(currentFilter))
            {
                cbGradeLevel.SelectedItem = currentFilter;
            }
        }

        private void DeleteSubject(string gradeLevel, string subjectName)
        {
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete '{subjectName}' for {gradeLevel}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                using (var conn = Database.GetConnection())
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM subjects WHERE grade_level = @grade AND subject_name = @subject";
                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@grade", gradeLevel);
                            cmd.Parameters.AddWithValue("@subject", subjectName);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Subject deleted successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                subjects(); // Refresh the list (filter is reapplied automatically)
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete subject.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting subject: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void tbSearch_TextChanged_1(object sender, EventArgs e)
        {
            if (_suppressSearchEvent) return; // Skip if triggered by grade level change

            // ✅ DON'T reset to "All" - keep current grade level filter
            // cbGradeLevel.SelectedIndex = 0; // ← REMOVED THIS LINE

            // ✅ Apply both filters together
            ApplyFilters();
        }

        private void btnAddSubject_Click_1(object sender, EventArgs e)
        {
            Form parentform = this.FindForm();
            OverlayForm overlay = new OverlayForm(parentform);
            overlay.Show();
            add_Subject addSubject = new add_Subject();
            addSubject.ShowDialog();
            overlay.Dispose();
            subjects(); // Filter is reapplied automatically
        }

        private void cbGradeLevel_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // Temporarily disable search box event
            _suppressSearchEvent = true;
            tbSearch.Clear(); // Clear search when changing grade level
            _suppressSearchEvent = false;

            ApplyFilters();
        }

        // ✅ Combined method to apply both grade level AND search filters
        private void ApplyFilters()
        {
            string selectedGrade = cbGradeLevel.SelectedItem?.ToString();
            string searchText = tbSearch.Text.ToLower();

            foreach (DataGridViewRow row in dtgSubjects.Rows)
            {
                string gradeLevel = row.Cells["grade_level"].Value?.ToString();
                string subjectName = row.Cells["subject_name"].Value?.ToString();

                // ✅ Check grade level filter
                bool matchesGrade = (selectedGrade == "All" || string.IsNullOrEmpty(selectedGrade) || gradeLevel == selectedGrade);

                // ✅ Check search filter
                bool matchesSearch = string.IsNullOrEmpty(searchText) ||
                                    subjectName.ToLower().Contains(searchText) ||
                                    gradeLevel.ToLower().Contains(searchText);

                // ✅ Show row only if it matches BOTH filters
                row.Visible = matchesGrade && matchesSearch;
            }
        }
    }
}