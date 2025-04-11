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
    public partial class Delivery : Form
    {
        public Delivery()
        {
            InitializeComponent();
            EmpNameLbl.Text = Login.EmpName;
        }



        //route halaman
        private void button1_Click(object sender, EventArgs e)
        {
            Home homeDelivery = new Home();
            homeDelivery.Show();
            this.Hide();
        }

        private void btnMenuProduk_Click(object sender, EventArgs e)
        {
            Produk produkDelivery = new Produk();
            produkDelivery.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Pegawai pegawaiDelivery = new Pegawai();
            pegawaiDelivery.Show();
            this.Hide();
        }

        private void btnMenuPelanggan_Click(object sender, EventArgs e)
        {
            Customer customerDelivery = new Customer();
            customerDelivery.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Billing billingDelivery = new Billing();
            billingDelivery.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Transaksi transaksiDelivery = new Transaksi();
            transaksiDelivery.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Delivery deliveryDelivery = new Delivery();
            deliveryDelivery.Show();
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
            Profile profileDelivery = new Profile();
            profileDelivery.Show();
            this.Hide();
        }


    }
}
