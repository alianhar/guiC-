using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static toko_laptop_tugas.ProdukCust;

namespace toko_laptop_tugas
{
    public partial class CheckoutPopupFormCust : Form
    {
        public CheckoutPopupFormCust()
        {
            InitializeComponent();
        }

        List<BillItem> billItems;

        public CheckoutPopupFormCust(List<BillItem> items)
        {
            InitializeComponent();
            billItems = items;
        }

        SqlConnection conn = new SqlConnection(DBConnection.ConnectionString);

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Filter hanya file gambar (jpg, jpeg, png, bmp)
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Pilih Gambar untuk Bukti Transfer";

                // Jika user memilih file dan klik OK
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    // Opsional: Tampilkan gambar di PictureBox
                    try
                    {
                        // Pastikan file yang dipilih dapat dibuka sebagai gambar
                        Image selectedImage = Image.FromFile(filePath);
                        pictureBoxPreview.Image = selectedImage;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal membuka gambar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Simpan gambar ke file terlebih dahulu
            string savePath = "";
            if (pictureBoxPreview.Image != null)
            {
                string folderPath = Path.Combine(Application.StartupPath, "BuktiTransfer");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileName = "bukti_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpg";
                savePath = Path.Combine(folderPath, fileName);
                pictureBoxPreview.Image.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                MessageBox.Show("Gambar berhasil disimpan", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Tidak ada gambar yang dipilih!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mulai simpan data transaksi ke database, sertakan path gambar sebagai parameter
            SqlTransaction transaction = null;
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                transaction = conn.BeginTransaction();

                string insertBillQuery = @"
            INSERT INTO BillTbl (BDate, CustId, CustName, Amt, PaymentDate, DeliveryAddress, PaymentProof)
            OUTPUT INSERTED.BNum
            VALUES (@BDate, @CustId, @CustName, @CustTotal, @PaymentDate, @CustAddress, @PaymentProof)";

                SqlCommand insertBill = new SqlCommand(insertBillQuery, conn, transaction);
                insertBill.Parameters.AddWithValue("@BDate", DateTime.Now);
                insertBill.Parameters.AddWithValue("@CustId", Login.CustId);  // Pastikan Login.CustId sudah diset saat login
                insertBill.Parameters.AddWithValue("@CustName", Login.CustName);
                insertBill.Parameters.AddWithValue("@CustTotal", billItems.Sum(i => i.Total));
                insertBill.Parameters.AddWithValue("@PaymentDate", DateTime.Now); // atau tanggal pembayaran yang sesuai
                insertBill.Parameters.AddWithValue("@CustAddress", addressDeliveryTb.Text);
                insertBill.Parameters.AddWithValue("@PaymentProof", savePath);

                int billId = (int)insertBill.ExecuteScalar();

                // Simpan detail transaksi ke BillDetailTbl
                foreach (var item in billItems)
                {
                    string insertDetailQuery = @"
                INSERT INTO BillDetailTbl (BillId, ProductId, Quantity)
                VALUES (@BillId, @ProductId, @Qty)";
                    SqlCommand insertDetail = new SqlCommand(insertDetailQuery, conn, transaction);
                    insertDetail.Parameters.AddWithValue("@BillId", billId);
                    insertDetail.Parameters.AddWithValue("@ProductId", item.ProductId);
                    insertDetail.Parameters.AddWithValue("@Qty", item.Qty);
                    insertDetail.ExecuteNonQuery();
                }

                transaction.Commit();
                MessageBox.Show("Transaksi berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                MessageBox.Show("Gagal menyimpan transaksi: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }



    }


}
