using System.Collections.Generic;
using System.Drawing;
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
                Name = "TTN-NCH-RZHS01"
            });
            lstComputers.Add(new Computers()
            {
                ID = 2,
                Name = "TTN-NCH-RZHS02"
            });
            dataGridView1.DataSource = lstComputers;
            
        }

        private void DataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Cut"));
                m.MenuItems.Add(new MenuItem("Copy"));
                m.MenuItems.Add(new MenuItem("Paste"));

                int currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    m.MenuItems.Add(new MenuItem(string.Format("Do something to row {0}", currentMouseOverRow.ToString())));
                }

                m.Show(dataGridView1, new Point(e.X, e.Y));

            }
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
    }
}
