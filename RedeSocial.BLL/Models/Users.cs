
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeSocial.BLL.Models;

public class Users {
    public int Id { get; set; }
    public string Nome { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }


}
