using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        private Bitmap bitmap;
        private Bitmap output_img;
        private int R;
        private int G;
        private int B;
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.jpg, *.png, *bmp) | *.jpg; *.png; *.bmp";

                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    bitmap = new Bitmap(dlg.FileName);
                    pictureBox1.Image = bitmap;
                }
            }
        }

        private void btnEgme_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap input_img = new Bitmap(pictureBox1.Image);               
                int width = input_img.Width;
                int height = input_img.Height;
                int sh = Convert.ToInt32(textBox1.Text);
                double aci = sh * 2 * Math.PI / 360;
                output_img = new Bitmap(width, height + sh);
                int x2, y2;

                for (int x1 = 0; x1 < width; x1++)
                {
                    for (int y1 = 0; y1 < height; y1++)
                    {
                        x2 = x1;
                        y2 = Convert.ToInt32(aci * x1 + y1);
                        if (x2 >= 0 && x2 < width && y2 >= 0 && y2 < height + sh)
                        {
                            R = input_img.GetPixel(x1, y1).R;
                            G = input_img.GetPixel(x1, y1).G;
                            B = input_img.GetPixel(x1, y1).B;
                            output_img.SetPixel(x2, y2, Color.FromArgb(R, G, B));
                        }
                    }
                }
                pictureBox2.Image = ImageFunction.zoomOut(output_img);
            }
            catch
            {
                MessageBox.Show("Lütfen Fotoğraf Seçiniz veya Girdiğiniz Değerleri Kontrol Ediniz!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1();
            frm1.Show();
        }
    }
}
