using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace toko_laptop_tugas
{
    public partial class Produk : Form
    {
        public Produk()
        {
            InitializeComponent();
            DisplayProducts();
            EmpNameLbl.Text = Login.EmpName;
        }

        //koneksi
        SqlConnection Conn = new SqlConnection(DBConnection.ConnectionString);

        //menampilkan data di dgv
        private void DisplayProducts()
        {

            try
            {
                Conn.Open();
                String Query = "SELECT * FROM ProductTbl";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Conn);
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                ProductsDGV.DataSource = ds.Tables[0];
                Conn.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Gagal Mendapatkan data: ", Ex.Message);
            }
        }

        //mengclear input
        private void Clear()
        {
            PrNameTb.Text = "";
            PrStokTb.Text = "";
            CatPrCmb.SelectedIndex = 0;
            PrPriceTb.Text = "";
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (PrNameTb.Text == "" || CatPrCmb.SelectedIndex == -1 || PrStokTb.Text == "" || PrPriceTb.Text == "")
            {
                MessageBox.Show("Data tidak Boleh Kosong!");
            }
            else
            {
                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO ProductTbl(PrName,PrCat,PrQty,PrPrice) VALUES (@PN,@PC,@PQ,@PP)", Conn);
                    cmd.Parameters.AddWithValue("@PN", PrNameTb.Text);
                    cmd.Parameters.AddWithValue("@PC", CatPrCmb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PQ", PrStokTb.Text);
                    cmd.Parameters.AddWithValue("@PP", PrPriceTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Berhasil Menambahkan Produk");
                    Conn.Close();
                    DisplayProducts();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        int key = 0;
        private void ProductsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = ProductsDGV.CurrentRow;

            if (row == null)
            {
                MessageBox.Show("Tidak ada baris yang dipilih!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            PrNameTb.Text = row.Cells[1].Value != null ? row.Cells[1].Value.ToString() : "";
            CatPrCmb.Text = row.Cells[2].Value != null ? row.Cells[2].Value.ToString() : "";
            PrStokTb.Text = row.Cells[3].Value != null ? row.Cells[3].Value.ToString() : "";
            PrPriceTb.Text = row.Cells[4].Value != null ? row.Cells[4].Value.ToString() : "";

            if (!string.IsNullOrEmpty(PrNameTb.Text))
            {
                key = Convert.ToInt32(row.Cells[0].Value);
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (PrNameTb.Text == "" || CatPrCmb.SelectedIndex == -1 || PrStokTb.Text == "" || PrPriceTb.Text == "")
            {
                MessageBox.Show("Data tidak Boleh Kosong!");
            }
            else
            {
                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE ProductTbl SET PrName=@PN,PrCat=@PC,PrQty=@PQ,PrPrice=@PP WHERE PrId =@PKey", Conn);
                    cmd.Parameters.AddWithValue("@PN", PrNameTb.Text);
                    cmd.Parameters.AddWithValue("@PC", CatPrCmb.Text);
                    cmd.Parameters.AddWithValue("@PP", PrPriceTb.Text);
                    cmd.Parameters.AddWithValue("@PQ", PrStokTb.Text);
                    cmd.Parameters.AddWithValue("@Pkey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Berhasil Mengedit Produk");
                    Conn.Close();
                    DisplayProducts();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("pilih data terlebih dahulu!");
            }
            else
            {
                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM ProductTbl WHERE PrId=@PrKey", Conn);

                    cmd.Parameters.AddWithValue("@Prkey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Berhasil Menghapus Produk");
                    Conn.Close();
                    DisplayProducts();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Home homeProduk = new Home();
            homeProduk.Show();
            this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Profile profileProduk = new Profile();
            profileProduk.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Pegawai pegawaiProduk = new Pegawai();
            pegawaiProduk.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Customer customerProduk = new Customer();
            customerProduk.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Billing billingProduk = new Billing();
            billingProduk.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Kosongkan variabel static agar tidak menyimpan sesi login sebelumnya
            Login.EmpName = null;

            // Tampilkan form login kembali
            Login loginForm = new Login();
            loginForm.Show();

            // Tutup form Home
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Transaksi transaksiProduk = new Transaksi();
            transaksiProduk.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Delivery deliveryProduk = new Delivery();
            deliveryProduk.Show();
            this.Hide();
        }
    }
}
