using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace KoronaHastanesi
{
    public partial class Form2 : Form
    {
        public Form1 fr1;
        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-LARUFH1;Initial Catalog=Hastane;Integrated Security=True");
        private void Form2_Load(object sender, EventArgs e)
        {
            fr1.odadurumu();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from odabilgileri where oda='" + comboBox1.SelectedItem + "'", baglanti);
            SqlDataReader okuyucu = komut.ExecuteReader();
            while (okuyucu.Read())
            {
                textBox6.Text = okuyucu["yataksayisi"].ToString();
            }
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update odabilgileri set yataksayisi='"+textBox6.Text+"'where oda='"+comboBox1.SelectedItem+"'",baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Oda Güncellendi");
            for (int i = 0; i < groupBox1.Controls.Count; i++)
            {
                if (groupBox1.Controls[i] is TextBox)
                {
                    groupBox1.Controls[i].Text = "";
                }
            }
            comboBox1.Text = "";
            comboBox1.Items.Clear();
            fr1.odadurumu();
        }
    }
}
