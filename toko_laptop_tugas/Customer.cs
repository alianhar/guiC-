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
    public partial class Customer : Form
    {
        public Customer()
        {
            InitializeComponent();
            EmpNameLbl.Text = Login.EmpName;
            DisplayCustomers();
        }

        //koneksi
        SqlConnection Conn = new SqlConnection(DBConnection.ConnectionString);

        //menampilkan data di dgv
        private void DisplayCustomers()
        {

            try
            {
                Conn.Open();
                String Query = "SELECT * FROM CustomerTbl";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Conn);
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                CustomersDGV.DataSource = ds.Tables[0];
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
            CustNameTb.Text = "";
            CustPhoneTb.Text = "";
            CustAddTb.Text = "";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (CustNameTb.Text == "" || CustAddTb.Text == "" || CustPhoneTb.Text == "" || CustUsernameTb.Text == "" || CustPasswordTb.Text == "")
            {
                MessageBox.Show("Data tidak Boleh Kosong!");
            }
            // Cek kecocokan Password dan Confirm Password
            if (CustPasswordTb.Text != CustConfirmPasswordTb.Text)
            {
                MessageBox.Show("Password dan konfirmasi password tidak cocok!");
                return;
            }
            else
            {
                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO CustomerTbl(CustName,CustAdd,CustPhone,CustUsername,CustPass) VALUES (@CN,@CA,@CP,@CU,@CPa)", Conn);
                    cmd.Parameters.AddWithValue("@CN", CustNameTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CustAddTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CustPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CU", CustUsernameTb.Text);
                    cmd.Parameters.AddWithValue("@CPa", CustPasswordTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Berhasil Menambahkan Customer");
                    Conn.Close();
                    DisplayCustomers();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        int key = 0;
        private void CustomersDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = CustomersDGV.CurrentRow;

            if (row == null)
            {
                MessageBox.Show("Tidak ada baris yang dipilih!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            CustNameTb.Text = row.Cells[1].Value != null ? row.Cells[1].Value.ToString() : "";
            CustAddTb.Text = row.Cells[2].Value != null ? row.Cells[2].Value.ToString() : "";
            CustPhoneTb.Text = row.Cells[3].Value != null ? row.Cells[3].Value.ToString() : "";
            CustPasswordTb.Text = row.Cells[4].Value != null ? row.Cells[4].Value.ToString() : "";
            CustUsernameTb.Text = row.Cells[5].Value != null ? row.Cells[5].Value.ToString() : "";

            if (!string.IsNullOrEmpty(CustNameTb.Text))
            {
                key = Convert.ToInt32(row.Cells[0].Value);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
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
                    SqlCommand cmd = new SqlCommand("DELETE FROM CustomerTbl WHERE CustId=@CustKey", Conn);

                    cmd.Parameters.AddWithValue("@Custkey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Berhasil Menghapus Customer");
                    Conn.Close();
                    DisplayCustomers();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (CustNameTb.Text == "" || CustAddTb.Text == "" || CustPhoneTb.Text == "" || CustUsernameTb.Text == "" || CustPasswordTb.Text == "")
            {
                MessageBox.Show("Data tidak Boleh Kosong!");
            }
            // Cek kecocokan Password dan Confirm Password
            if (CustPasswordTb.Text != CustConfirmPasswordTb.Text)
            {
                MessageBox.Show("Password dan konfirmasi password tidak cocok!");
                return;
            }

            else
            {
                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE CustomerTbl SET CustName=@CN,CustAdd=@CA,CustPhone=@CP,CustPass=@CPa,CustUsername=@CU WHERE CustId =@CKey", Conn);
                    cmd.Parameters.AddWithValue("@CN", CustNameTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CustAddTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CustPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CU", CustUsernameTb.Text);
                    cmd.Parameters.AddWithValue("@CPa", CustPasswordTb.Text);
                    cmd.Parameters.AddWithValue("@Ckey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Berhasil Mengedit Customer");
                    Conn.Close();
                    DisplayCustomers();
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
            Home homePelanggan = new Home();
            homePelanggan.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Produk produkPelanggan = new Produk();
            produkPelanggan.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Pegawai pegawaiPelanggan = new Pegawai();
            pegawaiPelanggan.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Billing billingPelanggan = new Billing();
            billingPelanggan.Show();
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

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Profile pr = new Profile();
            pr.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Transaksi transaksiCustomer = new Transaksi();
            transaksiCustomer.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Delivery deliveryCustomer = new Delivery();
            deliveryCustomer.Show();
            this.Hide();
        }
    }
}
