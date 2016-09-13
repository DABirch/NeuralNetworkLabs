using System.Linq;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NeuroNetwork.Network;
using System.Collections;
using System.Windows;
using Image = NeuroNetwork.ViewModels.Image;

namespace NeuroNetwork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private readonly KohonenNetwork _neuronNetwork;

        public MainWindow()
        {
            InitializeComponent();

            _neuronNetwork = new KohonenNetwork(45 * 45, 6);

            var files = Directory.EnumerateFiles(@".\images\", "*.png");

            var images = new List<ViewModels.Image>();
            foreach (var file in files)
            {
                var fullFileName = Path.Combine(Environment.CurrentDirectory, file);
                var uri = fullFileName;
                var image = new ViewModels.Image
                {
                    ShortFileName = Path.GetFileNameWithoutExtension(file),
                    Uri = uri,
                    Pixels = LoadPixels(fullFileName).ToArray(),
                    Answer = GetAnswerFromFileName(Path.GetFileNameWithoutExtension(file))
                };
                images.Add(image);

                _neuronNetwork.Study(image.Pixels, image.Answer);
                _neuronNetwork.Save(Environment.CurrentDirectory);
            }
        }


        private IEnumerable<int> LoadPixels(string fullFileName)
        {
            var bitmap = new Bitmap(fullFileName);
            for (var i = 0; i < bitmap.Height; i++)
            {
                for (var j = 0; j < bitmap.Width; j++)
                {
                    var color = bitmap.GetPixel(j, i);

                    yield return ToGrayScale(color);
                }
            }
        }

        private int ToGrayScale(Color color)
        {
            if (color.R == 255 && color.G == 255 && color.B == 255)
                return 255;
            return 0;
        }

        /// <summary>
        /// Gets the name of the answer from file.
        /// </summary>
        /// <returns>The answer from file name.</returns>
        /// <param name="shortFileName">Short file name.</param>
        private int GetAnswerFromFileName(string shortFileName)
        {
            shortFileName = shortFileName.Substring(shortFileName.LastIndexOf(" ") + 1, 1);
            string returnValue = Path.GetFileNameWithoutExtension(shortFileName);

            return Int32.Parse(returnValue);
        }


        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Files (*.png)|*.png";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                textBox.Text = filename;

                Image uploadImage = new Image();
                uploadImage.Pixels = LoadPixels(filename).ToArray();
                uploadImage.ShortFileName = Path.GetFileNameWithoutExtension(filename);
                uploadImage.Uri = filename;

                int neuroAnswer = _neuronNetwork.Handle(uploadImage.Pixels);

                uploadImage.Answer = neuroAnswer;
                value.Content = neuroAnswer.ToString();

            }
        }
    }
}
