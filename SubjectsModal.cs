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
    public partial class SubjectsModal : Form
    {
        public SubjectsModal(string subjects, string instructorName)
        {
            InitializeComponent();
            lblTitle.Text = $"Subjects for {instructorName}";
            tbSubjects.Text = string.Join(Environment.NewLine, subjects.Split(',').Select(s => s.Trim()));
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            tbSubjects.ScrollBars = ScrollBars.Vertical;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
        this.Close();
        }

        private void Subs_Load(object sender, EventArgs e)
        {

        }
    }
}
