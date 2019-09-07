using System;
using System.Drawing;
using System.Windows.Forms;


namespace TimeTracker
{
    public partial class TimeTrackerView : Form
    {

        public TimeTrackerController Controller { get; set; }


        public ListView lv_items { get; set; }

        public Button bt_delItem { get; set; }
        public Button bt_start_stop { get; set; }
        public Button bt_stop { get; set; }

        public Label lb_time { get; set; }

        //public TextBox tb_path_input { get; set; }
        public TextBox tb_new_item_input { get; set; }

        private readonly string col_1_name = "Name";
        private readonly string col_2_name = "Time";

        public int MIN_WIDTH = 826;
        public int MIN_HEIGHT = 260;

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

            CreateControls();
        }

        private void SetApplicationParameter()
        {
            AccessibleName = "TimeTracker5000";
            Icon = new Icon("icon.ico");
            Name = "TimeTracker5000";
            Text = "TimeTracker5000";
            Width = MIN_WIDTH;
            Height = MIN_HEIGHT;
            MinimumSize = new Size(MIN_WIDTH, MIN_HEIGHT);
        }

        private void CreateControls()
        {
            Resize += new EventHandler(Controller.TimeTrackerView_Resize);
            Load += new EventHandler(Controller.TimeTrackerView_Load);


            int x_lv_items = x_pad;
            int y_lv_items = y_pad;
            int w_lv_items = 200;
            int h_lv_items = Height - 80;

            lv_items = new ListView()
            {
                Bounds = new Rectangle(new Point(x_lv_items, y_lv_items), new Size(w_lv_items, h_lv_items)),
                View = View.Details,
                LabelEdit = true,
                AllowColumnReorder = true,
                CheckBoxes = true,
                Visible = true,
            };
            lv_items.Columns.Clear();
            lv_items.Columns.Add(col_1_name, -2, HorizontalAlignment.Left);
            lv_items.Columns.Add(col_2_name, -2, HorizontalAlignment.Left);
            lv_items.SelectedIndexChanged += new EventHandler(Controller.ListViewItem_Selected);
            Controls.Add(lv_items);


            int x_bt_start_stop = w_lv_items + x_lv_items; 
            int y_bt_start_stop = y_lv_items; 
            bt_start_stop = new Button()
            {
                Text = "Start",
                Location = new Point(x_bt_start_stop, y_bt_start_stop),
            };
            bt_start_stop.Click += new EventHandler(Controller.BT_Start_Stop_Clicked);
            Controls.Add(bt_start_stop);


            int x_bt_delItem = x_lv_items;
            int y_bt_delItem = y_lv_items + lv_items.Height + 5;
            bt_delItem = new Button()
            {
                Text = "Delete Item",
                Location = new Point(x_bt_delItem, y_bt_delItem),
                Width = w_lv_items
            };
            bt_delItem.Click += new EventHandler(Controller.BT_DelItem_Clicked);
            Controls.Add(bt_delItem);


            Font font = new Font("Arial", 80, FontStyle.Bold);
            string startVal = "00:00:00";
            lb_time = new Label()
            {
                AutoSize = true,
                Text = startVal,
                Font = font
            };
            int x_lb_time = x_bt_start_stop + bt_start_stop.Width;
            int y_lb_time = (Height / 2) - (font.Height / 2);

            lb_time.Location = new Point(x_lb_time, y_lb_time);
            Controls.Add(lb_time);


            int x_tb_path_input = lb_time.Location.X;
            int y_tb_path_input = bt_start_stop.Location.Y;
            int w_tb_path_input = MIN_WIDTH - x_tb_path_input - 80;
            //tb_path_input = new TextBox()
            //{
            //    Location = new Point(x_tb_path_input, y_tb_path_input),
            //    Width = w_tb_path_input
            //};
            //tb_path_input.KeyDown += new KeyEventHandler(Controller.TB_PathInput_EnterPressed);
            //Controls.Add(tb_path_input);


            int x_tb_new_item_input = x_tb_path_input;
            int y_tb_new_item_input = y_tb_path_input;
            int w_tb_new_item_input = w_tb_path_input;
            tb_new_item_input = new TextBox()
            {
                Location = new Point(x_tb_new_item_input, y_tb_new_item_input),
                Width = w_tb_new_item_input
            };
            tb_new_item_input.KeyDown += new KeyEventHandler(Controller.TB_NewItemInput_EnterPressed);
            Controls.Add(tb_new_item_input);
        }
    }
}
