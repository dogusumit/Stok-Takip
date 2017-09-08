/*
 * Created by SharpDevelop.
 * User: Meu
 * Date: 25.6.2015
 * Time: 15:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SQLite;
using System.Drawing.Printing;

namespace MUKutuphane
{
	/// <summary>
	/// Description of Emanet_İşlemleri.
	/// </summary>
	public partial class Girdi : Form
	{
		private  bool kontrol;
		public Girdi()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void GirdiLoad(object sender, EventArgs e)
		{
			kontrol=false;
			textBox4.Text=DateTime.Now.ToShortDateString();
			combo_Doldur();
		}
		
		
		PrintDocument yazdir = new PrintDocument();
		Ean13Barcode2005.Ean13 barkod = new Ean13Barcode2005.Ean13();
		
		void barkod_Olustur()
		{

			barkod.CountryCode = "00";
			barkod.ManufacturerCode = "00000";
			barkod.ProductCode = basa_sifir_koy(textBox1.Text.ToString());
			yazdir.PrintPage += new PrintPageEventHandler(yazdir_PrintPage);
			printPreview1.Document = yazdir;
			printPreview1.PrintPreviewControl.Zoom = 1.0;
			printPreview1.ShowDialog();
			//doc.Print();
			
			
		}
		public String basa_sifir_koy(String a)
		{
			return (a.PadLeft(5,'0'));
		}
		
		void yazdir_PrintPage(object sender, PrintPageEventArgs e)
		{
			e.Graphics.DrawString("MERSİN ÜNİVERSİTESİ", new System.Drawing.Font(new FontFamily("Arial"), 8, FontStyle.Bold), Brushes.Black, new PointF(0,10));
			e.Graphics.DrawString("BİLGİ İŞLEM DAİRE BAŞKANLIĞI", new System.Drawing.Font(new FontFamily("Arial"), 6, FontStyle.Bold), Brushes.Black, new PointF(0,25));

			barkod.DrawEan13Barcode(e.Graphics, (new PointF(0,10)));
			
			e.Graphics.DrawString(textBox4.Text, new System.Drawing.Font(new FontFamily("Arial"), 8, FontStyle.Bold), Brushes.Black, new PointF(0,130));
			e.Graphics.DrawString("Demirbaş = "+comboBox1.Text, new System.Drawing.Font(new FontFamily("Arial"), 8, FontStyle.Bold), Brushes.Black, new PointF(0,150));
			e.Graphics.DrawString("Adet = "+textBox5.Text, new System.Drawing.Font(new FontFamily("Arial"), 8, FontStyle.Bold), Brushes.Black, new PointF(0,170));
			e.Graphics.DrawString(textBox3.Text, new System.Drawing.Font(new FontFamily("Arial"), 8, FontStyle.Bold), Brushes.Black, new PointF(0,190));
			
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
		
		
		void Button1Click(object sender, EventArgs e)
		{
			if (!kontrol)
				MessageBox.Show("Lütfen Önce Demirbaşı Kaydediniz!","UYARI",MessageBoxButtons.OK,MessageBoxIcon.Information);
			else
			{
				barkod_Olustur();
			}
			
		}
		
		
		void Button2Click(object sender, EventArgs e)
		{
			if (textBox3.Text.Length<1 || textBox5.Text.Length<1 || comboBox1.Text.Length<1)
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
					String sorgu="INSERT INTO kayit(tip,adet,islem,aciklama,tarih) values('";
					sorgu+=comboBox1.Text.ToString()+"','";
					sorgu+=textBox5.Text.ToString()+"','GİRDİ','";
					sorgu+=textBox3.Text.ToString()+"','";
					sorgu+=textBox4.Text.ToString()+"');";
					SQLiteCommand cmd = new SQLiteCommand(sorgu, baglanti);
					cmd.ExecuteNonQuery();
					
					sorgu=@"select last_insert_rowid()";
					SQLiteCommand cmd2 = new SQLiteCommand(sorgu,baglanti);
					textBox1.Text=cmd2.ExecuteScalar().ToString();
					sorgu="update stok set adet=adet+'"+textBox5.Text.ToString()+"' where tip='"+comboBox1.Text.ToString()+"';";
					cmd = new SQLiteCommand(sorgu, baglanti);
					cmd.ExecuteNonQuery();
					
					sorgu="update kayit set barkod='"+textBox1.Text.ToString()+"' where id='"+textBox1.Text.ToString()+"';";
					cmd = new SQLiteCommand(sorgu, baglanti);
					cmd.ExecuteNonQuery();
				}
				
				try
				{
					baglanti.Close();
				}
				catch(System.Exception ex)
				{
					MessageBox.Show(ex.Message.ToString());
				}
				
				textBox3.ReadOnly=true;
				textBox5.ReadOnly=true;
				comboBox1.Enabled=false;
				kontrol=true;
				button2.Enabled=false;
				MessageBox.Show("Demirbaş Kaydedildi!");
			}
		}
		
		
		void TextBox5TextChanged(object sender, EventArgs e)
		{
			if (System.Text.RegularExpressions.Regex.IsMatch(textBox5.Text, "[^0-9]"))
			{
				MessageBox.Show("Sayı Giriniz!");
				textBox5.Text="";
			}
		}
		
		
		void Button3Click(object sender, EventArgs e)
		{
			demirbas a=new demirbas();
			a.ShowDialog();
			combo_Doldur();
		}
	}
}
