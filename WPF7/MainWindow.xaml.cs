﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Start.Visibility = Visibility.Hidden;
            Year.Visibility = Visibility.Hidden;
            Year.Text = "";
            LabelYear.Visibility = Visibility.Hidden;

            Num.Visibility = Visibility.Visible;
            Month.Visibility = Visibility.Visible;
            Start.Visibility = Visibility.Visible;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            Num.Visibility = Visibility.Hidden;
            Month.Visibility = Visibility.Hidden;
            Start.Visibility = Visibility.Hidden;
            Num.Text = "";
            Month.Text = "";

            Start.Visibility = Visibility.Visible;
            Year.Visibility = Visibility.Visible;
            LabelYear.Visibility = Visibility.Visible;
        }

        private void Year_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Year.Text != "")
            {
                try
                {
                    int s = Convert.ToInt32(Year.Text);
                }
                catch (System.FormatException) // Ошибка при попытке неправильного ввода
                {
                    MessageBox.Show("Вы ввели символ! Пожалуйста, введите цифрy!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((bool)DSG.IsChecked) // Древнеславянский гороскоп
                {
                    if(Month.Text == "" || Num.Text == "")
                        MessageBox.Show("Возможно вы что-то не выбрали или/и не ввели!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        if (((Month.SelectedIndex + 1 == 2) && (Num.SelectedIndex + 1 == 30)) || (((Month.SelectedIndex + 1 == 2) &&  (Num.SelectedIndex + 1 == 31)))) // Исключение для февраля
                        {
                            MessageBox.Show("Февраль короткий месяц, проверьте правильность данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            int[] date = new int[2];
                            date[0] = Convert.ToInt32(Num.SelectedIndex + 1); // День рождения
                            date[1] = Convert.ToInt32(Month.SelectedIndex + 1); // Месяц рождения
                            try
                            {
                                switch (date[1])
                                {
                                    case 1: MessageBox.Show((date[0] <= 30 ? "Мороз (Морозко, Трескун, Студенец)" : "Велес")); break;
                                    case 2: MessageBox.Show("Велес"); break;
                                    case 3: MessageBox.Show("Макошь"); break;
                                    case 4: MessageBox.Show("Жива"); break;
                                    case 5: MessageBox.Show((date[0] <= 14 ? "Ярила (Ярило)" : "Леля")); break;
                                    case 6: MessageBox.Show(((date[0] <= 2 && date[0] != 24) ? "Леля" : ((date[0] <= 12 && date[0] > 2) ? "Кострома" : (date[0] == 24 ? "Иван Купала" : "Додола")))); break;
                                    case 7: MessageBox.Show((date[0] <= 6 ? "Додола" : "Лада")); break;
                                    case 8: MessageBox.Show((date[0] <= 28 ? "Перун" : "Сева")); break;
                                    case 9: MessageBox.Show((date[0] <= 13 ? "Сева" : ((date[0] > 13 && date[0] <= 27) ? "Рожаница" : "Сварожичи"))); break;
                                    case 10: MessageBox.Show((date[0] <= 15 ? "Сварожичи" : "Морена")); break;
                                    case 11: MessageBox.Show((date[0] <= 8 ? "Морена" : ((date[0] > 8 && date[0] <= 28) ? "Морена" : "Карачун"))); break;
                                    case 12: MessageBox.Show((date[0] <= 23 ? "Карачун" : "Мороз (Морозко, Трескун, Студенец)")); break;
                                }
                            }
                            catch { MessageBox.Show("Возможно вы что-то не выбрали или/и не ввели!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
                        }
                    }
                }
                else // Японский гороскоп
                {
                    int date = Convert.ToInt32(Year.Text);
                    date %= 12;
                    date++;
                    switch (date)
                    {
                        case 1: MessageBox.Show("Обезьяна"); break;
                        case 2: MessageBox.Show("Петух"); break;
                        case 3: MessageBox.Show("Собака"); break;
                        case 4: MessageBox.Show("Кабан"); break;
                        case 5: MessageBox.Show("Крыса"); break;
                        case 6: MessageBox.Show("Вол"); break;
                        case 7: MessageBox.Show("Тигр"); break;
                        case 8: MessageBox.Show("Кролик"); break;
                        case 9: MessageBox.Show("Дракон"); break;
                        case 10: MessageBox.Show("Змея"); break;
                        case 11: MessageBox.Show("Лошадь"); break;
                        case 12: MessageBox.Show("Овца"); break;
                    }
                }
            }
            catch { MessageBox.Show("Возможно вы что-то не выбрали или/и не ввели!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); } // Ошибка на все случаи
        }

        string path;

        private void StartOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

            var result = ofd.ShowDialog();

            if (result == true)
            {
                path = ofd.FileName;
            }
            read();

            Finish.Visibility = Visibility.Visible;
        }
        void read()
        {
            var lines = File.ReadAllLines(path, Encoding.Default);
            string[] temp = new string[3];

            StreamWriter sw = new StreamWriter(path, false, Encoding.Default);

            int j = 0;
            foreach (var line in lines)
            {
                string[] parts = line.Split(';');

                if (j == 0)
                {
                    sw.Write($"{parts[0]};{parts[1]};{parts[2]};ДСГ;ЯГ");
                    j++;
                    sw.WriteLine();
                }
                else
                {
                    sw.Write($"{parts[0]};{parts[1]};{parts[2]}");
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (Regex.IsMatch(parts[i], @"^[0-9]+$"))
                        {
                            temp[i] = parts[i];
                        }
                        else
                        {
                            temp[i] = "";
                        }
                    }
                    if (!String.IsNullOrEmpty(temp[0]) && !String.IsNullOrEmpty(temp[1]))
                    {
                        int countDays = DateTime.DaysInMonth(DateTime.Now.Year, Convert.ToInt32(temp[1]) - 1);
                        if (Convert.ToInt32(temp[0]) <= countDays)
                        {
                            int[] date = new int[2];
                            string res = "";
                            date[0] = Convert.ToInt32(temp[0]); // День рождения
                            date[1] = Convert.ToInt32(temp[1]); // Месяц рождения
                            try
                            {
                                switch (date[1])
                                {
                                    case 1: res = ((date[0] <= 30 ? "Мороз (Морозко, Трескун, Студенец)" : "Велес")); break;
                                    case 2: res = ("Велес"); break;
                                    case 3: res = ("Макошь"); break;
                                    case 4: res = ("Жива"); break;
                                    case 5: res = ((date[0] <= 14 ? "Ярила (Ярило)" : "Леля")); break;
                                    case 6: res = (((date[0] <= 2 && date[0] != 24) ? "Леля" : ((date[0] <= 12 && date[0] > 2) ? "Кострома" : (date[0] == 24 ? "Иван Купала" : "Додола")))); break;
                                    case 7: res = ((date[0] <= 6 ? "Додола" : "Лада")); break;
                                    case 8: res = ((date[0] <= 28 ? "Перун" : "Сева")); break;
                                    case 9: res = ((date[0] <= 13 ? "Сева" : ((date[0] > 13 && date[0] <= 27) ? "Рожаница" : "Сварожичи"))); break;
                                    case 10: res = ((date[0] <= 15 ? "Сварожичи" : "Морена")); break;
                                    case 11: res = ((date[0] <= 8 ? "Морена" : ((date[0] > 8 && date[0] <= 28) ? "Морена" : "Карачун"))); break;
                                    case 12: res = ((date[0] <= 23 ? "Карачун" : "Мороз (Морозко, Трескун, Студенец)")); break;
                                    default: res = "Неопределено"; break;
                                }
                                sw.Write($";{res}");
                            }
                            catch
                            {
                                sw.Write($";{res}");
                            }
                        }
                        else
                        {
                            sw.Write($";Неопределено");
                        }
                    }
                    else
                    {
                        sw.Write($";Неопределено");
                    }
                    if (!String.IsNullOrEmpty(temp[2]))
                    {
                        string res = "";
                        try
                        {
                            int date = Convert.ToInt32(temp[2]);
                            date %= 12;
                            date++;
                            switch (date)
                            {
                                case 1: res = ("Обезьяна"); break;
                                case 2: res = ("Петух"); break;
                                case 3: res = ("Собака"); break;
                                case 4: res = ("Кабан"); break;
                                case 5: res = ("Крыса"); break;
                                case 6: res = ("Вол"); break;
                                case 7: res = ("Тигр"); break;
                                case 8: res = ("Кролик"); break;
                                case 9: res = ("Дракон"); break;
                                case 10: res = ("Змея"); break;
                                case 11: res = ("Лошадь"); break;
                                case 12: res = ("Овца"); break;
                                default: res = "Неопределено"; break;
                            }
                        }
                        catch
                        {
                            res = "Неопределено";
                        }
                        sw.Write($";{res}");
                    }
                    else
                    {
                        sw.Write($";Неопределено");
                    }
                    sw.WriteLine();
                }
            }
            sw.Close();
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(path);
        }
    }
}
