using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MySQL_Browser
{
    public partial class Browser : Form
    {
        MySqlConnection conn = new MySqlConnection();

        protected TextBox ActiveTextBox
        {
            get
            {
                return ((NewQueryPage)tabQuery.SelectedTab).txtSQL;
            }
        }

        protected DataGridView ActiveDataGridView
        {
            get
            {
                return ((NewQueryPage)tabQuery.SelectedTab).dgvResult;
            }
        }

        public Browser()
        {
            InitializeComponent();
        }

        private void Browser_Load(object sender, EventArgs e)
        {
            if (this.conn.State != ConnectionState.Open)
            {
                cboDatabases.Enabled = false;
                btnExec.Enabled = false;
            }
            tabQuery.TabPages.RemoveAt(0);
            btnNew.PerformClick();
        }

        [MTAThread]
        private void AnimateProgBar()
        {
            for (int i = 0; i < toolStripProgressBar1.Maximum; ++i)
            {
                toolStripProgressBar1.Value = i;
                System.Threading.Thread.Sleep(10);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeStatus ("Connecting...");
                AnimateProgBar();
                String connectionString = String.Format("server={0};UID={1};PWD={2};port={3}", txtServer.Text, txtUname.Text, txtPW.Text, txtPort.Text);
                this.conn = new MySqlConnection(connectionString);
                this.conn.Open();
                ChangeStatus("Connected");
                using (MySqlDataReader rdr = (new MySqlCommand("show databases;", this.conn)).ExecuteReader())
                {
                    cboDatabases.Items.Clear();
                    while (rdr.Read())
                    {
                        cboDatabases.Items.Add(rdr[0].ToString());
                    }
                    cboDatabases.Enabled = true;
                    btnExec.Enabled = true;
                }
            }
            catch (MySqlException my)
            {
                ChangeStatus(my.Message, true);
                btnExec.Enabled = false;
                cboDatabases.Enabled = false;
                tvTables.Nodes.Clear();
            }
        }

        private void cboDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDatabases.Text.Trim() != string.Empty)
            {
                using (MySqlDataReader rdr = (new MySqlCommand(String.Format("show tables from {0};",cboDatabases.Text), this.conn)).ExecuteReader())
                {
                    tvTables.Nodes.Clear();
                    while (rdr.Read())
                    {
                        tvTables.Nodes.Add(rdr[0].ToString());
                    }
                }
            }
        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            NewQueryPage page = (NewQueryPage)tabQuery.SelectedTab;
            DataGridView dgv = ActiveDataGridView;
            Execute(page.txtSQL.Text, ref dgv);
            ChangeStatus(String.Format("Number of rows is {0}",dgv.RowCount));
        }

        private void Execute(string SQL,ref DataGridView dgv)
        {
            if (SQL.Trim().Length <= 0)
            {
                ChangeStatus("SQL is empty.", true);
            }
            else
            {
                ChangeStatus("Executing...");
                try
                {
                    RenderToGridView(String.Format("USE {0};", cboDatabases.Text) + SQL, ref dgv);
                    ChangeStatus(String.Format("Number of rows : {0}", dgv.RowCount));
                }
                catch (Exception my)
                {
                    ChangeStatus(my.Message, true);
                }
            }
        }

        private void ChangeStatus(String text,Boolean is_error=false)
        {
            tssStatus.Text = text;
            if(! is_error){
                tssStatus.Font = new Font(FontFamily.GenericSansSerif, tssStatus.Font.Size, FontStyle.Regular);
                tssStatus.ForeColor = Color.Black;
            }
            else{
                tssStatus.ForeColor = Color.Red;
                tssStatus.Font = new Font(FontFamily.GenericSansSerif, tssStatus.Font.Size, FontStyle.Bold);
            }
        }

        private void RenderToGridView(String SQL, ref DataGridView dgv)
        {
            try
            {
                using (MySqlDataAdapter adp = new MySqlDataAdapter(SQL, this.conn))
                {
                    using (DataSet dst = new DataSet())
                    {
                        adp.Fill(dst);
                        dgv.DataSource = dst.Tables[0];
                        AnimateProgBar();
                    }
                }
            }
            catch { throw; }
        }

        private void tvTables_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                ChangeStatus("Loading table...");
                String SQL = String.Format("select * from {0}.{1};", cboDatabases.Text, e.Node.Text);
                DataGridView dgv = ActiveDataGridView;
                RenderToGridView(SQL, ref dgv);
                ActiveTextBox.Text = SQL;
                ChangeStatus(String.Format("Done. Number of rows is {0}",dgv.RowCount));
            }
            catch (Exception my)
            {
                ChangeStatus(my.Message, true);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            tabQuery.TabPages.Add(new NewQueryPage() { ContextMenuStrip = contextMenuStrip1 });
            tabQuery.SelectTab(tabQuery.TabCount - 1);
        }

        private void closeAllTabsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TabPage page in tabQuery.TabPages)
            {
                tabQuery.TabPages.Remove(page);
            }
        }

        private void closeCurrentTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabQuery.TabPages.Remove(tabQuery.SelectedTab);
        }
    }
}