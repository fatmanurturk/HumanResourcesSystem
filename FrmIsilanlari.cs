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
    public partial class FrmIsilanlari : Form
    {

        // Bağlantı dizesini ProgramDatabaseConfig sınıfından alıyoruz
        private readonly string connectionString = ProgramDatabaseConfig.ConnectionString;


        public FrmIsilanlari()
        {
            InitializeComponent();
        }

        private void Btnlistele_Click(object sender, EventArgs e)
        {
            Listele();
        }

        private void Btnekle_Click(object sender, EventArgs e)
        {
            // Ekleme işlemi
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
                    string komut = "INSERT INTO IsIlanlari (IlanBasligi, IlanAciklamasi, DepartmanID, YayimlanmaTarihi, KapanisTarihi, OlusturmaTarihi) " +
                                   "VALUES (@IlanBasligi, @IlanAciklamasi, @DepartmanID, @YayimlanmaTarihi, @KapanisTarihi, @OlusturmaTarihi)";

                    // SQLiteCommand nesnesi oluşturuyoruz
                    using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                    {
                        // Parametreleri ekliyoruz
                        cmd.Parameters.AddWithValue("@IlanBasligi", textEdit1.Text); // İlan Başlığı
                        cmd.Parameters.AddWithValue("@IlanAciklamasi", textEdit2.Text); // İlan Açıklaması
                        cmd.Parameters.AddWithValue("@DepartmanID",textEdit6.Text); // DepartmanID


                        cmd.Parameters.AddWithValue("@YayimlanmaTarihi", textEdit3.Text); // Yayınlanma Tarihi

                        cmd.Parameters.AddWithValue("@KapanisTarihi",textEdit4.Text); // Kapanış Tarihi

                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", textEdit5.Text); // Oluşturma Tarihi (şu anki tarih ve saat)

                        // Komutu çalıştırıyoruz (veriyi ekliyoruz)
                        cmd.ExecuteNonQuery();
                    }

                    // Listele metodunu çağırarak güncel veriyi listele
                    Listele();

                    MessageBox.Show("İş ilanı başarıyla eklendi!");

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        private void FrmIsilanlari_Load(object sender, EventArgs e)
        {
            Listele();

        }
        public void Listele()
        {
            try
            {
                // SQL sorgusu
                string komut = "SELECT * FROM IsIlanlari";

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

        private void Btnsil_Click(object sender, EventArgs e)
        {
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

                object idObj = gridView1.GetFocusedRowCellValue("IlanID");

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
                        using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM IsIlanlari WHERE IlanID = @id", conn))
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
        
        private void textEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txtdepartmanıd_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void Btnguncelle_Click(object sender, EventArgs e)
        {
            //Güncelle
           Guncelle();
          
        }


        public void Guncelle()
        {
            try
            {
                // Ilan ID boş mu veya geçersiz mi kontrol et
                if (string.IsNullOrWhiteSpace(textilanid.Text) || !int.TryParse(textilanid.Text, out int ilanId))
                {
                    MessageBox.Show("Geçersiz İlan ID!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Veritabanı bağlantısını aç
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();  // Bağlantıyı aç

                    // SQL UPDATE sorgusu
                    string komut = @"
            UPDATE IsIlanlari SET 
            IlanBasligi = @IlanBasligi, 
            IlanAciklamasi = @IlanAciklamasi, 
            DepartmanID = @DepartmanID, 
            YayimlanmaTarihi = @YayimlanmaTarihi, 
            KapanisTarihi = @KapanisTarihi, 
            OlusturmaTarihi = @OlusturmaTarihi 
            WHERE IlanID = @IlanID";

                    using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                    {
                        // Parametreleri ekle
                        cmd.Parameters.AddWithValue("@IlanBasligi", textEdit1.Text);
                        cmd.Parameters.AddWithValue("@IlanAciklamasi", textEdit2.Text);
                        cmd.Parameters.AddWithValue("@DepartmanID", textEdit6.Text);
                        cmd.Parameters.AddWithValue("@YayimlanmaTarihi", textEdit3.Text);
                        cmd.Parameters.AddWithValue("@KapanisTarihi", textEdit4.Text);
                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", textEdit5.Text);
                        cmd.Parameters.AddWithValue("@IlanID", textilanid.Text);

                        // Komutu çalıştır ve etkilenen satır sayısını kontrol et
                        int etkilenenSatir = cmd.ExecuteNonQuery();

                        if (etkilenenSatir > 0)
                        {
                            MessageBox.Show("İş ilanı başarıyla güncellendi!");
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
                textilanid.Text = gridView1.GetFocusedRowCellValue("IlanID").ToString();
                textEdit1.Text = gridView1.GetFocusedRowCellValue("IlanBasligi").ToString();
                textEdit2.Text = gridView1.GetFocusedRowCellValue("IlanAciklamasi").ToString();
                textEdit6.Text = gridView1.GetFocusedRowCellValue("DepartmanID").ToString();
                textEdit3.Text = gridView1.GetFocusedRowCellValue("YayimlanmaTarihi").ToString();
                textEdit4.Text = gridView1.GetFocusedRowCellValue("KapanisTarihi").ToString();
                textEdit5.Text = gridView1.GetFocusedRowCellValue("OlusturmaTarihi").ToString();

              //  simpleButton3.Text = "Değiştir";
            }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //Düzenle
            BilgiCek();
        }
    }
}
    