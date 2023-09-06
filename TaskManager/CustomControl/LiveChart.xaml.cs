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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskManager.CustomControl
{
    /// <summary>
    /// Interaction logic for LiveChart.xaml
    /// </summary>
    public partial class LiveChart : UserControl
    {
        List<int> data;
        int count;
        int yOffset = 100;



        public LiveChart()
        {
            InitializeComponent();

            data = new List<int>();
            count = 0;
        }

        public int YOffset 
        { get => yOffset;
            set { yOffset = value; this.yLine.Y1 = value; this.yLine.Y2 = value; } 
        }

        public void AddValue(int val)
        {
            data.Add(yOffset - val);

            if (this.count > 128) { data.RemoveAt(0); }
            else { this.count++; }
            this.Draw();
        }

        private void Draw()
        {
            this.chart.Points.Clear();
            this.line.Points.Clear();

            this.chart.Points.Add(new Point(0, yOffset));

            int x = 0;
            foreach (var item in data)
            {
                this.chart.Points.Add(new Point(x, item));
                this.line.Points.Add(new Point(x, item));
                x++;
            }

            if (x > 1)
            {
                this.chart.Points.Add(new Point(x - 1, yOffset));

            }
        }
    }
}
