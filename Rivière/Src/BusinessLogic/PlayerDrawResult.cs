namespace Rivière.BusinessLogic
{
    public class PlayerDrawResult
    {
        public Player player;
        public bool giving;
        public int numberOfSips;

        public PlayerDrawResult(Player player, bool giving, int numberOfSips)
        {
            this.player = player;
            this.giving = giving;
            this.numberOfSips = numberOfSips;
        }
    }
}
