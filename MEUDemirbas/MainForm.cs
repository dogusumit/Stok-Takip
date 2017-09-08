/*
 * Created by SharpDevelop.
 * User: Meu
 * Date: 25.6.2015
 * Time: 14:04
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SQLite;

namespace MUKutuphane
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		

		
		void MainFormLoad(object sender, EventArgs e)
		{
			liste_Yukle();
			stok_yukle();
		}
		

		void liste_Yukle()
		{
			
			
			SQLiteConnection baglanti = new SQLiteConnection("Data Source=veritabani.sqlite;Version=3;");
			DataTable dt = new DataTable();
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
				String sorgu="SELECT id,tarih AS Tarih,islem AS İşlem,tip AS Tip,adet AS Adet,barkod AS Barkod,aciklama AS Açıklama FROM kayit";
				SQLiteDataAdapter  adp = new SQLiteDataAdapter(sorgu, baglanti);
				adp.Fill(dt);
				dataGridView1.DataSource = dt;
				dataGridView1.Columns[0].Visible=false;
				adp.Dispose();
				
			}
			try{
				
				baglanti.Close();
			}
			catch(System.Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
			
			
		}
		
		void stok_yukle()
		{
			SQLiteConnection baglanti = new SQLiteConnection("Data Source=veritabani.sqlite;Version=3;");
			DataTable dt = new DataTable();
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
				String sorgu="SELECT * FROM stok";
				SQLiteDataAdapter  adp = new SQLiteDataAdapter(sorgu, baglanti);
				adp.Fill(dt);
				dataGridView2.DataSource = dt;
				adp.Dispose();
				
			}
			try{
				
				baglanti.Close();
			}
			catch(System.Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
		}
		
		
		void Button1Click(object sender, EventArgs e)
		{
			Girdi a=new Girdi();
			a.ShowDialog();
			liste_Yukle();
			stok_yukle();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			Cikti a=new Cikti();
			a.ShowDialog();
			liste_Yukle();
			stok_yukle();
		}

		
		void Button3Click(object sender, EventArgs e)
		{
			DialogResult yazdirmaIslemi;
			yazdirmaIslemi = printDialog1.ShowDialog();
			if (yazdirmaIslemi == DialogResult.OK)
			{
				printDocument1.Print();
			}
		}
		
		void PrintDocument1PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			
			Bitmap bm = new Bitmap(this.dataGridView1.Width, this.dataGridView1.Height);
			dataGridView1.DrawToBitmap(bm, new Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));
			e.Graphics.DrawImage(bm, 0, 0);
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			DialogResult yazdirmaIslemi;
			yazdirmaIslemi = printDialog1.ShowDialog();
			if (yazdirmaIslemi == DialogResult.OK)
			{
				printDocument2.Print();
			}
		}
		
		void PrintDocument2PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			Bitmap bm = new Bitmap(this.dataGridView2.Width, this.dataGridView2.Height);
			dataGridView2.DrawToBitmap(bm, new Rectangle(0, 0, this.dataGridView2.Width, this.dataGridView2.Height));
			e.Graphics.DrawImage(bm, 0, 0);
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			if(dataGridView1.SelectedRows.Count == 0)
			{
				MessageBox.Show("Kayıt Seçiniz");
				return;
			}
			else
			{
				String id=dataGridView1.CurrentRow.Cells[0].Value.ToString();
				String tip=dataGridView1.CurrentRow.Cells[3].Value.ToString();
				String islem=dataGridView1.CurrentRow.Cells[2].Value.ToString();
				String adet=dataGridView1.CurrentRow.Cells[4].Value.ToString();
				
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
					String sorgu="DELETE FROM kayit where id='"+id+"';";
					SQLiteCommand cmd = new SQLiteCommand(sorgu, baglanti);
					cmd.ExecuteNonQuery();
					
					if(islem.Equals("GİRDİ"))
					{
						sorgu="UPDATE stok SET adet=adet-'"+adet+"' where tip='"+tip+"';";
					}
					else
					{
						sorgu="UPDATE stok SET adet=adet+'"+adet+"' where tip='"+tip+"';";
					}
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
				
			}
			
			liste_Yukle();
			stok_yukle();
		}
	}
}
