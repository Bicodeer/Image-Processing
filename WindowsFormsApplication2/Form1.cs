using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        private Bitmap Asil;
        private Bitmap bitmap;
        private Bitmap yedek;
        private Bitmap gecici;
        private Bitmap temp;
        private Bitmap tmp;
        private Bitmap histogramTemp;
        private Bitmap logTemp;
        private Bitmap gamaTemp;
        private Bitmap meanTemp;
        private Bitmap medianTemp;
        private Bitmap kontrantTemp;
        private Bitmap sobelTemp;
        private Bitmap histGermeTemp;
        private Bitmap prewittTemp;
        private Bitmap dondurTemp;
        private Bitmap buyukOlcekTemp;
        private Bitmap ortaOlcekTemp;
        private Bitmap kucukOlcekTemp;
        private Bitmap kirkBesTemp;
        private Bitmap doksanTemp;
        private Bitmap yuzSeksenTemp;
        private Bitmap ikiYuzYetmisTemp;
        private Bitmap ikinciPict;
        private Bitmap output_img;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void bitmapRefresh()
        {
            Asil = (Bitmap)pictureBox1.Image;
            bitmap = Asil;
            yedek = Asil;
            gecici = Asil;
            temp = Asil;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.jpg, *.png, *bmp) | *.jpg; *.png; *.bmp";

                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    bitmap = new Bitmap(dlg.FileName);
                    yedek = new Bitmap(dlg.FileName);
                    gecici = new Bitmap(dlg.FileName);
                    temp = new Bitmap(dlg.FileName);
                    tmp = new Bitmap(dlg.FileName);
                    logTemp = new Bitmap(dlg.FileName);
                    histogramTemp = new Bitmap(dlg.FileName);
                    gamaTemp = new Bitmap(dlg.FileName);
                    meanTemp = new Bitmap(dlg.FileName);
                    medianTemp = new Bitmap(dlg.FileName);
                    kontrantTemp = new Bitmap(dlg.FileName);
                    sobelTemp = new Bitmap(dlg.FileName);
                    histGermeTemp = new Bitmap(dlg.FileName);
                    prewittTemp = new Bitmap(dlg.FileName);
                    dondurTemp = new Bitmap(dlg.FileName);
                    buyukOlcekTemp = new Bitmap(dlg.FileName);
                    ortaOlcekTemp = new Bitmap(dlg.FileName);
                    kucukOlcekTemp = new Bitmap(dlg.FileName);
                    kirkBesTemp = new Bitmap(dlg.FileName);
                    doksanTemp = new Bitmap(dlg.FileName);
                    yuzSeksenTemp = new Bitmap(dlg.FileName);
                    ikiYuzYetmisTemp = new Bitmap(dlg.FileName);
                    pictureBox1.Image = bitmap;
                }
            }
        }
        private void resmiSiyahBeyazaDonusturmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bitmap == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.SiyahBeyazaDonusur(bitmap);
            }
        }

        private void resmiGriTonaDönüştürmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tmp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.griton(tmp);
            }

        }

        private void resminNegatifiniAlmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (yedek == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.ResminNegatifi(yedek);
            }
        }

        private void zoomOutİşlemiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gecici == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.zoomOut(gecici);
            }
        }

        private void zoomInİşlemiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (temp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.zoomIn(temp, temp.Width, temp.Height);
            }
        }

        private void komşulukPikselininBulunmasıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show();
        }

        private void ikiResminToplanmasıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            bitmapRefresh();

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.jpg, *.png, *bmp) | *.jpg; *.png; *.bmp";

                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    ikinciPict = new Bitmap(dlg.FileName);
                }

                if (ikinciPict != null)
                {
                    bitmap = new Bitmap(bitmap, pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
                    ikinciPict = new Bitmap(ikinciPict, pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
                    pictureBox2.Image = null;


                    Bitmap bmp = new Bitmap(bitmap, pictureBox1.Width - 2, pictureBox1.Height - 2);

                    int nOp = (int)Enum.GetValues(typeof(ImageFunction.BlendOperation)).GetValue(0);
                    try
                    {
                        ImageFunction.BlendImages(bmp, 0, 0, bmp.Width, bmp.Height, ikinciPict, 0, 0, (ImageFunction.BlendOperation)nOp);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.ToString());
                    }

                    pictureBox2.Image = new Bitmap(bmp, pictureBox2.ClientSize.Width, pictureBox2.ClientSize.Height);
                }

            }
        }

        private void birResimdenDiğerininÇıkarmasıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            bitmapRefresh();

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.jpg, *.png, *bmp) | *.jpg; *.png; *.bmp";

                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    ikinciPict = new Bitmap(dlg.FileName);
                }

                if (ikinciPict != null)
                {
                    bitmap = new Bitmap(bitmap, pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
                    ikinciPict = new Bitmap(ikinciPict, pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
                    pictureBox2.Image = null;


                    Bitmap bmp = new Bitmap(bitmap, pictureBox1.Width - 2, pictureBox1.Height - 2);

                    int nOp = (int)Enum.GetValues(typeof(ImageFunction.BlendOperation)).GetValue(1);
                    try
                    {
                        ImageFunction.BlendImages(bmp, 0, 0, bmp.Width, bmp.Height, ikinciPict, 0, 0, (ImageFunction.BlendOperation)nOp);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.ToString());
                    }

                    pictureBox2.Image = new Bitmap(bmp, pictureBox2.ClientSize.Width, pictureBox2.ClientSize.Height);
                }

            }
        }

        private void büyükToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (buyukOlcekTemp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.buyukOlcekleme(buyukOlcekTemp);
            }
        }

        private void ortaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ortaOlcekTemp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.ortaOlcekleme(ortaOlcekTemp);
            }
        }

        private void küçükToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (kucukOlcekTemp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.kucukOlcekleme(kucukOlcekTemp);
            }
        }

        private void kirkbesDereceDondurmetoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (kirkBesTemp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.kirkBesDereceDondurme(kirkBesTemp);
            }
        }

        private void doksanDereceDondurmetoolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (doksanTemp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.doksanDereceDondurme(doksanTemp);
            }
        }

        private void yuzSeksenDereceDondurmetoolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (yuzSeksenTemp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.yuzSeksenDereceDondurme(yuzSeksenTemp);
            }
        }

        private void yuzYetmisDereceDondurmetoolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (ikiYuzYetmisTemp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.ikiYuzYetmisDereceDondurme(ikiYuzYetmisTemp);
            }
        }

        private void sağToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int x = pictureBox2.Location.X;
            int y = pictureBox2.Location.Y;

            pictureBox2.Image = bitmap;

            pictureBox2.Location = new Point(x + 50, y);
        }

        private void solToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int x = pictureBox2.Location.X;
            int y = pictureBox2.Location.Y;

            pictureBox2.Image = bitmap;

            pictureBox2.Location = new Point(x - 50, y);
        }

        private void aşağıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int x = pictureBox2.Location.X;
            int y = pictureBox2.Location.Y;

            pictureBox2.Image = bitmap;

            pictureBox2.Location = new Point(x, y + 50);
        }

        private void yukarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int x = pictureBox2.Location.X;
            int y = pictureBox2.Location.Y;

            pictureBox2.Image = bitmap;

            pictureBox2.Location = new Point(x, y - 50);
        }

        private void kontrastGenişletmeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (kontrantTemp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.Kontrant(kontrantTemp, 255);
            }
        }

        private void logaritmaDönüşümleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (logTemp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.LogritmaDonusum(logTemp, 100);
            }
        }

        private void kuvvetGamaDönüşümleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gamaTemp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.GammaDonusum(gamaTemp, 3.0);
            }
        }

        private void histogramGarfikÇizmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Lütfen Resim Yükleme İşlemi Yapınız !");
            }
            else
            {
                Bitmap r = new Bitmap(pictureBox1.Image);
                int[,] a = histogram(r);
                histogramciz(ref a);
            }

            int[,] histogram(Bitmap resim)
            {
                int[,] hist = new int[3, 256];     // burda 3 ana renkleri 256 ise ton aralığını belirten hist adında 2 boyutlu bir dizi oluşturyoruz

                int a = 0;

                if (resim.PixelFormat == PixelFormat.Format32bppArgb)          // burda ise resimin pixel formatı her 8bit alfa, kırmızı, yeşil ve mavi bileşenleri içeriyorsa a değerini 4 yapıyoruz
                {
                    a = 4;
                }
                else if (resim.PixelFormat == PixelFormat.Format24bppRgb)      // burda da her 8 bit kırmızı, yeşil ve mavi bileşenleri içeriyorsa a'yı 3 yapıyoruz
                {
                    a = 3;
                }

                unsafe
                {

                    BitmapData data = resim.LockBits(new Rectangle(0, 0, resim.Width, resim.Height), ImageLockMode.ReadWrite, resim.PixelFormat);
                    /* bir adet bitmapdata oluşturyoruz bu bizim grafiğimizi göstericek. ilk iki 0'la başlangıç noktasını belirtiyoruz ve resimin genişlik ve uzunluğuna göre bir dikdörtgen oluşturuyoruz ardından ardından resimi okuma ve yazma moduna alıyoruz ve pixel formatını resimin piksel formatına göre ayarlıyoruz */
                    byte* z = (byte*)data.Scan0;         //burda ise dataya aktardığımız pikselleri okuma işlemi yaptırıyoruz

                    for (int i = 0; i < data.Width; i++)    //datanın genişliğine göre döngü oluşturuyoruz
                    {
                        for (int j = 0; j < data.Height; j++)  // boyuna oluşturuyoruz
                        {
                            hist[0, z[0]]++;          // ve histograma kırmızı yeşil ve mavi değerleri olmak üzere yazdırma işlemleri yapıyoruz
                            hist[1, z[1]]++;
                            hist[2, z[2]]++;
                            z += a;

                            if ((i % 5) == 0)//her on satırda bir göstergeyi güncelle
                            {
                                Application.DoEvents();
                            }
                        }
                    }
                    resim.UnlockBits(data);         // datadaki değerleri resime tekrar bırakıyoruz
                }
                return hist;                      // ve hist dizisini geri döndürüyoruz
            }
            void histogramciz(ref int[,] hist)   // burda geri döndürdüğümüz hist dizisini çekiyoruz
            {
                pictureBox2.Refresh();
                Graphics g = pictureBox2.CreateGraphics();      // picturebox'a bir grafik oluşturuyoruz
                Pen pp = new Pen(Color.Red, 1);                            // burdada grafiği çizmek için kalem oluşturuyoruz rengi kırmızı ve kalınlığıda 1 yapıyoruz
                for (int i = 0; i < 256; i++)              //histogramı 0-255 arası renk aralığında çizmek için bir döngü oluşturuyoruz
                {

                    g.DrawLine(pp, new Point(i, pictureBox2.Height), new Point(i, pictureBox2.Height - hist[2, i] / 10));  // burda ise grafiğe çizim işlemi yaptırıyoruz
                }
                g.Dispose();  // ve tabi ramde yer kaplamaması için dispose ediyoruz

            }
        }

        private void histogramGermeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (histGermeTemp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.histogramGerme(histGermeTemp);
            }
        }

        private void histogramEşitlemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (histogramTemp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.histogramEsitleme(histogramTemp);
            }

        }

        private void meanFiltresiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (meanTemp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.MeanFiltresi(meanTemp);
            }
        }

        private void medianFiltresiToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList red = new ArrayList();
                ArrayList green = new ArrayList();
                ArrayList blue = new ArrayList();
                int R;
                int G;
                int B;
                Bitmap input_img = new Bitmap(pictureBox1.Image);
                int width = input_img.Width;
                int height = input_img.Height;
                output_img = new Bitmap(width, height);


                int a, b, c, d, ee, f, g, h, l;

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        try
                        {
                            if ((i + 2) < width && (j + 2) < height && (i - 1) >= 0 && (j - 1) >= 0)
                            {
                                ArrayList reds = new ArrayList();
                                ArrayList greens = new ArrayList();
                                ArrayList blues = new ArrayList();
                                a = Convert.ToInt32(input_img.GetPixel(i - 1, j).R);
                                b = Convert.ToInt32(input_img.GetPixel(i, j + 1).R);
                                c = Convert.ToInt32(input_img.GetPixel(i + 1, j).R);
                                d = Convert.ToInt32(input_img.GetPixel(i, j - 1).R);
                                ee = Convert.ToInt32(input_img.GetPixel(i - 1, j + 1).R);
                                f = Convert.ToInt32(input_img.GetPixel(i + 1, j + 1).R);
                                g = Convert.ToInt32(input_img.GetPixel(i + 1, j - 1).R);
                                h = Convert.ToInt32(input_img.GetPixel(i - 1, j - 1).R);
                                l = Convert.ToInt32(input_img.GetPixel(i, j).R);
                                reds.Add(a); reds.Add(b); reds.Add(c); reds.Add(d); reds.Add(ee); reds.Add(f);
                                reds.Add(g); reds.Add(h); reds.Add(l);
                                reds.Sort();
                                R = Convert.ToInt32(reds[4]);

                                a = Convert.ToInt32(input_img.GetPixel(i - 1, j).G);
                                b = Convert.ToInt32(input_img.GetPixel(i, j + 1).G);
                                c = Convert.ToInt32(input_img.GetPixel(i + 1, j).G);
                                d = Convert.ToInt32(input_img.GetPixel(i, j - 1).G);
                                ee = Convert.ToInt32(input_img.GetPixel(i - 1, j + 1).G);
                                f = Convert.ToInt32(input_img.GetPixel(i + 1, j + 1).G);
                                g = Convert.ToInt32(input_img.GetPixel(i + 1, j - 1).G);
                                h = Convert.ToInt32(input_img.GetPixel(i - 1, j - 1).G);
                                l = Convert.ToInt32(input_img.GetPixel(i, j).G);
                                greens.Add(a); greens.Add(b); greens.Add(c); greens.Add(d); greens.Add(ee); greens.Add(f);
                                greens.Add(g); greens.Add(h); greens.Add(l);
                                greens.Sort();
                                G = Convert.ToInt32(greens[4]);

                                a = Convert.ToInt32(input_img.GetPixel(i - 1, j).B);
                                b = Convert.ToInt32(input_img.GetPixel(i, j + 1).B);
                                c = Convert.ToInt32(input_img.GetPixel(i + 1, j).B);
                                d = Convert.ToInt32(input_img.GetPixel(i, j - 1).B);
                                ee = Convert.ToInt32(input_img.GetPixel(i - 1, j + 1).B);
                                f = Convert.ToInt32(input_img.GetPixel(i + 1, j + 1).B);
                                g = Convert.ToInt32(input_img.GetPixel(i + 1, j - 1).B);
                                h = Convert.ToInt32(input_img.GetPixel(i - 1, j - 1).B);
                                l = Convert.ToInt32(input_img.GetPixel(i, j).B);
                                blues.Add(a); blues.Add(b); blues.Add(c); blues.Add(d); blues.Add(ee); blues.Add(f);
                                blues.Add(g); blues.Add(h); blues.Add(l);
                                blues.Sort();
                                B = Convert.ToInt32(blues[4]);

                                output_img.SetPixel(i, j, Color.FromArgb(R, G, B));
                            }
                            else
                            {
                                output_img.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                            }
                        }
                        catch
                        {
                            MessageBox.Show("hataaa");
                        }
                    }
                }
                pictureBox2.Image = output_img;
            }
            catch
            {
                MessageBox.Show("Lütfen Fotoğraf Seçiniz!");
            }

        }

        private void sobelFiltresiToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (sobelTemp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.SobelFilter(sobelTemp);
            }
        }

        private void prewittFiltresiToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (prewittTemp == null)
            {
                MessageBox.Show("Lütfen Resim Yükleyiniz !!");
            }
            else
            {
                pictureBox2.Image = ImageFunction.prewittKenarBulma(prewittTemp);
            }
        }

        private void eğmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 frm3 = new Form3();
            frm3.Show();
        }
    }
}
