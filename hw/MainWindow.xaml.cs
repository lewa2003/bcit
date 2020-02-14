using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System;
using System.Threading.Tasks;

namespace hw
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
            if (!Int32.TryParse(levMaxValue.Text, out max))
            {
                MessageBox.Show("Unable to parse MaxValue");
                return;
            }
            int threadNumber;
            if (!Int32.TryParse(threadCount.Text, out threadNumber)) 
            {
                MessageBox.Show("Unable to parse thread count");
                return;
            }
            time.Restart();
            List<ParallelSearchResult> result = new List<ParallelSearchResult>();
            List<StartEnd> arrayDivList = SubArrays.DivideSubArrays(0, words.Count, threadNumber);
            int count = arrayDivList.Count;
            Task<List<ParallelSearchResult>>[] tasks = new Task<List<ParallelSearchResult>>[count];

            for (int i = 0; i < count; i++)
            {
                List<string> tmpTaskList = words.GetRange(arrayDivList[i].Start, arrayDivList[i].End - arrayDivList[i].Start);
                tasks[i] = new Task<List<ParallelSearchResult>>(ArrayThreadTask, new ParallelSearchThreadParams()
                {
                    tmpList = tmpTaskList,
                    levMaxValue = max,
                    threadNum = i,
                    searchWord = word
                });
                tasks[i].Start();
            }
            Task.WaitAll(tasks);
            time.Stop();
            for (int i = 0; i < count; i++)
                result.AddRange(tasks[i].Result);

            searchResult.Clear();
            foreach (var i in result)
            {
                searchResult.Add(new ItemOfList { Word = i.word });
            }
            searchTimeLabel.Content = "Search time: " + time.Elapsed.TotalMilliseconds + " ms";
            resultListBox.Items.Refresh();
        }

        private void onSaveButtonClick(object sender, EventArgs e)
        {
            string reportFileNameTmp = "Report_" + DateTime.Now.ToString("dd_MM_yyyy_hhmmss");
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = reportFileNameTmp;
            saveFileDialog.DefaultExt = ".html";
            saveFileDialog.Filter = "HTML reports (.html)|*.html";

            if (saveFileDialog.ShowDialog() == true)
            {
                string ReportFileName = saveFileDialog.FileName; ;

                //Формирование отчета
                StringBuilder b = new StringBuilder();
                b.AppendLine("<html>");

                b.AppendLine("<head>");
                b.AppendLine("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/>");
                b.AppendLine("<title>" + "Отчет: " + ReportFileName + "</title>");
                b.AppendLine("</head>");

                b.AppendLine("<body>");

                b.AppendLine("<h1>" + "Отчет: " + ReportFileName + "</h1>");
                b.AppendLine("<table border='1'>");

                b.AppendLine("<tr>");
                b.AppendLine("<td>Время чтения из файла</td>");
                b.AppendLine("<td>" + readTimeLabel.Content.ToString() + "</td>");
                b.AppendLine("</tr>");

                b.AppendLine("<tr>");
                b.AppendLine("<td>Количество уникальных слов в файле</td>");
                b.AppendLine("<td>" + words.Count.ToString() + "</td>");
                b.AppendLine("</tr>");

                b.AppendLine("<tr>");
                b.AppendLine("<td>Слово для поиска</td>");
                b.AppendLine("<td>" + searchWord.Text + "</td>");
                b.AppendLine("</tr>");

                b.AppendLine("<tr>");
                b.AppendLine("<td>Максимальное расстояние для нечеткого поиска</td>");
                b.AppendLine("<td>" + levMaxValue.Text + "</td>");
                b.AppendLine("</tr>");

                b.AppendLine("<tr>");
                b.AppendLine("<td>Время нечеткого поиска</td>");
                b.AppendLine("<td>" + searchTimeLabel.Content.ToString() + "</td>");
                b.AppendLine("</tr>");

                b.AppendLine("<tr valign='top'>");
                b.AppendLine("<td>Результаты поиска</td>");
                b.AppendLine("<td>");
                b.AppendLine("<ul>");

                foreach (var x in resultListBox.Items)
                {
                    b.AppendLine("<li>" + x.ToString() + "</li>");
                }

                b.AppendLine("</ul>");
                b.AppendLine("</td>");
                b.AppendLine("</tr>");


                b.AppendLine("</table>");

                b.AppendLine("</body>");
                b.AppendLine("</html>");

                File.AppendAllText(ReportFileName, b.ToString());

                MessageBox.Show("Отчет сформирован. Файл: " + ReportFileName);
            }
        }

        class ItemOfList
        {
            public string Word { get; set; }
        }

        static int Distance(string s1, string s2)
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

        class ParallelSearchThreadParams
        {
            public List<string> tmpList { get; set; }
            public string searchWord { get; set; }
            public int levMaxValue { get; set; }
            public int threadNum { get; set; }
        }

        class ParallelSearchResult
        {
            public string word { get; set; }
            public int dist { get; set; }
            public int threadCount { get; set; }
        }

        static List<ParallelSearchResult> ArrayThreadTask(object paramObj)
        {
            ParallelSearchThreadParams param = (ParallelSearchThreadParams)paramObj;
            string tmpStr = param.searchWord.Trim().ToUpper();
            List<ParallelSearchResult> Result = new List<ParallelSearchResult>();

            foreach (string str in param.tmpList)
            {
                var distance = Distance(str.ToUpper(), tmpStr);
                if (distance <= param.levMaxValue)
                {
                    ParallelSearchResult tmp = new ParallelSearchResult()
                    {
                        word = str,
                        dist = distance,
                        threadCount = param.threadNum
                    };
                    Result.Add(tmp);
                }
            }
            return Result;
        }
    }
}
