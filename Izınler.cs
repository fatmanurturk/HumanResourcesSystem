using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace p1.Formlar
{
    public partial class Izınler : Form
    {



        // Bağlantı dizesini ProgramDatabaseConfig sınıfından alıyoruz
        private readonly string connectionString = ProgramDatabaseConfig.ConnectionString;

        public Izınler()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
          //  Ekle();
        }


        public void Ekle()
        {
            try
            {
                // Veritabanı bağlantısını açıyoruz
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open(); // Bağlantıyı açıyoruz

                    // INSERT komutunu hazırlıyoruz
                    string komut = "INSERT INTO Izinler (CalisanID, IzinTuru, BaslangicTarihi, Bitistarihi, ToplamGun, Durum, Aciklama, OlusturmaTarihi) " +
                                   "VALUES (@CalisanID, @IzinTuru, @BaslangicTarihi, @Bitistarihi, @ToplamGun, @Durum, @Aciklama, @OlusturmaTarihi)";

                    // SQLiteCommand nesnesi oluşturuyoruz
                    using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                    {
                        // Parametreleri ekliyoruz
                        cmd.Parameters.AddWithValue("@CalisanID", textEdit1.Text); // Ad
                        cmd.Parameters.AddWithValue("@IzinTuru", textEdit2.Text); // Soyad
                        cmd.Parameters.AddWithValue("@BaslangicTarihi", textEdit3.Text); // Eposta (Integer olarak alınıyor)
                        cmd.Parameters.AddWithValue("@BitisTarihi", textEdit4.Text); // Telefon
                                                                                     //  cmd.Parameters.AddWithValue("@YayimlanmaTarihi", DateTime.Parse(textEdit3.Text).ToString("yyyy-MM-dd")); // Yayınlanma Tarihi

                        cmd.Parameters.AddWithValue("@ToplamGun", textEdit5.Text); // Işe giriş tarihi (yyyyMMdd formatında)

                        cmd.Parameters.AddWithValue("@Durum", textEdit6.Text); // DepartmanID
                        cmd.Parameters.AddWithValue("@Aciklama", textEdit7.Text); // Görev

                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", textEdit9.Text); // Oluşturma tarihi (şu anki tarih)

                        // Komutu çalıştırıyoruz (veriyi ekliyoruz)
                        cmd.ExecuteNonQuery();
                    }
                    //  Listele();

                    MessageBox.Show("Çalışan başarıyla eklendi!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Listele();
        }


        public void Listele()
        {
            try
            {
                // SQL sorgusu
                string komut = "SELECT * FROM Izinler";

                // SQLiteConnection nesnesini burada oluşturuyoruz
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();  // Bağlantıyı aç

                    // SQLiteDataAdapter ile veriyi çekiyoruz
                    SQLiteDataAdapter da = new SQLiteDataAdapter(komut, conn);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    // Veriyi DataGridView'e yüklüyoruz
                    //  dataGridView1.DataSource = ds.Tables[0];

                    gridControl1.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void Izınler_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void textEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //Güncelle
          
        }






        public void BilgiCek()
        {

            textEdit1.Text = gridView1.GetFocusedRowCellValue("IzinID").ToString();
            textEdit2.Text = gridView1.GetFocusedRowCellValue("CalisanID").ToString();
            textEdit3.Text = gridView1.GetFocusedRowCellValue("IzinTuru").ToString();
            textEdit4.Text = gridView1.GetFocusedRowCellValue("BaslangicTarihi").ToString();
            textEdit5.Text = gridView1.GetFocusedRowCellValue("BitisTarihi").ToString();
            textEdit6.Text = gridView1.GetFocusedRowCellValue("ToplamGun").ToString();
            textEdit7.Text = gridView1.GetFocusedRowCellValue("Durum").ToString();
            textEdit8.Text = gridView1.GetFocusedRowCellValue("Aciklama").ToString();
            textEdit9.Text = gridView1.GetFocusedRowCellValue("OlusturmaTarihi").ToString();
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            //Düzenle
            BilgiCek();
        }
    }
}
