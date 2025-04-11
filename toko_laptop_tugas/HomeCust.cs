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
    public partial class HomeCust : Form
    {
        public HomeCust()
        {
            InitializeComponent();
            CustNameLbl.Text = Login.CustName;
        }

    

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            
        
            ProfileCust profileHome = new ProfileCust();
            profileHome.Show();
            this.Hide();
        
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            HomeCust homeHome =  new HomeCust();
            homeHome.Show();
            this.Hide();
        }

        private void ProdukBtn_Click(object sender, EventArgs e)
        {
            ProdukCust produkCust = new ProdukCust();
            produkCust.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TransaksiCust transaksiCust = new TransaksiCust();
            transaksiCust.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeliveryCust deliveryCust = new DeliveryCust();
            deliveryCust.Show();
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
    }


}
