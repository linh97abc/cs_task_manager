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
    /// Interaction logic for PV_SP_LiveChart.xaml
    /// </summary>
    public partial class PV_SP_LiveChart : UserControl
    {
        List<int> data;
        List<int> data2;
        int count;
        int yOffset = 50;

        public PV_SP_LiveChart()
        {
            InitializeComponent();
            this.data = new List<int>();
            this.data2 = new List<int>();
        }

        public void AddValue(int y1, int y2)
        {
            data.Add(yOffset - y1);
            data2.Add(yOffset - y2);

            if (this.count > 128) { data.RemoveAt(0); data2.RemoveAt(0); }
            else { this.count++; }
            this.Draw();
        }

        private void Draw()
        {
            this.line.Points.Clear();
            this.line2.Points.Clear();

            int x = 0;
            foreach (var item in data)
            {
                this.line.Points.Add(new Point(x, item));
                x++;
            }

            x = 0;
            foreach (var item in data2)
            {
                this.line2.Points.Add(new Point(x, item));
                x++;
            }


        }

    }


}
