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
using DevExpress.XtraEditors;

namespace p1
{
    public partial class Form1 : Form
    {
        // SQLite veritabanı bağlantı dizesi
        private readonly string connectionString = ProgramDatabaseConfig.ConnectionString;

        public Form1()
        {
            InitializeComponent();
        }

        private void butongirisyap_Click(object sender, EventArgs e)
        {
            string email = txtemail.Text.Trim();
            string sifre = txtsifre.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sifre))
            {
                XtraMessageBox.Show("Lütfen tüm alanları doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM yöneticiler WHERE email = @Email AND şifre = @Sifre";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Sifre", sifre);

                        int count = Convert.ToInt32(command.ExecuteScalar());

                        if (count > 0)
                        {
                            XtraMessageBox.Show("Giriş başarılı! Hoş geldiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ANASAYFA anasayfa = new ANASAYFA();
                            anasayfa.Show();
                            this.Hide();
                        }
                        else
                        {
                            XtraMessageBox.Show("Giriş başarısız. Kayıtlı değilseniz, lütfen kayıt olun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void labelControl2_Click(object sender, EventArgs e)
        {

        }

        private void txtemail_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

