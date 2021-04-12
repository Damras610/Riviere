namespace Rivière.BusinessLogic
{
    public class PlayerDrawResult
    {
        public Player player;
        public bool giving;
        public int numberOfOccurences;
        public int numberOfSips;

        public PlayerDrawResult(Player player, bool giving, int numberOfOccurences, int numberOfSips)
        {
            this.player = player;
            this.giving = giving;
            this.numberOfOccurences = numberOfOccurences;
            this.numberOfSips = numberOfSips;
        }
    }
}
