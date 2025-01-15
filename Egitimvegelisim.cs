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
    public partial class Egitimvegelisim : Form
    {


        // Bağlantı dizesini ProgramDatabaseConfig sınıfından alıyoruz
        private readonly string connectionString = ProgramDatabaseConfig.ConnectionString;


        public Egitimvegelisim()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

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
                string komut = "SELECT * FROM Egitim";

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

        private void Egitimvegelisim_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Ekle();

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
                    string komut = "INSERT INTO Egitim (EgitimTuru, EgitimAdi, EgitimTarihi, EgitimSuresi, KatilanSayisi, BasariYuzdesi, Aciklama, OlusturmaTarihi) " +
                                   "VALUES (@EgitimTuru, @EgitimAdi, @EgitimTarihi, @EgitimSuresi, @KatilanSayisi, @BasariYuzdesi, @Aciklama, @OlusturmaTarihi)";

                    // SQLiteCommand nesnesi oluşturuyoruz
                    using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                    {
                        // Parametreleri ekliyoruz
                        cmd.Parameters.AddWithValue("@EgitimTuru", textEdit2.Text); // Eğitim Türü
                        cmd.Parameters.AddWithValue("@EgitimAdi", textEdit3.Text);  // Eğitim Adı
                        cmd.Parameters.AddWithValue("@EgitimTarihi", textEdit4.Text); // Eğitim Tarihi
                        cmd.Parameters.AddWithValue("@EgitimSuresi", textEdit5.Text); // Eğitim Süresi
                        cmd.Parameters.AddWithValue("@KatilanSayisi", textEdit6.Text); // Katılan Sayısı
                        cmd.Parameters.AddWithValue("@BasariYuzdesi", textEdit7.Text); // Başarı Yüzdesi
                        cmd.Parameters.AddWithValue("@Aciklama", textEdit8.Text); // Açıklama (isteğe bağlı)
                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", textEdit9.Text); // Oluşturma Tarihi

                        // Komutu çalıştırıyoruz (veriyi ekliyoruz)
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Eğitim ve gelişim kaydı başarıyla eklendi!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Sil();

        }
   
            public void Sil()
            {
                // Veritabanı bağlantısını açıyoruz
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {

                    //   DialogResult onay = new MessageBox.Show("Kaydı silmek istediğinize emin misiniz ? ", "Onay Kutusu", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    DialogResult onay = MessageBox.Show("Kaydı silmek istediğinize emin misiniz?", "Onay Kutusu", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (onay == DialogResult.Yes)
                    {
                        conn.Open();
                        string id = gridView1.GetFocusedRowCellValue("EgitimID").ToString();
                        SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Egitim WHERE EgitimID='" + id + "'", conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        Listele();


                        // Silme işlemini gerçekleştir
                    }
                }
            }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //Güncelle
            Guncelle();
          
        }

        public void Guncelle()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string komut = "UPDATE Egitim SET EgitimTuru = @EgitimTuru, EgitimAdi = @EgitimAdi, EgitimTarihi = @EgitimTarihi, " +
                                   "EgitimSuresi = @EgitimSuresi, KatilanSayisi = @KatilanSayisi, BasariYuzdesi = @BasariYuzdesi, " +
                                   "Aciklama = @Aciklama, OlusturmaTarihi = @OlusturmaTarihi WHERE EgitimID = @EgitimID";

                    using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                    {
                        cmd.Parameters.AddWithValue("@EgitimTuru", textEdit2.Text);
                        cmd.Parameters.AddWithValue("@EgitimAdi", textEdit3.Text);
                        cmd.Parameters.AddWithValue("@EgitimTarihi", textEdit4.Text);
                        cmd.Parameters.AddWithValue("@EgitimSuresi", textEdit5.Text);
                        cmd.Parameters.AddWithValue("@KatilanSayisi", textEdit6.Text);
                        cmd.Parameters.AddWithValue("@BasariYuzdesi", textEdit7.Text);
                        cmd.Parameters.AddWithValue("@Aciklama", textEdit8.Text);
                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", textEdit9.Text);
                        cmd.Parameters.AddWithValue("@EgitimID", textEdit1.Text);

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Eğitim kaydı başarıyla güncellendi!");
                Listele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        public void BilgiCek()
        {
            textEdit1.Text = gridView1.GetFocusedRowCellValue("EgitimID").ToString();
            textEdit2.Text = gridView1.GetFocusedRowCellValue("EgitimTuru").ToString();
            textEdit3.Text = gridView1.GetFocusedRowCellValue("EgitimAdi").ToString();
            textEdit4.Text = gridView1.GetFocusedRowCellValue("EgitimTarihi").ToString();
            textEdit5.Text = gridView1.GetFocusedRowCellValue("EgitimSuresi").ToString();
            textEdit6.Text = gridView1.GetFocusedRowCellValue("KatilanSayisi").ToString();
            textEdit7.Text = gridView1.GetFocusedRowCellValue("BasariYuzdesi").ToString();
            textEdit8.Text = gridView1.GetFocusedRowCellValue("Aciklama").ToString();
            textEdit9.Text = gridView1.GetFocusedRowCellValue("OlusturmaTarihi").ToString();

          //  simpleButton3.Text = "Değiştir";
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //Düzenle
            BilgiCek();
        }
    }

    }
    

