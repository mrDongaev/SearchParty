namespace DataAccess.Entities
{
    public class PlayerHero
    {
        public int HeroId { get; set; }

        public Hero Hero { get; set; }

        public Guid PlayerId { get; set; }

        public Player Player { get; set; }
    }
}
