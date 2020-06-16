using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using sim = System.Security.Permissions;

namespace GrafikaProjekt2
{
    public partial class Form1 : Form
    {
        
   
        _3Ddemo _3Ddemo;
        Petla petla = new Petla();

        public Form1()
        {
            InitializeComponent();
            _3Ddemo = new _3Ddemo(pictureBox1);
            petla.Load(_3Ddemo);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {




        }

        //start button
        private void button1_Click(object sender, EventArgs e)
        {
            petla.Start();

        }
        //stop button
        private void button2_Click(object sender, EventArgs e)
        {
            petla.Stop();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {


        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
