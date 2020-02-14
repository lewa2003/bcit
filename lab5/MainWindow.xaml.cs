using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System;

namespace lab5
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
            int max;
            if (!Int32.TryParse(levMaxValue.Text, out max)) {
                MessageBox.Show("Unable to parse MaxValue");
                return;
            }
            searchResult.Clear();
            time.Restart();
            foreach (string s in words)
            {
                if (Distance(s, word) <= max)
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

        public static int Distance(string s1, string s2)
        {
            int len1 = s1.Length, len2 = s2.Length;
            if (len1 == 0 || len2 == 0)
                return Math.Max(len1, len2);
            string tmp1 = s1.ToUpper();
            string tmp2 = s2.ToUpper();

            int[,] d = new int[len1 + 1, len2 + 1];
            for (int i = 0; i <= len1; ++i) d[i, 0] = i;
            for (int j = 0; j <= len2; ++j) d[0, j] = j;

            for (int i = 1; i <= len1; ++i)
                for (int j = 1; j <= len2; ++j)
                {
                    int indicator = (tmp1[i - 1] == tmp2[j - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                    d[i - 1, j] + 1,
                    Math.Min(d[i, j - 1] + 1,
                    d[i - 1, j - 1] + indicator)
                    );
                }
            return d[len1, len2];
        }
    }
}
