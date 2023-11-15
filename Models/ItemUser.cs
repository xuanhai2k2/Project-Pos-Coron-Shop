using System.ComponentModel.DataAnnotations;// tag key
using System.ComponentModel.DataAnnotations.Schema;//tag table
namespace devpro_project.Models
{
    //Class này tác độg đến table nào
    [Table("Users")]
    public class ItemUser
    {
        //Định nghĩa Key
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
