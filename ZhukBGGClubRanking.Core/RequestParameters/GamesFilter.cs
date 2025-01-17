using System;
using System.Collections.Generic;
using System.Text;

namespace ZhukBGGClubRanking.Core
{
    public class GamesFilter
    {
        /// <summary>
        /// Только игры клуба
        /// </summary>
        public bool GetOnlyClubCollection { get; set; }
        
        /// <summary>
        /// Минимально учитываемый порог рейтинга BGG
        /// </summary>
        public double MinAverageBGGRating { get; set; }

        /// <summary>
        /// Минимальное количество пользователей BGG, участвовавших в формировании рейтинга
        /// </summary>
        public int MinUsersRatedBGG { get; set; }

        /// <summary>
        /// Только самостоятельные игры
        /// </summary>
        public bool OnlyStandaloneGames { get; set; }

        /// <summary>
        /// Только дополнения
        /// </summary>
        public bool OnlyAdditions { get; set; }
    }
}
