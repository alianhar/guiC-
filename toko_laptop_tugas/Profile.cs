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
    public partial class Profile : Form
    {

        public Profile()
        {
            InitializeComponent();
            EmpNameLbl.Text = Login.EmpName; // Menampilkan nama pengguna yang login
            LoadProfileData(); // Memuat data profil pengguna
        }

        SqlConnection Conn = new SqlConnection(@"Data Source=DESKTOP-UB2KSKP\SQLEXPRESS;Initial Catalog=db_toko_laptop_tugas;Integrated Security=True");
        private void LoadProfileData()
        {
            try
            {
                // Pastikan koneksi dalam keadaan tertutup sebelum membuka
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                // Query untuk mengambil data pengguna berdasarkan nama pengguna
                string query = "SELECT EmpName, EmpPhone, EmpAdd, EmpDOB FROM EmployeeTbl WHERE EmpName=@EmpName";
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@EmpName", Login.EmpName);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Mengisi TextBox dengan data yang diambil
                    UsernameTb.Text = reader["EmpName"].ToString();
                    PhoneTb.Text = reader["EmpPhone"].ToString();
                    AddressTb.Text = reader["EmpAdd"].ToString();
                    TglLahirDTP.Value = Convert.ToDateTime(reader["EmpDOB"]); // Mengisi DateTimePicker
                }
                else
                {
                    MessageBox.Show("Data profil tidak ditemukan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }

        // Event handler untuk tombol-tombol navigasi
        private void HomeBtn_Click(object sender, EventArgs e)
        {
            Home homeProfile = new Home();
            homeProfile.Show();
            this.Hide();
        }

        private void ProdukBtn_Click(object sender, EventArgs e)
        {
            Produk produkProfile = new Produk();
            produkProfile.Show();
            this.Hide();
        }

        private void PegawaiBtn_Click(object sender, EventArgs e)
        {
            Pegawai pegawaiProfile = new Pegawai();
            pegawaiProfile.Show();
            this.Hide();
        }

        private void PelangganBtn_Click(object sender, EventArgs e)
        {
            Customer pelangganProfile = new Customer();
            pelangganProfile.Show();
            this.Hide();
        }

        private void BillingBtn_Click(object sender, EventArgs e)
        {
            Billing billingProfile = new Billing();
            billingProfile.Show();
            this.Hide();
        }

        private void SignOutBtn_Click(object sender, EventArgs e)
        {
            // Kosongkan variabel static agar tidak menyimpan sesi login sebelumnya
            Login.EmpName = null;

            // Tampilkan form login kembali
            Login loginForm = new Login();
            loginForm.Show();

            // Tutup form Home
            this.Close();
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            // Pastikan semua field tidak kosong (kecuali Address opsional)
            if (UsernameTb.Text.Trim() == "" || PhoneTb.Text.Trim() == "")
            {
                MessageBox.Show("Username dan Nomor HP wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Pastikan koneksi dalam keadaan tertutup sebelum membuka
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                // Query UPDATE untuk mengubah data pengguna
                string updateQuery = @"UPDATE EmployeeTbl 
                               SET EmpPhone = @phone, 
                                   EmpAdd = @address, 
                                   EmpDOB = @dob 
                               WHERE EmpName = @username";

                using (SqlCommand cmd = new SqlCommand(updateQuery, Conn))
                {
                    cmd.Parameters.AddWithValue("@username", UsernameTb.Text.Trim()); // Username tidak boleh berubah
                    cmd.Parameters.AddWithValue("@phone", PhoneTb.Text.Trim());
                    cmd.Parameters.AddWithValue("@address", AddressTb.Text.Trim() == "" ? "Belum Disetel" : AddressTb.Text.Trim());
                    cmd.Parameters.AddWithValue("@dob", TglLahirDTP.Value); // Ambil tanggal dari DateTimePicker

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Profil berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Gagal memperbarui profil. Pastikan data benar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }

    }

}
