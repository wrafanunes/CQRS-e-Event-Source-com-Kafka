using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Post.Query.Domain.Entities
{
    [Table("Publicacao", Schema = "dbo")]
    public class PublicacaoEntity
    {
        [Key]
        public Guid IdPublicacao { get; set; }
        public string Autor { get; set; }
        public DateTime DataPublicacao { get; set; }
        public string Mensagem { get; set; }
        public int Curtidas { get; set; }

        //este é um exemplo de uma propriedade de navegação, elas sempre devem ser virtuais
        public virtual ICollection<ComentarioEntity> Comentarios { get; set; }
    }
}