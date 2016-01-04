using System.ComponentModel.DataAnnotations;

namespace Lohmann.HALight.Sample.Model
{
    public class User
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public string Mail { get; set; }

        public string Address { get; set; }
    }
}