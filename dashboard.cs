using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class dashboard : UserControl
    {
        private LiveCharts.WinForms.CartesianChart cartesianChart;
        private LiveCharts.WinForms.PieChart pieChartControl;

        public dashboard()
        {
            InitializeComponent();

            cartesianChart = new LiveCharts.WinForms.CartesianChart
            {
                Dock = DockStyle.Fill
            };

            pieChartControl = new LiveCharts.WinForms.PieChart
            {
                Dock = DockStyle.Fill
            };
        }
        private void CenterLabelInPanel(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel != null && panel.Controls.Count > 0)
            {
                foreach (Control ctrl in panel.Controls)
                {
                    if (ctrl is Label lbl)
                    {
                        lbl.Left = (panel.Width - lbl.Width) / 2;
                        lbl.Top = (panel.Height - lbl.Height) / 2;
                    }
                }
            }
        }

        private void dashboard_Load(object sender, EventArgs e)
        {
            chartpanel.Controls.Add(cartesianChart);
            pieChart.Controls.Add(pieChartControl);
            TotalEnrollments();
            lblEnrollments.Text = TotalEnrollments().ToString("N0");
            TotalInstructors();
            lblTotalInstructor.Text = TotalInstructors().ToString("N0");
            TotalStudents();
            lblTotalStudents.Text = TotalStudents().ToString("N0");
            lblEnrolleesToday.Text = "+" + EnrolleesToday().ToString("N0");
            lblStudentsToday.Text = "+" + StudentsToday().ToString("N0");
            lblInstructorsToday.Text = "+" + InstructorsToday().ToString("N0");
            LoadEnrollmentData();

            // Center the labels after setting their text
            CenterLabel(lblEnrolleesToday, panelEnrollees);
            CenterLabel(lblStudentsToday, panelStudents);
            CenterLabel(lblInstructorsToday, panelInstructors);

        }

        // Replace the CenterLabel method to accept Control instead of Label
        private void CenterLabel(Control ctrl, Panel panel)
        {
            ctrl.Left = (panel.Width - ctrl.Width) / 2;
            ctrl.Top = (panel.Height - ctrl.Height) / 2;
        }


        private void LoadEnrollmentData()
        {
            int[] basicEdData = new int[12];
            int[] jhsData = new int[12];
            int[] shsData = new int[12];

            int totalBasicEd = 0;
            int totalJhs = 0;
            int totalShs = 0;

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                // --- Basic Ed ---
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT MONTH(submitted_at) AS m, COUNT(*) AS total FROM basic_ed_enrollment GROUP BY MONTH(submitted_at)", conn))
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        int month = rdr.GetInt32("m");
                        int total = rdr.GetInt32("total");
                        basicEdData[month - 1] = total;
                        totalBasicEd += total;
                    }
                }

                // --- JHS ---
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT MONTH(date_created) AS m, COUNT(*) AS total FROM jhs_enrollments GROUP BY MONTH(date_created)", conn))
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        int month = rdr.GetInt32("m");
                        int total = rdr.GetInt32("total");
                        jhsData[month - 1] = total;
                        totalJhs += total;
                    }
                }

                // --- SHS ---
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT MONTH(created_at) AS m, COUNT(*) AS total FROM shs_enrollments GROUP BY MONTH(created_at)", conn))
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        int month = rdr.GetInt32("m");
                        int total = rdr.GetInt32("total");
                        shsData[month - 1] = total;
                        totalShs += total;
                    }
                }
            }

            // --- Line Chart (Trends) ---
            cartesianChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Basic Ed",
                    Values = new ChartValues<int>(basicEdData)
                },
                new LineSeries
                {
                    Title = "JHS",
                    Values = new ChartValues<int>(jhsData)
                },
                new LineSeries
                {
                    Title = "SHS",
                    Values = new ChartValues<int>(shsData)
                }
            };

            cartesianChart.AxisX.Clear();
            cartesianChart.AxisX.Add(new Axis
            {
                Title = "Months",
                Labels = new[]
                {
                    "Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"
                }
            });

            cartesianChart.AxisY.Clear();
            cartesianChart.AxisY.Add(new Axis
            {
                Title = "Count",
                LabelFormatter = value => value.ToString("N0")
            });

            cartesianChart.LegendLocation = LegendLocation.Right;

            // --- Pie Chart (Percentages) ---
            pieChartControl.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Basic Ed",
                    Values = new ChartValues<int> { totalBasicEd },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "JHS",
                    Values = new ChartValues<int> { totalJhs },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "SHS",
                    Values = new ChartValues<int> { totalShs },
                    DataLabels = true
                }
            };

            pieChartControl.LegendLocation = LegendLocation.Right;
        }

        public int TotalEnrollments()
        {
            const string query = @"SELECT 
                (SELECT COUNT(*) FROM basic_ed_enrollment) +
                (SELECT COUNT(*) FROM jhs_enrollments) +
                (SELECT COUNT(*) FROM shs_enrollments)";

            using (var conn = Database.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int TotalInstructors()
        {
            const string query = "SELECT COUNT(*) FROM instructor_accounts";
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int TotalStudents()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM students";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int EnrolleesToday()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = @"SELECT 
                    (SELECT COUNT(*) FROM basic_ed_enrollment WHERE DATE(submitted_at) = CURDATE()) +
                    (SELECT COUNT(*) FROM jhs_enrollments WHERE DATE(date_created) = CURDATE()) +
                    (SELECT COUNT(*) FROM shs_enrollments WHERE DATE(created_at) = CURDATE())";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int StudentsToday()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM students WHERE DATE(accepted_at) = CURDATE()";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int InstructorsToday()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM instructor_accounts WHERE DATE(created_at) = CURDATE()";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private void guna2Panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnInstructor_Click_1(object sender, EventArgs e)
        {
            Form parentform = this.FindForm();
            OverlayForm overlay = new OverlayForm(parentform);
            overlay.Show();
            add_instructor AddInstructor = new add_instructor();
            AddInstructor.ShowDialog();
            overlay.Dispose();
        }

        private void btnSched_Click_1(object sender, EventArgs e)
        {
            Form parentform = this.FindForm();
            OverlayForm overlay = new OverlayForm(parentform);
            overlay.Show();
            add_Schedule AddScheds = new add_Schedule(null);
            AddScheds.ShowDialog();
            overlay.Dispose();
        }

        private void btnSubs_Click_1(object sender, EventArgs e)
        {
            Form parentform = this.FindForm();
            OverlayForm overlay = new OverlayForm(parentform);
            overlay.Show();
            add_Subject AddScheds = new add_Subject();
            AddScheds.ShowDialog();
            overlay.Dispose();
        }
    }
}
