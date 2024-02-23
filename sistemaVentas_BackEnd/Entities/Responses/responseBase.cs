namespace sistemaVentas_BackEnd.Entities.Responses
{
    public class responseBase<t>:responseMain
    {
        public t data { get; set; }
    }
}
