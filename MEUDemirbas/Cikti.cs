/*
 * Created by SharpDevelop.
 * User: Meu
 * Date: 25.6.2015
 * Time: 15:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;

namespace MUKutuphane
{
	/// <summary>
	/// Description of Cikti.
	/// </summary>
	public partial class Cikti : Form
	{
		public Cikti()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void CiktiLoad(object sender, EventArgs e)
		{
			textBox4.Text=DateTime.Now.ToShortDateString();
			combo_Doldur();
		}
		
		void TextBox5TextChanged(object sender, EventArgs e)
		{
			
			if (System.Text.RegularExpressions.Regex.IsMatch(textBox5.Text, "[^0-9]"))
			{
				MessageBox.Show("Sayı Giriniz!");
				textBox5.Text="";
			}
			
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			
			if (textBox5.Text.Length<1 || textBox3.Text.Length<1 || comboBox1.Text.Length<1)
				MessageBox.Show("Lütfen Bütün Alanları Doldurunuz!","UYARI",MessageBoxButtons.OK,MessageBoxIcon.Information);
			
			else
			{
				SQLiteConnection  baglanti = new SQLiteConnection("Data Source=veritabani.sqlite;Version=3;");
				try{
					baglanti.Open();
				}
				catch(System.Exception ex)
				{
					MessageBox.Show(ex.Message.ToString());
				}
				if (baglanti.State == ConnectionState.Open)
				{
					String sorgu="INSERT INTO kayit(barkod,tip,adet,islem,aciklama,tarih) values('";
					sorgu+=textBox1.Text.ToString()+"','";
					sorgu+=comboBox1.Text.ToString()+"','";
					sorgu+=textBox5.Text.ToString()+"','ÇIKTI','";
					sorgu+=textBox3.Text.ToString()+"','";
					sorgu+=textBox4.Text.ToString()+"');";
					SQLiteCommand cmd = new SQLiteCommand(sorgu, baglanti);
					cmd.ExecuteNonQuery();
					
					sorgu="update stok set adet=adet-'"+textBox5.Text.ToString()+"' where tip='"+comboBox1.Text.ToString()+"';";
					cmd = new SQLiteCommand(sorgu, baglanti);
					cmd.ExecuteNonQuery();
				}
				try{
					baglanti.Close();
				}
				catch(System.Exception ex)
				{
					MessageBox.Show(ex.Message.ToString());
				}
				
				button1.Enabled=false;
				MessageBox.Show("ÇIKTI Kaydedildi!");
			}
		}
		void combo_Doldur()
		{
			comboBox1.Items.Clear();
			SQLiteConnection baglanti = new SQLiteConnection("Data Source=veritabani.sqlite;Version=3;");
			DataSet ds = new DataSet();
			try
			{
				baglanti.Open();
			}
			catch(System.Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
			if (baglanti.State == ConnectionState.Open)
			{
				String sorgu="SELECT tip FROM demirbas";

				SQLiteCommand veri = new SQLiteCommand(sorgu,baglanti);
				SQLiteDataReader oku;
				oku = veri.ExecuteReader();
				while (oku.Read())
				{
					comboBox1.Items.Add(oku["tip"].ToString());
				}
				oku.Close();
			}
			
			
			try{
				
				baglanti.Close();
			}
			catch(System.Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
			if(comboBox1.Items.Count>0)
				comboBox1.SelectedIndex=0;
		}
		
	}
}

