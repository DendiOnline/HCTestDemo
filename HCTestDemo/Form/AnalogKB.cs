using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HCTestDemo
{
    
    public partial class AnalogKB : Form
    {
        Action<double, double> func;
        public AnalogKB(Action<double, double> func2)
        {
            this.func = func2;
            InitializeComponent();
        }
        public AnalogKB()
        {
          
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double[] msg = new double[2];
            func(Convert.ToDouble(Slope.Text), Convert.ToDouble(Intercept.Text));
            this.Close();
        }

        private void bt_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
