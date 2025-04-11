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
    public partial class TransaksiCust : Form
    {
        public TransaksiCust()
        {
            InitializeComponent();
            CustNameLbl.Text = Login.CustName;
        }





        //route halaman
        private void HomeBtn_Click(object sender, EventArgs e)
        {
            HomeCust homeCustTransaksi = new HomeCust();
            homeCustTransaksi.Show();
            this.Hide();
        }

        private void ProdukBtn_Click(object sender, EventArgs e)
        {
            ProdukCust produkCustTransaksi = new ProdukCust();
            produkCustTransaksi.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TransaksiCust transaksiCustTransaksi = new TransaksiCust();
            transaksiCustTransaksi.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeliveryCust deliveryCustTransaksi = new DeliveryCust();
            deliveryCustTransaksi.Show();
            this.Hide();
        }

        private void SignOutBtn_Click(object sender, EventArgs e)
        { 
            // Kosongkan variabel static agar tidak menyimpan sesi login sebelumnya
            Login.CustName = null;

            // Tampilkan form login kembali
            Login loginForm = new Login();
            loginForm.Show();

            // Tutup form Home
            this.Close();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            ProfileCust profileCustTransaksi = new ProfileCust();
            profileCustTransaksi.Show();
            this.Hide();
        }
    }
}
