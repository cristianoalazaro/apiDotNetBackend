namespace Api.Models
{
    public class TrocarSenha
    {
        public Guid ID { get; set; }
        public int Usuario_ID { get; set; }
        public DateTime Expira_Em { get; set; }
    }
}
