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
        private object lockObj = new object();

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
                //Load the image in another thread to avoid blocking the GUI
                Thread thread = new Thread(() =>
                {
                    originalImage = new Bitmap(fileDialog.FileName);
                    pictureBox1.Image = originalImage;
                });
                thread.Start();
            }

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            int downscaleFactor = (int)numericUpDown1.Value;
            int newWidth = (int)(originalImage.Width * downscaleFactor / 100);
            int newHeight = (int)(originalImage.Height * downscaleFactor / 100);
            int factorRatio = (int)(originalImage.Width / newWidth);
            int totalPixels = factorRatio * factorRatio;

            if (downscaleFactor > 0)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                pictureBox2.Image = DownscaleMethod(newWidth, newHeight, factorRatio, totalPixels);

                stopwatch.Stop();

                lblTime.Text = $"Time without threads: {stopwatch.ElapsedMilliseconds} ms";

                Stopwatch stopwatchWithThreads = new Stopwatch();
                stopwatchWithThreads.Start();

                pictureBox2.Image = DownscaleMethodWithThreads(newWidth, newHeight, factorRatio, totalPixels);

                stopwatchWithThreads.Stop();
                lblTimeWithThreads.Text = $"Time with threads: {stopwatchWithThreads.ElapsedMilliseconds} ms";
            }
            else
            {
                MessageBox.Show("Please enter a downscale factor greater than 0.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        private void DownscaleLines(byte[] originalRgbValues, byte[] downscaledRgbValues, int newWidth, int newHeight, int factorRatio, int yStart, int yEnd, int totalPixels)
        {
            for (int y = yStart; y < yEnd; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    int originalX = x * factorRatio;
                    int originalY = y * factorRatio;

                    int sumR = 0, sumG = 0, sumB = 0;

                    for (int dy = 0; dy < factorRatio; dy++)
                    {
                        for (int dx = 0; dx < factorRatio; dx++)
                        {
                            int pixelX = originalX + dx;
                            int pixelY = originalY + dy;

                            int index = (pixelY * newWidth * factorRatio + pixelX) * 3; // Assuming 24 bits per pixel (3 bytes per pixel)                                                     
                            sumB += originalRgbValues[index];
                            sumG += originalRgbValues[index + 1];
                            sumR += originalRgbValues[index + 2];
                        }
                    }
                    byte averagedR = (byte)(sumR / totalPixels);
                    byte averagedG = (byte)(sumG / totalPixels);
                    byte averagedB = (byte)(sumB / totalPixels);

                    int downscaledIndex = (y * newWidth + x) * 3;
                    downscaledRgbValues[downscaledIndex] = averagedB;
                    downscaledRgbValues[downscaledIndex + 1] = averagedG;
                    downscaledRgbValues[downscaledIndex + 2] = averagedR;
                }
            }
        }

        private Bitmap DownscaleMethodWithThreads(int newWidth, int newHeight, int factorRatio, int totalPixels)
        {
            // Declare an array to hold the bytes of the bitmap.
            byte[] originalRgbValues = CopyBitmapToArray(originalImage);
            byte[] downscaledRgbValues = new byte[newWidth * newHeight * 3]; // 24 bits per pixel (3 bytes per pixel)

            int threadCount = Environment.ProcessorCount; //The number of logical CPUs we have - 8
            int linesPerThread = (int)Math.Ceiling((double)newWidth / threadCount);          
           
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < threadCount; i++)
            {
                int yStart = i * linesPerThread;
                int yEnd = Math.Min(yStart + linesPerThread, newHeight);

                Thread thread = new Thread(() =>
                {
                    DownscaleLines(originalRgbValues, downscaledRgbValues, newWidth, newHeight, factorRatio, yStart, yEnd, totalPixels);
                });
                threads.Add(thread);
                thread.Start();
            }

            //Now wait for all to finish.
            foreach (Thread t in threads)
            {
                t.Join();
            }

            Bitmap downscaledImage = new Bitmap(newWidth, newHeight);
            SetBitmapFromArray(downscaledImage, downscaledRgbValues);

            return downscaledImage;
        }
        private Bitmap DownscaleMethod(int newWidth, int newHeight, int factorRatio, int totalPixels)
        {
            // Declare an array to hold the bytes of the bitmap.
            byte[] originalRgbValues = CopyBitmapToArray(originalImage);
            byte[] downscaledRgbValues = new byte[newWidth * newHeight * 3]; // 24 bits per pixel (3 bytes per pixel)

            DownscaleLines(originalRgbValues, downscaledRgbValues, newWidth, newHeight, factorRatio, 0, newHeight, totalPixels);

            Bitmap downscaledImage = new Bitmap(newWidth, newHeight);
            SetBitmapFromArray(downscaledImage, downscaledRgbValues);

            return downscaledImage;
        }
    }
}