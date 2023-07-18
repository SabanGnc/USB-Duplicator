using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Usb_Duplicator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

      
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // USB cihazlarının bağlı olduğu sürücüleri al
            string[] driveList = Directory.GetLogicalDrives();

            // İlk USB sürücüsünü bul
            string sourceDrive = "";
            foreach (string drive in driveList)
            {
                DriveInfo driveInfo = new DriveInfo(drive);
                if (driveInfo.DriveType == DriveType.Removable)
                {
                    sourceDrive = drive;
                    break;
                }
            }

            // İlk USB sürücüsü bulunamazsa hata mesajı göster
            if (sourceDrive == "")
            {
                MessageBox.Show("USB cihazı bulunamadı!");
                return;
            }

            // İkinci USB sürücüsünü bul
            string destinationDrive = "";
            foreach (string drive in driveList)
            {
                DriveInfo driveInfo = new DriveInfo(drive);
                if (driveInfo.DriveType == DriveType.Removable && drive != sourceDrive)
                {
                    destinationDrive = drive;
                    break;
                }
            }

            // İkinci USB sürücüsü bulunamazsa hata mesajı göster
            if (destinationDrive == "")
            {
                MessageBox.Show("İkinci USB cihazı bulunamadı!");
                return;
            }

            // Kaynak ve hedef dizinleri oluştur
            string sourceDirectory = sourceDrive + "\\";
            string destinationDirectory = destinationDrive + "\\";

            // Kaynak dizinindeki dosyaları ve alt klasörleri hedef dizine kopyala
            try
            {
                CopyFiles(sourceDirectory, destinationDirectory);

                MessageBox.Show("Dosyalar başarıyla kopyalandı!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }

        private void CopyFiles(string sourceDir, string destinationDir)
        {
            Directory.CreateDirectory(destinationDir);

            // Kaynak dizinindeki dosyaları kopyala
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(file);
                string destinationFile = Path.Combine(destinationDir, fileName);
                File.Copy(file, destinationFile, true);
            }

            // Kaynak dizinindeki alt klasörleri dolaşarak kopyala
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string dirName = Path.GetFileName(subDir);
                string destinationSubDir = Path.Combine(destinationDir, dirName);
                CopyFiles(subDir, destinationSubDir);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Formun ekranın ortasında konumlanması için gereken hesaplamaları yap
            int x = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            int y = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;

            // Formun konumunu güncelle
            this.Location = new Point(x, y);

            //-------------------------------------------

            // Formun adını değiştir
            this.Text = "USB DUPLICATOR";
        }
    }
}
