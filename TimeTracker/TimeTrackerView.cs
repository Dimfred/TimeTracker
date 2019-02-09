using System;
using System.Drawing;
using System.Windows.Forms;


namespace TimeTracker
{
    public partial class TimeTrackerView : Form
    {

        public TimeTrackerController Controller { get; set; }


        public ListView lv_trackedTopics { get; set; }

        public Button bt_addItem { get; set; }
        public Button bt_delItem { get; set; }
        public Button bt_start { get; set; }
        public Button bt_stop { get; set; }

        public Label lb_time { get; set; }

        public TextBox tb_path_input { get; set; }

        private readonly string col_1_name = "Name";
        private readonly string col_2_name = "Time";
        //private readonly string col_3_name = "";


        public int MIN_WIDTH = 826;
        public int MIN_HEIGHT = 228;

        // positions
        // general
        int x_pad = 20;
        int y_pad = 20;


        public TimeTrackerView(TimeTrackerController controller)
        {
            Controller = controller;
            Controller.SetView(this);

            InitializeComponent();
            SetApplicationParameter();

            CreateTrackedItemsListView();
            CreateButtons();

            CreateTimeLabels();

            CreatePathInput();

            AddControls();
            SetEventHandler();


        }

        private void SetApplicationParameter()
        {
            Text = "TimeTracker5000";
            Width = MIN_WIDTH;
            Height = MIN_HEIGHT;
            MinimumSize = new Size(MIN_WIDTH, MIN_HEIGHT);
        }

        private void SetEventHandler()
        {
            Resize += new EventHandler(Controller.TimeTrackerView_Resize);
            Load += new EventHandler(Controller.TimeTrackerView_Load);
            lv_trackedTopics.SelectedIndexChanged += new EventHandler(Controller.ListViewItem_Selected);
            bt_addItem.Click += new EventHandler(Controller.BT_AddItem_Clicked);
            bt_delItem.Click += new EventHandler(Controller.BT_DelItem_Clicked);
            bt_start.Click += new EventHandler(Controller.BT_Start_Clicked);
            bt_stop.Click += new EventHandler(Controller.BT_Stop_Clicked);
            tb_path_input.KeyDown += new KeyEventHandler(Controller.TB_PathInput_EnterPressed);
        }

        private void AddControls()
        {
            Controls.Add(lv_trackedTopics);
            Controls.Add(bt_addItem);
            Controls.Add(bt_delItem);
            Controls.Add(lb_time);
            Controls.Add(bt_start);
            Controls.Add(bt_stop);
            Controls.Add(tb_path_input);
        }

        private void CreateTrackedItemsListView()
        {
            int x = x_pad, y = y_pad;
            int w = 200, h = Height - 80;

            lv_trackedTopics = new ListView()
            {
                Bounds = new Rectangle(new Point(x, y), new Size(w, h)),
                View = View.Details,
                LabelEdit = true,
                AllowColumnReorder = true,
                CheckBoxes = true,
                Visible = true,
            };
            lv_trackedTopics.Columns.Clear();
            lv_trackedTopics.Columns.Add(col_1_name, -2, HorizontalAlignment.Left);
            lv_trackedTopics.Columns.Add(col_2_name, -2, HorizontalAlignment.Left);

        }

        private void CreateButtons()
        {
            int x_bt_addItem = lv_trackedTopics.Width + lv_trackedTopics.Location.X;
            int y_bt_addItem = lv_trackedTopics.Location.Y;
            bt_addItem = new Button()
            {
                Text = "Add Item",
                Location = new Point(x_bt_addItem, y_bt_addItem)
            };


            int x_bt_delItem = x_bt_addItem;
            int y_bt_delItem = y_bt_addItem + bt_addItem.Height;
            bt_delItem = new Button()
            {
                Text = "Delete Item",
                Location = new Point(x_bt_delItem, y_bt_delItem)
            };

            int x_bt_start = x_bt_addItem;
            int y_bt_start = y_bt_delItem + bt_delItem.Height;
            bt_start = new Button()
            {
                Text = "Start",
                Location = new Point(x_bt_start, y_bt_start),
            };

            int x_bt_stop = x_bt_addItem;
            int y_bt_stop = y_bt_start + bt_start.Height;
            bt_stop = new Button()
            {
                Text = "Stop",
                Location = new Point(x_bt_stop, y_bt_stop)
            };

        }

        private void CreateTimeLabels()
        {
            Font font = new Font("Arial", 80, FontStyle.Bold);
            string startVal = "00:00:00";


            lb_time = new Label()
            {
                AutoSize = true,
                Text = startVal,
                Font = font
            };
            int x = bt_delItem.Location.X + bt_delItem.Width;
            int y = (Height / 2) - (font.Height / 2);

            lb_time.Location = new Point(x, y);

        }

        private void CreatePathInput()
        {
            int x = lb_time.Location.X;
            int y = bt_addItem.Location.Y;
            int width = MIN_WIDTH - x - 80;
            tb_path_input = new TextBox()
            {
                Location = new Point(x, y),
                Width = width
            };
        }
    }
}
