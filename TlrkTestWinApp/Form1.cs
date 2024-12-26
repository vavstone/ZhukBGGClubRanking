using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;
using Telerik.WinControls.VirtualKeyboard;
using ZhukBGGClubRanking.Core;

namespace TlrkTestWinApp
{
    public partial class Form1 : Form
    {

        public GamesNamesTranslateFile GamesTranslate { get; set; }
        public BGGCollection CommonCollection { get; set; }
        private List<GameRatingListFile> usersRatingListFiles;
        public bool TeseraPreferable = false;


        public Form1()
        {
            InitializeComponent();
            radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            PrepareDataGrid(radGridView1);
            LoadGamesTranslate();
            LoadCommonCollection();
            LoadUsersRatings();

            PrepareGrid(radGridView1);
            SubscribeForGridEvents(radGridView1);
        }

        private void PrepareGrid(RadGridView grid)
        {
            grid.AllowDrop = true;
            grid.AllowEditRow = false;
            grid.AllowAddNewRow = false;
            grid.AllowRowReorder = true;
            grid.MultiSelect = true;
            grid.AllowDragToGroup = false;
            grid.GridBehavior = new CustomGridBehavior();
            grid.TableElement.RowHeight = 70;
        }

        private void SubscribeForGridEvents(RadGridView grid)
        {
            RadDragDropService dragDropService = grid.GridViewElement.GetService<RadDragDropService>();
            dragDropService.PreviewDragOver += dragDropService_PreviewDragOver;
            dragDropService.PreviewDragDrop += dragDropService_PreviewDragDrop;
            dragDropService.PreviewDragHint += dragDropService_PreviewDragHint;
            grid.CellClick += Grid_CellClick;
            grid.CellFormatting += Grid_CellFormatting;
            grid.SortChanged += Grid_SortChanged;
            //grid.CustomSorting += Grid_CustomSorting;
        }

        private void Grid_SortChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            if (e.Action==NotifyCollectionChangedAction.Remove)
                radGridView1.AllowRowReorder = true;
            else
                radGridView1.AllowRowReorder = false;
        }

        private void Grid_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            e.CellElement.DrawFill = true;
            e.CellElement.NumberOfColors = 1;

            e.CellElement.DrawBorder = true;
            e.CellElement.BorderBoxStyle = Telerik.WinControls.BorderBoxStyle.FourBorders;
            e.CellElement.BorderLeftWidth = 1;
            e.CellElement.BorderRightWidth = 0;
            e.CellElement.BorderBottomWidth = 1;
            e.CellElement.BorderTopWidth = 0;
            e.CellElement.BorderBottomColor = Color.Gray;
            e.CellElement.BorderTopColor = Color.Gray;
            e.CellElement.BorderLeftColor = Color.Gray;
            e.CellElement.BorderRightColor = Color.Gray;
        }

        //private void Grid_CustomSorting(object sender, GridViewCustomSortingEventArgs e)
        //{
        //    var k = e;
        //}

        private void Grid_CellClick(object sender, GridViewCellEventArgs e)
        {
            

            //if (checkBoxCellElement != null)
            //{
            //    RadCheckBoxEditorElement element = checkBoxCellElement.Children[0] as RadCheckBoxEditorElement;
            //    Point mousPos = element.ElementTree.Control.PointToClient(Control.MousePosition);

            //    if (element != null && element.Checkmark.ControlBoundingRectangle.Contains(mousPos))
            //    {
            //        //checkbox is clicked
            //    }
            //}

            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    GridHyperlinkCellElement cell = sender as GridHyperlinkCellElement;
                    if (cell != null)
                    {
                        var element = cell.Children[0] as GridHyperlinkCellContentElement;
                        if (element != null)
                        {
                            var mousPos = element.ElementTree.Control.PointToClient(Control.MousePosition);
                            if (element.ControlBoundingRectangle.Contains(mousPos))
                            {
                                var gameEngName = e.Row.Tag.ToString();
                                var gameInBGGColl = CommonCollection.GetItemByName(gameEngName);
                                string url = string.Empty;
                                var teseraUrl = GetTeseraCardUrl(gameEngName);
                                var bggUrl = string.Empty;
                                if (gameInBGGColl != null)
                                    bggUrl = GetBGGCardUrl(gameInBGGColl.ObjectId);
                                if (TeseraPreferable)
                                    url = teseraUrl;
                                if (string.IsNullOrWhiteSpace(url))
                                    url = bggUrl;
                                ProcessStartInfo sInfo = new ProcessStartInfo(url);
                                Process.Start(sInfo);
                            }
                        }

                    }
                }
                else if (e.ColumnIndex==3)
                {
                    var gameEngName = e.Row.Tag.ToString();
                    var gameInBGGColl = CommonCollection.GetItemByName(gameEngName);
                    if (gameInBGGColl != null)
                    {
                        var largeImgPathWithoutExt = Application.StartupPath + "\\images\\large\\" + gameInBGGColl.ObjectId;
                        var largeImgPath = largeImgPathWithoutExt + ".jpg";
                        if (!File.Exists(largeImgPath))
                            largeImgPath = largeImgPathWithoutExt + ".png";
                        var img = Image.FromFile(largeImgPath);
                        GridImageCellElement cell = sender as GridImageCellElement;
                        if (cell != null)
                        {
                            
                                var mousPos = cell.ElementTree.Control.PointToClient(Control.MousePosition);
                            //radPictureBox1.Top = mousPos.Y - 100;
                            //radPictureBox1.Left = mousPos.X - 100;
                            radPictureBox1.Image = img;
                            radPictureBox1.MinimumSize = new Size(500, 500);
                            //radPictureBox1.Width = 300;
                            radPictureBox1.Left = (ClientSize.Width - radPictureBox1.Width) / 2;
                            radPictureBox1.Top = (ClientSize.Height - radPictureBox1.Height) / 2;

                            radPictureBox1.Show();
                            

                        }
                    }
                }




            }
        }

        public string GetTeseraCardUrl(string gameEng)
        {
            var teseraName = GamesTranslate.GetTeseraName(gameEng);
            if (!string.IsNullOrEmpty(teseraName))
                return Settings.TeseraCardPrefixUrl + teseraName;
            return null;
        }

        public string GetBGGCardUrl(int bggObjectId)
        {
            return Settings.BGGCardPrefixUrl + bggObjectId;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
           
        }

        void LoadCommonCollection()
        {
            CommonCollection = BGGCollection.LoadFromFile(GamesTranslate);
        }

        void LoadGamesTranslate()
        {
            GamesTranslate = GamesNamesTranslateFile.LoadFromFile();
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
            //grid.AutoGenerateColumns = false;
            //grid.AllowUserToAddRows = false;
            //grid.AllowUserToDeleteRows = false;
            //grid.ShowEditingIcon = false;
            //grid.RowHeadersVisible = false;
           


            var colName = new GridViewHyperlinkColumn();
            colName.Width = 350;
            colName.HeaderText = "Название";
            colName.Name = "Game";
            colName.FieldName = "Game";
            colName.AllowSort = true;
            grid.Columns.Add(colName);

            var colRate = new GridViewTextBoxColumn();
            colRate.Width = 60;
            colRate.HeaderText = "Рейтинг";
            colRate.Name = "Rating";
            colRate.FieldName = "Rating";
            colRate.TextAlignment = ContentAlignment.MiddleCenter;
            grid.Columns.Add(colRate);

            var colHost = new GridViewTextBoxColumn();
            colHost.HeaderText = "Владеют";
            colHost.Name = "BGGComments";
            colHost.FieldName = "BGGComments";
            colHost.Width = 150;
            colHost.TextAlignment = ContentAlignment.MiddleCenter;
            grid.Columns.Add(colHost);

            var colImage = new GridViewImageColumn();
            colImage.HeaderText = "Фото";
            colImage.ImageLayout = ImageLayout.Zoom;
            colImage.TextAlignment = ContentAlignment.MiddleCenter;
            grid.Columns.Add(colImage);

        }

        void LoadUsersRatings()
        {
            radGridView1.Rows.Clear();
            usersRatingListFiles = GameRatingListFile.LoadFromFolder(CommonCollection);

            foreach (var item in usersRatingListFiles)
            {
                UpdateGamesCrossRatings(item.RatingList, usersRatingListFiles.Select(c => c.RatingList));
            }

            foreach (var gameRating in usersRatingListFiles.First().RatingList.GameList.OrderBy(c => c.Rating).ThenBy(c => c.Game).ToList())
            {
                GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(radGridView1.MasterView);
                rowInfo.Cells[0].Value = gameRating.Game;
                rowInfo.Cells[1].Value = gameRating.Rating;
                rowInfo.Cells[2].Value = gameRating.BGGComments;
                rowInfo.Tag = gameRating.GameEng;
                radGridView1.Rows.Add(rowInfo);

                if (gameRating.BGGItem != null)
                {
                    var smallImgPathWithoutExt = Application.StartupPath + "\\images\\small\\" + gameRating.BGGItem.ObjectId;
                    var smallImgPath = smallImgPathWithoutExt +".jpg";
                    if (!File.Exists(smallImgPath))
                        smallImgPath = smallImgPathWithoutExt + ".png";
                    var img = Image.FromFile(smallImgPath);
                    rowInfo.Cells[3].Value = img;
                }
            }
            //radGridView1.GridNavigator.ScrollToRow(0);
            radGridView1.CurrentRow = radGridView1.Rows[0];
        }

        void ReCalcRatingInDataGrid(RadGridView grid)
        {
            for (int i = 1; i < grid.Rows.Count; i++)
            {
                var row = grid.Rows[i-1];
                row.Cells[1].Value = i;
            }
            
        }



        private void dragDropService_PreviewDragOver(object sender, RadDragOverEventArgs e)
        {
            if (e.DragInstance is SnapshotDragItem)
            {
                e.CanDrop = e.HitTarget is GridDataRowElement || e.HitTarget is GridTableElement || e.HitTarget is GridSummaryRowElement;
            }
        }

        private void dragDropService_PreviewDragDrop(object sender, RadDropEventArgs e)
        {
            SnapshotDragItem dragInstance = e.DragInstance as SnapshotDragItem;
            if (dragInstance == null)
            {
                return;
            }

            RadItem dropTarget = e.HitTarget as RadItem;
            RadGridView targetGrid = dropTarget.ElementTree.Control as RadGridView;
            if (targetGrid == null)
            {
                return;
            }

            RadGridView dragGrid = dragInstance.Item.ElementTree.Control as RadGridView;

            e.Handled = true;
            CustomGridBehavior behavior = (CustomGridBehavior)dragGrid.GridBehavior;
            GridDataRowElement dropTargetRow = dropTarget as GridDataRowElement;
            int index = dropTargetRow != null ? this.GetTargetRowIndex(dropTargetRow, e.DropLocation) : targetGrid.RowCount;
            this.MoveRows(targetGrid, dragGrid, behavior.SelectedRows, index);
        }

        private void dragDropService_PreviewDragHint(object sender, PreviewDragHintEventArgs e)
        {
            SnapshotDragItem dragInstance = e.DragInstance as SnapshotDragItem;

            if (dragInstance == null)
            {
                return;
            }

            //GridViewRowInfo rowInfo = e.DragInstance.GetDataContext() as GridViewRowInfo;
        }

        private void MoveRows(RadGridView targetGrid, RadGridView dragGrid, IList<GridViewRowInfo> dragRows, int index)
        {
            for (int i = 0; i <= dragRows.Count - 1; i++)
            {
                GridViewRowInfo row = dragRows[i];
                if (row is GridViewSummaryRowInfo)
                {
                    continue;
                }

                GridViewRowInfo newRow = targetGrid.Rows.NewRow();
                this.InitializeRow(newRow, row);
                if (index > targetGrid.Rows.Count)
                {
                    index = targetGrid.Rows.Count;
                }
                targetGrid.Rows.Insert(index, newRow);
                row.IsSelected = false;
                dragGrid.Rows.Remove(row);
                index++;
            }
            ReCalcRatingInDataGrid(radGridView1);
        }

        private void InitializeRow(GridViewRowInfo destRow, GridViewRowInfo sourceRow)
        {
            for (int i = 0; i < sourceRow.Cells.Count; i++)
            {
                destRow.Cells[i].Value = sourceRow.Cells[i].Value;
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

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var row in radGridView1.Rows)
            {
                var k = row;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadUsersRatings();
        }

        private void radPictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            radPictureBox1.Hide();
        }

        private void radPictureBox1_MouseLeave(object sender, EventArgs e)
        {
            radPictureBox1.Hide();
        }
    }
}
