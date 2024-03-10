using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ImageDownsizer
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage;
        private Bitmap downscaledImage;

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
            DownscaleMethod(downscaleFactor);
            pictureBox2.Image = downscaledImage;

            stopwatch.Stop();
            lblTime.Text = $"Time without threads: {stopwatch.ElapsedMilliseconds} ms";

        }
        private void DownscaleMethod(int downscaleFactor)
        {
            int newWidth = (int)(originalImage.Width * downscaleFactor / 100);
            int newHeight = (int)(originalImage.Height * downscaleFactor / 100);
            int factorRatio = (int)(originalImage.Width / newWidth);

            downscaledImage = new Bitmap(newWidth, newHeight);

            // Lock the bitmap's bits.  
            Rectangle originalRect = new Rectangle(0, 0, originalImage.Width, originalImage.Height);
            Rectangle downscaleRect = new Rectangle(0, 0, newWidth, newHeight);

            BitmapData originalBmpData =
                originalImage.LockBits(originalRect, ImageLockMode.ReadOnly,
                originalImage.PixelFormat);

            BitmapData downscaledBmpData =
               downscaledImage.LockBits(downscaleRect, ImageLockMode.ReadWrite,
               downscaledImage.PixelFormat);

            // Get the address of the first line.
            IntPtr ptrOriginal = originalBmpData.Scan0;
            IntPtr ptrDownscaled = downscaledBmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(originalBmpData.Stride) * originalImage.Height;
            byte[] originalRgbValues = new byte[bytes];
            byte[] downscaledRgbValues = new byte[Math.Abs(downscaledBmpData.Stride) * downscaledImage.Height];

            // Copy the RGB values into the array.
            Marshal.Copy(ptrOriginal, originalRgbValues, 0, bytes);

            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    // downscaledRgbValues[y*newWidth + x] = originalRgbValues[factorRatio*(y * newWidth + x)];
                    int originalX = x * factorRatio;
                    int originalY = y * factorRatio;

                   
                    int downscaledIndex = (y * downscaledBmpData.Stride) + (x * factorRatio);

                    int sum = 0;
                    for (int i = 0; i < factorRatio; i++)
                    { 
                        int originalIndex = (originalY+i) * originalBmpData.Stride + (originalX * factorRatio);
                        sum += originalRgbValues[originalIndex+i];
                    }
                    byte average = (byte)(sum/factorRatio);

                    for (int i = 0; i < factorRatio; i++)
                    {
                        downscaledRgbValues[downscaledIndex + i] = average;
                    }
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(downscaledRgbValues, 0, ptrDownscaled, downscaledRgbValues.Length);

            // Unlock the bits.
            originalImage.UnlockBits(originalBmpData);
            downscaledImage.UnlockBits(downscaledBmpData);
        }
       
    }
}