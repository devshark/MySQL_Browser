using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySQL_Browser
{
    class NewQueryPage:System.Windows.Forms.TabPage
    {
        public NewQueryPage()
        {
            // 
            // dgvResult
            // 
            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.AllowUserToOrderColumns = true;
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResult.Location = new System.Drawing.Point(0, 0);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.ReadOnly = true;
            this.dgvResult.Size = new System.Drawing.Size(780, 205);
            this.dgvResult.TabIndex = 0;
            // 
            // txtSQL
            // 
            this.txtSQL.AcceptsReturn = true;
            this.txtSQL.AcceptsTab = true;
            this.txtSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSQL.Location = new System.Drawing.Point(3, 3);
            this.txtSQL.Multiline = true;
            this.txtSQL.Name = "txtSQL";
            //this.txtSQL.Size = new System.Drawing.Size(771, 126);
            this.txtSQL.Dock = DockStyle.Fill;
            this.txtSQL.TabIndex = 3;
            txtSQL.ScrollBars = ScrollBars.Vertical;
            // 
            // tabPage1
            // 
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.txtSQL);
            this.Controls.Add(this.dgvResult);
            this.Location = new System.Drawing.Point(4, 22);
            this.Name = "tabPage1";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(786, 347);
            this.TabIndex = 0;
            this.Text = "New Query";
            this.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtSQL);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvResult);
            this.splitContainer1.Size = new System.Drawing.Size(780, 341);
            this.splitContainer1.SplitterDistance = 132;
            this.splitContainer1.TabIndex = 0;
        }

        public SplitContainer splitContainer1 = new SplitContainer();
        public TextBox txtSQL = new TextBox();
        public DataGridView dgvResult = new DataGridView();
        
    }
}
