/*
 * Created by SharpDevelop.
 * User: Meu
 * Date: 26.6.2015
 * Time: 14:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SQLite;

namespace MUKutuphane
{
	/// <summary>
	/// Description of demirbas.
	/// </summary>
	public partial class demirbas : Form
	{
		public demirbas()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			
			if (textBox1.Text.Length<1)
				MessageBox.Show("Demirbaş Tipi Giriniz!","UYARI",MessageBoxButtons.OK,MessageBoxIcon.Information);
			
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
					String sorgu="INSERT INTO demirbas(tip) values('";
					sorgu+=textBox1.Text.ToString()+"')";
					SQLiteCommand cmd = new SQLiteCommand(sorgu, baglanti);
					cmd.ExecuteNonQuery();
					sorgu="INSERT INTO stok values('";
					sorgu+=textBox1.Text.ToString()+"','0')";
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
				this.Close();
			}
		}
	}
}
