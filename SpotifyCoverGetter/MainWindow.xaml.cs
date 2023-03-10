using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpotifyCoverGetter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void getCover(object sender, RoutedEventArgs e)
        {
            string data = null;
            if (link.Text != "")
            {
                String enter_link = link.Text;
                string prefix_link = "https://open.spotify.com/oembed?url=";
                string full_link = prefix_link + enter_link;
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(full_link);
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream receiveStream = response.GetResponseStream();
                        StreamReader readStream = null;
                        if (response.CharacterSet == null)
                            readStream = new StreamReader(receiveStream);
                        else
                            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                        data = readStream.ReadToEnd();
                        response.Close();
                        readStream.Close();
                    }
                    //Serialize the JSON
                    Model modelJson = JsonSerializer.Deserialize<Model>(data);

                    //Manage the Width and Height label
                    widthLabel.Content = "Width : " + modelJson.width;
                    heightLabel.Content = "Height : " + modelJson.height;

                    //Manage the Image
                    Uri imgLink = new Uri(modelJson.thumbnail_url);
                    image_box.Source = new BitmapImage(imgLink);

                    //Add click on the image to save it on the desktop ? or select the file

                }catch(WebException error)
                {
                    MessageBox.Show("Please, enter a valid spotify url.\n\n\nExample: https://open.spotify.com/track/...", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please, enter a spotify URL.\n\n\nExample : https://open.spotify.com/track/...", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            }
        }
}
