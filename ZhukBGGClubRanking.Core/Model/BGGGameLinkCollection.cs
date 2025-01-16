using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZhukBGGClubRanking.Core.Model
{
    public class BGGGameLinkCollection
    {
        public List<BoardGameAccessory> BGAccessories { get; set; } = new List<BoardGameAccessory>();
        public List<BoardGameArtist> BGArtists { get; set; } = new List<BoardGameArtist>();
        public List<BoardGameCategory> BGCategories { get; set; } = new List<BoardGameCategory>();
        public List<BoardGameCompilation> BGCompilations { get; set; } = new List<BoardGameCompilation>();
        public List<BoardGameDesigner> BGDesigners { get; set; } = new List<BoardGameDesigner>();
        public List<BoardGameExpansion> BGGExpansions { get; set; } = new List<BoardGameExpansion>();
        public List<BoardGameFamily> BGFamilies { get; set; } = new List<BoardGameFamily>();
        public List<BoardGameImplementation> BGImplementations { get; set; } = new List<BoardGameImplementation>();
        public List<BoardGameIntegration> BGIntegrations { get; set; } = new List<BoardGameIntegration>();
        public List<BoardGameMechanic> BGMechanics { get; set; } = new List<BoardGameMechanic>();
        public List<BoardGamePublisher> BGPublishers { get; set; } = new List<BoardGamePublisher>();
        public List<BoardUnknownLinkType> BGUnknownLinks { get; set; } = new List<BoardUnknownLinkType>();

        public bool Any()
        {
            return
                BGAccessories.Any() ||
                BGArtists.Any() ||
                BGCategories.Any() ||
                BGCompilations.Any() ||
                BGDesigners.Any() ||
                BGGExpansions.Any() ||
                BGFamilies.Any() ||
                BGImplementations.Any() ||
                BGIntegrations.Any() ||
                BGMechanics.Any() ||
                BGPublishers.Any() ||
                BGUnknownLinks.Any();
        }

        public string DesignersString
        {
            get
            {
                return BGDesigners.Select(c => c.TitleEng).JoinToString(", ");
            }
        }

        public string MechanicsString
        {
            get
            {
                return BGMechanics.Select(c => c.TitleEng).JoinToString(", ");
            }
        }

        public string CategoriesString
        {
            get
            {
                return BGCategories.Select(c => c.TitleEng).JoinToString(", ");
            }
        }
        public string FamiliesString
        {
            get
            {
                return BGFamilies.Select(c => c.TitleEng).JoinToString(", ");
            }
        }
    }
}
