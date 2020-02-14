using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace lab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> words = new List<string>();
        private string[] data;
        private char[] delimeters = new char[] { '\n', '\r', ' ', '.', ',', '!', '?' };
        private Stopwatch time;
        private List<ItemOfList> searchResult = new List<ItemOfList>();
        public MainWindow()
        {
            time = new Stopwatch();
            InitializeComponent();
            resultListBox.ItemsSource = searchResult;
        }

        private void onReadButton(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = "Document";
            openFileDialog.DefaultExt = ".txt";
            openFileDialog.Filter = "Text documents (.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                time.Restart();
                data = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8).Split(delimeters);
                words.Clear();
                foreach (string s in data)
                {
                    if (s.Trim() != "" && !words.Contains(s))
                    {
                        words.Add(s);
                    }
                }
            }
            time.Stop();
            readTimeLabel.Content = "Read time: " + time.Elapsed.TotalMilliseconds + " ms";
        }

        private void onSearchButton(object sender, RoutedEventArgs e)
        {
            string word = searchWord.Text;
            if (words.Count == 0)
            {
                MessageBox.Show("Read file first");
                return;
            }
            searchResult.Clear();
            time.Restart();
            foreach (string s in words)
            {
                if (s.ToUpper().Contains(word.ToUpper()))
                {
                    searchResult.Add(new ItemOfList() { Word = s });
                }
            }
            time.Stop();
            searchTimeLabel.Content = "Search time: " + time.Elapsed.TotalMilliseconds + " ms";
            resultListBox.Items.Refresh();
        }

        public class ItemOfList
        {
            public string Word { get; set; }
        }
    }
}
