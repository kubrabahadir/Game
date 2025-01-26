using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class Final : Form
    {
        public int skor;
        public Final(int correct, int wrong)
        {
            InitializeComponent();
            label2.Text = $"Doğru: {correct},  Yanlış: {wrong}";
            skor = (correct*10)-(wrong*5);
            label1.Text = $"Skor: {skor}";
        }

        private void Final_Load(object sender, EventArgs e)
        {

        }
    }
}
