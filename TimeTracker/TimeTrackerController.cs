using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TimeTracker
{
    public class TimeTrackerController
    {
        public ConfigHandler ConfigHandler { get; set; }

        public TimeTrackerView View { get; set; }

        private Timer timer { get; set; }
        private bool Counting = false;

        private TrackedItem Current_TI;
        private ListViewItem Current_LVI;

        private string RemoteConfig { get; set; }

        public TimeTrackerController()
        {
            ConfigHandler = new ConfigHandler();
            InitTimer();
        }

        public void SetView(TimeTrackerView view)
        {
            View = view;
        }



        public void TimeTrackerView_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;

            View.lv_items.Height = View.Height - 80;
            View.lb_time.Location = new Point(View.lb_time.Location.X, View.Height / 2 - View.lb_time.Height / 2);
        }

        public void TimeTrackerView_Load(object sender, EventArgs e)
        {
            FillTrackedItemsListView();
            RemoteConfig = ConfigHandler.GetConfigPath();
            View.tb_path_input.Text = RemoteConfig;

        }

        public void BT_DelItem_Clicked(object sender, EventArgs e)
        {
            foreach (ListViewItem item in View.lv_items.Items)
            {
                if (item.Checked)
                {
                    View.lv_items.Items.Remove(item);
                    ConfigHandler.DeleteItem(new TrackedItem(item.Text));
                    View.lv_items.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
        }

        public void BT_Start_Stop_Clicked(object sender, EventArgs e)
        {
            if (!Counting)
            {
                Counting = true;
                foreach (ListViewItem lvItem in View.lv_items.Items)
                {
                    if (lvItem.Selected)
                    {
                        Current_TI = new TrackedItem(lvItem.Text, lvItem.SubItems[1].Text);
                        timer.Start();
                        break;
                    }
                }
                View.bt_start_stop.Text = "Stop";
            }
            else
            {
                Counting = false;
                if (Current_TI != null)
                {
                    timer.Stop();
                    ConfigHandler.UpdateItem(Current_TI);
                    Current_LVI.SubItems[1].Text = Current_TI.Time.ToString();
                }
                View.bt_start_stop.Text = "Start";
            }
        }

        public void TB_PathInput_EnterPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string path = View.tb_path_input.Text;
                path = path[path.Length - 1] == '/' ? path : path + "/";
                ConfigHandler.AddNewConfigPath(path);
                ConfigHandler.MoveTrackedItemsFile(path);
            }
        }

        public void ListViewItem_Selected(object sender, EventArgs e)
        {
            if (Current_TI != null && Counting)
            {
                Counting = false;
                timer.Stop();
                ConfigHandler.UpdateItem(Current_TI);
                Current_LVI.SubItems[1].Text = Current_TI.Time.ToString();
                View.bt_start_stop.Text = "Start";
            }

            foreach (ListViewItem lvItem in View.lv_items.Items)
            {
                if (lvItem.Selected)
                {
                    Current_LVI = lvItem;
                    View.lb_time.Text = lvItem.SubItems[1].Text;
                    break;
                }
            }

        }

        public void TB_NewItemInput_EnterPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string itemName = View.tb_new_item_input.Text;
                if (!string.IsNullOrWhiteSpace(itemName))
                {
                    itemName = itemName.Replace(' ', '_');
                    TrackedItem trackedItem = new TrackedItem(itemName);
                    if (ConfigHandler.GetItem(trackedItem) == null)
                    {
                        ConfigHandler.AddItem(trackedItem);
                        AddListViewItem(trackedItem);
                    }
                }
                View.tb_new_item_input.Text = "";
            }
        }

        //PRIVATE
        private void AddListViewItem(TrackedItem trackedItem)
        {
            ListViewItem lvItem = new ListViewItem()
            {
                Text = trackedItem.Name,
            };
            lvItem.SubItems.Add(trackedItem.Time.ToString());
            View.lv_items.Items.Add(lvItem);
            View.lv_items.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void InitTimer()
        {
            timer = new Timer();
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = 1000;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Current_TI.Time.Count();
            View.lb_time.Text = Current_TI.Time.ToString();
        }

        private void FillTrackedItemsListView()
        {
            var trackedItems = GetTrackedItems();
            foreach (var trackedItem in trackedItems)
            {
                AddListViewItem(trackedItem);
            }
        }

        private List<TrackedItem> GetTrackedItems()
        {
            return ConfigHandler.GetTrackedItems();
        }
    }
}
