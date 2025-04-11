using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace toko_laptop_tugas
{
    public partial class Transaksi : Form
    {
        public Transaksi()
        {
            InitializeComponent();
            EmpNameLbl.Text = Login.EmpName;
        }


        //route halaman
        private void button1_Click(object sender, EventArgs e)
        {
            Home homeTransaksi = new Home();
            homeTransaksi.Show();
            this.Hide();
        }

        private void btnMenuProduk_Click(object sender, EventArgs e)
        {
            Produk produkTransaksi = new Produk();
            produkTransaksi.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Pegawai pegawaiTransaksi = new Pegawai();
            pegawaiTransaksi.Show();
            this.Hide();
        }

        private void btnMenuPelanggan_Click(object sender, EventArgs e)
        {
            Customer customerTransaksi = new Customer();
            customerTransaksi.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Billing billingTransaksi = new Billing();
            billingTransaksi.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Transaksi transaksiTransaksi = new Transaksi();
            transaksiTransaksi.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Delivery deliveryTransaksi = new Delivery();
            deliveryTransaksi.Show();
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
            Profile profileTransaksi = new Profile();
            profileTransaksi.Show();
            this.Hide();
        }
    }
}
