using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

namespace Lab9Snippety
{

    public class PageResponse
    {
        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        [JsonPropertyName("pagesCount")]
        public int PagesCount { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("batches")]
        public List<SnippetResponse> Batches { get; set; }
    }

    public class SnippetResponse
    {
        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("creationTime")]
        public DateTime? CreationTime { get; set; }

        [JsonPropertyName("updateTime")]
        public DateTime? UpdateTime { get; set; }
    }

    class SnippetTypeDef
    {
        public String Name;
        public String Value;
        public Color color;
    }

    static class ColorHelpers
    {
        public static float GetBrightness(this System.Windows.Media.Color c) =>
                System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B).GetBrightness();
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<SnippetTypeDef> snippetTypes = new List<SnippetTypeDef> {
            new SnippetTypeDef {Name = "Text", Value = "text", color = Colors.Gray},
            new SnippetTypeDef {Name = "Bash", Value = "bash", color = Colors.Green},
            new SnippetTypeDef {Name = "C++", Value = "cpp", color = Colors.DarkBlue},
            new SnippetTypeDef {Name = "C#", Value = "cs", color = Colors.Purple},
            new SnippetTypeDef {Name = "Java", Value = "java", color = Colors.DeepPink},
            new SnippetTypeDef {Name = "CSS", Value = "css", color = Colors.DeepSkyBlue},
            new SnippetTypeDef {Name = "HTML", Value = "html", color = Colors.Red},
            new SnippetTypeDef {Name = "JavaScript", Value = "javascript", color = Colors.Orange},
            new SnippetTypeDef {Name = "PHP", Value = "php", color = Colors.BlueViolet},
            new SnippetTypeDef {Name = "Python", Value = "python", color = Colors.Yellow},
            new SnippetTypeDef {Name = "SQL", Value = "sql", color = Colors.GreenYellow},
        };

        public string SnippetType = "text";
        public int Page = 1;
        public int PageSize = 5;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void snippetsTypePanel_Initialized(object sender, EventArgs e)
        {
            foreach (var st in snippetTypes)
            {
                Console.WriteLine("ASffddffd");
                var foregroundColor = Colors.Black;
                if (st.color.GetBrightness() < 0.5)
                {
                    foregroundColor = Colors.White;
                }
                var btn = new Button
                {
                    Padding = new Thickness(3),
                    Content = st.Name,
                    Background = new SolidColorBrush { Color = st.color },
                    Margin = new Thickness(1, 1, 10, 1),
                    Foreground = new SolidColorBrush { Color = foregroundColor },
                };
                btn.Click += SnippetsTypeClicked;
                snippetsTypePanel.Children.Add(btn);
            }
           
        }

        private void SnippetsTypeClicked(object sender, RoutedEventArgs e)
        {
            var def = snippetTypes.Find(s => s.Name == ((sender as Button).Content as string));
            SnippetType = def.Value;
            FetchSnippets();
        }

        private void paginatorStackPanel_Initialized(object sender, EventArgs e)
        {

            var pagesCount = 10;
            for (int i = 0; i < pagesCount; i++)
            {
                var btn = new Button
                {
                    Content = i.ToString(),
                    Margin = new Thickness(1, 1, 10, 1),
                    Padding = new Thickness(3),

                };
                btn.Click += PaginatorBtn_Click;
                paginatorStackPanel.Children.Add(btn);
            }
        }

        private void PaginatorBtn_Click(object sender, RoutedEventArgs e)
        {
            Page = int.Parse((sender as Button).Content as string);
            FetchSnippets();
        }

        public string FetchData(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                ContentType type = new ContentType(response.ContentType ?? "text/plain;charset=" + Encoding.UTF8.WebName);
                Encoding encoding = Encoding.GetEncoding(type.CharSet ?? Encoding.UTF8.WebName);

                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public void FetchSnippets()
        {
          
            string url = $"https://dirask.com/api/snippets?pageNumber={Page}&pageSize={PageSize}&dataOrder=newest&dataGroup=batches&snippetsType={Uri.EscapeUriString(SnippetType)}";
            string data = FetchData(url);

            var parsed = JsonSerializer.Deserialize<PageResponse>(data);
            snippetsDataGrid.ItemsSource = parsed.Batches;
        }

        private void snippetsDataGrid_Initialized(object sender, EventArgs e)
        {
            FetchSnippets();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            PageSize = int.Parse((cb.SelectedItem as Label).Content as string);
            FetchSnippets();
        }
    }
}
