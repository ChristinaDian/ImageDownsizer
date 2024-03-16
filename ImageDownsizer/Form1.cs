using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;

namespace ImageDownsizer
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnOpenFileDialog_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {

                originalImage = new Bitmap(fileDialog.FileName);
                pictureBox1.Image = originalImage;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int downscaleFactor = (int)numericUpDown1.Value;

            pictureBox2.Image = DownscaleMethod(downscaleFactor);

            stopwatch.Stop();
            lblTime.Text = $"Time without threads: {stopwatch.ElapsedMilliseconds} ms";
        }

        private byte[] CopyBitmapToArray(Bitmap bitmap)
        {
            // Lock the bitmap's bits. 
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * bmpData.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values back to the bitmap
            Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);

            // Unlock the bits.
            bitmap.UnlockBits(bmpData);

            return rgbValues;
        }
        private void SetBitmapFromArray(Bitmap bitmap, byte[] rgbValues)
        {
            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, bmpData.Scan0, rgbValues.Length);

            // Unlock the bits.
            bitmap.UnlockBits(bmpData);
        }

        private void FindAverageColors(byte[] originalRgbValues, int originalWidth, int startX, int startY, int blockWidth, int blockHeight, out byte averagedR, out byte averagedG, out byte averagedB)
        {
            int sumR = 0, sumG = 0, sumB = 0;
            int totalPixels = blockHeight*blockWidth;

            for (int y = startY; y < startY + blockHeight; y++)
            {
                for (int x = startX; x < startX + blockWidth; x++)
                {
                    int index = (y * originalWidth + x) * 3; // Assuming 24 bits per pixel (3 bytes per pixel)
                    sumB += originalRgbValues[index];
                    sumG += originalRgbValues[index + 1];
                    sumR += originalRgbValues[index + 2];
                }
            }

            averagedR = (byte)(sumR / totalPixels);
            averagedG = (byte)(sumG / totalPixels);
            averagedB = (byte)(sumB / totalPixels);
        }
        private void DownscaleLines(byte[] originalRgbValues, byte[] downscaledRgbValues, int newWidth, int newHeight, int factorRatio)
        {
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    int originalX = x * factorRatio;
                    int originalY = y * factorRatio;

                    byte averagedR, averagedG, averagedB;
                    FindAverageColors(originalRgbValues, originalImage.Width, originalX, originalY, originalImage.Width / newWidth, originalImage.Height / newHeight, out averagedR, out averagedG, out averagedB);

                    int downscaledIndex = (y * newWidth + x) * 3;
                    downscaledRgbValues[downscaledIndex] = averagedB;
                    downscaledRgbValues[downscaledIndex + 1] = averagedG;
                    downscaledRgbValues[downscaledIndex + 2] = averagedR;
                }
            }
        }
        private Bitmap DownscaleMethod(int downscaleFactor)
        {
            int newWidth = (int)(originalImage.Width * downscaleFactor / 100);
            int newHeight = (int)(originalImage.Height * downscaleFactor / 100);
            int factorRatio = (int)(originalImage.Width / newWidth);

            Bitmap downscaledImage = new Bitmap(newWidth, newHeight);

            // Declare an array to hold the bytes of the bitmap.
            byte[] originalRgbValues = CopyBitmapToArray(originalImage);
            byte[] downscaledRgbValues = new byte[newWidth * newHeight * 3]; // 24 bits per pixel (3 bytes per pixel)

            DownscaleLines(originalRgbValues, downscaledRgbValues, newWidth, newHeight, factorRatio);
            
            SetBitmapFromArray(downscaledImage, downscaledRgbValues);

            return downscaledImage;
        }
       
    }
}