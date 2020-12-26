using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CasaDoCodigo.Models
{
    //Este atributo é necessario para permitir a serialização do do objeto.
    //
    [DataContract]
    public class BaseModel
    {
        ///
        [DataMember]
        public int Id { get; protected set; }
    }

    public class Produto : BaseModel
    {
        public Produto()
        {

        }

        [Required]
        public string Codigo { get; private set; }
        [Required]
        public string Nome { get; private set; }
        [Required]
        public decimal Preco { get; private set; }

        public Produto(string codigo, string nome, decimal preco)
        {
            this.Codigo = codigo;
            this.Nome = nome;
            this.Preco = preco;
        }
    }

    public class Cadastro : BaseModel
    {
        public Cadastro()
        {
        }

        public virtual Pedido Pedido { get; set; }

        [MinLength(5, ErrorMessage = "Nome deve ter no mínimo 5 caracteres")]
        [MaxLength(55, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
        [Required(ErrorMessage = nameof(Nome) + " é obrigatório!")]
        public string Nome { get; set; } = "";

        [Required(ErrorMessage = nameof(Email) + " é obrigatório!")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = nameof(Telefone) + " é obrigatório!")]
        public string Telefone { get; set; } = "";

        [Required(ErrorMessage = nameof(Endereco) + " é obrigatório!")]
        public string Endereco { get; set; } = "";

        [Required(ErrorMessage = nameof(Complemento) + " é obrigatório!")]
        public string Complemento { get; set; } = "";

        [Required(ErrorMessage = nameof(Bairro) + " é obrigatório!")]
        public string Bairro { get; set; } = "";

        [Required(ErrorMessage = nameof(Municipio) + " é obrigatório!")]
        public string Municipio { get; set; } = "";

        [Required(ErrorMessage = " O Estado é obrigatório!")]
        public string UF { get; set; } = "";

        [Required(ErrorMessage = nameof(CEP) + " é obrigatório!")]
        public string CEP { get; set; } = "";

        internal void Update(Cadastro novoCadastro)
        {
            this.Bairro = novoCadastro.Bairro;
            this.Municipio = novoCadastro.Municipio;
            this.CEP = novoCadastro.CEP;
            this.UF = novoCadastro.UF;
            this.Telefone = novoCadastro.Telefone;
            this.Complemento = novoCadastro.Complemento;
            this.Endereco = novoCadastro.Endereco;
            this.Nome = novoCadastro.Nome;
            this.Email = novoCadastro.Email;
        }
    }

    [DataContract]
    public class ItemPedido : BaseModel
    {   
        [Required]
        [DataMember]
        public Pedido Pedido { get; private set; }
        
        [Required]
        [DataMember]
        public Produto Produto { get; private set; }
        
        [Required]
        [DataMember]
        public int Quantidade { get; private set; }

        [Required]
        [DataMember]
        public decimal PrecoUnitario { get; private set; }

        [DataMember]
        public decimal Subtotal => Quantidade * PrecoUnitario;

        public ItemPedido()
        {

        }

        public ItemPedido(Pedido pedido, Produto produto, int quantidade, decimal precoUnitario)
        {
            Pedido = pedido;
            Produto = produto;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }

        public void AtualizaQuantidade(int Quantidade)
        {
            this.Quantidade = Quantidade;
        }
    }

    public class Pedido : BaseModel
    {
        public Pedido()
        {
            Cadastro = new Cadastro();
        }

        public Pedido(Cadastro cadastro)
        {
            Cadastro = cadastro;
        }

        public List<ItemPedido> Itens { get; private set; } = new List<ItemPedido>();
        [Required]
        public virtual Cadastro Cadastro { get; private set; }
    }
}
