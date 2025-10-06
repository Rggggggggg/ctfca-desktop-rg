using System;
using System.Drawing;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    public partial class OverlayForm : Form
    {
        public OverlayForm(Form owner)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;
            this.Opacity = 0.5;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = false;

            if (owner != null)
            {
                this.Bounds = owner.Bounds;
                this.Location = owner.Location;
            }
        }

        private void OverlayForm_Load(object sender, EventArgs e)
        {

        }
    }
}
