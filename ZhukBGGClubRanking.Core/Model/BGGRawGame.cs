using System;
using System.Collections.Generic;
using System.Text;

namespace ZhukBGGClubRanking.Core.Model
{
    public class BGGRawGame
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? YearPublished { get; set; }
        public int? Rank { get; set; }
        public double? Bayesaverage { get; set; }
        public double? Average { get; set; }
        public int? Usersrated { get; set; }
        public bool IsExpansion { get; set; }
        public int? AbstractsRank { get; set; }
        public int? CgsRank { get; set; }
        public int? ChildrensgamesRank { get; set; }
        public int? FamilygamesRank { get; set; }
        public int? PartygamesRank { get; set; }
        public int? StrategygamesRank { get; set; }
        public int? ThematicRank { get; set; }
        public int? WargamesRank { get; set; }
    }
}
