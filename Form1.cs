using System.Collections.Generic;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Diagnostics;
using Microsoft.Management.Infrastructure;
using System;
using System.Security.Principal;
using Microsoft.Management.Infrastructure.Options;

namespace Remote_Commander
{
    public partial class Form1 : Form
    {
        private WindowsIdentity user;

        public class Computers
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Online { get; set; }
            public string Cpu { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
            List <Computers> lstComputers = new List<Computers>();
            /*
            lstComputers.Add(new Computers()
            {
                ID = 1,
                Name = "TTN-NCH-RZHS01",
                Online = "",
            });
            lstComputers.Add(new Computers()
            {
                ID = 2,
                Name = "TTN-NCH-RZHS02",
                Online = "",
            });
            lstComputers.Add(new Computers()
            {
                ID = 3,
                Name = "TTN-NCH-RZHS03",
                Online = "",
            });
            lstComputers.Add(new Computers()
            {
                ID = 4,
                Name = "TTN-NCH-RZHS04",
                Online = "",
            });
            */
            lstComputers.Add(new Computers()
            {
                ID = 5,
                Name = "TTN-NCH-RZHS10",
                Online = "",
            });
            dataGridView1.DataSource = lstComputers;
            // dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void DataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Ignore if a column or row header is clicked
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridViewCell clickedCell = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex];

                    // Here you can do whatever you want with the cell
                    this.dataGridView1.CurrentCell = clickedCell;  // Select the clicked cell, for instance
                    
                    // Get mouse position relative to the vehicles grid
                    var relativeMousePosition = dataGridView1.PointToClient(Cursor.Position);

                    // Show the context menu
                    this.contextMenuStrip1.Show(dataGridView1, relativeMousePosition);
                }
            }
        }

        private string GetCpuInfo(string computerName)
        {
            user = WindowsIdentity.GetCurrent();
            string Namespace = @"root\cimv2";
            string cpuQuery = "SELECT * FROM Win32_Processor";

            CimSession mySession = CimSession.Create(computerName);
            IEnumerable<CimInstance> queryInstance = mySession.QueryInstances(Namespace, "WQL", cpuQuery);
            List<string> cpuNames = new List<string>();
            foreach (CimInstance item in queryInstance)
            {
                cpuNames.Add(item.CimInstanceProperties["Name"].Value.ToString());
            }
            return cpuNames[0];
        }
        private bool IsOnline(string computerName)
        {
            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send(computerName, timeout:500);
            bool onlineStatus = reply.Status == IPStatus.Success ? true : false;
            return onlineStatus;
        }

        private void GetCpuInfoToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            string computerName = dataGridView1.CurrentCell.OwningRow.Cells[1].Value.ToString();
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/k PsExec.exe \\\\" + computerName + " cmd /k ipconfig /all";
            process.Start();
        }

        private void PingToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Ping pingSender = new Ping();
            string computerName = dataGridView1.CurrentCell.OwningRow.Cells[1].Value.ToString();
            int timeout = 1000;
            PingReply reply = pingSender.Send(computerName, timeout);
            if (reply.Status == IPStatus.Success)
            {
                dataGridView1.CurrentCell.OwningRow.Cells[2].Value = "Yes";
                dataGridView1.CurrentCell.OwningRow.Cells[2].Style.BackColor = System.Drawing.Color.Green;
                dataGridView1.CurrentCell.OwningRow.Cells[2].Style.ForeColor = System.Drawing.Color.White;
            }
            else
            {
                dataGridView1.CurrentCell.OwningRow.Cells[2].Value = "No";
                dataGridView1.CurrentCell.OwningRow.Cells[2].Style.BackColor = System.Drawing.Color.Red;
                dataGridView1.CurrentCell.OwningRow.Cells[2].Style.ForeColor = System.Drawing.Color.White;
            }
        }

        private void Button1_Click(object sender, System.EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string computerName = row.Cells["Name"].Value.ToString();
                row.Cells["Online"].Value = IsOnline(computerName) ? "Yes" : "No";
                row.Cells["Cpu"].Value = GetCpuInfo(computerName);
            }
        }
    }
}
