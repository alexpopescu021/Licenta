namespace Licenta.Model
{
    public class VehicleDriver : DataEntity
    {
        public Vehicle Vehicle { get; set; }
        public Driver Driver { get; set; }
    }
}
