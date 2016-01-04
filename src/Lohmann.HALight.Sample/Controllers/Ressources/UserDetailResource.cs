namespace Lohmann.HALight.Sample.Controllers.Ressources
{
    public class UserDetailResource : Resource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Mail { get; set; }

        public string Address { get; set; }
    }
}