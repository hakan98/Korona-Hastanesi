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
    public partial class Form1 : Form
    {
        public Form2 fr2 = new Form2();
        public Form1()
        {
            InitializeComponent();
            fr2.fr1 = this;
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-LARUFH1;Initial Catalog=Hastane;Integrated Security=True");
        public void odadurumu()
        {
            int sayi = 1;
            foreach (Control buton in Controls)
                if (buton is Button)
                {
                    if (buton.Name != "button1" && buton.Name !="button8" )
                    {
                        buton.BackColor = Color.White;
                        buton.Text = "ODA-" + sayi.ToString();
                        sayi++;
                    }

                }

            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT * FROM OdaBilgileri", baglanti);
            SqlDataReader okuyucu = komut.ExecuteReader();
            while (okuyucu.Read())
            {
                foreach (Control oda in Controls)
                {
                    if (oda is Button)
                    {
                        if (okuyucu["oda"].ToString() == oda.Text && okuyucu["durum"].ToString() == "BOŞ")
                        {
                            oda.BackColor = Color.Green;
                            comboBox1.Items.Add(okuyucu["oda"].ToString());
                            fr2.comboBox1.Items.Add(okuyucu["oda"].ToString());
                        }
                        if (okuyucu["oda"].ToString() == oda.Text && okuyucu["durum"].ToString() == "DOLU")
                        {
                            oda.BackColor = Color.Red;
                        }
                    }
                }
            }
            baglanti.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (Control renkdegis in Controls)
            {
                if (renkdegis is Button)
                {
                    
                        renkdegis.Click += renkdegis_Click;
                    
                }
            }
            odadurumu();
            textBox1.MaxLength = 11;
            textBox5.MaxLength = 11;
        }

        private void renkdegis_Click(object sender, EventArgs e)
        {
            Button b=sender as Button;
            if (b.BackColor==Color.Red)
            {
                DialogResult cevap = MessageBox.Show("Oda Çıkışı Yapılsın mı?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (cevap==DialogResult.Yes)
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("update HastaKayit set Oda='Çıkış Yapıldı' where Oda='" + b.Text + "'", baglanti);
                    komut.ExecuteNonQuery();

                    SqlCommand komut2 = new SqlCommand("update OdaBilgileri set durum='BOŞ' where oda='" + b.Text + "'", baglanti);
                    komut2.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Oda Çıkışı yapıldı");
                    comboBox1.Items.Clear();
                    odadurumu();

                }
                
            }
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
            SqlCommand komut = new SqlCommand("insert into HastaKayit (TCKN,Ad,Soyad,Dogum_Tarihi,Telefon,Oda)values(@TCKN,@Ad,@Soyad,@Dogum_Tarihi,@Telefon,@Oda)", baglanti);
            komut.Parameters.AddWithValue("@TCKN", textBox1.Text);
            komut.Parameters.AddWithValue("@Ad", textBox2.Text);
            komut.Parameters.AddWithValue("@Soyad", textBox3.Text);
            komut.Parameters.AddWithValue("@Dogum_Tarihi", textBox4.Text);
            komut.Parameters.AddWithValue("@Telefon", textBox5.Text);
            komut.Parameters.AddWithValue("@Oda", comboBox1.SelectedItem);
            komut.ExecuteNonQuery();
            SqlCommand komut2 = new SqlCommand("UPDATE OdaBilgileri set durum='DOLU' where oda='" + comboBox1.SelectedItem + "'", baglanti);
            komut2.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ekleme Başarılı");
            comboBox1.Text = "";
            comboBox1.Items.Clear();
            odadurumu();
            for (int i = 0; i < groupBox1.Controls.Count; i++)
            {
                if (groupBox1.Controls[i] is TextBox)
                {
                    groupBox1.Controls[i].Text = "";
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            fr2.ShowDialog();
        }
    }
}
