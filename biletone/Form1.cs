using System;
using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;

namespace biletone
{
    public partial class Form1 : Form
    {
        private Function function;
        private LeftRectangleIntegration integration;
        private GraphPlotter graphPlotter;

        public Form1()
        {
            InitializeComponent();
            graphPlotter = new GraphPlotter(panel1);
        }

        public class Function
        {
            public delegate double FunctionDelegate(double x);

            public FunctionDelegate FunctionBody { get; set; }

            public Function(FunctionDelegate functionBody)
            {
                FunctionBody = functionBody;
            }

            public double Evaluate(double x)
            {
                return FunctionBody(x);
            }
        }

         public class LeftRectangleIntegration
        {
            public Function Function { get; set; }
            public double LowerBound { get; set; }
            public double UpperBound { get; set; }
            public int NumberOfSteps { get; set; }

            public LeftRectangleIntegration(Function function, double lowerBound, double upperBound, int numberOfSteps)
            {
                Function = function;
                LowerBound = lowerBound;
                UpperBound = upperBound;
                NumberOfSteps = numberOfSteps;
            }

            public double Integrate()
            {
                double stepSize = (UpperBound - LowerBound) / NumberOfSteps;

                double sum = 0;
                for (int i = 0; i < NumberOfSteps; i++)
                {
                    double x = LowerBound + i * stepSize;
                    sum += Function.Evaluate(x);
                }

                sum *= stepSize;

                return sum;
            }
        }

        private void CalculateIntegral()
        {
            string input = textBox1.Text;
            // Разбор входного текста и создание экземпляра Function, например:
            function = new Function(x => Math.Sqrt(4*(1-(x*x)/25)));

            // Получение границ и количества шагов
            double lowerBound = Convert.ToDouble(textBox2.Text);
            double upperBound = Convert.ToDouble(textBox3.Text);
            int numberOfSteps = Convert.ToInt32(textBox4.Text);

            integration = new LeftRectangleIntegration(function, lowerBound, upperBound, numberOfSteps);
            double result = integration.Integrate();
            label1.Text = "Интеграл: " + result.ToString();

            graphPlotter.PlotFunction(function, lowerBound, upperBound);
            graphPlotter.PlotRectangles(integration, lowerBound, upperBound);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CalculateIntegral();
        }
    }

    public class GraphPlotter
    {
        private Panel graphPanel;
        private const double MAX_DRAW_VALUE = 1E5; // Максимальное значение для рисования графика

        public GraphPlotter(Panel panel)
        {
            graphPanel = panel;
        }

        public void PlotFunction(Form1.Function function, double lowerBound, double upperBound)
        {
            if (graphPanel == null) return;

            Graphics g = graphPanel.CreateGraphics();
            g.Clear(Color.White);
            DrawAxes(g);

            int width = graphPanel.Width;
            int height = graphPanel.Height;
            Pen pen = new Pen(Color.Blue, 2);

            for (int i = 1; i < width; i++)
            {
                double x1 = (i - 1 - width / 2) / 50.0;
                double x2 = (i - width / 2) / 50.0;
                double y1 = function.Evaluate(x1) * 50;
                double y2 = function.Evaluate(x2) * 50;

                if (double.IsInfinity(y1))
                {
                    y1 = Math.Sign(y1) * MAX_DRAW_VALUE;
                }
                if (double.IsInfinity(y2))
                {
                    y2 = Math.Sign(y2) * MAX_DRAW_VALUE;
                }

                g.DrawLine(pen, i - 1, (float)(height / 2 - y1), i, (float)(height / 2 - y2));
            }

            g.Dispose();
        }

        public void PlotRectangles(Form1.LeftRectangleIntegration integration, double lowerBound, double upperBound)
        {
            if (graphPanel == null) return;

            Graphics g = graphPanel.CreateGraphics();
            int width = graphPanel.Width;
            int height = graphPanel.Height;
            Pen pen = new Pen(Color.Red, 1);

            double stepSize = (upperBound - lowerBound) / integration.NumberOfSteps;
            for (int i = 0; i < integration.NumberOfSteps; i++)
            {
                double x1 = lowerBound + i * stepSize;
                double x2 = lowerBound + (i + 1) * stepSize;
                double y1 = integration.Function.Evaluate(x1);
                double y2 = integration.Function.Evaluate(x2);

                int screenX1 = (int)((x1 * 50) + width / 2);
                int screenX2 = (int)((x2 * 50) + width / 2);
                int screenY1 = height / 2 - (int)(y1 * 50);
                int screenY2 = height / 2 - (int)(y2 * 50);

                if (double.IsInfinity(y1))
                {
                    y1 = Math.Sign(y1) * MAX_DRAW_VALUE;
                }
                if (double.IsInfinity(y2))
                {
                    y2 = Math.Sign(y2) * MAX_DRAW_VALUE;
                }

                screenY1 = Math.Max(screenY1, height / 2 - (int)(MAX_DRAW_VALUE * 50));
                screenY2 = Math.Max(screenY2, height / 2 - (int)(MAX_DRAW_VALUE * 50));

                g.DrawLine(pen, screenX1, screenY1, screenX2, screenY1); // Верхняя сторона прямоугольника
                g.DrawLine(pen, screenX1, screenY1, screenX1, height / 2); // Левая сторона прямоугольника
                g.DrawLine(pen, screenX2, screenY1, screenX2, height / 2); // Правая сторона прямоугольника
            }

            g.Dispose();
        }

        private void DrawAxes(Graphics g)
        {
            if (graphPanel == null) return;

            int width = graphPanel.Width;
            int height = graphPanel.Height;
            Pen pen = new Pen(Color.Black, 1);

            g.DrawLine(pen, 0, height / 2, width, height / 2);
            g.DrawLine(pen, width / 2, 0, width / 2, height);
        }
    }
}
