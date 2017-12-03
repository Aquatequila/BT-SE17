using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_BT_SE17
{
    public class ButtonHandler
    {
        public void ToPng(Canvas canvas, int width, int height)
        {
            var saveFileDialog = new SaveFileDialog();
            
            saveFileDialog.InitialDirectory = @"C:\Users\ntecm\desktop\temp";
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
                canvas.Background = new SolidColorBrush(Colors.White);
                bmp.Render(canvas);

                var png = new PngBitmapEncoder();
                png.Frames.Add(BitmapFrame.Create(bmp));
                using (var stm = File.Create(saveFileDialog.FileName + ".png"))
                {
                    png.Save(stm);
                }
            }
        }
    }
}
