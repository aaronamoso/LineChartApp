namespace LineChartApp
{
    public partial class Form1 : Form
    {
        private LineChartControl lineChart;
        public Form1()
        {
            InitializeComponent();

            // Create an instance of the LineChartControl
            lineChart = new LineChartControl
            {
                Dock = DockStyle.Top, // Fill the top part of the form
                Height = 300
            };

            // Add the line chart to the form
            Controls.Add(lineChart);

            // Setup user input controls for data input
            SetupInputControls();
        }

        // Method to set up input controls for the user
        private void SetupInputControls()
        {
            // Create a label and textbox for the first line data
            Label label = new Label
            {
                Text = "Enter data for Line 1 (comma separated):",
                Location = new Point(10, 320),
                Width = 250
            };
            TextBox textBox = new TextBox
            {
                Name = "TextBoxLine1",
                Location = new Point(260, 320),
                Width = 200
            };

            // Add the label and textbox to the form
            Controls.Add(label);
            Controls.Add(textBox);

            // Repeat for additional lines
            label = new Label
            {
                Text = "Enter data for Line 2 (comma separated):",
                Location = new Point(10, 350),
                Width = 250
            };
            TextBox textBox2 = new TextBox
            {
                Name = "TextBoxLine2",
                Location = new Point(260, 350),
                Width = 200
            };
            Controls.Add(label);
            Controls.Add(textBox2);

            // Add a button to update the chart
            Button updateButton = new Button
            {
                Text = "Update Chart",
                Location = new Point(10, 380),
                Width = 100
            };

            // Button click event to update the chart
            updateButton.Click += (sender, args) =>
            {
                var lines = new List<List<float>>();
                var labels = new List<string>();

                // Collect data for Line 1
                var line1Data = textBox.Text.Split(',');
                List<float> line1 = new List<float>();
                foreach (var data in line1Data)
                {
                    if (float.TryParse(data.Trim(), out float value))
                        line1.Add(value);
                }
                lines.Add(line1);
                labels.Add("Line 1");

                // Collect data for Line 2
                var line2Data = textBox2.Text.Split(',');
                List<float> line2 = new List<float>();
                foreach (var data in line2Data)
                {
                    if (float.TryParse(data.Trim(), out float value))
                        line2.Add(value);
                }
                lines.Add(line2);
                labels.Add("Line 2");

                // Update the chart with the new data
                lineChart.UpdateChart(lines, labels);
            };

            Controls.Add(updateButton);
        }
    }
}

