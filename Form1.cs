using System.Collections.Generic;
using System.Windows.Forms;

namespace Remote_Commander
{
    public partial class Form1 : Form
    {
        public class Computers
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
            List<Computers> lstComputers = new List<Computers>();
            lstComputers.Add(new Computers()
            {
                ID = 1,
                Name = "TTN-NCH-RZHS10"
            });
            dataGridView1.DataSource = lstComputers;
            
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

        private void GetCpuInfoToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "psexec /C copy /b Image1.jpg + Archive.rar Image2.jpg";
            process.Start();
        }
    }
}
