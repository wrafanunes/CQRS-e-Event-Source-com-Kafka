using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Post.Query.Domain.Entities
{
    [Table("Comentario", Schema = "dbo")]
    public class ComentarioEntity
    {
        [Key]
        public Guid IdComentario { get; set; }
        public string NomeUsuario { get; set; }
        public DateTime DataComentario { get; set; }
        public string Comentario { get; set; }
        public bool Editado { get; set; }
        public Guid IdPublicacao { get; set; }

        /* aqui o JsonIgnore é usado para evitar que a classe ComentarioEntity seja retornada em forma de json e caia em uma referência circular, já que a classe PublicacaoEntity
        contém a classe ComentarioEntity na forma de composição */
        [JsonIgnore]
        public virtual PublicacaoEntity Publicacao { get; set; }
    }
}