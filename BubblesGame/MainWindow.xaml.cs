using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Wpf_Jewels
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer timer2 = new System.Windows.Threading.DispatcherTimer();

        private int size = 10;
        static Random Rand = new Random();
        bool is_selected;
        int start_position, start_x, start_y;
        bool flag;
        public Ellipse Temp = new Ellipse();
        public int Time = 30;
        public int Score = 0;

        Canvas CanvasPanel;
        Ellipse[,] EllipseBox;
        Ellipse EllipseTemp = new Ellipse();

        public MainWindow()
        {
            InitializeComponent();
            InitGame();
            InitJewels();

            timer.Tick += new EventHandler(UpdateText);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            timer2.Tick += new EventHandler(UpdateTime);
            timer2.Interval = new TimeSpan(0, 0, 0, 0, 300);
            timer2.Start();

        }

        private void UpdateText(object sender, EventArgs e)
        {
            Time--;
        }

        private void UpdateTime(object sender, EventArgs e)
        {
            lb_time.Content = Time.ToString();
            lb_score.Content = Score.ToString();
            if (Time <= 0)
            {
                MessageBox.Show("Game over! You got " + Score + " scores!");
                window.Close();
            }
        }

        private void InitGame()
        {
            lb_time.Content = Time.ToString();
            lb_score.Content = Score.ToString();
            CanvasPanel = new Canvas
            {
                Background = new SolidColorBrush(Colors.AntiqueWhite)
            };
            int startWidth = 500; 
            int startHeight = 500; 
            CanvasPanel.Width = startWidth + 27;
            CanvasPanel.Height = startHeight + 27;
            grid1.Children.Add(CanvasPanel);
            int w, h;
            w = (int)startWidth / size;
            h = (int)startHeight / size;
            EllipseBox = new Ellipse[size, size];
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    Ellipse Ellipse = new Ellipse
                    {
                        Stroke = Brushes.Black,
                        Width = w,
                        Height = h,
                        Tag = y * size + x
                    };
                    Ellipse.MouseUp += EllipseMouseUp;
                    Canvas.SetLeft(Ellipse, x * (w + 3));
                    Canvas.SetTop(Ellipse, y * (h + 3));
                    CanvasPanel.Children.Add(Ellipse);
                    EllipseBox[x, y] = Ellipse;
                }
            FillTheField(w, h);
        }

        private void FillTheField(int w, int h)
        {
            for (int x = 0; x <= size * w + w; x += w + 3)
            {
                Line l = new Line
                {
                    X1 = x - 2,
                    X2 = x - 2,
                    Y1 = 0,
                    Y2 = size * h + h / 1.5,
                    Stroke = new SolidColorBrush(Colors.BlueViolet),
                    StrokeThickness = 0.1
                };
                CanvasPanel.Children.Add(l);
            }
            for (int y = 0; y <= size * h + h; y += h + 3)
            {
                Line l = new Line
                {
                    X1 = 0,
                    X2 = size * w + w / 1.8,
                    Y1 = y - 1,
                    Y2 = y - 1,
                    Stroke = new SolidColorBrush(Colors.BlueViolet),
                    StrokeThickness = 0.1
                };
                CanvasPanel.Children.Add(l);
            }
        }

        private void InitJewels()
        {
            for (int x = 0; x < size * size; x++)
            {
                switch (Rand.Next(0, 6))
                {
                    case 0: EllipseBox[x % size, x / size].Fill = Brushes.LightPink; break;
                    case 1: EllipseBox[x % size, x / size].Fill = Brushes.DarkOrchid; break;
                    case 2: EllipseBox[x % size, x / size].Fill = Brushes.MediumSeaGreen; break;
                    case 3: EllipseBox[x % size, x / size].Fill = Brushes.Yellow; break;
                    case 4: EllipseBox[x % size, x / size].Fill = Brushes.SkyBlue; break;
                    case 5: EllipseBox[x % size, x / size].Fill = Brushes.Orange; break;
                }
            }
            do
            {
                for (int x = 0; x < size - 2; x++)       
                    for (int y = 0; y < size; y++)

                        if (EllipseBox[x, y].Fill == EllipseBox[x + 1, y].Fill && EllipseBox[x + 1, y].Fill == EllipseBox[x + 2, y].Fill)
                            EllipseBox[x, y].Fill = EllipseBox[Rand.Next(0, size), Rand.Next(0, size)].Fill;

                for (int x = 0; x < size; x++)      
                    for (int y = 0; y < size - 2; y++)
                        if (EllipseBox[x, y].Fill == EllipseBox[x, y + 1].Fill && EllipseBox[x, y + 1].Fill == EllipseBox[x, y + 2].Fill)
                            EllipseBox[x, y].Fill = EllipseBox[Rand.Next(0, size), Rand.Next(0, size)].Fill;
            }
            while (CheckForDestructionHorizontal() || CheckForDestructionVertical());
        }


        private bool CheckForDestructionHorizontal()
        {
            for (int x = 0; x < size - 2; x++)      
                for (int y = 0; y < size; y++)

                    if (EllipseBox[x, y].Fill == EllipseBox[x + 1, y].Fill && EllipseBox[x + 1, y].Fill == EllipseBox[x + 2, y].Fill)
                    {
                        return true;
                    }
            return false;
        }

        private bool CheckForDestructionVertical()
        {
            for (int x = 0; x < size; x++)     
                for (int y = 0; y < size - 2; y++)
                    if (EllipseBox[x, y].Fill == EllipseBox[x, y + 1].Fill && EllipseBox[x, y + 1].Fill == EllipseBox[x, y + 2].Fill)
                    {
                        return true;
                    }
            return false;
        }


        private void EllipseMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (flag == true) return; 
            if (is_selected)
            {
                BallWinkAlternative(Temp);
                int x, y;
                int dest_position = Convert.ToInt16(((Ellipse)sender).Tag);
                PositionToCoords(dest_position, out x, out y);
                if (CheckRange(start_x, start_y, x, y))
                {
                    EllipseTemp.Fill = EllipseBox[x, y].Fill;
                    EllipseBox[x, y].Fill = EllipseBox[start_x, start_y].Fill;
                    EllipseBox[start_x, start_y].Fill = EllipseTemp.Fill;

                    if ((!CheckForDestructionHorizontal()) && (!CheckForDestructionVertical()))
                    {
                        EllipseTemp.Fill = EllipseBox[start_x, start_y].Fill;
                        EllipseBox[start_x, start_y].Fill = EllipseBox[x, y].Fill;
                        EllipseBox[x, y].Fill = EllipseTemp.Fill;
                    }
                    else
                    {
                        Vanish();
                    }
                }
                is_selected = false;
            }

            else 
            {
                start_position = Convert.ToInt16(((Ellipse)sender).Tag);
                PositionToCoords(start_position, out start_x, out start_y);
                is_selected = true;
                BallWink(sender);
            }
        }


        public Ellipse BallWink(object s)
        {
            Ellipse t = s as Ellipse;
            DoubleAnimation da = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            t.BeginAnimation(Ellipse.OpacityProperty, da);
            return Temp = t;
        }

        public void BallWinkAlternative(object s)
        {
            Ellipse t = s as Ellipse;
            DoubleAnimation da = new DoubleAnimation
            {
                From = 1,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.1),
                AutoReverse = true
            };
            t.BeginAnimation(Ellipse.OpacityProperty, da);

        }

        private bool CheckRange(int start_x, int start_y, int dest_x, int dest_y)
        {
            if ((dest_x == start_x && dest_y == start_y - 1) || (dest_x == start_x + 1 && dest_y == start_y) || (dest_x == start_x && dest_y == start_y + 1) || (dest_x == start_x - 1 && dest_y == start_y))
                return true;
            else
                return false;
        }
        

        public void HideBalls(Ellipse e1, Ellipse e2, Ellipse e3) //плавно исчезаем шарики
        {
            DoubleAnimation da = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            e1.BeginAnimation(Ellipse.OpacityProperty, da);
            e2.BeginAnimation(Ellipse.OpacityProperty, da);
            e3.BeginAnimation(Ellipse.OpacityProperty, da);
        }

        public void ShowBalls(Ellipse e1, Ellipse e2, Ellipse e3)
        {
            DoubleAnimation da = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.8)
            };
            e1.BeginAnimation(Ellipse.OpacityProperty, da);
            e2.BeginAnimation(Ellipse.OpacityProperty, da);
            e3.BeginAnimation(Ellipse.OpacityProperty, da);
        }


        async void Vanish()
        {
            await CheckThreeInRowHorizontal();
            await CheckThreeInRowVertical();
            CheckThreeInRowControl();
        }

        private void CheckThreeInRowControl()
        {
            for (int x = 0; x < size - 2; x++)
                for (int y = 0; y < size; y++)
                {
                    if (EllipseBox[x, y].Fill == EllipseBox[x + 1, y].Fill && EllipseBox[x + 1, y].Fill == EllipseBox[x + 2, y].Fill) Vanish(); 
                }
            for (int x = 0; x < size; x++) 
                for (int y = 0; y < size - 2; y++)
                {
                    if (EllipseBox[x, y].Fill == EllipseBox[x, y + 1].Fill && EllipseBox[x, y + 1].Fill == EllipseBox[x, y + 2].Fill) Vanish(); 
                }
        }

        private async Task CheckThreeInRowVertical()
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size - 2; y++)
                {
                    while (EllipseBox[x, y].Fill == EllipseBox[x, y + 1].Fill && EllipseBox[x, y + 1].Fill == EllipseBox[x, y + 2].Fill) 
                    {
                        flag = true;
                        HideBalls(EllipseBox[x, y], EllipseBox[x, y + 1], EllipseBox[x, y + 2]);
                        Score += 5; 
                        Time += 3;
                        await Task.Delay(800); 
                        for (int k = 2, j = 1; (y + k >= 0 && y - j >= 0); k--, j++)
                        EllipseBox[x, y + k].Fill = EllipseBox[x, y - j].Fill;  
                        ShowBalls(EllipseBox[x, y], EllipseBox[x, y + 1], EllipseBox[x, y + 2]);
                        EllipseBox[x, 0].Fill = EllipseBox[Rand.Next(0, size), Rand.Next(0, size)].Fill; 
                        EllipseBox[x, 1].Fill = EllipseBox[Rand.Next(0, size), Rand.Next(0, size)].Fill;
                        EllipseBox[x, 2].Fill = EllipseBox[Rand.Next(0, size), Rand.Next(0, size)].Fill;
                        await Task.Delay(200); 
                        flag = false;
                    }
                }
        }

        private async Task CheckThreeInRowHorizontal()
        {
            for (int x = 0; x < size - 2; x++) 
                for (int y = 0; y < size; y++)
                {
                    while (EllipseBox[x, y].Fill == EllipseBox[x + 1, y].Fill && EllipseBox[x + 1, y].Fill == EllipseBox[x + 2, y].Fill) 
                    {
                        flag = true;
                        HideBalls(EllipseBox[x, y], EllipseBox[x + 1, y], EllipseBox[x + 2, y]);
                        Score += 5;
                        Time += 3;
                        await Task.Delay(800);
                        for (int k = 0, j = 1; (y - k >= 0 && y - j >= 0); k++, j++)
                        {
                            EllipseBox[x, y - k].Fill = EllipseBox[x, y - j].Fill;
                            EllipseBox[x + 1, y - k].Fill = EllipseBox[x + 1, y - j].Fill;
                            EllipseBox[x + 2, y - k].Fill = EllipseBox[x + 2, y - j].Fill;
                        }
                        ShowBalls(EllipseBox[x, y], EllipseBox[x + 1, y], EllipseBox[x + 2, y]);
                        EllipseBox[x, 0].Fill = EllipseBox[Rand.Next(0, size), Rand.Next(0, size)].Fill;
                        EllipseBox[x + 1, 0].Fill = EllipseBox[Rand.Next(0, size), Rand.Next(0, size)].Fill;
                        EllipseBox[x + 2, 0].Fill = EllipseBox[Rand.Next(0, size), Rand.Next(0, size)].Fill;
                        await Task.Delay(200);
                        flag = false;
                    }
                }
        }

        public int CoordsToPosition(int x, int y)
        {
            if (x < 0) x = 0;
            if (x > size - 1) x = size - 1;
            if (y < 0) y = 0;
            if (y > size - 1) y = size - 1;
            return y * size + x;
        }

        private void PositionToCoords(int position, out int x, out int y)
        {
            if (position < 0) position = 0;
            if (position > size * size - 1) position = size * size - 1;
            x = position % size;
            y = position / size;
        }
    }
}
