using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFCA_ADMIN
{
    partial class Enrollees
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Enrollees));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtgEnrollees = new Guna.UI2.WinForms.Guna2DataGridView();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.tbSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.cbStatusFilter = new Guna.UI2.WinForms.Guna2ComboBox();
            this.colStudentNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLevelApplied = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colContact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubmittedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnConfirm = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnDelete = new System.Windows.Forms.DataGridViewImageColumn();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dtgEnrollees)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgEnrollees
            // 
            this.dtgEnrollees.AllowUserToAddRows = false;
            this.dtgEnrollees.AllowUserToDeleteRows = false;
            this.dtgEnrollees.AllowUserToResizeColumns = false;
            this.dtgEnrollees.AllowUserToResizeRows = false;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.White;
            this.dtgEnrollees.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle17;
            this.dtgEnrollees.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgEnrollees.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgEnrollees.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.dtgEnrollees.ColumnHeadersHeight = 50;
            this.dtgEnrollees.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStudentNumber,
            this.colName,
            this.colLevelApplied,
            this.colGender,
            this.colAge,
            this.colContact,
            this.colSubmittedAt,
            this.colEmail,
            this.btnConfirm,
            this.btnDelete});
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgEnrollees.DefaultCellStyle = dataGridViewCellStyle21;
            this.dtgEnrollees.EnableHeadersVisualStyles = true;
            this.dtgEnrollees.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgEnrollees.Location = new System.Drawing.Point(12, 86);
            this.dtgEnrollees.Name = "dtgEnrollees";
            this.dtgEnrollees.ReadOnly = true;
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgEnrollees.RowHeadersDefaultCellStyle = dataGridViewCellStyle22;
            this.dtgEnrollees.RowHeadersVisible = false;
            this.dtgEnrollees.RowHeadersWidth = 51;
            this.dtgEnrollees.RowTemplate.Height = 45;
            this.dtgEnrollees.Size = new System.Drawing.Size(928, 433);
            this.dtgEnrollees.TabIndex = 0;
            this.dtgEnrollees.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dtgEnrollees.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dtgEnrollees.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dtgEnrollees.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dtgEnrollees.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dtgEnrollees.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dtgEnrollees.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgEnrollees.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dtgEnrollees.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dtgEnrollees.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtgEnrollees.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dtgEnrollees.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtgEnrollees.ThemeStyle.HeaderStyle.Height = 50;
            this.dtgEnrollees.ThemeStyle.ReadOnly = true;
            this.dtgEnrollees.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dtgEnrollees.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dtgEnrollees.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtgEnrollees.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dtgEnrollees.ThemeStyle.RowsStyle.Height = 45;
            this.dtgEnrollees.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dtgEnrollees.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dtgEnrollees.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DtgEnrollees_CellContentClick);
            // 
            // dataGridViewImageColumn1
            // 
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle23.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle23.NullValue")));
            dataGridViewCellStyle23.Padding = new System.Windows.Forms.Padding(10);
            this.dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle23;
            this.dataGridViewImageColumn1.FillWeight = 40F;
            this.dataGridViewImageColumn1.HeaderText = "Accept";
            this.dataGridViewImageColumn1.Image = global::CFCA_ADMIN.Properties.Resources.edit_interface_sign;
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn1.MinimumWidth = 6;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.Width = 58;
            // 
            // dataGridViewImageColumn2
            // 
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle24.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle24.NullValue")));
            dataGridViewCellStyle24.Padding = new System.Windows.Forms.Padding(10);
            this.dataGridViewImageColumn2.DefaultCellStyle = dataGridViewCellStyle24;
            this.dataGridViewImageColumn2.FillWeight = 40F;
            this.dataGridViewImageColumn2.HeaderText = "Decline";
            this.dataGridViewImageColumn2.Image = global::CFCA_ADMIN.Properties.Resources.delete_square;
            this.dataGridViewImageColumn2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn2.MinimumWidth = 6;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn2.Width = 58;
            // 
            // tbSearch
            // 
            this.tbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearch.BorderRadius = 17;
            this.tbSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbSearch.DefaultText = "";
            this.tbSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tbSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tbSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tbSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tbSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tbSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tbSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tbSearch.IconLeft = ((System.Drawing.Image)(resources.GetObject("tbSearch.IconLeft")));
            this.tbSearch.Location = new System.Drawing.Point(760, 39);
            this.tbSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.PlaceholderText = "Search";
            this.tbSearch.SelectedText = "";
            this.tbSearch.Size = new System.Drawing.Size(180, 36);
            this.tbSearch.TabIndex = 3;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged_1);
            // 
            // cbStatusFilter
            // 
            this.cbStatusFilter.AutoRoundedCorners = true;
            this.cbStatusFilter.BackColor = System.Drawing.Color.Transparent;
            this.cbStatusFilter.BorderRadius = 17;
            this.cbStatusFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatusFilter.DropDownWidth = 155;
            this.cbStatusFilter.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbStatusFilter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbStatusFilter.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbStatusFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbStatusFilter.ItemHeight = 30;
            this.cbStatusFilter.Items.AddRange(new object[] {
            "Pending",
            "Confirmed",
            "Rejected",
            "All"});
            this.cbStatusFilter.Location = new System.Drawing.Point(12, 44);
            this.cbStatusFilter.Name = "cbStatusFilter";
            this.cbStatusFilter.Size = new System.Drawing.Size(152, 36);
            this.cbStatusFilter.TabIndex = 4;
            this.cbStatusFilter.SelectedIndexChanged += new System.EventHandler(this.cbStatusFilter_SelectedIndexChanged_1);
            // 
            // colStudentNumber
            // 
            this.colStudentNumber.FillWeight = 80F;
            this.colStudentNumber.HeaderText = "Student No.";
            this.colStudentNumber.MinimumWidth = 6;
            this.colStudentNumber.Name = "colStudentNumber";
            this.colStudentNumber.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.FillWeight = 160F;
            this.colName.HeaderText = "Name";
            this.colName.MinimumWidth = 6;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colLevelApplied
            // 
            this.colLevelApplied.FillWeight = 80F;
            this.colLevelApplied.HeaderText = "Level Applied";
            this.colLevelApplied.MinimumWidth = 6;
            this.colLevelApplied.Name = "colLevelApplied";
            this.colLevelApplied.ReadOnly = true;
            // 
            // colGender
            // 
            this.colGender.FillWeight = 60F;
            this.colGender.HeaderText = "Gender";
            this.colGender.MinimumWidth = 6;
            this.colGender.Name = "colGender";
            this.colGender.ReadOnly = true;
            // 
            // colAge
            // 
            this.colAge.FillWeight = 50F;
            this.colAge.HeaderText = "Age";
            this.colAge.MinimumWidth = 6;
            this.colAge.Name = "colAge";
            this.colAge.ReadOnly = true;
            // 
            // colContact
            // 
            this.colContact.FillWeight = 110F;
            this.colContact.HeaderText = "Contact";
            this.colContact.MinimumWidth = 6;
            this.colContact.Name = "colContact";
            this.colContact.ReadOnly = true;
            // 
            // colSubmittedAt
            // 
            this.colSubmittedAt.FillWeight = 110F;
            this.colSubmittedAt.HeaderText = "Submitted At";
            this.colSubmittedAt.MinimumWidth = 6;
            this.colSubmittedAt.Name = "colSubmittedAt";
            this.colSubmittedAt.ReadOnly = true;
            // 
            // colEmail
            // 
            this.colEmail.FillWeight = 150F;
            this.colEmail.HeaderText = "Email";
            this.colEmail.MinimumWidth = 6;
            this.colEmail.Name = "colEmail";
            this.colEmail.ReadOnly = true;
            // 
            // btnConfirm
            // 
            this.btnConfirm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle19.NullValue")));
            dataGridViewCellStyle19.Padding = new System.Windows.Forms.Padding(10);
            this.btnConfirm.DefaultCellStyle = dataGridViewCellStyle19;
            this.btnConfirm.FillWeight = 40F;
            this.btnConfirm.HeaderText = "Accept";
            this.btnConfirm.Image = global::CFCA_ADMIN.Properties.Resources.edit_interface_sign;
            this.btnConfirm.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.btnConfirm.MinimumWidth = 6;
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.ReadOnly = true;
            this.btnConfirm.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnConfirm.Width = 45;
            // 
            // btnDelete
            // 
            this.btnDelete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle20.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle20.NullValue")));
            dataGridViewCellStyle20.Padding = new System.Windows.Forms.Padding(10);
            this.btnDelete.DefaultCellStyle = dataGridViewCellStyle20;
            this.btnDelete.FillWeight = 40F;
            this.btnDelete.HeaderText = "Decline";
            this.btnDelete.Image = global::CFCA_ADMIN.Properties.Resources.delete_square;
            this.btnDelete.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.btnDelete.MinimumWidth = 6;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.ReadOnly = true;
            this.btnDelete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnDelete.Width = 45;
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 20;
            this.guna2Elipse1.TargetControl = this.dtgEnrollees;
            // 
            // Enrollees
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.cbStatusFilter);
            this.Controls.Add(this.dtgEnrollees);
            this.Name = "Enrollees";
            this.Size = new System.Drawing.Size(956, 539);
            this.Load += new System.EventHandler(this.Enrollees_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgEnrollees)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dtgEnrollees;
        private DataGridViewImageColumn dataGridViewImageColumn1;
        private DataGridViewImageColumn dataGridViewImageColumn2;
        private Guna.UI2.WinForms.Guna2TextBox tbSearch;
        private Guna.UI2.WinForms.Guna2ComboBox cbStatusFilter;
        private DataGridViewTextBoxColumn colStudentNumber;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colLevelApplied;
        private DataGridViewTextBoxColumn colGender;
        private DataGridViewTextBoxColumn colAge;
        private DataGridViewTextBoxColumn colContact;
        private DataGridViewTextBoxColumn colSubmittedAt;
        private DataGridViewTextBoxColumn colEmail;
        private DataGridViewImageColumn btnConfirm;
        private DataGridViewImageColumn btnDelete;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
    }
}