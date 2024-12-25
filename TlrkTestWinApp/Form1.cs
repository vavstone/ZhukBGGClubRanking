using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using ZhukBGGClubRanking.Core;

namespace TlrkTestWinApp
{
    public partial class Form1 : Form
    {

        public GamesNamesTranslateFile GamesTranslate { get; set; }
        public BGGCollection CommonCollection { get; set; }
        private List<GameRatingListFile> usersRatingListFiles;
        private GameRatingList currentAvarageRatingList;

        public Form1()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //PrepareDataGrid(radGridView1);
            //LoadGamesTranslate();
            //LoadCommonCollection();
            //LoadUsersRatings();
            GridViewRowsReorderBoundMode();
        }

        void LoadCommonCollection()
        {
            CommonCollection = BGGCollection.LoadFromFile(GamesTranslate);
        }

        void LoadGamesTranslate()
        {
            GamesTranslate = GamesNamesTranslateFile.LoadFromFile();
        }

        void LoadUsersRatings()
        {
            usersRatingListFiles = GameRatingListFile.LoadFromFolder(CommonCollection);

            foreach (var item in usersRatingListFiles)
            {
                UpdateGamesCrossRatings(item.RatingList, usersRatingListFiles.Select(c => c.RatingList));
            }
            radGridView1.DataSource = usersRatingListFiles.First().RatingList.GameList.OrderBy(c => c.Rating).ThenBy(c => c.Game).ToList();

        }

        private void UpdateGamesCrossRatings(GameRatingList currentList, IEnumerable<GameRatingList> allLists)
        {
            foreach (var gameToFill in currentList.GameList)
            {
                var currentListUser = currentList.UserNames.FirstOrDefault();
                if (currentListUser != null)
                {
                    foreach (var list in allLists.Where(c => c.UserNames.FirstOrDefault() != currentListUser))
                    {
                        var game = list.GameList.FirstOrDefault(
                            c => c.GameEng.ToUpper() == gameToFill.GameEng.ToUpper());
                        if (game != null)
                        {
                            gameToFill.UsersRating.UserRating.Add(new UserRating
                                { UserName = list.UserNames.FirstOrDefault(), Rating = game.Rating });
                        }
                    }

                    if (gameToFill.UsersRating.UserRating.Any())
                    {
                        var splitter = "; ";
                        var resultStr = "";
                        foreach (var item in gameToFill.UsersRating.UserRating.OrderBy(c => c.Rating).ThenBy(c => c.UserName))
                            resultStr += string.Format("{0} - {1}{2}", item.Rating, item.UserName, splitter);
                        gameToFill.UserRatingString = resultStr.Substring(0, resultStr.Length - splitter.Length);
                    }
                }
            }
        }

        public void PrepareDataGrid(RadGridView grid)
        {
            grid.AutoGenerateColumns = false;
            //grid.AllowUserToAddRows = false;
            //grid.AllowUserToDeleteRows = false;
            //grid.ShowEditingIcon = false;
            //grid.RowHeadersVisible = false;


            var colName = new GridViewTextBoxColumn();
            colName.Width = 350;
            //colName.Width =246;
            colName.HeaderText = "Название";
            colName.Name = "Game";
            colName.FieldName = "Game";
            grid.Columns.Add(colName);
            var colRate = new GridViewTextBoxColumn();
            colRate.Width = 60;
            colRate.HeaderText = "Рейтинг";
            colRate.Name = "Rating";
            colRate.FieldName = "Rating";
            colRate.ReadOnly = true;
            grid.Columns.Add(colRate);

            var col = new GridViewTextBoxColumn();
            col.HeaderText = "Владеют";
            col.Name = "BGGComments";
            col.FieldName = "BGGComments";
            col.Width = 150;
            grid.Columns.Add(col);

            //grid.CellContentClick += Grid_CellContentClick;
            //grid.RowsDefaultCellStyle.SelectionBackColor = Color.LightGray;
            //grid.RowsDefaultCellStyle.SelectionForeColor = Color.Black;

            //grid.MouseWheel += DataGridView_MouseWheel;
            //grid.MouseEnter += DataGridView_MouseEnter;

            //grid.ColumnHeaderMouseClick += Grid_ColumnHeaderMouseClick;

        }


        public void GridViewRowsReorderBoundMode()
        {
            //InitializeComponent();
            BindingList<Item> items = new BindingList<Item>();
            for (int i = 0; i < 50; i++)
            {
                items.Add(new Item(i, "Item" + i));
            }
            this.radGridView1.DataSource = items;
            this.radGridView1.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            //register the custom row behavior
            BaseGridBehavior gridBehavior = this.radGridView1.GridBehavior as BaseGridBehavior;
            gridBehavior.UnregisterBehavior(typeof(GridViewDataRowInfo));
            gridBehavior.RegisterBehavior(typeof(GridViewDataRowInfo), new CustomGridDataRowBehavior());
            //handle drag and drop events for the grid through the DragDrop service
            RadDragDropService svc =
                this.radGridView1.GridViewElement.GetService<RadDragDropService>();
            svc.PreviewDragStart += svc_PreviewDragStart;
            svc.PreviewDragDrop += svc_PreviewDragDrop;
            svc.PreviewDragOver += svc_PreviewDragOver;
        }
        //required to initiate drag and drop when grid is in bound mode
        private void svc_PreviewDragStart(object sender, PreviewDragStartEventArgs e)
        {
            e.CanStart = true;
        }
        private void svc_PreviewDragOver(object sender, RadDragOverEventArgs e)
        {
            if (e.DragInstance is GridDataRowElement)
            {
                e.CanDrop = e.HitTarget is GridDataRowElement ||
                            e.HitTarget is GridTableElement ||
                            e.HitTarget is GridSummaryRowElement;
            }
        }
        //initiate the move of selected row
        private void svc_PreviewDragDrop(object sender, RadDropEventArgs e)
        {
            GridDataRowElement rowElement = e.DragInstance as GridDataRowElement;
            if (rowElement == null)
            {
                return;
            }
            e.Handled = true;
            RadItem dropTarget = e.HitTarget as RadItem;
            RadGridView targetGrid = dropTarget.ElementTree.Control as RadGridView;
            if (targetGrid == null)
            {
                return;
            }
            var dragGrid = rowElement.ElementTree.Control as RadGridView;
            if (targetGrid == dragGrid)
            {
                e.Handled = true;
                GridDataRowElement dropTargetRow = dropTarget as GridDataRowElement;
                int index = dropTargetRow != null ? this.GetTargetRowIndex(dropTargetRow, e.DropLocation) : targetGrid.RowCount;
                GridViewRowInfo rowToDrag = dragGrid.SelectedRows[0];
                this.MoveRows(dragGrid, rowToDrag, index);
            }
        }
        private int GetTargetRowIndex(GridDataRowElement row, Point dropLocation)
        {
            int halfHeight = row.Size.Height / 2;
            int index = row.RowInfo.Index;
            if (dropLocation.Y > halfHeight)
            {
                index++;
            }
            return index;
        }
        private void MoveRows(RadGridView dragGrid,
            GridViewRowInfo dragRow, int index)
        {
            dragGrid.BeginUpdate();
            GridViewRowInfo row = dragRow;
            if (row is GridViewSummaryRowInfo)
            {
                return;
            }
            if (dragGrid.DataSource != null && typeof(System.Collections.IList).IsAssignableFrom(dragGrid.DataSource.GetType()))
            {
                //bound to a list of objects scenario
                var sourceCollection = (System.Collections.IList)dragGrid.DataSource;
                if (row.Index < index)
                {
                    index--;
                }
                sourceCollection.Remove(row.DataBoundItem);
                sourceCollection.Insert(index, row.DataBoundItem);
            }
            else
            {
                throw new ApplicationException("Unhandled Scenario");
            }
            dragGrid.EndUpdate(true);
        }
        public class CustomGridDataRowBehavior : GridDataRowBehavior
        {
            protected override bool OnMouseDownLeft(MouseEventArgs e)
            {
                GridDataRowElement row = this.GetRowAtPoint(e.Location) as GridDataRowElement;
                if (row != null)
                {
                    RadGridViewDragDropService svc = this.GridViewElement.GetService<RadGridViewDragDropService>();
                    svc.AllowAutoScrollColumnsWhileDragging = false;
                    svc.AllowAutoScrollRowsWhileDragging = false;
                    svc.Start(row);
                }
                return base.OnMouseDownLeft(e);
            }
        }
        public class Item
        {
            public Item(int id, string name)
            {
                this.Id = id;
                this.Name = name;
            }
            public int Id { get; set; }
            public string Name { get; set; }
        }

    }




}
