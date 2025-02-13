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
    public partial class Pegawai : Form
    {
        public Pegawai()
        {
            InitializeComponent();
            EmpNameLbl.Text = Login.EmpName;
            DisplayEmployees();
        }
        
        //koneksi
        SqlConnection Conn = new SqlConnection(@"Data Source=DESKTOP-UB2KSKP\SQLEXPRESS;Initial Catalog=db_toko_laptop_tugas;Integrated Security=True");
        
        //menampilkan data di dgv
        private void DisplayEmployees()
        {
            
            try
            {
                Conn.Open();
                String Query = "SELECT * FROM EmployeeTbl";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Conn);
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                EmployeesDGV.DataSource = ds.Tables[0];
                Conn.Close();
            }
            catch(Exception Ex){
                MessageBox.Show("Gagal Mendapatkan data: ",Ex.Message);
            }
        }

        //mengclear input
        private void Clear()
        {
            EmpNameTb.Text = "";
            PasswordTb.Text = "";
            EmpPhoneTb.Text = "";
            EmpAddTb.Text = "";
        }

        //simpan data
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (EmpNameTb.Text == "" || EmpAddTb.Text == "" || EmpPhoneTb.Text == "" || EmpDOB.Text == "" || PasswordTb.Text == "")
            {
                MessageBox.Show("Data tidak Boleh Kosong!");
            }
            else
            {
                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO EmployeeTbl(EmpName,EmpAdd,EmpDOB,EmpPhone,EmpPass) VALUES (@EN,@EA,@ED,@EP,@EPa)", Conn);
                    cmd.Parameters.AddWithValue("@EN",EmpNameTb.Text);
                    cmd.Parameters.AddWithValue("@EA", EmpAddTb.Text);
                    cmd.Parameters.AddWithValue("@ED", EmpDOB.Value.Date);
                    cmd.Parameters.AddWithValue("@EP", EmpPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@EPa", PasswordTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Berhasil Menambahkan Pegawai");
                    Conn.Close();
                    DisplayEmployees();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        int key = 0;
        private void EmployeesDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = EmployeesDGV.CurrentRow;

            if (row == null)
            {
                MessageBox.Show("Tidak ada baris yang dipilih!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            EmpNameTb.Text = row.Cells[1].Value != null ? row.Cells[1].Value.ToString() : "";
            EmpAddTb.Text = row.Cells[2].Value != null ? row.Cells[2].Value.ToString() : "";
            EmpDOB.Text = row.Cells[3].Value != null ? row.Cells[3].Value.ToString() : "";
            EmpPhoneTb.Text = row.Cells[4].Value != null ? row.Cells[4].Value.ToString() : "";
            PasswordTb.Text = row.Cells[5].Value != null ? row.Cells[5].Value.ToString() : "";

            if (!string.IsNullOrEmpty(EmpNameTb.Text))
            {
                key = Convert.ToInt32(row.Cells[0].Value);
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (EmpNameTb.Text == "" || EmpAddTb.Text == "" || EmpPhoneTb.Text == "" || EmpDOB.Text == "" || PasswordTb.Text == "")
            {
                MessageBox.Show("Data tidak Boleh Kosong!");
            }
            else
            {
                try
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE EmployeeTbl SET EmpName=@EN,EmpAdd=@EA,EmpDOB=@ED,EmpPhone=@EP,EmpPass=@EPa WHERE EmpNum =@EKey",Conn);
                    cmd.Parameters.AddWithValue("@EN", EmpNameTb.Text);
                    cmd.Parameters.AddWithValue("@EA", EmpAddTb.Text);
                    cmd.Parameters.AddWithValue("@ED", EmpDOB.Value.Date);
                    cmd.Parameters.AddWithValue("@EP", EmpPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@EPa", PasswordTb.Text);
                    cmd.Parameters.AddWithValue("@Ekey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Berhasil Mengedit Pegawai");
                    Conn.Close();
                    DisplayEmployees();
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
                    SqlCommand cmd = new SqlCommand("DELETE FROM EmployeeTbl WHERE EmpNum=@EmpKey", Conn);

                    cmd.Parameters.AddWithValue("@Empkey", key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Berhasil Menghapus Pegawai");
                    Conn.Close();
                    DisplayEmployees();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void btnMenuPelanggan_Click(object sender, EventArgs e)
        {
            Customer customerPegawai = new Customer();
            customerPegawai.Show();
            this.Hide();
        }

        private void btnMenuProduk_Click(object sender, EventArgs e)
        {
            Produk produkPegawai = new Produk();
            produkPegawai.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Home homePegawai = new Home();
            homePegawai.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Billing billingPegawai = new Billing();
            billingPegawai.Show();
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
            Profile profilePegawai = new Profile();
            profilePegawai.Show();
            this.Hide();
        }
    }
}
