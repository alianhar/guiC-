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
    public partial class DeliveryCust : Form
    {
        public DeliveryCust()
        {
            InitializeComponent();
            CustNameLbl.Text = Login.CustName;
        }


        //route halaman
        private void HomeBtn_Click(object sender, EventArgs e)
        {
            HomeCust homeCustDelivery = new HomeCust();
            homeCustDelivery.Show();
            this.Hide();
        }

        private void ProdukBtn_Click(object sender, EventArgs e)
        {
            ProdukCust produkCustDelivery = new ProdukCust();
            produkCustDelivery.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TransaksiCust transaksiCustDelivery = new TransaksiCust();
            transaksiCustDelivery.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeliveryCust deliveryCustDelivery = new DeliveryCust();
            deliveryCustDelivery.Show();
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
            ProfileCust profileCustDelivery = new ProfileCust();
            profileCustDelivery.Show();
            this.Hide();
        }
    }
}
