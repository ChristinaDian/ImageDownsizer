using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ImageDownsizer
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage;
        private Bitmap downscaledImage;
        private BitmapData originalImageData;
        private BitmapData downscaledImageData;
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
                downscaledImage  = new Bitmap(fileDialog.FileName);
                pictureBox1.Image = originalImage;
            }
        } 
        
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (originalImage == null)
            {
                MessageBox.Show("Please select a file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int downscaleFactor = (int)numericUpDown1.Value;   
            DownscaleMethod(downscaleFactor);
            pictureBox2.Image = downscaledImage;
        }
        private void DownscaleMethod(int downscaleFactor)
        {
            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, downscaledImage.Width, downscaledImage.Height);
            BitmapData bmpData =
                downscaledImage.LockBits(rect, ImageLockMode.ReadWrite,
                downscaledImage.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * downscaledImage.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Set every third value to 255. A 24bpp bitmap will look red.  
            for (int counter = 2; counter < rgbValues.Length; counter += 3)
                rgbValues[counter] = 255;

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            downscaledImage.UnlockBits(bmpData);

            // Draw the modified image.
            //e.Graphics.DrawImage(originalImage, 0, 150);
        }

    
    }
}