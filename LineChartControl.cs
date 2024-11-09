using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineChartApp
{
    public partial class LineChartControl : UserControl
    {
        // Data structure for storing the values of multiple lines
        public List<List<float>> Lines { get; set; } = new List<List<float>>();
        // Labels for each line (optional, for legend)
        public List<string> LineLabels { get; set; } = new List<string>();
        // Customizable line colors
        public List<Color> LineColors { get; set; } = new List<Color>();

        public LineChartControl()
        {
            // Default size for the chart
            this.Size = new Size(500, 400);
            // Default values
            this.Lines = new List<List<float>>();
            this.LineLabels = new List<string>();
            this.LineColors = new List<Color> { Color.Red, Color.Blue, Color.Green, Color.Purple, Color.Orange };
        }

        // Method to update the chart with new data
        public void UpdateChart(List<List<float>> newLines, List<string> newLabels)
        {
            this.Lines = newLines;
            this.LineLabels = newLabels;
            Invalidate(); // Redraw the chart
        }

        // Override the OnPaint method to draw the chart
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawChart(e.Graphics);
        }

        // Method to draw the line chart
        private void DrawChart(Graphics g)
        {
            // Get the bounds of the control
            var bounds = ClientRectangle;

            // Define chart margins
            int margin = 40;
            var chartArea = new Rectangle(margin, margin, bounds.Width - 2 * margin, bounds.Height - 2 * margin);

            // Calculate the maximum and minimum values for scaling the Y-axis
            float maxY = float.MinValue;
            float minY = float.MaxValue;
            foreach (var line in Lines)
            {
                foreach (var point in line)
                {
                    if (point > maxY) maxY = point;
                    if (point < minY) minY = point;
                }
            }

            // Add some padding to the max/min values
            float padding = (maxY - minY) * 0.1f;
            maxY += padding;
            minY -= padding;

            // Ensure that the chartArea has a valid size before drawing anything
            if (chartArea.Width <= 0 || chartArea.Height <= 0)
                return;

            // Draw the axes
            Pen axisPen = new Pen(Color.Black, 2);
            g.DrawLine(axisPen, chartArea.Left, chartArea.Bottom, chartArea.Right, chartArea.Bottom); // X-axis
            g.DrawLine(axisPen, chartArea.Left, chartArea.Top, chartArea.Left, chartArea.Bottom);  // Y-axis

            // Draw the Y-axis ticks and labels
            int numberOfTicks = 5;
            for (int i = 0; i <= numberOfTicks; i++)
            {
                float yValue = minY + (maxY - minY) * i / numberOfTicks;
                int yPosition = chartArea.Bottom - (int)((yValue - minY) / (maxY - minY) * chartArea.Height);

                // Boundary check for yPosition before drawing
                if (yPosition >= chartArea.Top && yPosition <= chartArea.Bottom)
                {
                    g.DrawLine(axisPen, chartArea.Left - 5, yPosition, chartArea.Left, yPosition); // Y-axis ticks
                    g.DrawString(yValue.ToString("F2"), Font, Brushes.Black, chartArea.Left - 40, yPosition - 10);
                }
            }

            // Draw the X-axis ticks (evenly spaced)
            int numberOfPoints = Lines.Count > 0 ? Lines[0].Count : 0;
            for (int i = 0; i < numberOfPoints; i++)
            {
                int xPosition = chartArea.Left + (int)((float)i / (numberOfPoints - 1) * chartArea.Width);

                // Boundary check for xPosition before drawing
                if (xPosition >= chartArea.Left && xPosition <= chartArea.Right)
                {
                    g.DrawLine(axisPen, xPosition, chartArea.Bottom, xPosition, chartArea.Bottom + 5); // X-axis ticks
                    g.DrawString(i.ToString(), Font, Brushes.Black, xPosition - 10, chartArea.Bottom + 5);
                }
            }

            // Draw the lines
            for (int i = 0; i < Lines.Count; i++)
            {
                var line = Lines[i];
                var pen = new Pen(LineColors[i % LineColors.Count], 2);

                if (line.Count > 1)
                {
                    PointF prevPoint = new PointF(chartArea.Left, chartArea.Bottom - (line[0] - minY) / (maxY - minY) * chartArea.Height);
                    for (int j = 1; j < line.Count; j++)
                    {
                        PointF currentPoint = new PointF(
                            chartArea.Left + (j * chartArea.Width / (line.Count - 1)),
                            chartArea.Bottom - (line[j] - minY) / (maxY - minY) * chartArea.Height
                        );
                        g.DrawLine(pen, prevPoint, currentPoint);
                        prevPoint = currentPoint;
                    }
                }
            }

            // Draw the legend
            DrawLegend(g, chartArea);
        }

        // Method to draw the legend
        private void DrawLegend(Graphics g, Rectangle chartArea)
        {
            int legendX = chartArea.Right + 10;
            int legendY = chartArea.Top;
            int legendWidth = 100;
            int legendHeight = 20;

            for (int i = 0; i < LineLabels.Count; i++)
            {
                g.FillRectangle(new SolidBrush(LineColors[i % LineColors.Count]), legendX, legendY + i * (legendHeight + 5), legendWidth, legendHeight);
                g.DrawString(LineLabels[i], Font, Brushes.Black, legendX + legendWidth + 5, legendY + i * (legendHeight + 5));
            }
        }
    }
}
