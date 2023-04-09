using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeSocial.BLL.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string evento { get; set; }

        public DateTime DataEvento { get; set; }

        public string localEvento { get; set; }

        public string descricaoEvento { get; set; }

    }
}
