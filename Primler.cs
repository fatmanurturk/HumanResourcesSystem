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
    public partial class Primler : Form
    {


        // Bağlantı dizesini ProgramDatabaseConfig sınıfından alıyoruz
        private readonly string connectionString = ProgramDatabaseConfig.ConnectionString;

        public Primler()
        {
            InitializeComponent();
        }

      private void simpleButton1_Click(object sender, EventArgs e)
        {
            //Ekle
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
                    string komut = "INSERT INTO Primler (CalisanID, BordroID, PrimTutari, PrimTarihi, Aciklama, OlusturmaTarihi) " +
                                   "VALUES (@CalisanID, @BordroID, @PrimTutari, @PrimTarihi, @Aciklama, @OlusturmaTarihi)";

                    // SQLiteCommand nesnesi oluşturuyoruz
                    using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                    {
                        // Parametreleri ekliyoruz
                        cmd.Parameters.AddWithValue("@CalisanID", textEdit1.Text); // Çalışan ID (örneğin TextEdit1'e yazılan değer)
                        cmd.Parameters.AddWithValue("@BordroID", textEdit2.Text); // Bordro ID
                        cmd.Parameters.AddWithValue("@PrimTutari", textEdit3.Text); // Prim Tutarı
                        cmd.Parameters.AddWithValue("@PrimTarihi", textEdit4.Text); // Prim Tarihi (gün/ay/yıl formatında)
                        cmd.Parameters.AddWithValue("@Aciklama", textEdit5.Text); // Açıklama
                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", textEdit6.Text); // Oluşturma Tarihi (şu anki tarih)

                        // Komutu çalıştırıyoruz (veriyi ekliyoruz)
                        cmd.ExecuteNonQuery();
                    }

                    // Listele metodunu çağırarak güncel veriyi listele
                    Listele();

                    MessageBox.Show("Prim başarıyla eklendi!");

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
        

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //Sil
            Sil();
        }

       public void Sil()
        {
            DialogResult onay = MessageBox.Show("Kaydı silmek istediğinize emin misiniz?", "Onay Kutusu", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (onay == DialogResult.Yes)
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    MessageBox.Show("Lütfen silmek için bir satır seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                object idObj = gridView1.GetFocusedRowCellValue("PrimID").ToString();

                if (idObj == null || string.IsNullOrEmpty(idObj.ToString()))
                {
                    MessageBox.Show("Seçili kaydın ID bilgisi bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string id = idObj.ToString();

                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                    {
                        conn.Open();
                        using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Primler WHERE PrimID = @id", conn))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    Listele();
                    MessageBox.Show("Kayıt başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        


        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //  Guncelle();
            Guncelle();
        }

        public void Guncelle()
        {
            try
            {
                // Prim ID boş veya geçersiz mi kontrol et
                if (string.IsNullOrWhiteSpace(textEdit7.Text) || !int.TryParse(textEdit7.Text, out int primId))
                {
                    MessageBox.Show("Geçersiz Prim ID!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Veritabanı bağlantısını aç
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();  // Bağlantıyı aç

                    // SQL UPDATE sorgusu
                    string komut = @"
            UPDATE Primler SET 
            CalisanID = @CalisanID, 
            BordroID = @BordroID, 
            PrimTutari = @PrimTutari, 
            PrimTarihi = @PrimTarihi, 
            Aciklama = @Aciklama, 
            OlusturmaTarihi = @OlusturmaTarihi 
            WHERE PrimID = @PrimID";

                    using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                    {
                        // Parametreleri ekle
                        cmd.Parameters.AddWithValue("@CalisanID", textEdit1.Text);
                        cmd.Parameters.AddWithValue("@BordroID", textEdit2.Text);
                        cmd.Parameters.AddWithValue("@PrimTutari", textEdit3.Text);
                        cmd.Parameters.AddWithValue("@PrimTarihi", textEdit4.Text);
                        cmd.Parameters.AddWithValue("@Aciklama", textEdit5.Text);
                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", textEdit6.Text);
                        cmd.Parameters.AddWithValue("@PrimID", textEdit7.Text);

                        // Komutu çalıştır ve etkilenen satır sayısını kontrol et
                        int etkilenenSatir = cmd.ExecuteNonQuery();

                        if (etkilenenSatir > 0)
                        {
                            MessageBox.Show("Prim başarıyla güncellendi!");
                            Listele(); // Güncellemeden sonra listeyi yenile
                        }
                        else
                        {
                            MessageBox.Show("Güncellenecek kayıt bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Güncelleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void BilgiCek()
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                MessageBox.Show("Lütfen bir satır seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            textEdit7.Text = gridView1.GetFocusedRowCellValue("PrimID").ToString();
            textEdit1.Text = gridView1.GetFocusedRowCellValue("CalisanID")?.ToString();
            textEdit2.Text = gridView1.GetFocusedRowCellValue("BordroID")?.ToString();
            textEdit3.Text = gridView1.GetFocusedRowCellValue("PrimTutari")?.ToString();
            textEdit4.Text = gridView1.GetFocusedRowCellValue("PrimTarihi")?.ToString();
            textEdit5.Text = gridView1.GetFocusedRowCellValue("Aciklama")?.ToString();
            textEdit6.Text = gridView1.GetFocusedRowCellValue("OlusturmaTarihi")?.ToString();

            simpleButton3.Text = "Güncelle";
        }


        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //Listele
            Listele();
        }
        public void Listele()
        {
            try
            {
                // SQL sorgusu
                string komut = "SELECT * FROM Primler";

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
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void Primler_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //Düzenle
            BilgiCek();
        }
    }
}
