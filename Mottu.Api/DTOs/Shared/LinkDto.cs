namespace Mottu.Api.DTOs.Shared
{
    /// <summary>
    /// Representa um link HATEOAS. Usado para informar ao cliente
    /// quais outras ações podem ser realizadas a partir de um recurso.
    /// </summary>
    public class LinkDto
    {
        /// <summary>
        /// O URL do link.
        /// </summary>
        /// <example>/api/filiais/1</example>
        public string Href { get; private set; }

        /// <summary>
        /// A relação do link com o recurso atual (o que o link faz).
        /// </summary>
        /// <example>self</example>
        public string Rel { get; private set; }

        /// <summary>
        /// O método HTTP a ser usado para a ação.
        /// </summary>
        /// <example>GET</example>
        public string Method { get; private set; }

        public LinkDto(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}