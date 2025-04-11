using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace toko_laptop_tugas
{
    public partial class ProdukCust : Form
    {
        public ProdukCust()
        {
            InitializeComponent();
            CustNameLbl.Text = Login.CustName;
            DisplayProducts();

        }

        public class BillItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int Qty { get; set; }
            public int Price { get; set; }
            public int Total { get; set; }
        }


        SqlConnection Conn = new SqlConnection(DBConnection.ConnectionString);
        
        int key = 0, Stock = 0;

        private void reset()
        {
            PrNameTb.Text = "";
            PrPriceTb.Text = "";
            QtyTb.Text = "";
            Stock = 0;
            key = 0;
        }

        private void UpdateProductStock(int productId, int qtyToReduce)
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string query = "UPDATE ProductTbl SET PrQty = PrQty - @qty WHERE PrId = @id";
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@qty", qtyToReduce);
                cmd.Parameters.AddWithValue("@id", productId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal update stok: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }


        private int GetCurrentStock(int productId)
        {
            int currentStock = 0;
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string query = "SELECT PrQty FROM ProductTbl WHERE PrId = @ProductId";
                using (SqlCommand cmd = new SqlCommand(query, Conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        currentStock = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error mengambil stok produk: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
            return currentStock;
        }


        //route halaman
        private void ProdukBtn_Click(object sender, EventArgs e)
        {
            ProdukCust produkCustProduk = new ProdukCust();
            produkCustProduk.Show();
            this.Hide();
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            HomeCust homeCustProduk = new HomeCust();
            homeCustProduk.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TransaksiCust transaksiCustProduk = new TransaksiCust();
            transaksiCustProduk.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeliveryCust deliveryCustProduk = new DeliveryCust();
            deliveryCustProduk.Show();
            this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            ProfileCust profileCustProduk = new ProfileCust();
            profileCustProduk.Show();
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

        private void button3_Click(object sender, EventArgs e)
        {
            // Dictionary untuk total Qty tiap produk
            Dictionary<int, (string productName, int totalQty)> productQtyDict = new Dictionary<int, (string, int)>();

            foreach (DataGridViewRow row in BillDGV.Rows)
            {
                if (!row.IsNewRow)
                {
                    int productId = Convert.ToInt32(row.Cells["ProductID"].Value);
                    string productName = row.Cells["ProductName"].Value.ToString();
                    int qty = Convert.ToInt32(row.Cells["Quantity"].Value);

                    // Validasi jumlah negatif
                    if (qty <= 0)
                    {
                        MessageBox.Show($"Jumlah untuk produk {productName} tidak valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (productQtyDict.ContainsKey(productId))
                    {
                        productQtyDict[productId] = (productName, productQtyDict[productId].totalQty + qty);
                    }
                    else
                    {
                        productQtyDict[productId] = (productName, qty);
                    }
                }
            }

            // Validasi stok untuk setiap produk
            foreach (var entry in productQtyDict)
            {
                int productId = entry.Key;
                string productName = entry.Value.productName;
                int totalQty = entry.Value.totalQty;

                int currentStock = GetCurrentStock(productId);
                if (totalQty > currentStock)
                {
                    MessageBox.Show($"Stok tidak cukup untuk produk {productName}. Dipilih: {totalQty}, Stok tersedia: {currentStock}",
                                    "Stok Tidak Cukup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Kalau semua valid → lanjutkan
            List<BillItem> items = new List<BillItem>();
            foreach (DataGridViewRow row in BillDGV.Rows)
            {
                if (!row.IsNewRow)
                {
                    items.Add(new BillItem
                    {
                        ProductId = Convert.ToInt32(row.Cells["ProductID"].Value),
                        ProductName = row.Cells["ProductName"].Value.ToString(),
                        Qty = Convert.ToInt32(row.Cells["Quantity"].Value),
                        Price = Convert.ToInt32(row.Cells["Price"].Value),
                        Total = Convert.ToInt32(row.Cells["Total"].Value)
                    });
                }
            }
            
            CheckoutPopupFormCust checkout = new CheckoutPopupFormCust(items)
            {
                Owner = this
            };
            checkout.ShowDialog();
        }

        public void DisplayProducts()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string Query = "SELECT * FROM ProductTbl";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Conn);
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                productsDGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error di DisplayProduct: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }


        public void ResetBillDGV()
        {
            BillDGV.Rows.Clear();
            TotalLbl.Text = "0";
            n = 0;       // Variabel untuk penomoran baris
            GrdTotal = 0;
        }



        private void productsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                DataGridViewRow row = productsDGV.Rows[e.RowIndex];
                PrNameTb.Text = row.Cells[1].Value.ToString();
                Stock = Convert.ToInt32(row.Cells[3].Value.ToString());
                PrPriceTb.Text = row.Cells[4].Value.ToString();

                if (PrNameTb.Text == "")
                    key = 0;
                else
                    key = Convert.ToInt32(row.Cells[0].Value.ToString());
            }
        }

        private void BillDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }



        private void ProdukCust_Load(object sender, EventArgs e)
        {
            // Pastikan DataGridView kosong terlebih dahulu
            BillDGV.Columns.Clear();
            BillDGV.Columns.Add("No", "No");
            BillDGV.Columns.Add("ProductID", "Product ID");  // Kolom ini bisa disembunyikan
            BillDGV.Columns.Add("ProductName", "Product Name");
            BillDGV.Columns.Add("Quantity", "Quantity");
            BillDGV.Columns.Add("Price", "Price");
            BillDGV.Columns.Add("Total", "Total");

            // Sembunyikan kolom ProductID agar tidak tampil ke user
            BillDGV.Columns["ProductID"].Visible = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            reset();
        }


        int n = 0, GrdTotal = 0;

        private void btnHapusBill_Click(object sender, EventArgs e)
        {
            // Pastikan ada baris yang dipilih
            if (BillDGV.SelectedRows.Count > 0)
            {
                // Ambil baris yang dipilih (misalnya baris pertama dari yang terpilih)
                DataGridViewRow selectedRow = BillDGV.SelectedRows[0];

                // Konfirmasi apakah user yakin ingin menghapus item
                DialogResult result = MessageBox.Show("Apakah Anda yakin ingin menghapus item ini dari bill?",
                                                      "Konfirmasi Hapus",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Ambil nilai Total dari baris terpilih
                    int rowTotal = Convert.ToInt32(selectedRow.Cells["Total"].Value);

                    // Kurangi total keseluruhan (GrdTotal) dengan total dari baris tersebut
                    GrdTotal -= rowTotal;

                    // Hapus baris dari DataGridView
                    BillDGV.Rows.Remove(selectedRow);

                    // Perbarui label total agar sesuai dengan GrdTotal yang baru
                    TotalLbl.Text = GrdTotal.ToString();

                    // Jika kamu perlu mengupdate data juga di database, lakukan di sini
                    // Misalnya, panggil method untuk update tabel BillTbl atau BillDetailTbl sesuai kebutuhan.
                }
            }
            else
            {
                MessageBox.Show("Pilih item/ baris yang akan dihapus!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnAddBill_Click(object sender, EventArgs e)
        {
            // Pastikan Qty tidak kosong
            if (string.IsNullOrEmpty(QtyTb.Text.Trim()))
            {
                MessageBox.Show("Masukkan jumlah produk!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Parsing nilai quantity yang dimasukkan
            int qty;
            if (!int.TryParse(QtyTb.Text.Trim(), out qty))
            {
                MessageBox.Show("Jumlah produk tidak valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Mengecek apakah qty yang diinput melebihi stok yang tersedia
            if (qty > Stock)
            {
                MessageBox.Show("Stok tidak cukup!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Pastikan ada produk yang dipilih (misalnya key tidak 0)
            if (key == 0)
            {
                MessageBox.Show("Pilih produk terlebih dahulu", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Jika semua validasi terpenuhi, hitung total harga
            int total = qty * Convert.ToInt32(PrPriceTb.Text);

            // Membuat baris baru pada DataGridView BillDGV
            DataGridViewRow newRow = new DataGridViewRow();
            // Pastikan untuk memasukkan ProductID ke kolom tersembunyi, dsb.
            newRow.CreateCells(BillDGV, n + 1, key, PrNameTb.Text, qty, PrPriceTb.Text, total);

            // Update total keseluruhan
            GrdTotal += total;
            BillDGV.Rows.Add(newRow);
            n++;

            TotalLbl.Text = GrdTotal.ToString();
            // UpdateStock(); // Panggil fungsi update stok jika diperlukan
            reset(); // Reset form input setelah menambahkan item
        }

    }
}
