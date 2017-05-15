using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;


namespace EyeTracker
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void filesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void openTextInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openTrackingTest_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tracker tracker = new Tracker();
            tracker.Show();
                      
        }
        private void openReadingTest_ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void test1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PDF_1 reader1 = new PDF_1();
            reader1.Show();
        }
    }
}
