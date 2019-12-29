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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private Bitmap bitmap;
        private int R;
        private int G;
        private int B;
        private void komsulukBul_Click(object sender, EventArgs e)
        {
            try
            {
                int px = Convert.ToInt32(textBox1.Text);
                int py = Convert.ToInt32(textBox2.Text);
                int c = Convert.ToInt32(textBox3.Text);
                Bitmap input_img = new Bitmap(pictureBox1.Image);
                int width = input_img.Width;
                int height = input_img.Height;

                if (c != 1 && c != 2 && c != 3)
                {
                    MessageBox.Show("Lütfen 1, 2 ya da 3 değerlerinden birisini girin!");
                }
                else
                {
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            // 4'lü komşuluklar
                            if (i == px && j == py && c == 1)
                            {
                                R = input_img.GetPixel(i - 1, j).R;
                                label6.Text = Convert.ToString(R);

                                R = input_img.GetPixel(i, j + 1).R;
                                label7.Text = Convert.ToString(R);

                                R = input_img.GetPixel(i + 1, j).R;
                                label8.Text = Convert.ToString(R);

                                R = input_img.GetPixel(i, j - 1).R;
                                label9.Text = Convert.ToString(R);

                                label19.Text = "P(X,Y)";

                            }
                            if (i == px && j == py && c == 2)
                            {
                                G = input_img.GetPixel(i - 1, j).G;
                                label6.Text = Convert.ToString(G);

                                G = input_img.GetPixel(i, j + 1).G;
                                label7.Text = Convert.ToString(G);

                                G = input_img.GetPixel(i + 1, j).G;
                                label8.Text = Convert.ToString(G);

                                G = input_img.GetPixel(i, j - 1).G;
                                label9.Text = Convert.ToString(G);

                                label19.Text = "P(X,Y)";
                            }
                            if (i == px && j == py && c == 3)
                            {
                                B = input_img.GetPixel(i - 1, j).B;
                                label6.Text = Convert.ToString(B);

                                B = input_img.GetPixel(i, j + 1).B;
                                label7.Text = Convert.ToString(B);

                                B = input_img.GetPixel(i + 1, j).B;
                                label8.Text = Convert.ToString(B);

                                B = input_img.GetPixel(i, j - 1).B;
                                label9.Text = Convert.ToString(B);

                                label19.Text = "P(X,Y)";
                            }
                            // 8'li Komşuluklar
                            if (i == px && j == py && c == 1)
                            {
                                R = input_img.GetPixel(i - 1, j).R;
                                label11.Text = Convert.ToString(R);

                                R = input_img.GetPixel(i, j + 1).R;
                                label12.Text = Convert.ToString(R);

                                R = input_img.GetPixel(i + 1, j).R;
                                label13.Text = Convert.ToString(R);

                                R = input_img.GetPixel(i, j - 1).R;
                                label14.Text = Convert.ToString(R);

                                R = input_img.GetPixel(i - 1, j + 1).R;
                                label15.Text = Convert.ToString(R);

                                R = input_img.GetPixel(i + 1, j + 1).R;
                                label16.Text = Convert.ToString(R);

                                R = input_img.GetPixel(i + 1, j - 1).R;
                                label17.Text = Convert.ToString(R);

                                R = input_img.GetPixel(i - 1, j - 1).R;
                                label18.Text = Convert.ToString(R);

                                label20.Text = "P(X,Y)";
                            }
                            if (i == px && j == py && c == 2)
                            {
                                G = input_img.GetPixel(i - 1, j).G;
                                label11.Text = Convert.ToString(G);

                                G = input_img.GetPixel(i, j + 1).G;
                                label12.Text = Convert.ToString(G);

                                G = input_img.GetPixel(i + 1, j).G;
                                label13.Text = Convert.ToString(G);

                                G = input_img.GetPixel(i, j - 1).G;
                                label14.Text = Convert.ToString(G);

                                G = input_img.GetPixel(i - 1, j + 1).G;
                                label15.Text = Convert.ToString(G);

                                G = input_img.GetPixel(i + 1, j + 1).G;
                                label16.Text = Convert.ToString(G);

                                G = input_img.GetPixel(i + 1, j - 1).G;
                                label17.Text = Convert.ToString(G);

                                G = input_img.GetPixel(i - 1, j - 1).G;
                                label18.Text = Convert.ToString(G);

                                label20.Text = "P(X,Y)";
                            }
                            if (i == px && j == py && c == 3)
                            {
                                B = input_img.GetPixel(i - 1, j).B;
                                label11.Text = Convert.ToString(B);

                                B = input_img.GetPixel(i, j + 1).B;
                                label12.Text = Convert.ToString(B);

                                B = input_img.GetPixel(i + 1, j).B;
                                label13.Text = Convert.ToString(B);

                                B = input_img.GetPixel(i, j - 1).B;
                                label14.Text = Convert.ToString(B);

                                B = input_img.GetPixel(i - 1, j + 1).B;
                                label15.Text = Convert.ToString(B);

                                B = input_img.GetPixel(i + 1, j + 1).B;
                                label16.Text = Convert.ToString(B);

                                B = input_img.GetPixel(i + 1, j - 1).B;
                                label17.Text = Convert.ToString(B);

                                B = input_img.GetPixel(i - 1, j - 1).B;
                                label18.Text = Convert.ToString(B);

                                label20.Text = "P(X,Y)";
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Lütfen Fotoğraf Seçiniz veya Girdiğiniz Değerleri Kontrol Ediniz!");
            }
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
                    pictureBox1.Image = bitmap;
                }
            }
        }

    }
}
