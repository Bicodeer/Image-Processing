using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public static class ImageFunction
    {
        private delegate byte PerChannelProcessDelegate(ref byte nSrc, ref byte nDst);
        private delegate void RGBProcessDelegate(byte sR, byte sG, byte sB, ref byte dR, ref byte dG, ref byte dB);
        public enum BlendOperation : int
        {
            Blend_Overlay,
            Blend_Difference
        }
        public static Bitmap ResminNegatifi(Bitmap bmp)
        {
            //piksellerini al ve sonra red blue ve green değerlerini resimdeki piksellerden çıkart
            unsafe
            {
                //Once bitmapten fotografı alıyoruz.
                BitmapData veri = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);

                int pikselByte = System.Drawing.Bitmap.GetPixelFormatSize(bmp.PixelFormat) / 8; //piksellere ulaşıyoruz.
                int pikselYukseklik = veri.Height;
                int pikselGenislik = veri.Width * pikselByte;
                byte* PtrFirstPixel = (byte*)veri.Scan0;

                Parallel.For(0, pikselYukseklik, y =>
                {
                    byte* mevcutVeri = PtrFirstPixel + (y * veri.Stride);
                    for (int x = 0; x < pikselGenislik; x = x + pikselByte)
                    {
                        //blue 1 green 2 red 3 ver  
                        int ilkBlue = mevcutVeri[x];
                        int ilkGreen = mevcutVeri[x + 1];
                        int ilkRed = mevcutVeri[x + 2];

                        ilkBlue = 255 - ilkBlue; //ilk resimdeki blue değerini 255 den cıkartırrız 
                        ilkGreen = 255 - ilkGreen;
                        ilkRed = 255 - ilkRed;

                        mevcutVeri[x] = (byte)ilkBlue;
                        mevcutVeri[x + 1] = (byte)ilkGreen;
                        mevcutVeri[x + 2] = (byte)ilkRed;
                    }
                });
                bmp.UnlockBits(veri);
            }
            return bmp;
        }

        public static Bitmap SiyahBeyazaDonusur(Bitmap orginalResim)
        {
            Bitmap sonucResmi = new Bitmap(orginalResim.Width, orginalResim.Height);

            for (int i = 0; i < orginalResim.Width; i++)
            {

                for (int j = 0; j < orginalResim.Height; j++)
                {

                    Color c = orginalResim.GetPixel(i, j);

                    int average = ((c.R + c.B + c.G) / 3);

                    if (average < 200)
                        sonucResmi.SetPixel(i, j, Color.Black);

                    else
                        sonucResmi.SetPixel(i, j, Color.White);

                }
            }
            return sonucResmi;
        }

        public static Bitmap zoomOut(Bitmap bitmap)
        {
            Size newSize = new Size((int)(bitmap.Width / 2), (int)(bitmap.Height / 2));
            Bitmap bmp = new Bitmap(bitmap, newSize);
            return bmp;
        }

        public static Bitmap zoomIn(Image image, int width, int height)
        {
            var rect = new Rectangle(0, 0, width * 2, height * 2);
            var goruntu = new Bitmap(width * 2, height * 2);

            goruntu.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(goruntu))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return goruntu;
        }

        public static Bitmap histogramEsitleme(Bitmap KaynakResim)
        {
            Bitmap islenenGoruntu = KaynakResim;

            uint pixels = (uint)islenenGoruntu.Height * (uint)islenenGoruntu.Width;
            decimal Const = 255 / (decimal)pixels; // piksel değerini decimal

            int x, y, R, G, B;


            int[] HistogramRed2 = new int[256];
            int[] HistogramGreen2 = new int[256];
            int[] HistogramBlue2 = new int[256];


            for (var i = 0; i < islenenGoruntu.Width; i++)
            {
                for (var j = 0; j < islenenGoruntu.Height; j++)
                {
                    var piksel = islenenGoruntu.GetPixel(i, j);

                    HistogramRed2[(int)piksel.R]++;
                    HistogramGreen2[(int)piksel.G]++;
                    HistogramBlue2[(int)piksel.B]++;

                }
            }

            int[] cdfR = HistogramRed2;
            int[] cdfG = HistogramGreen2;
            int[] cdfB = HistogramBlue2;

            for (int r = 1; r <= 255; r++)
            {
                cdfR[r] = cdfR[r] + cdfR[r - 1];
                cdfG[r] = cdfG[r] + cdfG[r - 1];
                cdfB[r] = cdfB[r] + cdfB[r - 1];
            }

            for (y = 0; y < islenenGoruntu.Height; y++)
            {
                for (x = 0; x < islenenGoruntu.Width; x++)
                {
                    Color pixelColor = islenenGoruntu.GetPixel(x, y);

                    R = (int)((decimal)cdfR[pixelColor.R] * Const);
                    G = (int)((decimal)cdfG[pixelColor.G] * Const);
                    B = (int)((decimal)cdfB[pixelColor.B] * Const);

                    Color newColor = Color.FromArgb(R, G, B);
                    islenenGoruntu.SetPixel(x, y, newColor);
                }
            }
            return islenenGoruntu;
        }
        public static Bitmap LogritmaDonusum(Bitmap img, int sabit)
        {
            int w = img.Width;
            int h = img.Height;

            img = griton(img);

            BitmapData sd = img.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = sd.Stride * sd.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(sd.Scan0, buffer, 0, bytes);
            img.UnlockBits(sd);
            int current = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    current = y * sd.Stride + x * 4;
                    for (int i = 0; i < 3; i++)
                    {
                        result[current + i] = (byte)(sabit * Math.Log10(buffer[current + i] + 1));
                    }
                    result[current + 3] = 255;
                }
            }
            Bitmap resimg = new Bitmap(w, h);
            BitmapData rd = resimg.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(result, 0, rd.Scan0, bytes);
            resimg.UnlockBits(rd);
            return resimg;
        }
        public static Bitmap griton(Bitmap bmp)
        {
            // Bitmap resim = new Bitmap(pictureBox1.Image);    //Burda resmimizi bir bitmap ortamına alıyoruz
            for (int i = 0; i < bmp.Height - 1; i++)                            // ardından enlem ve boylam şeklinde tarama yapıcağımız için resmin boyuna göre bir döngü oluşturuyoruz
            {
                for (int j = 0; j < bmp.Width - 1; j++)                           // birde enine döngü oluşturuyoruz
                {
                    int deger = (bmp.GetPixel(j, i).R + bmp.GetPixel(j, i).G + bmp.GetPixel(j, i).B) / 3;  // ardından yukarıda da bahsettiğim pikseldeki RGB değerinin aritmatik ortalamasını alıyoruz ve deger değişkenine atıyoruz
                    Color renk;
                    renk = Color.FromArgb(deger, deger, deger);  //burda ise bir üst satırda oluşturduğumuz renk değişkeninin RGB değerlerine aritmetik ortalamasını aldırdığımız yeni rengi veriyoruz
                    bmp.SetPixel(j, i, renk);     //ve pikselimizi bitmapimizin j boylamında i enlemindeki noktaya yerleştiriyoruz
                }
            }
            return bmp;
        }
        public static Bitmap GammaDonusum(Bitmap img, double gamma, double c = 1d)
        {
            int width = img.Width;
            int height = img.Height;
            //img = griton(img);
            BitmapData srcData = img.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;
            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);
            img.UnlockBits(srcData);
            int current = 0;
            int cChannels = 3;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    current = y * srcData.Stride + x * 4;
                    for (int i = 0; i < cChannels; i++)
                    {
                        double range = (double)buffer[current + i] / 255;
                        double correction = c * Math.Pow(range, gamma);
                        result[current + i] = (byte)(correction * 255);
                    }
                    result[current + 3] = 255;
                }
            }
            Bitmap resImg = new Bitmap(width, height);
            BitmapData resData = resImg.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(result, 0, resData.Scan0, bytes);
            resImg.UnlockBits(resData);
            return resImg;
        }
        public static Bitmap MeanFiltresi(Bitmap AnaResim)
        {
            Bitmap resim = new Bitmap(AnaResim);
            byte[,] matris = new byte[resim.Height, resim.Width];
            byte[,] matris1 = new byte[resim.Height, resim.Width];
            byte[,] matris2 = new byte[resim.Height, resim.Width];
            byte[,] k = new byte[resim.Height, resim.Width];
            byte[,] m = new byte[resim.Height, resim.Width];
            byte[,] y = new byte[resim.Height, resim.Width];
            unsafe
            {
                BitmapData bmpdata = resim.LockBits(new Rectangle(0, 0, resim.Width, resim.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                byte* p = (byte*)bmpdata.Scan0;

                for (int a = 0; a < bmpdata.Height; a++)
                {
                    for (int b = 0; b < bmpdata.Width; b++)
                    {
                        matris[a, b] = p[0];
                        matris1[a, b] = p[1];
                        matris2[a, b] = p[2];
                        p += 4;

                    }
                }

                int top = 0, top1 = 0, top2 = 0;
                for (int a = 1; a < bmpdata.Height - 1; a++)
                {
                    for (int b = 1; b < bmpdata.Width - 1; b++)
                    {

                        top = matris[a, b] + matris[a - 1, b] + matris[a + 1, b] +
                            matris[a - 1, b - 1] + matris[a, b - 1] + matris[a + 1, b - 1] +
                            matris[a - 1, b + 1] + matris[a, b + 1] + matris[a + 1, b + 1];

                        top = top / 9;

                        top1 = matris1[a, b] + matris1[a - 1, b] + matris1[a + 1, b] +
                            matris1[a - 1, b - 1] + matris1[a, b - 1] + matris1[a + 1, b - 1] +
                            matris1[a - 1, b + 1] + matris1[a, b + 1] + matris1[a + 1, b + 1];

                        top1 = top1 / 9;

                        top2 = matris2[a, b] + matris2[a - 1, b] + matris2[a + 1, b] +
                            matris2[a - 1, b - 1] + matris2[a, b - 1] + matris2[a + 1, b - 1] +
                               matris2[a - 1, b + 1] + matris2[a, b + 1] + matris2[a + 1, b + 1];

                        top2 = top2 / 9;

                        m[a, b] = (byte)top;
                        y[a, b] = (byte)top1;
                        k[a, b] = (byte)top2;
                    }
                }

                Bitmap resim1 = new Bitmap(AnaResim);
                unsafe
                {
                    BitmapData bmpdata1 = resim1.LockBits(new Rectangle(0, 0, resim1.Width, resim1.Height),
                    ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                    byte* z = (byte*)bmpdata1.Scan0;

                    for (int a = 0; a < bmpdata1.Height; a++)
                    {
                        for (int b = 0; b < bmpdata1.Width; b++)
                        {

                            z[0] = m[a, b];

                            z[1] = y[a, b];

                            z[2] = k[a, b];

                            z += 4;

                        }
                    }
                    resim1.UnlockBits(bmpdata1);
                }

                return resim1;

            }
        }
       /* public static Bitmap MedianFilter(Bitmap resim, int matrixSize, int bias = 0, bool griton = false)
        {
            
        }*/

        public static Bitmap Kontrant(Bitmap sourceBitmap, int threshold)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);


            double contrastLevel = Math.Pow((100.0 + threshold) / 100.0, 2);


            double blue = 0;
            double green = 0;
            double red = 0;


            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                blue = ((((pixelBuffer[k] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;


                green = ((((pixelBuffer[k + 1] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;


                red = ((((pixelBuffer[k + 2] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;


                if (blue > 255)
                { blue = 255; }
                else if (blue < 0)
                { blue = 0; }


                if (green > 255)
                { green = 255; }
                else if (green < 0)
                { green = 0; }


                if (red > 255)
                { red = 255; }
                else if (red < 0)
                { red = 0; }


                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                        resultBitmap.Width, resultBitmap.Height),
                                        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }

        public static Bitmap SobelFilter(Bitmap originalR)
        {
            int width = originalR.Width;
            int height = originalR.Height;

            int pikselBasinaBitSayisi = Image.GetPixelFormatSize(originalR.PixelFormat);
            int tekRenkBits = pikselBasinaBitSayisi / 8;

            BitmapData bmpData = originalR.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, originalR.PixelFormat);
            int pozisyon;
            int[,] gx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] gy = new int[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };
            byte Threshold = 128;

            Bitmap dstBmp = new Bitmap(width, height, originalR.PixelFormat);
            BitmapData dstData = dstBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, dstBmp.PixelFormat);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                byte* dst = (byte*)dstData.Scan0.ToPointer();

                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int yeniX = 0, yeniY = 0;

                        for (int ii = 0; ii < 3; ii++)
                        {
                            for (int jj = 0; jj < 3; jj++)
                            {
                                int I = i + ii - 1;
                                int J = j + jj - 1;
                                byte Current = *(ptr + (I * width + J) * tekRenkBits);
                                yeniX += gx[ii, jj] * Current;
                                yeniY += gy[ii, jj] * Current;
                            }
                        }
                        pozisyon = ((i * width + j) * tekRenkBits);
                        if (yeniX * yeniX + yeniY * yeniY > Threshold * Threshold)
                            dst[pozisyon] = dst[pozisyon + 1] = dst[pozisyon + 2] = 255;
                        else
                            dst[pozisyon] = dst[pozisyon + 1] = dst[pozisyon + 2] = 0;
                    }
                }
            }
            originalR.UnlockBits(bmpData);
            dstBmp.UnlockBits(dstData);

            return dstBmp;
        }


        public static Bitmap histogramGerme(Bitmap bm)
        {
            int R = 0, G = 0, B = 0;
            Color OkunanRenk, DonusenRenk;
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(bm);
            int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor. Boyutları giriş resmi

            double KontrastSeviyesi = 128;
            double KontrastFaktoru = (259 * (KontrastSeviyesi + 255)) / (255 * (259 - KontrastSeviyesi));
            int i = 0, j = 0; //Çıkış resminin x ve y si olacak.
            for (int x = 0; x < ResimGenisligi; x++)
            {
                j = 0;
                for (int y = 0; y < ResimYuksekligi; y++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x, y);
                    R = OkunanRenk.R;
                    G = OkunanRenk.G;
                    B = OkunanRenk.B;
                    R = (int)((KontrastFaktoru * (R - 128)) + 128);
                    G = (int)((KontrastFaktoru * (G - 128)) + 128);
                    B = (int)((KontrastFaktoru * (B - 128)) + 128);
                    //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;
                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;
                    DonusenRenk = Color.FromArgb(R, G, B);
                    CikisResmi.SetPixel(i, j, DonusenRenk);
                    j++;
                }
                i++;

            }
            return CikisResmi;

        }

        public static Bitmap prewittKenarBulma(Bitmap bitmap) //Bitmap sınıfı bizim resim üzerinde işlemler yapmamızı sağlıyor
        {
            Bitmap gri = griton(bitmap);
            Bitmap buffer = new Bitmap(gri.Width, gri.Height);
            Color renk;
            int valX, valY, gradient;
            int[,] GX = new int[3, 3];
            int[,] GY = new int[3, 3];
            //yatay yöndeki kenar değerleri
            GX[0, 0] = -1; GX[0, 1] = -1; GX[0, 2] = -1;
            GX[1, 0] = 0; GX[1, 1] = 0; GX[1, 2] = 0;
            GX[2, 0] = 1; GX[2, 1] = 1; GX[2, 2] = 1;
            //Dikey yöndeki kenar değerleri
            GY[0, 0] = -1; GY[0, 1] = 0; GY[0, 2] = 1;
            GY[1, 0] = -1; GY[1, 1] = 0; GY[1, 2] = 1;
            GY[2, 0] = -1; GY[2, 1] = 0; GY[2, 2] = 1;
            for (int i = 0; i < gri.Height; i++)
            {
                for (int j = 0; j < gri.Width; j++)
                {
                    if (i == 0 || i == gri.Height - 1 || j == 0 || j == gri.Width - 1)
                    {
                        renk = Color.FromArgb(255, 255, 255);
                        buffer.SetPixel(j, i, renk);
                        valX = 0;
                        valY = 0;
                    }
                    else
                    {
                        valX = gri.GetPixel(j - 1, i - 1).R * GX[0, 0] +
                            gri.GetPixel(j, i - 1).R * GX[0, 1] +
                            gri.GetPixel(j + 1, i - 1).R * GX[0, 2] +
                            gri.GetPixel(j - 1, i).R * GX[1, 0] +
                            gri.GetPixel(j, i).R * GX[1, 1] +
                            gri.GetPixel(j + 1, i).R * GX[1, 2] +
                            gri.GetPixel(j - 1, i + 1).R * GX[2, 0] +
                            gri.GetPixel(j, i + 1).R * GX[2, 1] +
                            gri.GetPixel(j + 1, i + 1).R * GX[2, 2];

                        valY = gri.GetPixel(j - 1, i - 1).R * GY[0, 0] +
                         gri.GetPixel(j, i - 1).R * GY[0, 1] +
                         gri.GetPixel(j + 1, i - 1).R * GY[0, 2] +
                         gri.GetPixel(j - 1, i).R * GY[1, 0] +
                         gri.GetPixel(j, i).R * GY[1, 1] +
                         gri.GetPixel(j + 1, i).R * GY[1, 2] +
                         gri.GetPixel(j - 1, i + 1).R * GY[2, 0] +
                         gri.GetPixel(j, i + 1).R * GY[2, 1] +
                         gri.GetPixel(j + 1, i + 1).R * GY[2, 2];
                        gradient = (int)(Math.Abs(valX) + Math.Abs(valY));
                        if (gradient < 0)
                        {
                            gradient = 0;
                        }
                        if (gradient > 255)
                        {
                            gradient = 255;
                        }
                        renk = Color.FromArgb(gradient, gradient, gradient);
                        buffer.SetPixel(j, i, renk);
                    }
                }

            }
            return buffer;
        }

        public static Bitmap dondur(Bitmap bmap, RotateFlipType rotateFlipType)
        {

            bmap.RotateFlip(rotateFlipType);
            return bmap;
        }
        //görüntüyü yeniden boyutlandırmak için kullanılır.
        public static Bitmap goruntuBoyutlandırma(int yeniWidth, int yeniHeight, Bitmap stPhoto)
        {

            Bitmap bitmap;

            Image imgPhoto = (Image)stPhoto;

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            if (sourceWidth < sourceHeight)
            {
                int buff = yeniWidth;

                yeniWidth = yeniHeight;
                yeniHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)yeniWidth / (float)sourceWidth);
            nPercentH = ((float)yeniHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((yeniWidth -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((yeniHeight -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(yeniWidth, yeniHeight,
                          PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                         imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            imgPhoto.Dispose();
            bitmap = new Bitmap(bmPhoto);
            return bitmap;
        }

        public static Bitmap resmiDondur(Bitmap image, float aciDegeri, bool buyukDeger,
                                 bool kucukDeger, Color backgroundColor)
        {
            if (aciDegeri == 0f)
                return (Bitmap)image.Clone();

            int ilkWidth = image.Width;
            int ilkHeight = image.Height;
            int yeniWidth = ilkWidth;
            int yeniHeight = ilkHeight;
            float olcek = 1f;

            if (buyukDeger || !kucukDeger)
            {
                double aciRadyan = aciDegeri * Math.PI / 180d;

                double cos = Math.Abs(Math.Cos(aciRadyan));
                double sin = Math.Abs(Math.Sin(aciRadyan));
                yeniWidth = (int)Math.Round(ilkWidth * cos + ilkHeight * sin);
                yeniHeight = (int)Math.Round(ilkWidth * sin + ilkHeight * cos);
            }

            if (!buyukDeger && !kucukDeger)
            {
                olcek = Math.Min((float)ilkWidth / yeniWidth, (float)ilkHeight / yeniHeight);
                yeniWidth = ilkWidth;
                yeniHeight = ilkHeight;
            }

            Bitmap temp = new Bitmap(yeniWidth, yeniHeight, backgroundColor == Color.Transparent ?
                                             PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);
            temp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics grafikObj = Graphics.FromImage(temp))
            {
                grafikObj.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grafikObj.PixelOffsetMode = PixelOffsetMode.HighQuality;
                grafikObj.SmoothingMode = SmoothingMode.HighQuality;

                if (backgroundColor != Color.Transparent)
                    grafikObj.Clear(backgroundColor);

                grafikObj.TranslateTransform(yeniWidth / 2f, yeniHeight / 2f);

                if (olcek != 1f)
                    grafikObj.ScaleTransform(olcek, olcek);

                grafikObj.RotateTransform(aciDegeri);
                grafikObj.TranslateTransform(-ilkWidth / 2f, -ilkHeight / 2f);

                grafikObj.DrawImage(image, 0, 0);
            }

            return temp;
        }

        #region BlendImages functions ...
        /* 
			destImage - image that will be used as background
			destX, destY - define position on destination image where to start applying blend operation
			destWidth, destHeight - width and height of the area to apply blending
			srcImage - image to use as foreground (source of blending)	
			srcX, srcY - starting position of the source image 	  
		*/
        public static void BlendImages(Image destImage, int destX, int destY, int destWidth, int destHeight,
                                Image srcImage, int srcX, int srcY, BlendOperation BlendOp)
        {
            if (destImage == null)
                throw new Exception("Destination image must be provided");

            if (destImage.Width < destX + destWidth || destImage.Height < destY + destHeight)
                throw new Exception("Destination image is smaller than requested dimentions");

            if (srcImage == null)
                throw new Exception("Source image must be provided");

            if (srcImage.Width < srcX + destWidth || srcImage.Height < srcY + destHeight)
                throw new Exception("Source image is smaller than requested dimentions");

            Bitmap tempBmp = null;
            Graphics gr = Graphics.FromImage(destImage);
            gr.CompositingMode = CompositingMode.SourceCopy;

            switch (BlendOp)
            {
                case BlendOperation.Blend_Overlay:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                ref srcImage, srcX, srcY, new PerChannelProcessDelegate(BlendOverlay));
                    break;

                case BlendOperation.Blend_Difference:
                    tempBmp = PerChannelProcess(ref destImage, destX, destY, destWidth, destHeight,
                ref srcImage, srcX, srcY, new PerChannelProcessDelegate(BlendDifference));
                    break;
            }



            if (tempBmp != null)
            {
                gr.DrawImage(tempBmp, 0, 0, tempBmp.Width, tempBmp.Height);
                tempBmp.Dispose();
                tempBmp = null;
            }

            gr.Dispose();
            gr = null;
        }

        private static byte BlendDifference(ref byte Src, ref byte Dst)
        {
            return (byte)((Src > Dst) ? Src - Dst : Dst - Src);
        }
        public static void BlendImages(Image destImage, Image srcImage, BlendOperation BlendOp)
        {
            BlendImages(destImage, 0, 0, destImage.Width, destImage.Height, srcImage, 0, 0, BlendOp);
        }

        public static void BlendImages(Image destImage, BlendOperation BlendOp)
        {
            BlendImages(destImage, 0, 0, destImage.Width, destImage.Height, null, 0, 0, BlendOp);
        }

        public static void BlendImages(Image destImage, int destX, int destY, BlendOperation BlendOp)
        {
            BlendImages(destImage, destX, destY, destImage.Width - destX, destImage.Height - destY, null, 0, 0, BlendOp);
        }

        public static void BlendImages(Image destImage, int destX, int destY, int destWidth, int destHeight, BlendOperation BlendOp)
        {
            BlendImages(destImage, destX, destY, destWidth, destHeight, null, 0, 0, BlendOp);
        }
        #endregion

        #region Private Blending Functions ...

        private static Bitmap PerChannelProcess(ref Image destImg, int destX, int destY, int destWidth, int destHeight,
                                ref Image srcImg, int srcX, int srcY,
                                PerChannelProcessDelegate ChannelProcessFunction)
        {
            Bitmap dst = new Bitmap(destImg);
            Bitmap src = new Bitmap(srcImg);

            BitmapData dstBD = dst.LockBits(new Rectangle(destX, destY, destWidth, destHeight), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData srcBD = src.LockBits(new Rectangle(srcX, srcY, destWidth, destHeight), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int dstStride = dstBD.Stride;
            int srcStride = srcBD.Stride;

            System.IntPtr dstScan0 = dstBD.Scan0;
            System.IntPtr srcScan0 = srcBD.Scan0;

            unsafe
            {
                byte* pDst = (byte*)(void*)dstScan0;
                byte* pSrc = (byte*)(void*)srcScan0;

                for (int y = 0; y < destHeight; y++)
                {
                    for (int x = 0; x < destWidth * 3; x++)
                    {
                        pDst[x + y * dstStride] = ChannelProcessFunction(ref pSrc[x + y * srcStride], ref pDst[x + y * dstStride]);
                    }
                }
            }

            src.UnlockBits(srcBD);
            dst.UnlockBits(dstBD);

            src.Dispose();

            return dst;
        }


        // overlay 
        private static byte BlendOverlay(ref byte Src, ref byte Dst)
        {
            return ((Dst < 128) ? (byte)Math.Max(Math.Min((Src / 255.0f * Dst / 255.0f) * 255.0f * 2, 255), 0) : (byte)Math.Max(Math.Min(255 - ((255 - Src) / 255.0f * (255 - Dst) / 255.0f) * 255.0f * 2, 255), 0));
        }

        #endregion
        public static Bitmap buyukOlcekleme(Bitmap bitmap)
        {
            Bitmap BMP = ImageFunction.goruntuBoyutlandırma(bitmap.Width * 2, bitmap.Height * 2, bitmap);

            return BMP;
        }
        public static Bitmap ortaOlcekleme(Bitmap temp)
        {

            Bitmap BMP = ImageFunction.goruntuBoyutlandırma(temp.Width * 1, temp.Height * 1, temp);
            return BMP;
        }
        public static Bitmap kucukOlcekleme(Bitmap gecici)
        {
            Bitmap BMP = ImageFunction.goruntuBoyutlandırma(gecici.Width, gecici.Height / 2, gecici);

            return BMP;
        }

        public static Bitmap kirkBesDereceDondurme(Bitmap img)
        {
            Bitmap bmp = ImageFunction.resmiDondur(img, 45, true, true, Color.White);
            return bmp;
        }
        public static Bitmap doksanDereceDondurme(Bitmap bitmap)
        {
            Bitmap bmp = ImageFunction.resmiDondur(bitmap, 90, true, true, Color.White);
            return bmp;
        }
        public static Bitmap yuzSeksenDereceDondurme(Bitmap bitmap)
        {
            Bitmap bmp = ImageFunction.resmiDondur(bitmap, 180, true, true, Color.White);
            return bmp;
        }
        public static Bitmap ikiYuzYetmisDereceDondurme(Bitmap bitmap)
        {
            Bitmap bmp = ImageFunction.resmiDondur(bitmap, 270, true, true, Color.White);
            return bmp;
        }




    }
}
